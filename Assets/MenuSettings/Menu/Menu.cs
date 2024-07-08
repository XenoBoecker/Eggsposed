using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Image loadingBarFill;

    [SerializeField] string gameSceneName = "Game";

    private void Start()
    {
        if(loadingPanel != null) loadingPanel.SetActive(false);
    }

    public void StartGame()
    {
        LoadScene(gameSceneName);
    }

    public void GoToMenu()
    {
        LoadScene("Menu");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        // StartCoroutine(LoadSceneAsync(sceneName)); // if loading screen is needed (needs loading Panel with loading bar)
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        loadingPanel.SetActive(true);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            loadingBarFill.fillAmount = asyncLoad.progress / 0.9f;

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}