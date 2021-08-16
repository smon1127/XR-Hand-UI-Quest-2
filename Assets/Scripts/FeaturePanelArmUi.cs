using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeaturePanelArmUi : MonoBehaviour
{

    public GameObject environment;
    public bool passthroughOn = false;
    public MeshRenderer articulatedHand;
    public bool handsOn = false;


    // Update is called once per frame
    void Update()
    {
        if (passthroughOn)
        {
            environment.SetActive(false);
        }
        else
        {
            environment.SetActive(true);
        }

        if (handsOn)
        {
            articulatedHand.enabled = true;
        }
        else
        {
            articulatedHand.enabled = false;
        }

        
    }

    public void PassthroughToggle()
    {
        passthroughOn = !passthroughOn;
    }

    public void HandToggle()
    {
        handsOn = !handsOn;
    }



}
