using UnityEngine;
using UnityEngine.UI;

public class SetTarget : MonoBehaviour
{
    private Transform currentTarget;
    [SerializeField] private TargetLockHandler targetLockHandler;
    public Image visual;
    public void SetTargetPos(Transform target)
    {
        currentTarget = target;

        if (target != null)
        {
            IEnemy targetPoint = target.gameObject.GetComponent<IEnemy>();
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
            gameObject.transform.position = currentTarget.position;
        }
    }
}

