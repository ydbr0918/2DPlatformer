using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreOpen : MonoBehaviour
{

    public GameObject scoreUI;
    public void GameStart()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void RankPanelOpen()
    {
        scoreUI.SetActive(true);

    }
    public void RankPanelDown()
    {
        scoreUI.SetActive(false);
    }
}
