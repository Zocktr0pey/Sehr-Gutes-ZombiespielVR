using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        // Sorgt dafür das immer nur eine Instanz dieses Managers existiert
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
                SceneManager.LoadScene("MainMenu");
                return;
            }

            isSpawningNextWave = true;

            StartCoroutine(StartNextWaveAfterDelay());
        }
    }

    // Startet die nächste Welle im SpawnManager und setzt aktuelle Welle (+ index)
    private IEnumerator StartNextWaveAfterDelay()
    {
        currentEnemyWave = enemySpawnWaves[currentEnemyWaveIndex];
        // Cooldown 5s
        yield return new WaitForSeconds(cooldownBetweenWaves);

        // startet nächste Gegnerwelle
        _spawnManager.SpawnWave(currentEnemyWave);

        currentEnemyWaveIndex++;
        isSpawningNextWave = false;
    }

    //Zählt lebende Gegner um 1 hoch
    public void UpdateEnemiesCountPlus()
    {
        aliveEnemiesCount++;
    }
    //Zählt lebende Gegner um 1 herunter
    public void UpdateEnemiesCountMinus()
    {
        aliveEnemiesCount--;
    }

    public int GetCurrentWave()
    {
        return this.currentEnemyWaveIndex;
    }
}