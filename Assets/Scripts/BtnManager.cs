using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{

    public GameObject helpUI;
    public void GameStart()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void PanelOpen()
    {
        helpUI.SetActive(true);

    }
    public void PanelDown()
    {
        helpUI.SetActive(false);
    }
}
