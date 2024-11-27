using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsBehaviour : MonoBehaviour
{
    public bool requiresLoadingBox;
    public GameObject uiMainBox;
    public LoadingBoxBehaviour loadingBox;
    public bool regenerateLinks = false;
    
    private void OnEnable()
    {
        // при активации интерфейса деактивируем другие интерфейсы, с учётом необходимости интерфейса загрузки сцен
        GameObject[] uiLayers = GameObject.FindGameObjectsWithTag("uiLayer");
        foreach (GameObject ui in uiLayers)
        {
            if (ui.name != name || (ui.name=="Loading Box" && requiresLoadingBox==false))
            {
                ui.SetActive(false);
            }
            else
            {
                ui.SetActive(true);
            }
            loadingBox.EnableLoaderUI();
        }
    }
}
