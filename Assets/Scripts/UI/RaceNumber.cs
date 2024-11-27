using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RaceNumber : MonoBehaviour
{
    public void UpdateRaceNumberUI()
    {
        this.GetComponent<TextMeshProUGUI>().text = "Заезд: " + CrossSceneData.RaceNumberLevel1;
    }

    private void Start()
    {
        UpdateRaceNumberUI();
    }

    private void OnEnable()
    {
        UpdateRaceNumberUI();
    }
}

