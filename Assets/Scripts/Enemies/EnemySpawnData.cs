using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemySpawnData", menuName = "QuotaGame/Enemy Spawn Data")]
public class EnemySpawnData : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
    public int budgetCost;
    public float weight;
    public int maxAllowed;
    public bool specialSpawnConditions;
}
