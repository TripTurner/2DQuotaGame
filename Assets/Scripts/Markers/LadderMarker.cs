using UnityEngine;

public class LadderMarker : MarkerData
{
    [SerializeField] private GameObject ladderGO;
    [SerializeField] private float height;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override GameObject createObject() {
        GameObject toInstantiate = Instantiate(ladderGO, transform.position, transform.rotation);
        toInstantiate.GetComponent<SpriteRenderer>().size = new Vector2(1,height);
        toInstantiate.GetComponent<BoxCollider2D>().size = new Vector2(0.8f,height);
        return toInstantiate;
    }
}
