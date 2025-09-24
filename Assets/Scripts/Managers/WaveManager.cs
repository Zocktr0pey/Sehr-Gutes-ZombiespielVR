using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private static WaveManager instance;
    [SerializeField] private EnemySpawnWave[] enemySpawnWaves;
    [SerializeField] private int cooldownBetweenWaves = 5;

    private SpawnManager _spawnManager;

    private EnemySpawnWave currentEnemyWave;
    private int currentEnemyWaveIndex = 0;

    private int aliveEnemiesCount = 0;

    //States
    bool isSpawningNextWave = false;

    // Erlaubt anderen Klassen auf den Manager zuzugreifen
    public static WaveManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Sorgt daf�r das immer nur eine Instanz dieses Managers existiert
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        _spawnManager = SpawnManager.Instance;

        if (enemySpawnWaves == null)
        {
            Debug.Log("No waves assigned to WaveManager yet");
            //get them guys
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (aliveEnemiesCount == 0 && !isSpawningNextWave)
        {
            // Alle Wellen besiegt
            if (currentEnemyWaveIndex >= enemySpawnWaves.Length)
            {
                Debug.Log("You won... but to which prize? You now have Zombie-Aids...");
                return;
            }

            isSpawningNextWave = true;

            StartCoroutine(StartNextWaveAfterDelay());
        }
    }

    // Startet die n�chste Welle im SpawnManager und setzt aktuelle Welle (+ index)
    private IEnumerator StartNextWaveAfterDelay()
    {
        currentEnemyWave = enemySpawnWaves[currentEnemyWaveIndex];
        // Cooldown 5s
        yield return new WaitForSeconds(5f);

        // startet n�chste Gegnerwelle
        Debug.Log($"Starting wave {currentEnemyWaveIndex+1}!");
        _spawnManager.SpawnWave(currentEnemyWave);

        currentEnemyWaveIndex++;
        isSpawningNextWave = false;
    }

    //Z�hlt lebende Gegner um 1 hoch
    public void UpdateEnemiesCountPlus()
    {
        aliveEnemiesCount++;
    }
    //Z�hlt lebende Gegner um 1 herunter
    public void UpdateEnemiesCountMinus()
    {
        aliveEnemiesCount--;
    }
}
