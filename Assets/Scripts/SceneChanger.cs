using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public TextMeshProUGUI LoadingText;
    public Image LoadingImage;

    private AsyncOperation asyncOperation;

    IEnumerator LoadSceneCorutine(int scene)
    {
        
        yield return new WaitForSeconds(1.5f);
        asyncOperation = SceneManager.LoadSceneAsync(scene);
        while (!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress / 0.9f;
            LoadingImage.fillAmount = progress;
            LoadingText.text = "ЗАГРУЗКА " +progress * 100f+"%";
            yield return null;
        }
        
    }

    IEnumerator LoadSceneCorutine(string scene)
    {
        yield return new WaitForSeconds(1.5f);
        asyncOperation = SceneManager.LoadSceneAsync(scene);
        while (!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress / 0.9f;
            LoadingImage.fillAmount = progress;
            LoadingText.text = "ЗАГРУЗКА " +progress * 100f+"%";
            yield return null;
        }
    }

    public void LoadScene(int scene)
    {
        LoadingText.color = new Color(LoadingText.color.r, LoadingText.color.g, LoadingText.color.b, 1f);
        StartCoroutine(LoadSceneCorutine(scene));
    }

    public void ResetScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(currentScene.name);

    }


}