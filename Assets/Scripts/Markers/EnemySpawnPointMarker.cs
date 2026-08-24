using UnityEngine;

public class EnemySpawnPointMarker : MonoBehaviour
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

    public GameObject createObject() {
        GameObject toInstantiate = Instantiate(GO, transform.position, transform.rotation);
        return toInstantiate;
    }
}
