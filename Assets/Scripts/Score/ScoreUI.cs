using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI combotText;
    [SerializeField] private TMPro.TextMeshProUGUI hightScoreText;
    private int scoreDisplay = 0;

    private void Start()
    {
        hightScoreText.text = HightScore.instance.hightScore.ToString();
    }
    void Update()
    {
        if (scoreDisplay != ScoreManager.instance.score)
        {
            scoreDisplay++;
            scoreText.text = scoreDisplay.ToString();
        }

        combotText.text = $"x{ScoreManager.instance.combot}";

    }
}
