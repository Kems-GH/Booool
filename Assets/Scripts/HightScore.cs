using UnityEngine;

public class HightScore : MonoBehaviour
{
    public int hightScore { get; private set; } = 0;

    private const string FILE_NAME = "hightScore.txt";

    public static HightScore Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        LoadHightScore();
    }

    private void LoadHightScore()
    {
        if (System.IO.File.Exists(FILE_NAME))
        {
            string data = System.IO.File.ReadAllText(Application.dataPath + FILE_NAME);
            hightScore = int.Parse(data);
        }
    }

    private void SaveHightScore()
    {
        System.IO.File.WriteAllText(Application.dataPath + FILE_NAME, hightScore.ToString());
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
