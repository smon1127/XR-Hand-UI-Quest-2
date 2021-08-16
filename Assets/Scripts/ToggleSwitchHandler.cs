using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSwitchHandler : MonoBehaviour
{
    public GameObject toggleSelected;
    public GameObject toggleDeselected;
    public Interactable circularButtonInteractable;
    public bool isMenuToggle = true;
    public PinchSlider pinchSliderSwitch = null;


    // Update is called once per frame

    void Update()
    {
        if (isMenuToggle)
        {
            toggleSelected.SetActive(true);
            toggleDeselected.SetActive(false);
            circularButtonInteractable.IsToggled = true;
        }
        else
        {
            toggleSelected.SetActive(false);
            toggleDeselected.SetActive(true);
            circularButtonInteractable.IsToggled = false;
        }        
    }



    public void ToggleSwitch()
    {
        if (pinchSliderSwitch.SliderValue < 1)
        {

            isMenuToggle = false;
        }
        else if (pinchSliderSwitch.SliderValue > 0)
        {

            isMenuToggle = true;
        }
    }


}
