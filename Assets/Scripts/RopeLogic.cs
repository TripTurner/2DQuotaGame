using UnityEngine;

public class RopeLogic : MonoBehaviour
{
    public float maxHeight = 7;
    private float height;
    public LayerMask groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position-new Vector3(0,0.1f,0),Vector2.down,maxHeight,groundLayer);
        if (hit.collider!=null) {
            height = transform.position.y - hit.point.y;
        } else {
            height = maxHeight;
        }
        createRope();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createRope() {
        transform.position = new Vector3(transform.position.x,transform.position.y-height/2,transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x,height,transform.localScale.z);
    }
}
