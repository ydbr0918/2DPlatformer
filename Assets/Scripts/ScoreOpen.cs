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

    public void Panel2Open()
    {
        scoreUI.SetActive(true);

    }
    public void Panel2Down()
    {
        scoreUI.SetActive(false);
    }
}
