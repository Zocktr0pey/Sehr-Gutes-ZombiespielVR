using UnityEngine;

public class GlobalValues
{
    private static int lastScore = 0;
    private static int lastWave = 0;

    public void SetLastScore(int score)
    {
        lastScore = score;
    }
    public void SetLastWave(int wave)
    {
        lastWave = wave;
    }

    public void GetLastScore()
    {
        return lastScore;
    }
    public void GetLastWave()
    {
        return lastWave;
    }
}
