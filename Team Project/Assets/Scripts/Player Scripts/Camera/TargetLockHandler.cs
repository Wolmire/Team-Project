using System;
using Unity.Cinemachine;
using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;

public class TargetLockHandler : MonoBehaviour
{
    [SerializeField] private CharacterController playerController;
    public SetTarget setTarget;
    [SerializeField] private Animator cameraAnimator;

    public bool activeTarget;
    [SerializeField][Range(5, 50)] private float enemyDetectionRange;
    [ReadOnly][SerializeField] private List<Transform> nearbyTargets = new List<Transform>();
    public Transform currentTarget;

    [SerializeField] private GameObject freeLookCamera;
    [SerializeField] private GameObject targetLockCamera;

    [SerializeField] private LayerMask enemyLayer;

    public static Action<bool> OnTargetLock;

    [SerializeField] private float switchLockCooldown = 0.3f;
    private float switchAfterTime = 0f;

    private float accumulatedMouseDistance = 0f;
    private float lastMouseMovementTime = 0f;

    [SerializeField] private float mouseDistanceThreshold = 50f; 
    [SerializeField] private float mouseResetTime = 0.15f;

    private void Start()
    {
        SwitchCamera();
        if (setTarget?.visual != null) setTarget.visual.enabled = false;
    }

    private void FixedUpdate()
    {
        if (activeTarget && currentTarget != null)
        {
            float distance = Vector3.Distance(playerController.transform.position, currentTarget.position);
            if (distance > enemyDetectionRange * 1.5f) TargetLock(false);
        }
    }

    public void HandleSwitchTargetInput(string deviceType, float lookInputX)
    {
        if (!activeTarget) return;

        if (Time.time < switchAfterTime)
        {
            if (deviceType == "Gamepad" && Mathf.Abs(lookInputX) < 0.1f) switchAfterTime = 0;
            return;
        }

        if (deviceType == "Keyboard&Mouse")
        {
            if (Time.time - lastMouseMovementTime > mouseResetTime) accumulatedMouseDistance = 0f;

            if (Mathf.Abs(lookInputX) > 0.1f)
            {
                accumulatedMouseDistance += lookInputX;
                lastMouseMovementTime = Time.time;
            }

            if (Mathf.Abs(accumulatedMouseDistance) >= mouseDistanceThreshold)
            {
                SwitchTarget(switchToLeft: accumulatedMouseDistance < 0);

                accumulatedMouseDistance = 0f;
                switchAfterTime = Time.time + switchLockCooldown;
            }
        }
        else
        {
            float deadzone = 0.4f;

            if (lookInputX < -deadzone)
            {
                SwitchTarget(switchToLeft: true);
                switchAfterTime = Time.time + switchLockCooldown;
            }
            else if (lookInputX > deadzone)
            {
                SwitchTarget(switchToLeft: false);
                switchAfterTime = Time.time + switchLockCooldown;
            }
        }
    }
    public void TargetLock(bool boolval)
    {
        if (boolval)
        {
            CollectTargetsAndGetMostInFrontTarget(out Transform targetMostInFront);

            if (targetMostInFront != null)
            {
                currentTarget = targetMostInFront;
                setTarget.SetTargetPos(currentTarget);
                activeTarget = true;
                SwitchCamera();
                if (setTarget.visual != null) setTarget.visual.enabled = true;
            }
        }
        else
        {
            nearbyTargets.Clear();
            currentTarget = null;
            if (setTarget?.visual != null) setTarget.visual.enabled = false;
            activeTarget = false;
            SwitchCamera();
        }
        OnTargetLock?.Invoke(activeTarget);
    }

    private void SwitchTarget(bool switchToLeft)
    {
        if (!activeTarget) return;

        CollectTargetsAndGetMostInFrontTarget(out _);

        if (nearbyTargets.Count <= 1) return;

        int currentTargetIndex = GetTargetIndex(currentTarget);

        if (currentTargetIndex == -1)
        {
            CollectTargetsAndGetMostInFrontTarget(out Transform targetMostInFront);
            currentTargetIndex = GetTargetIndex(targetMostInFront);
        }

        if (switchToLeft)
        {
            if (currentTargetIndex > 0) currentTargetIndex--;

            else return;
        }
        else
        {
            if (currentTargetIndex < nearbyTargets.Count - 1) currentTargetIndex++;

            else return;
        }

        currentTarget = nearbyTargets[currentTargetIndex];

        if (currentTarget != null)
        {
            CinemachineGroupFraming groupFraming = targetLockCamera.GetComponent<CinemachineGroupFraming>();
            if (groupFraming != null) groupFraming.Damping = 1f;

            setTarget.SetTargetPos(currentTarget);
        }
    }

