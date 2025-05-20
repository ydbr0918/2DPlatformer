using UnityEngine;
using UnityEngine.SceneManagement;

public class RankOpen : MonoBehaviour
{

    public GameObject rankUI;
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
        rankUI.SetActive(true);

    }
    public void Panel2Down()
    {
        rankUI.SetActive(false);
    }
}
