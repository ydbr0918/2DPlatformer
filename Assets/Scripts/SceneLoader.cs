using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static string nextScene;

    private float timer = 0f;
    private bool isLoading = false;

    void Start()
    {
        isLoading = true;
    }

    void Update()
    {
        if (isLoading)
        {
            timer += Time.deltaTime;

            if (timer >= 3f)
            {
                isLoading = false;
                LoadNextScene();
            }
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    public static void LoadSceneWithLoading(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
}