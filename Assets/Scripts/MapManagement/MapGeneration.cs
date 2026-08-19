using UnityEngine;
using System.Collections.Generic;

public class MapGeneration : MonoBehaviour
{
    private int[,] tileArr;
    [Header("Width of array; should be an odd number")]
    [SerializeField] private int width;
    [Header("Height of array; should be ideal height + 2")]
    [SerializeField] private int height;
    [SerializeField] private int mainTTL = 30;
    [SerializeField] private float adjacentFilledWeight = 0.25f;
    [SerializeField] private float filledWeight = 0.05f;
    [SerializeField] private float moFalloff = .75f;

    public GameObject tempEmpty;
    public GameObject tempFilled;

    public ChunkGenerator generator;
    public ItemGenerator itemGenerator;
    [SerializeField] private float maxItemsPerChunk;
    [SerializeField] private float minReduction = .9f;

    public TileDestroyer tileDestroyer;

    private List<Walker> walkers = new List<Walker>();

    private List<GameObject> visualizationObjects = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tileArr = new int[height, width];
        Walker.maxX = width;
        Walker.maxY = height;
        Walker.tileArr = tileArr;
        Walker.adjacentFilledWeight = adjacentFilledWeight;
        Walker.filledWeight = filledWeight;
        Walker.moFalloff = moFalloff;
        int midpoint = width/2;
        tileArr[0,midpoint] = 1;
        tileArr[1,midpoint] = 1;
        tileArr[2,midpoint] = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Generate Map")]
    void generateMap() {
        tileDestroyer.clearSavedMaps();
        tileArr = new int[height, width];
        int midpoint = width/2;
        tileArr[0,midpoint] = 1;
        tileArr[1,midpoint] = 1;
        tileArr[2,midpoint] = 1;
        Walker.tileArr = tileArr;
        Walker.resetEligibleSpawns();
        walkers = new List<Walker>();
        walkers.Add(new Walker(midpoint, 2, mainTTL, "east"));
        walkers.Add(new Walker(midpoint, 2, mainTTL, "west"));
        walkers.Add(new Walker(midpoint, 2, mainTTL, "south"));

        foreach (GameObject GO in visualizationObjects) {
            Destroy(GO);
        }
        visualizationObjects = new List<GameObject>();

        bool walkersAlive = true;
        int iterations = 0;
        while (walkersAlive) {
            foreach (Walker w in walkers) {
                w.move();
            }
            // drawArr();
            walkers.RemoveAll(w => w.ttl==0);
            if (walkers.Count<=0) walkersAlive = false;
            iterations++;
            if (iterations>300) {
                walkersAlive = false;
                Debug.Log("Somehow went through 300 iterations");
            }
        }
        for (int i=0; i<tileArr.GetLength(0); i++) {
            for (int j=0; j<tileArr.GetLength(1); j++) {
                GameObject tileInstantiated;
                if (tileArr[i,j]==1) {
                    tileInstantiated = Instantiate(tempFilled, new Vector3(j,-i,-5), Quaternion.identity);
                } else {
                    tileInstantiated = Instantiate(tempEmpty, new Vector3(j,-i,-5), Quaternion.identity);
                }
                visualizationObjects.Add(tileInstantiated);
            }
        }
        generator.generateChunks(tileArr);

        int itemAmount = Mathf.FloorToInt(Random.Range(minReduction,1f) * maxItemsPerChunk * width * height);
        itemGenerator.generateItems(itemAmount, width, height, 12);
    }

    public class Walker { //x and y are zero-indexed
        public int x;
        public int y;
        public int ttl;
        private string dir;
        private float mo;

        public static int maxX;
        public static int maxY;
        private static float northWeightMult = 0.7f;
        private static float maxMo = 1.5f;
        public static float adjacentFilledWeight = 0.25f;
        public static float filledWeight = 0.05f;
        public static float moFalloff = .75f;

