using System;
using Unity.Cinemachine;
using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;

public class TargetLockHandler : MonoBehaviour
{
    [SerializeField] CharacterController playerController;
    public SetTarget setTarget;
    [SerializeField] private Animator cameraAnimator;

    public bool activeTarget;
    [SerializeField][Range(5, 50)] float enemyDetectionRange;
    [ReadOnly][SerializeField] private List<Transform> nearbyTargets;
    public Transform currentTarget;

    [SerializeField] private GameObject freeLookCamera;
    [SerializeField] private GameObject targetLockCamera;

    [SerializeField] LayerMask enemyLayer;

    public static Action<bool> OnTargetLock;

    float switchLockCooldown = 0.5f;
    float switchAfterTime = 0;

    private void Start()
    {
        SwitchCamera();
        //setTarget.graphics.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(activeTarget)
        {
            float distance = Vector3.Distance(playerController.transform.position, currentTarget.position);
            if(distance > enemyDetectionRange + enemyDetectionRange / 2)
            {
                TargetLock(false);
            }
        }
    }
    public void HandleSwitchTargetInput(string deviceType, float lookInputX)
    {
        if(activeTarget)
        {
            if ((deviceType == "Gamepad" && switchAfterTime < Time.time) || deviceType == "Keyboard&Mouse")
            {
                float rightX = lookInputX;

                float deadzone = 0.2f;
                if (Mathf.Abs(rightX) < deadzone) rightX = 0f;

                if (rightX < 0.5f)
                {
                    SwitchTarget(true);
                    switchAfterTime = Time.time + switchLockCooldown;
                }
                else if (rightX > 0.5f)
                {
                    SwitchTarget(true);
                    switchAfterTime = Time.time + switchLockCooldown;
                }
            }
        }
    }

    public void TargetLock(bool boolval)
    {
        if (boolval)
        {
            CollectTargetsAndGetMostInFrontTarget(out Transform targetMostInFront);
            currentTarget = targetMostInFront;

            if (targetMostInFront != null)
            {
                setTarget.SetTargetPos(targetMostInFront);
                //setTarget.graphics.SetActive(true);
                activeTarget = true;
                SwitchCamera();
            }
        }
        else
        {
            nearbyTargets.Clear();
            //setTarget.graphics.SetActive(false);
            activeTarget = false;
            SwitchCamera();
        }
        OnTargetLock?.Invoke(activeTarget);
    }

    private void SwitchTarget(bool switchToLeft)
    {
        if(!activeTarget) return;

        CollectTargetsAndGetMostInFrontTarget(out Transform targetMostInFront);

        if(nearbyTargets.Count <= 0)
        {
            TargetLock(false);
            return;
        }

        int currentTargetIndex = GetTargetIndex(currentTarget);

        if (currentTargetIndex == -1)
        {
            currentTargetIndex = GetTargetIndex(targetMostInFront);
        }

        if (switchToLeft)
        {
            if (currentTargetIndex > 0) currentTargetIndex -= 1;
            else currentTargetIndex = currentTargetIndex = 0;
        }
        else
            if (currentTargetIndex < nearbyTargets.Count - 1)
        {
            currentTargetIndex += 1;
        }
        else currentTargetIndex = nearbyTargets.Count - 1;

        currentTarget = nearbyTargets[currentTargetIndex];

        if(currentTarget != null)
        {
            CinemachineGroupFraming cinemachineGroupFraming = targetLockCamera.GetComponent<CinemachineGroupFraming>();
            cinemachineGroupFraming.Damping = 1f;
            setTarget.SetTargetPos(currentTarget);
        }
    }

    private void CollectTargetsAndGetMostInFrontTarget(out Transform targetMostInFront)
    {
        List<Transform> foundTargets = new List<Transform>();

        Collider[] enemyColliders = Physics.OverlapSphere(playerController.transform.position, enemyDetectionRange);
        foreach (Collider collider in enemyColliders)
        {
            if (collider.gameObject.TryGetComponent(out IEnemy enemyStatus))
            {
                if (enemyStatus != null && enemyStatus.isAlive)
                {
                    Vector3 directionToEnemy = (collider.transform.position - playerController.transform.position).normalized;
                    float dot = Vector3.Dot(GetActiveCamera().transform.forward, directionToEnemy);
                    if (dot > 0.5f)
                    {
                        bool hitSomething = Physics.Raycast(playerController.transform.position, directionToEnemy, out RaycastHit hit, enemyDetectionRange);
                        if (hitSomething)
                        {
                            if (hit.collider == collider)
                            {
                                foundTargets.Add(collider.transform);
                            }
                        }
                        else
                        {
                            foundTargets.Add(collider.transform);
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

        foundTargets.Sort((enemy, enemy2) =>
        {
            Vector3 directionToEnemy = (enemy.position - playerController.transform.position).normalized;
            Vector3 directionToEnemy2 = (enemy2.position - playerController.transform.position).normalized;

            float crossEnemy = Vector3.Cross(GetActiveCamera().transform.forward, directionToEnemy).y;
            float crossEnemy2 = Vector3.Cross(GetActiveCamera().transform.forward, directionToEnemy2).y;

            return crossEnemy.CompareTo(crossEnemy2);
        });

        nearbyTargets = foundTargets;

        float maxDot = -1f;
        int mostInFrontIndex = 0;
        for (int i = 0; i < foundTargets.Count; i++)
        {
            Vector3 directionToEnemy = (foundTargets[i].position - playerController.transform.position).normalized;
            float dot = Vector3.Dot(GetActiveCamera().transform.forward, directionToEnemy);
            if (dot > maxDot)
            {
                maxDot = dot;
                mostInFrontIndex = i;
            }
        }

        targetMostInFront = nearbyTargets[mostInFrontIndex];
    }

    private void SwitchCamera()
    {
        CinemachineInputAxisController axisControllerFreeLook = freeLookCamera.GetComponent<CinemachineInputAxisController>();
        CinemachineCamera cinemachineFreeLookCamera = freeLookCamera.GetComponent<CinemachineCamera>();
        CinemachineCamera cinemachineLockCamera = targetLockCamera.GetComponent<CinemachineCamera>();
        CinemachineGroupFraming cinemachineLockedCameraGroupFraming = targetLockCamera.GetComponent<CinemachineGroupFraming>();

        if (axisControllerFreeLook != null)
        {
            axisControllerFreeLook.enabled = !activeTarget;
        }
        if(activeTarget)
        {
            cinemachineFreeLookCamera.ForceCameraPosition(cinemachineFreeLookCamera.State.GetFinalPosition(), cinemachineFreeLookCamera.State.GetFinalOrientation());
            cameraAnimator.Play("Locked On Camera");
        }
        else
        {
            cinemachineLockedCameraGroupFraming.Damping = 0;
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
        if(targetLockCamera.GetComponent<CinemachineCamera>().IsLive) return targetLockCamera;
        if(freeLookCamera.GetComponent<CinemachineCamera>().IsLive) return freeLookCamera;

        return null;
    }
}
