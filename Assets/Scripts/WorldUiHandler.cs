using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class WorldUiHandler : MonoBehaviour
{

    public bool isPositioning = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    //private Handedness pickedHand;
    //public bool isPickedUp = false;

    //public void OnInputDown(InputEventData eventData)
    //{
    //    if (eventData.InputSource.SourceType == InputSourceType.Controller && isPickedUp == false)
    //    {
    //        Debug.Log("pickedHand: " + pickedHand);
    //        isPickedUp = true;
    //        pickedHand = eventData.Handedness;
    //    }
    //}

    //public void OnInputUp(InputEventData eventData)
    //{
    //    if (eventData.InputSource.SourceType == InputSourceType.Controller && isPickedUp == true)
    //    {
    //        Debug.Log("pickedHand: " + pickedHand);
    //        isPickedUp = false;
    //        pickedHand = eventData.Handedness;
    //    }
    //}



// Update is called once per frame
void Update()
    {

        
    }
    public void TogglePositioningSlider()
    {
        isPositioning= !isPositioning;
    }
}
