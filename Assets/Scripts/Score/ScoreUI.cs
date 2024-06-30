using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI _combotText;
    [SerializeField] private TMPro.TextMeshProUGUI _hightScoreText;
    private int scoreDisplay = 0;

    private void Start()
    {
        _hightScoreText.text = HightScore.Instance.hightScore.ToString();
    }
    void Update()
    {
        if (scoreDisplay != ScoreManager.Instance.score)
        {
            scoreDisplay++;
            _scoreText.text = scoreDisplay.ToString();
        }

        _combotText.text = $"x{ScoreManager.Instance.combot}";

    }
}
