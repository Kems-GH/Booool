using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool isGamePause { get; private set; } = false;

    private GameOverUI _gameOverUI;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }
    private void Start()
    {
        _gameOverUI = FindAnyObjectByType<GameOverUI>();
        _gameOverUI.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        isGamePause = true;
        _gameOverUI.gameObject.SetActive(true);
        HightScore.Instance.UpdateHightScore(ScoreManager.Instance.score);
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
