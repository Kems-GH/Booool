using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public void Restart()
    {
        GameManager.Instance.Restart();
    }
    public void Quit()
    {
        GameManager.Instance.Quit();
    }
}
