using System;
using System.Collections;
using System.Collections.Generic;
using Ashsvp;
using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    [SerializeField]
    private Main main;
    [SerializeField]
    private TextMeshProUGUI speed;
    [SerializeField]
    private TextMeshProUGUI measure;

    enum SpeedMeasureSystem
    {
        KilometersPerHour,
        MilesPerHour
    }

    private List<string> speedMeasureSystemStrings = new List<string>(){"Км/ч","Mph" };
    private List<float> speedMeasureSystemMultipliers = new List<float>(){1f,0,621371f};

    public GearSystem carSpeedSource;


    [SerializeField] private SpeedMeasureSystem currentSpeedMeasureSystem;
    private float currentSpeedMeasureSystemMultiplier;
    private void OnEnable()
    {
        if (carSpeedSource == null)
        {
            carSpeedSource = main.currentCar.GetComponent<GearSystem>();
        }
        
        switch (currentSpeedMeasureSystem)
        {
            case SpeedMeasureSystem.KilometersPerHour:
                measure.text = speedMeasureSystemStrings[(int)SpeedMeasureSystem.KilometersPerHour];
                currentSpeedMeasureSystemMultiplier = speedMeasureSystemMultipliers[(int)SpeedMeasureSystem.KilometersPerHour];
                break;
            case SpeedMeasureSystem.MilesPerHour:
                measure.text = speedMeasureSystemStrings[(int)SpeedMeasureSystem.MilesPerHour];
                currentSpeedMeasureSystemMultiplier = speedMeasureSystemMultipliers[(int)SpeedMeasureSystem.KilometersPerHour];
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        speed.text = carSpeedSource.carSpeed.ToString();
    }
}
