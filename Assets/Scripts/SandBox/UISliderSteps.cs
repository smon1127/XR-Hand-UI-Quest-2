using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISliderSteps : MonoBehaviour
{
    int stepAmount = 100;
    Slider mySlider = null;

    int numberOfSteps = 0;

    // Start is called before the first frame update
    void Start()
    {
        mySlider = GetComponent<Slider>();
        numberOfSteps = (int)mySlider.maxValue / stepAmount;
    }

    public void UpdateStep()
    {
        float range = (mySlider.value / mySlider.maxValue) * numberOfSteps;
        int ceil = Mathf.CeilToInt(range);
        mySlider.value = ceil * stepAmount;
    }
}