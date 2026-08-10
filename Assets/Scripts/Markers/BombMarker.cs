using UnityEngine;

public class BombMarker : MarkerData
{
    [SerializeField]private GameObject bombGO;
    [SerializeField]private float explodeTimer;
    [SerializeField]private float explodeRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override GameObject createObject() {
        GameObject toInstantiate = Instantiate(bombGO, transform.position, transform.rotation);
        toInstantiate.GetComponent<BombLogic>().setExplodeTimer(explodeTimer);
        toInstantiate.GetComponent<BombLogic>().explodeRadius = explodeRadius;
        return toInstantiate;
    }
}
