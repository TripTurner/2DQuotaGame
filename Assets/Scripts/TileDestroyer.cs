using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public class TileDestroyer : MonoBehaviour
{
    List<Tilemap> tilemaps = new List<Tilemap>();
    public LayerMask destructLayer;
    private Grid grid;
    [SerializeField] private Tilemap parentTilemap;
    [SerializeField] private Tilemap parentDangerTilemap;
    [SerializeField] private TileBase dangerTile;
    [SerializeField] private TileBase normalTile;



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
        // Vector3Int castLocationCell = parentTilemap.WorldToCell(pos);
        // Vector3 castLocation = grid.GetCellCenterWorld(castLocationCell);
        // Collider2D hit = Physics2D.OverlapPoint(castLocation, destructLayer);
        // if (hit!=null) Destroy(hit.gameObject);
    }

    public void destroyTile(Vector3Int pos) {
        destroyTile((Vector3)pos);
    }

    public void destroyTile(Vector2 pos) {
        destroyTile((Vector3)pos);
    }

    public void destroyTile(Vector2Int pos) {
        destroyTile((Vector3Int)pos);
    }

    public void switchTile(Vector3Int pos, string type, bool temp, float time = 0) {
        switchTile((Vector3) pos, type, temp, time);
    }
    
    public void switchTile(Vector2 pos, string type, bool temp, float time = 0) {
        switchTile((Vector3) pos, type, temp, time);
    }

    public void switchTile(Vector2Int pos, string type, bool temp, float time = 0) {
        switchTile((Vector3Int) pos, type, temp, time);
    }

    public void switchTile(Vector3 pos, string type, bool temp, float time = 0) {
        // Debug.Log($"Switching tile to {type}");
        if (type=="danger") {
            // Debug.Log($"Confirming type is {type}");
            Vector3Int cellPos = parentTilemap.WorldToCell(pos);
            // Debug.Log($"Changed position {pos} to cell position {cellPos}");
            if (parentTilemap.HasTile(cellPos)) {
                // Debug.Log("ParentTilemap has it");
                parentTilemap.SetTile(cellPos,null);
                parentDangerTilemap.SetTile(cellPos,dangerTile);
                if (temp) StartCoroutine(runWithDelay(time, () => switchTile(pos,"normal",false)));
                // Debug.Log("dw i switched the tile for you bro");
            }
        } else if (type=="normal") {
            Vector3Int dangerCellPos = parentDangerTilemap.WorldToCell(pos);
            if (parentDangerTilemap.HasTile(dangerCellPos)) {
                parentDangerTilemap.SetTile(dangerCellPos,null);
                parentTilemap.SetTile(dangerCellPos,normalTile);
                if (temp) StartCoroutine(runWithDelay(time, () => switchTile(pos,"danger",false)));
            }
        } else if (type=="destroy") {
            Vector3Int cellPos = parentTilemap.WorldToCell(pos);
            if (parentTilemap.HasTile(cellPos)) {
                parentTilemap.SetTile(cellPos,null);
                if (temp) StartCoroutine(runWithDelay(time, () => switchTile(pos,"normal",false)));
            }
            Vector3Int dangerCellPos = parentDangerTilemap.WorldToCell(pos);
            if (parentDangerTilemap.HasTile(dangerCellPos)) {
                parentDangerTilemap.SetTile(dangerCellPos,null);
                if (temp) StartCoroutine(runWithDelay(time, () => switchTile(pos,"danger",false)));
            }
        }
    }

    IEnumerator runWithDelay(float delay, System.Action action) {
        yield return new WaitForSeconds(delay);
        action();
    }

    public void destroyTilesInLine(Vector2 startPos, Vector2 endPos) {
        List<Vector2Int> cellsToDestroy = new List<Vector2Int>();

        Vector2Int currentCell = new Vector2Int(Mathf.FloorToInt(startPos.x), Mathf.FloorToInt(startPos.y));
        Vector2Int endCell = new Vector2Int(Mathf.FloorToInt(endPos.x), Mathf.FloorToInt(endPos.y));
        cellsToDestroy.Add(currentCell);
        Vector2 dir = endPos - startPos;
        int stepX = dir.x >0? 1: -1;
        int stepY = dir.y >0? 1: -1;

        float tMaxX = (stepX > 0) ? (currentCell.x + 1 - startPos.x) : (startPos.x - currentCell.x);
        float tMaxY = (stepY > 0) ? (currentCell.y + 1 - startPos.y) : (startPos.y - currentCell.y);

        float tDeltaX = Mathf.Abs(1f / (Mathf.Approximately(dir.x, 0) ? 0.00001f : dir.x));
        float tDeltaY = Mathf.Abs(1f / (Mathf.Approximately(dir.y, 0) ? 0.00001f : dir.y));

        tMaxX *= tDeltaX;
        tMaxY *= tDeltaY;

        while (currentCell != endCell)
        {
            if (tMaxX < tMaxY)
            {
                tMaxX += tDeltaX;
                currentCell.x += stepX;
            }
            else if (tMaxX > tMaxY)
            {
                tMaxY += tDeltaY;
                currentCell.y += stepY;
            }
            else
            {
                cellsToDestroy.Add(new Vector2Int(currentCell.x + stepX, currentCell.y));
                cellsToDestroy.Add(new Vector2Int(currentCell.x, currentCell.y + stepY));
                
                tMaxX += tDeltaX;
                tMaxY += tDeltaY;
                currentCell.x += stepX;
                currentCell.y += stepY;
            }

            cellsToDestroy.Add(currentCell);
        }

        foreach (Vector2Int cell in cellsToDestroy) {
            destroyTile(cell);
        }
    }

    // public void addMap(GameObject map) {
    //     tilemaps.Add(map.GetComponent<Tilemap>());
    // }

    public void clearSavedMaps() {
        tilemaps = new List<Tilemap>();
    }
}
