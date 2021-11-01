using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CivilianWait : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void SetMaxValue(float val)
    {
        slider.maxValue = val;
        slider.value = val;
    }

    public void SetValue(float val)
    {
        slider.value = val;
    }
}
