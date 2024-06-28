using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    private int scoreDisplay = 0;

    void Update()
    {
        if(scoreDisplay != ScoreManager.instance.score)
        {
            scoreDisplay++;
            scoreText.text = scoreDisplay.ToString();
        }
    }
}
