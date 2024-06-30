using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score { get; private set; } = 0;
    public static ScoreManager Instance { get; private set; }
    public int combot { get; private set; } = 1;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    public void AddScore(int score)
    {
        this.score += score * 1;
        AddCombot();
    }

    public void AddCombot()
    {
        combot++;
    }
    public void ResetCombot()
    {
        combot = 1;
    }
}
