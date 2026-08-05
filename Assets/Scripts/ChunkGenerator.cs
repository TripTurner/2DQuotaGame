using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

// |= adds a flag, &= ~ removes, ^= toggles, use .HasFlags()  

public class ChunkGenerator : MonoBehaviour
{
    //public Openings openings;

    private int chunkHeight = 12;
    private int chunkWidth = 12;

    public List<ChunkInitialization> chunkPrefabs;

    public GameObject parentGrid;
    public ChunkInitialization chunkUpDown;
    public ChunkInitialization chunkLeftRight;
    public ChunkInitialization chunkUpLeft;
    public ChunkInitialization chunkUpRight;
    public ChunkInitialization chunkDownLeft;
    public ChunkInitialization chunkDownRight;
    public ChunkInitialization chunkUpDownLeftRight;
    public ChunkInitialization chunkWall;

    public List<ChunkInitialization> chunkInitList;

    private List<GameObject> placedChunks = new List<GameObject>();

    [SerializeField] Tilemap parentTilemap;
    [SerializeField] Tilemap parentDangerTilemap;
    [SerializeField] GameObject parentGameObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //chunkPrefabs = new List<ChunkInitialization>{chunkDownLeft,chunkDownRight,chunkLeftRight,chunkUpDown,chunkUpDownLeftRight,chunkUpLeft,chunkUpRight};
        foreach (ChunkInitialization ci in chunkInitList) {
            chunkPrefabs.Add(ci);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateChunks(int[,] tileArr) {
        foreach (GameObject GO in placedChunks) {
            Destroy(GO);
        }
        placedChunks = new List<GameObject>();

        int filledChunkNum = 0;

        for (int i=0; i<tileArr.GetLength(0); i++) {
            for (int j=0; j<tileArr.GetLength(1); j++) {
                bool upOpen = false;
                bool downOpen = false;
                bool rightOpen = false;
                bool leftOpen = false;
                if (i>0&&tileArr[i-1,j]==1) upOpen = true;
                if (j>0&&tileArr[i,j-1]==1) leftOpen = true;
                if (i<tileArr.GetLength(0)-1 && tileArr[i+1,j]==1) downOpen = true;
                if (j<tileArr.GetLength(1)-1 && tileArr[i,j+1]==1) rightOpen = true;

                // Debug.Log($"Flags: {upOpen}, {leftOpen}, {downOpen}, {rightOpen}");

                List<ChunkInitialization> chunksToTest = new List<ChunkInitialization>(chunkPrefabs);
                GameObject toInstantiate = chunkWall.gameObject;
                while (true) {
                    if (chunksToTest.Count==0) {
                        Debug.Log($"Couldn't fit tile at {j},{i}\n upOpen: {upOpen}\n leftOpen: {leftOpen}\n downOpen: {downOpen}\n rightOpen:{rightOpen}");
                        break;
                    }
                    if (!(upOpen||downOpen||leftOpen||rightOpen) || tileArr[i,j]==0) {
                        toInstantiate = chunkWall.gameObject;
                        break;
                    }
                    int index = Random.Range(0,chunksToTest.Count);
                    ChunkInitialization testChunk = chunksToTest[index];
                    chunksToTest.RemoveAt(index);
                    Openings check = Openings.None;
                    if (upOpen) check|= Openings.Up;
                    if (downOpen) check |= Openings.Down;
                    if (leftOpen) check|= Openings.Left;
                    if (rightOpen) check|= Openings.Right;
                    if ((testChunk.openings&check)==check) {
                        toInstantiate = testChunk.gameObject;
                        filledChunkNum++;
                        break;
                    } else {
                        continue;
                    }
                }
                GameObject chunkToPlace;
                chunkToPlace = Instantiate(toInstantiate, new Vector3(j*chunkWidth,-i*chunkHeight,0), Quaternion.identity, parentGrid.transform);
                chunkToPlace.GetComponent<ChunkInitialization>().copyTilesTo(parentTilemap, parentDangerTilemap, parentGameObject);
                //placedChunks.Add(chunkToPlace);
            }
        }
        Debug.Log($"Filled Chunk Num: {filledChunkNum}");
    }
}