using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    public int lastWave { get; private set; }
    public int lastScore { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Wenn Spieler stirbt aufrufen
    public void GameOver(int wave, int score)
    {
        lastWave = wave;
        lastScore = score;
        SceneManager.LoadScene("MainMenu");
    }

    public int GetLastWave()
    {
        return lastWave;
    }

    public int GetLastScore()
    {
        return lastScore;
    }
}
