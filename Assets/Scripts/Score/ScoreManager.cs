using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score { get; private set; } = 0;
    public static ScoreManager instance { get; private set; }
    public int combot { get; private set; } = 1;
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
