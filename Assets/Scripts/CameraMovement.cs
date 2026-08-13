using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject toFollow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(toFollow.transform.position.x, toFollow.transform.position.y, transform.position.z);
    }
}
