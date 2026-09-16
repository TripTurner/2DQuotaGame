using UnityEngine;

public class EnemySpawnPointMarker : MarkerData
{
    [SerializeField] private GameObject GO;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override GameObject createObject() {
        GameObject toInstantiate = Instantiate(GO, transform.position, transform.rotation);
        Debug.Log("Made the object!!!!");
        return toInstantiate;
    }
}
