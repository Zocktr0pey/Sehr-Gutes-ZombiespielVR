using NUnit.Framework;
using UnityEngine;

public class NetworkShit : MonoBehaviour
{
    private static NetworkShit instance;
    private System.Collections.Generic.List<GameObject> players = new System.Collections.Generic.List<GameObject>();

    // Erlaubt anderen Klassen auf den Manager zuzugreifen
    public static NetworkShit Instance
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

    public void AddPlayer(GameObject newPlayer)
    {
        players.Add(newPlayer);
    }

    public GameObject[] GetPlayers()
    {
        Debug.Log(players.ToArray());
        return players.ToArray();
    }
}
