using UnityEngine;
using UnityEngine.SceneManagement;

public class FuckUnity : MonoBehaviour
{
    private static FuckUnity instance;
    public static FuckUnity Instance
    {
        get { return instance; }
    }

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

        //In GlobalValues speichern für Hauptmenü
        GlobalValues.SetLastScore(score);
        GlobalValues.SetLastWave(currentWave);

        SceneManager.LoadScene("MainMenu");
    }
}
