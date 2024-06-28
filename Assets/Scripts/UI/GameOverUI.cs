using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public void Restart()
    {
        GameManager.instance.Restart();
    }
    public void Quit()
    {
        GameManager.instance.Quit();
    }
}
