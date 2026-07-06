using UnityEngine;

public class LockOnUI : MonoBehaviour
{
    private Transform mainCamera;
    private float initialDistance;

    void Start()
    {
        mainCamera = Camera.main.transform;
        initialDistance = Vector3.Distance(transform.position, mainCamera.position);
    }

    void Update()
    {
        float currentDistance = Vector3.Distance(transform.position, mainCamera.position);

        transform.localScale = Vector3.one * (currentDistance / initialDistance);

        transform.rotation = mainCamera.rotation;
    }
}
