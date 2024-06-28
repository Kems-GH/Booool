using UnityEngine;

public class HightScore : MonoBehaviour
{
    public int hightScore { get; private set; } = 0;

    private const string fileName = "hightScore.txt";

    public static HightScore instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        LoadHightScore();
    }

    private void LoadHightScore()
    {
        if (System.IO.File.Exists(fileName))
        {
            string data = System.IO.File.ReadAllText(Application.dataPath + fileName);
            hightScore = int.Parse(data);
        }
    }

    private void SaveHightScore()
    {
        System.IO.File.WriteAllText(Application.dataPath + fileName, hightScore.ToString());
    }

    public void UpdateHightScore(int score)
    {
        if (score > hightScore)
        {
            hightScore = score;
            SaveHightScore();
        }
    }
}
