using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeOpacityInfinite : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmp;
    private float multiplier = -1f;
    private float opacityMax = 1f;
    private float opacityMin = 0.3f;
    public float step = 0.08f;

    private void Start()
    {
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1);
    }

    private void AnimateOpacity()
    {
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a + step*multiplier);
        if (tmp.color.a <= opacityMin || tmp.color.a>= opacityMax)
        {
            multiplier *= -1f;
        }
    }

    void Update()
    {
        AnimateOpacity();
    }
}
