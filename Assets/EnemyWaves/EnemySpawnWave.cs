using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnWave", menuName = "Scriptable Objects/EnemySpawnWave")]
public class EnemySpawnWave : ScriptableObject
{
    public int amountOfEnemies;
    public GameObject enemyCommon;
    public GameObject enemyUncommon;
    public GameObject enemyRare;
    public float spawnRate; // Zeit zwischen spawns
}
