using UnityEngine;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour
{
    private TileDestroyer world;
    public List<GameObject> items;

    void Awake() {
        world = GameObject.FindWithTag("World").GetComponent<TileDestroyer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateItems(int itemAmount, int width, int height, int chunkSize) {
        for (int i=0; i<itemAmount; i++) {
            int w = Random.Range(2,width*chunkSize-2);
            int h = Random.Range(2,height*chunkSize-2);
            GameObject item = items[Random.Range(0,items.Count)];
            tryToPlace(item, w, -h);
        }
    }

    public void tryToPlace(GameObject item, int x, int y) {
        for (int i=x-2; i<=x+2; i++) {
            for (int j=y-2; j<=y+2; j++) {
                float localX = i + .5f;
                float localY = j + .5f;
                if (world.cellTypeAt(new Vector2(localX,localY),"destroy")) {
                    Debug.Log($"Empty spot at {i}, {j}");
                    Instantiate(item,new Vector3(localX,localY), Quaternion.identity);
                    return;
                }
            }
        }
        world.destroyTile(new Vector2(x+.5f,y+.5f));
        Debug.Log($"No empty spots, destroying {x}, {y}");
        Instantiate(item, new Vector3(x+.5f,y+.5f), Quaternion.identity);
    }
}
