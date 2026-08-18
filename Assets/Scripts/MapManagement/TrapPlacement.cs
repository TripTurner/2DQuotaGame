using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct tileLocation {
    public int x;
    public int y;
}

public class TrapPlacement : MonoBehaviour
{
    public GameObject trap;
    public List<tileLocation> possiblePlacements;
    public float weight;
    public int maxTraps;
    private TileDestroyer world;
    private ChunkInitialization CI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        world = GameObject.FindWithTag("World").GetComponent<TileDestroyer>();
        CI = transform.parent.GetComponent<ChunkInitialization>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void placeTraps() {
        if (maxTraps>possiblePlacements.Count) maxTraps = possiblePlacements.Count;
        for (int i=0; i<maxTraps; i++) {
            float willPlaceTrap = Random.Range(0.0f,1.0f);
            if (willPlaceTrap > weight) continue;
            int index = Random.Range(0,possiblePlacements.Count);
            tileLocation placeLocation = possiblePlacements[index];
            // CI.placeTrap(trap,x,y);
            world.placeTrap(trap, transform.position.x + placeLocation.x, transform.position.y - placeLocation.y);
            possiblePlacements.RemoveAt(index);
        }
    }
}
