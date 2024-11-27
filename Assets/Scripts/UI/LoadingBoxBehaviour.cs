using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBoxBehaviour : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Image image;
    
    public void DisableLoaderUI()
    {
        this.gameObject.SetActive(false);
    }

    public void EnableLoaderUI()
    {
        this.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        text.text = "ЗАГРУЗКА 0%";
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        image.fillAmount = 0;
    }
}

