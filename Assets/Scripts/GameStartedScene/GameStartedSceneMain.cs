using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartedSceneMain : MonoBehaviour
{
    public GameObject PressButtonText;
    public TextMeshProUGUI LoadingText;
    public Image LoadingImage;
    public int SceneID;

    private AsyncOperation asyncOperation;
    private bool readyToStart = false;

    IEnumerator LoadSceneCorutine()
    {
        yield return new WaitForSeconds(1.5f);
        asyncOperation = SceneManager.LoadSceneAsync(SceneID);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            float progress = asyncOperation.progress / 0.9f;
            LoadingImage.fillAmount = progress;
            LoadingText.text = "ЗАГРУЗКА " +progress * 100f+"%";

            if (progress == 1)
            {
                PressButtonText.SetActive(true);
            }

            if (readyToStart == true)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        
    }

    private void Awake()
    {
        PressButtonText.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(LoadSceneCorutine());
        
    }

    private void Update()
    {
        if (!readyToStart && PressButtonText.activeSelf && Input.anyKey)
        {
            readyToStart = true;
        }
    }
}


