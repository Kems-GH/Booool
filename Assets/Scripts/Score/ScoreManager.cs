using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score { get; private set; } = 0;
    public static ScoreManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    public void AddScore(int score)
    {
        this.score += score;
    }
}
