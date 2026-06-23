using UnityEngine;

public class CopyLocation : MonoBehaviour
{
    public GameObject Target;

    public Vector3 Offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    // Update is called once per frame
    void Update()
    {
        transform.position = Target.transform.position + Offset;
    }
}
