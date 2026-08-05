using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileDestroyer : MonoBehaviour
{
    List<Tilemap> tilemaps = new List<Tilemap>();
    public LayerMask destructLayer;
    private Grid grid;
    [SerializeField] private Tilemap parentTilemap;
    [SerializeField] private Tilemap parentDangerTilemap;

    void Awake() {
        grid = GetComponent<Grid>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void destroyTile(Vector3 pos) {
        // foreach (Tilemap tmap in tilemaps) {
        //     Vector3Int cellPos = tmap.WorldToCell(pos);
        //     if (tmap.HasTile(cellPos)) {
        //         tmap.SetTile(cellPos,null);
        //         break;
        //     }
        // }

        Vector3Int cellPos = parentTilemap.WorldToCell(pos);
        if (parentTilemap.HasTile(cellPos)) {
            parentTilemap.SetTile(cellPos,null);
            // Debug.Log($"Destroyed normal tile at {cellPos}");
        }
        Vector3Int dangerCellPos = parentDangerTilemap.WorldToCell(pos);
        if (parentDangerTilemap.HasTile(dangerCellPos)) {
            parentDangerTilemap.SetTile(dangerCellPos,null);
            // Debug.Log($"Destroyed dangerous tile at {dangerCellPos}");
        }
        Vector3Int castLocationCell = parentTilemap.WorldToCell(pos);
        Vector3 castLocation = grid.GetCellCenterWorld(castLocationCell);
        Collider2D hit = Physics2D.OverlapPoint(castLocation, destructLayer);
        if (hit!=null) Destroy(hit.gameObject);
    }

    public void destroyTile(Vector3Int pos) {
        destroyTile((Vector3)pos);
    }

    // public void addMap(GameObject map) {
    //     tilemaps.Add(map.GetComponent<Tilemap>());
    // }

    public void clearSavedMaps() {
        tilemaps = new List<Tilemap>();
    }
}
