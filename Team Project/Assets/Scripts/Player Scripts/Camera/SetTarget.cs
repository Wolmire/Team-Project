using UnityEngine;

public class SetTarget : MonoBehaviour
{
    private Transform currentTarget;
    [SerializeField] private TargetLockHandler targetLockHandler;
    //[SerializeField] public GameObject graphics;
    public void SetTargetPos(Transform target)
    {
        currentTarget = target;

        if (target != null)
        {
            IEnemy targetPoint = target.gameObject.GetComponent<IEnemy>();//It will be availabel if its attached with target.
            if (targetPoint != null)
            {
                currentTarget = targetPoint.lockOnPosition;
            }
        }
    }

    private void LateUpdate()
    {
        if (targetLockHandler.activeTarget)
        {
            gameObject.transform.position = currentTarget.position + Vector3.up; //Adding up just for proper cover focus on the target when player is vary close to target.
        }
    }
}

