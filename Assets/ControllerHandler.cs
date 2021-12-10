using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerHandler : MonoBehaviour, IMixedRealityInputHandler
{
    public bool isSliderGrabbed = false;
    public bool isControllerEnabled = false;
    public Transform leftController;
    public Transform rightController;
    public SolverHandler solverHandler;
    public Handedness handedness;


    private void OnEnable()
    {
        // Instruct Input System that we would like to receive all input events of type
        // IMixedRealitySourceStateHandler and IMixedRealityHandJointHandler
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
    }

    private void OnDisable()
    {
        // This component is being destroyed
        // Instruct the Input System to disregard us for input event handling
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);
    }

    public void OnInputDown(InputEventData eventData)
    {
        handedness = eventData.Handedness;
        Debug.Log("handedness: " + handedness);

    }
    public void OnInputUp(InputEventData eventData)
    {
        handedness = eventData.Handedness;
        Debug.Log("handedness: " + handedness);
    }



    public void ToggleGrabSlider()
    {
        
        if (handedness.IsRight())
        {           
            solverHandler.TransformOverride = rightController.GetChild(0);
        }else
        {          
            solverHandler.TransformOverride = leftController.GetChild(0);
        }        
        

        if (isControllerEnabled) {
            isSliderGrabbed = !isSliderGrabbed;         

            if (isSliderGrabbed)
            {
                solverHandler.UpdateSolvers = true;                
            }
            else
            {
                solverHandler.UpdateSolvers = false;
            }
        }        
    }

    // Update is called once per frame
    void Update()
    {
        ////Detect Controller Button Events

        //foreach (var controller in CoreServices.InputSystem.DetectedControllers)
        //{
        //    foreach (MixedRealityInteractionMapping inputMapping in controller.Interactions)
        //    {              

        //        if (controller.ControllerHandedness.IsRight())
        //        {
        //            if (inputMapping.InputType == DeviceInputType.PrimaryButtonPress || inputMapping.InputType == DeviceInputType.SecondaryButtonPress)
        //            {
        //                Debug.Log("ButtonPress Right: " + inputMapping.BoolData);                        
        //            }
        //        }

        //        if (controller.ControllerHandedness.IsLeft())
        //        {
        //            if (inputMapping.InputType == DeviceInputType.PrimaryButtonPress || inputMapping.InputType == DeviceInputType.SecondaryButtonPress)
        //            {
        //                Debug.Log("ButtonPress Left: " + inputMapping.BoolData);                      

        //            }
        //        }
        //    }

        //}

        //Detect Controller Positions
        foreach (var controller in CoreServices.InputSystem.DetectedControllers)
        {
            if (controller.InputSource.SourceType == InputSourceType.Controller)
            {                
                Debug.Log("Controller enabled");
                isControllerEnabled = true;
                // Interactions for a controller is the list of inputs that this controller exposes
                foreach (MixedRealityInteractionMapping inputMapping in controller.Interactions)
                {
                    
                    // 6DOF controllers support the "SpatialPointer" type (pointing direction)
                    // or "GripPointer" type (direction of the 6DOF controller)
                    if (inputMapping.InputType == DeviceInputType.SpatialPointer && controller.ControllerHandedness.IsRight())
                    {
                        //Debug.Log("spatial pointer PositionData: " + inputMapping.PositionData);
                        //Debug.Log("spatial pointer RotationData: " + inputMapping.RotationData);
                       
                        rightController.position = inputMapping.PositionData;
                        rightController.localRotation = inputMapping.RotationData;
                    }

                    if (inputMapping.InputType == DeviceInputType.SpatialPointer && controller.ControllerHandedness.IsLeft())
                    {
                        //Debug.Log("spatial pointer PositionData: " + inputMapping.PositionData);
                        //Debug.Log("spatial pointer RotationData: " + inputMapping.RotationData);
               
                        leftController.position = inputMapping.PositionData;
                        leftController.localRotation = inputMapping.RotationData;
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
                isControllerEnabled = false;
                Debug.Log("Hands enabled");
            }
        }
    }
}