    private void CollectTargetsAndGetMostInFrontTarget(out Transform targetMostInFront)
    {
        List<Transform> foundTargets = new List<Transform>();
        Camera mainCam = Camera.main;

        if (mainCam == null)
        {
            targetMostInFront = null;
            return;
        }

        Collider[] enemyColliders = Physics.OverlapSphere(playerController.transform.position, enemyDetectionRange, enemyLayer);

        foreach (Collider collider in enemyColliders)
        {
            if (collider.gameObject.TryGetComponent(out IEnemy enemyStatus))
            {
                if (enemyStatus != null && enemyStatus.isAlive)
                {
                    Vector3 directionToEnemy = (collider.transform.position - playerController.transform.position).normalized;
                    float dot = Vector3.Dot(mainCam.transform.forward, directionToEnemy);

                    if (dot > 0.3f)
                    {
                        if (Physics.Raycast(playerController.transform.position + Vector3.up, directionToEnemy, out RaycastHit hit, enemyDetectionRange))
                        {
                            if (hit.collider == collider || hit.transform.IsChildOf(collider.transform))
                            {
                                foundTargets.Add(collider.transform);
                            }
                        }
                    }
                }
            }
        }

        if (foundTargets.Count <= 0)
        {
            targetMostInFront = null;
            nearbyTargets.Clear();
            return;
        }

        foundTargets.Sort((enemyA, enemyB) =>
        {
            Vector3 screenPosA = mainCam.WorldToViewportPoint(enemyA.position);
            Vector3 screenPosB = mainCam.WorldToViewportPoint(enemyB.position);
            return screenPosA.x.CompareTo(screenPosB.x);
        });

        nearbyTargets = foundTargets;

        float closestToCenter = float.MaxValue;
        int targetIndex = 0;

        for (int i = 0; i < nearbyTargets.Count; i++)
        {
            Vector3 viewportPos = mainCam.WorldToViewportPoint(nearbyTargets[i].position);
            float offsetFromCenter = Mathf.Abs(viewportPos.x - 0.5f);
            if (offsetFromCenter < closestToCenter)
            {
                closestToCenter = offsetFromCenter;
                targetIndex = i;
            }
        }

        targetMostInFront = nearbyTargets[targetIndex];
    }

    private void SwitchCamera()
    {
        CinemachineInputAxisController axisControllerFreeLook = freeLookCamera.GetComponent<CinemachineInputAxisController>();
        CinemachineCamera cinemachineFreeLookCamera = freeLookCamera.GetComponent<CinemachineCamera>();
        CinemachineCamera cinemachineLockCamera = targetLockCamera.GetComponent<CinemachineCamera>();
        CinemachineGroupFraming cinemachineLockedCameraGroupFraming = targetLockCamera.GetComponent<CinemachineGroupFraming>();

        if (axisControllerFreeLook != null) axisControllerFreeLook.enabled = !activeTarget;

        if (activeTarget)
        {
            cinemachineLockCamera.ForceCameraPosition(cinemachineFreeLookCamera.State.GetFinalPosition(), cinemachineFreeLookCamera.State.GetFinalOrientation());
            cameraAnimator.Play("Locked On Camera");
        }
        else
        {
            if (cinemachineLockedCameraGroupFraming != null) cinemachineLockedCameraGroupFraming.Damping = 0;
            cinemachineFreeLookCamera.ForceCameraPosition(cinemachineLockCamera.State.GetFinalPosition(), cinemachineLockCamera.State.GetFinalOrientation());
            cameraAnimator.Play("Free Look Camera");
        }
    }

    private int GetTargetIndex(Transform target)
    {
        return nearbyTargets.IndexOf(target);
    }

    public GameObject GetActiveCamera()
    {
        if (targetLockCamera.GetComponent<CinemachineCamera>().IsLive) return targetLockCamera;
        if (freeLookCamera.GetComponent<CinemachineCamera>().IsLive) return freeLookCamera;
        return null;
    }
}
