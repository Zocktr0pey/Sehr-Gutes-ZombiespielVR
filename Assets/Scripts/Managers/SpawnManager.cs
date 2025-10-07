using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager instance;
    [SerializeField] private float spawnRadius = 75 / 2f;

    // Erlaubt anderen Klassen auf den Manager zuzugreifen
    public static SpawnManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Sorgt dafür das immer nur eine Instanz dieses Managers existiert
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Starte Coroutine zum Spawnen der Welle
    public void SpawnWave(EnemySpawnWave wave)
    {
        StartCoroutine(SpawnWaveCoroutine(wave));
    }

    //Schleife zum spawnen aller Gegner einer Welle
    private IEnumerator SpawnWaveCoroutine(EnemySpawnWave wave)
    {
        for (int i = 0; i < wave.amountOfEnemies; i++)
        {
            float rand = Random.value * 100;
            if (rand >= 0 && rand < 70 )
                SpawnEnemy(wave.enemyCommon);
            if (rand >= 70 && rand < 95)
                SpawnEnemy(wave.enemyUncommon);
            if (rand >= 95 && rand < 100)
                SpawnEnemy(wave.enemyRare);
            yield return new WaitForSeconds(wave.spawnRate);
        }

        ////!ONLY FOR DEBUG
        for (int i = 0; i < wave.amountOfEnemies; i++)
        {
            yield return new WaitForSeconds(0.5f);
            WaveManager.Instance.UpdateEnemiesCountMinus();
        }
    }

    //Spawnt einzelne Gegner
    public void SpawnEnemy(GameObject enemyObject)
    {
        // Errechne Spawnlocation
        Vector3 spawnLocation = RandomPointByRadius(spawnRadius);

        // gedreht zum Punkt (0, 0, 0)
        Quaternion rotation = Quaternion.Euler(0, 0, 0);

        //Spawne Gegner
        Instantiate(enemyObject, spawnLocation, rotation);

        WaveManager.Instance.UpdateEnemiesCountPlus();

        return;
    }

    //Errechnet zufällige Koordinaten anahnd des Radius
    private static Vector3 RandomPointByRadius(float radius)
    {
        //Höhe
        float y = 0.5f;

        // Zufallswinkel zwischen 0 und 2*Pi
        float alpha = Random.value * 2f * Mathf.PI;

        // Punkte auf dem Kreisrand
        float x = (float)(radius * Mathf.Cos(alpha));
        float z = (float)(radius * Mathf.Sin(alpha));

        return new Vector3(x, y, z);
    }
}