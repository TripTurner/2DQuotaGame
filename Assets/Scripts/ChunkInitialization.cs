using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[Flags]
public enum Openings { 
    None = 0,
    Up = 1,
    Left = 2,
    Down = 4,
    Right = 8
}

public class ChunkInitialization : MonoBehaviour
{

    public Openings openings;
    private TileDestroyer grid;
    public List<GameObject> markerList;
    public bool hasBaseTilemap = true;
    public Tilemap baseTilemap;
    public bool hasDangerTilemap = true;
    public Tilemap dangerTilemap;

    void Awake() {
        grid = gameObject.GetComponentInParent<TileDestroyer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //grid.addMap(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void copyTilesTo(Tilemap parentTilemap, Tilemap parentDangerTilemap, GameObject parentGameObject) {
        if (hasBaseTilemap) {
            if (baseTilemap==null) {
                baseTilemap = GetComponent<Tilemap>();
            }
            BoundsInt bounds = baseTilemap.cellBounds;
            foreach (Vector3Int point in bounds.allPositionsWithin) {
                TileBase tile = baseTilemap.GetTile(point);
                if (tile==null) continue;
                parentTilemap.SetTile(point + Vector3Int.FloorToInt(transform.position),tile);
            }
        }
        if (hasDangerTilemap) {
            BoundsInt bounds = dangerTilemap.cellBounds;
            foreach (Vector3Int point in bounds.allPositionsWithin) {
                TileBase tile = dangerTilemap.GetTile(point);
                if (tile==null) continue;
                parentDangerTilemap.SetTile(point + Vector3Int.FloorToInt(transform.position),tile);
            }
        }
        foreach (GameObject GO in markerList) {
            MarkerData MD = GO.GetComponent<MarkerData>();
            GameObject toInstantiate = Instantiate(MD.prefab,MD.transform.position,Quaternion.identity,parentGameObject.transform);
            if (MD.type=="ladder") {
                toInstantiate.GetComponent<SpriteRenderer>().size = new Vector2(1,MD.float1);
                toInstantiate.GetComponent<BoxCollider2D>().size = new Vector2(0.8f,MD.float1);
            } else if (MD.type=="bomb") {
                toInstantiate.GetComponent<BombLogic>().setExplodeTimer(MD.float1);
                toInstantiate.GetComponent<BombLogic>().explodeRadius = MD.float2;
            }
        }
        Destroy(gameObject);
    }
}
