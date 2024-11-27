using System.Collections.Generic;
using UnityEngine;

public class PanelSelector : MonoBehaviour
{
    public GameObject defaultPanel;

    public GameObject levelSelectionPanel;
    public GameObject startPreparationPanel;
    public GameObject racingPanel;
    public GameObject pausePanel;
    public GameObject finishedPanel;
    private void Start()
    {
        defaultPanel.SetActive(true);
    }

    private void OnValidate()
    {
        defaultPanel.SetActive(true);
    }
}
