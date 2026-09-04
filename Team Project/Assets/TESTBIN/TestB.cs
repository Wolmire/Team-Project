using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Testing", menuName = "testingB")]

public class TestB : MonoBehaviour
{
    public float Dist = 1.0f;
    public Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Dist))
        {
            //Debug.Log("RayHitting");
            transform.position = hit.point + offset;

            transform.rotation = Quaternion.LookRotation(transform.forward, hit.normal);
        }
    }
}