        public static int [,] tileArr;
        private static List<int[]> eligibleSpawns = new List<int[]>();

        public Walker(int x, int y, int ttl) {
            this.x = x;
            this.y = y;
            this.ttl = ttl;
            dir = "south";
            mo = maxMo;
        }

        public Walker(int x, int y, int ttl, string dir) {
            this.x = x;
            this.y = y;
            this.ttl = ttl;
            this.dir = dir;
            mo = maxMo;
        }

        public void move() { //find adjacent tiles, calculate odds *weight tiles based on adjacent tiles, going up, linear momentum, etc

            //Calculate south (y+1)
            float southWeight = 1;
            if (dir=="south") southWeight*=this.mo;
            if (y==maxY-1) southWeight=0;
            if (y<maxY-1 && tileArr[y+1,x]==1) southWeight*=filledWeight;
            if ((y<maxY-1 && ((x>0 && tileArr[y+1,x-1]==1) || (x<maxX-1 && tileArr[y+1,x+1]==1)))||(y<maxY-2&&tileArr[y+2,x]==1)) {
                southWeight*=adjacentFilledWeight;
            }
            
            //Calculate east (x+1)
            float eastWeight = 1;
            if (dir=="east") eastWeight*=this.mo;
            if (x==maxX-1) eastWeight=0;
            if (x<maxX-1 && tileArr[y,x+1]==1) eastWeight*=filledWeight;
            if ((x<maxX-1 && ((y>0 && tileArr[y-1,x+1]==1) || (y<maxY-1 && tileArr[y+1,x+1]==1)))||(x<maxX-2&&tileArr[y,x+2]==1)) {
                eastWeight*=adjacentFilledWeight;
            }


            //Calculate west (x-1)
            float westWeight = 1;
            if (dir=="west") westWeight*=this.mo;
            if (x==0) westWeight=0;
            if (x>0 && tileArr[y,x-1]==1) westWeight*=filledWeight;
            if ((x>0 && ((y>0 && tileArr[y-1,x-1]==1) || (y<maxY-1 && tileArr[y+1,x-1]==1)))||(x>1&&tileArr[y,x-2]==1)) {
                westWeight*=adjacentFilledWeight;
            }
            
            //Calculate north (y-1) (weight against north)
            float northWeight = northWeightMult;
            if (dir=="north") northWeight *= this.mo;
            if (y==0) northWeight=0;
            if (y>0 && tileArr[y-1,x]==1) northWeight*=filledWeight;
            if ((y>0 && ((x>0 && tileArr[y-1,x-1]==1) || (x<maxX-1 && tileArr[y-1,x+1]==1)))||(y>1&&tileArr[y-2,x]==1)) {
                northWeight*=adjacentFilledWeight;
            }

            mo*=moFalloff;

            //Add weights and calculate new weight based on total (like sol cesto), use to find new dir
            float aggregateWeight = northWeight+eastWeight+southWeight+westWeight;

            if (aggregateWeight==0) {
                respawn();
                return;
            }
            float northPercent = northWeight/aggregateWeight;
            float eastPercent = eastWeight/aggregateWeight;
            float southPercent = southWeight/aggregateWeight;
            float westPercent = westWeight/aggregateWeight;

            // Debug.Log($"North: {northPercent}. East: {eastPercent}. South: {southPercent}. West: {westPercent}.");

            float roll = Random.value;
            if (roll<northPercent) {
                if (x>0&&tileArr[y,x-1]!=1) eligibleSpawns.Add(new int[] {y,x-1});
                if (x<maxX-1&&tileArr[y,x+1]!=1) eligibleSpawns.Add(new int[] {y,x+1});
                y--;
                if (dir!="north") {
                    dir = "north";
                    mo = maxMo;
                }
            } else if (roll<northPercent+eastPercent) {
                if (y>0&&tileArr[y-1,x]!=1) eligibleSpawns.Add(new int[] {y-1,x});
                if (y<maxY-1&&tileArr[y+1,x]!=1) eligibleSpawns.Add(new int[] {y+1,x});
                x++;
                if (dir!="east") {
                    dir = "east";
                    mo = maxMo;
                }
            } else if (roll<northPercent+eastPercent+westPercent) {
                if (y>0&&tileArr[y-1,x]!=1) eligibleSpawns.Add(new int[] {y-1,x});
                if (y<maxY-1&&tileArr[y+1,x]!=1) eligibleSpawns.Add(new int[] {y+1,x});
                x--;
                if (dir!="west") {
                    dir = "west";
                    mo = maxMo;
                }
            } else {
                if (x>0&&tileArr[y,x-1]!=1) eligibleSpawns.Add(new int[] {y,x-1});
                if (x<maxX-1&&tileArr[y,x+1]!=1) eligibleSpawns.Add(new int[] {y,x+1});
                y++;
                if (dir!="south") {
                    dir = "south";
                    mo = maxMo;
                }
            }

            tileArr[y,x]=1;
            ttl--;
            // Debug.Log($"move ttl: {ttl}");
        }

