using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class WorldUiHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

        //Detect Controller Events

        foreach (var controller in CoreServices.InputSystem.DetectedControllers)
        {

            if (controller.InputSource.SourceType == InputSourceType.Controller)
            {
                Debug.Log("Controller enabled");

                // Interactions for a controller is the list of inputs that this controller exposes
                foreach (MixedRealityInteractionMapping inputMapping in controller.Interactions)
                {
                    if (inputMapping.InputType == DeviceInputType.PrimaryButtonPress || inputMapping.InputType == DeviceInputType.SecondaryButtonPress)
                    {
                        Debug.Log("ButtonPress: " + inputMapping.BoolData);
                    }

                    // 6DOF controllers support the "SpatialPointer" type (pointing direction)
                    // or "GripPointer" type (direction of the 6DOF controller)
                    if (inputMapping.InputType == DeviceInputType.SpatialPointer)
                    {
                        //Debug.Log("spatial pointer PositionData: " + inputMapping.PositionData);
                        //Debug.Log("spatial pointer RotationData: " + inputMapping.RotationData);
                    }

                    if (inputMapping.InputType == DeviceInputType.SpatialGrip)
                    {
                        //Debug.Log("spatial grip PositionData: " + inputMapping.PositionData);
                        //Debug.Log("spatial grip RotationData: " + inputMapping.RotationData);
                    }                  

                }
            }
            else if (controller.InputSource.SourceType == InputSourceType.Hand)
            {
                Debug.Log("Hands enabled");
            }
        }
    }
}
