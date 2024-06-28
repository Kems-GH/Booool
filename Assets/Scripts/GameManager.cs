using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public bool isGamePause { get; private set; } = false;

    private GameOverUI gameOverUI;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
    }
    private void Start()
    {
        gameOverUI = FindAnyObjectByType<GameOverUI>();
        gameOverUI.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        isGamePause = true;
        gameOverUI.gameObject.SetActive(true);
        HightScore.instance.UpdateHightScore(ScoreManager.instance.score);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
}