        public void respawn() {
            if (eligibleSpawns.Count==0) {
                Debug.Log("Couldn't find eligible spawn");
                ttl=0;
                return;
            }
            int index = Random.Range(0,eligibleSpawns.Count);
            int[] newSpawn = eligibleSpawns[index];
            eligibleSpawns.RemoveAt(index);
            x = newSpawn[1];
            y = newSpawn[0];

            //check to make sure spawn can travel, if not get a new one
            float southWeight = 1;
            if (y==maxY-1) southWeight=0;
            if (y<maxY-1 && tileArr[y+1,x]==1) southWeight=0;
            if ((y<maxY-1 && ((x>0 && tileArr[y+1,x-1]==1) || (x<maxX-1 && tileArr[y+1,x+1]==1)))||(y<maxY-2&&tileArr[y+2,x]==1)) {
                southWeight=0;
            }
            
            float eastWeight = 1;
            if (x==maxX-1) eastWeight=0;
            if (x<maxX-1 && tileArr[y,x+1]==1) eastWeight=0;
            if ((x<maxX-1 && ((y>0 && tileArr[y-1,x+1]==1) || (y<maxY-1 && tileArr[y+1,x+1]==1)))||(x<maxX-2&&tileArr[y,x+2]==1)) {
                eastWeight=0;
            }


            float westWeight = 1;
            if (x==0) westWeight=0;
            if (x>0 && tileArr[y,x-1]==1) westWeight=0;
            if ((x>0 && ((y>0 && tileArr[y-1,x-1]==1) || (y<maxY-1 && tileArr[y+1,x-1]==1)))||(x>1&&tileArr[y,x-2]==1)) {
                westWeight=0;
            }
            
            float northWeight = 1;
            if (y==0) northWeight=0;
            if (y>0 && tileArr[y-1,x]==1) northWeight=0;
            if ((y>0 && ((x>0 && tileArr[y-1,x-1]==1) || (x<maxX-1 && tileArr[y-1,x+1]==1)))||(y>1&&tileArr[y-2,x]==1)) {
                northWeight=0;
            }

            if (tileArr[y,x]==1 || (southWeight==0&&eastWeight==0&&northWeight==0&&westWeight==0)) {
                respawn();
                return;
            }

            tileArr[y,x] = 1;
            ttl--;
            // Debug.Log($"respawn ttl :{ttl}");
            move();
        }

        public static void resetEligibleSpawns() {
            eligibleSpawns = new List<int[]>();
        }
    }

    void drawArr() {
        string logMessage = "";
        for (int i=0; i<tileArr.GetLength(0); i++) {
            for (int j=0; j<tileArr.GetLength(1); j++) {
                logMessage+=tileArr[i,j] + " ";
            }
            logMessage+="\n";
        }
        Debug.Log(logMessage);
    }
}
