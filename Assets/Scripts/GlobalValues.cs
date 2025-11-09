using UnityEngine;

public class GlobalValues
{
    private int lastScore = 0;
    private int lastWave = 0;

    public void SetLastScore(int score)
    {
        lastScore = score;
    }
    public void SetLastWave(int wave)
    {
        lastWave = wave;
    }

    public int GetLastScore()
    {
        return lastScore;
    }
    public int GetLastWave()
    {
        return lastWave;
    }
}
