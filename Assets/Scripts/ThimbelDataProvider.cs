using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using System;

public class ThimbelDataProvider : InputSystemGlobalHandlerListener, IMixedRealityInputHandler
{

    [Header("MRTK Input Actions")]
    [SerializeField] private MixedRealityInputAction outputAction;
    public MixedRealityInputAction inputAction;

    [SerializeField] private bool thimbelTouched = false;
    private bool isActive = false;

    public KeyCode[] btn = new KeyCode[] {KeyCode.JoystickButton0, KeyCode.JoystickButton1, KeyCode.JoystickButton2};

    [Flags]public enum ActiveThimbel
    {
        thimbel0 = 1,
        thimbel1 = 2,
        thimbel2 = 4,
        //thimbel0AndThimbel1 = thimbel0 | thimbel1
    }
    public ActiveThimbel activeThimbel;



    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            activeThimbel ^= ActiveThimbel.thimbel0;
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            activeThimbel ^= ActiveThimbel.thimbel1;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            activeThimbel ^= ActiveThimbel.thimbel2;
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            activeThimbel ^= ActiveThimbel.thimbel0;
            activeThimbel ^= ActiveThimbel.thimbel1;
            activeThimbel ^= ActiveThimbel.thimbel2;
        }


        // Don´t get confused. (int)activeThimble displays the flag value. Check following Debug to understand.
        //Debug.Log("activeThimbel: " + (int)activeThimbel);

        if ((int)activeThimbel == 7 || (int)activeThimbel == -1)
        {
            if (Input.GetKey(btn[0]) || Input.GetKey(btn[1]) || Input.GetKey(btn[2]))
            {
                ActivateButton(true);
            }
            else
            {
                ActivateButton(false);
            }

        }
        else if ((int)activeThimbel == 3)
        {
            if (Input.GetKey(btn[0]) || Input.GetKey(btn[1]))
            {
                ActivateButton(true);
            }
            else
            {
                ActivateButton(false);
            }

        }
        else if ((int)activeThimbel == 6)
        {
            if (Input.GetKey(btn[1]) || Input.GetKey(btn[2]))
            {
                ActivateButton(true);
            }
            else
            {
                ActivateButton(false);
            }

        }
        else if ((int)activeThimbel == 5)
        {
            if (Input.GetKey(btn[0]) || Input.GetKey(btn[2]))
            {
                ActivateButton(true);
            }
            else
            {
                ActivateButton(false);
            }

        }
        else if ((int)activeThimbel == 1)
        {
            if (Input.GetKey(btn[0]))
            {
                ActivateButton(true);
            }
            else
            {
                ActivateButton(false);
            }

        }
        else if ((int)activeThimbel == 2)
        {
            if (Input.GetKey(btn[1]))
            {
                ActivateButton(true);
            }
            else
            {
                ActivateButton(false);
            }

        }
        else if ((int)activeThimbel == 4)
        {
            if (Input.GetKey(btn[2]))
            {
                ActivateButton(true);
            }
            else
            {
                ActivateButton(false);
            }
        }
        else
        {
            ActivateButton(false);
        }



    }

    public void ActivateButton(bool isActive)
    {
        foreach (var controller in CoreServices.InputSystem.DetectedControllers)
        {
            if (controller.InputSource.SourceType == InputSourceType.Controller)
            {

                if (isActive)
                {
                    CoreServices.InputSystem?.RaiseOnInputDown(controller.InputSource, Handedness.Any, inputAction);
                    Debug.Log("Input down with Thimbel Flag: " + (int)activeThimbel);
                }
                else
                {
                    CoreServices.InputSystem?.RaiseOnInputUp(controller.InputSource, Handedness.Any, inputAction);
                    //Debug.Log("RaiseOnInputUp");
                }
            }
        }
    }

    #region On Input Up/Down
    public void OnInputUp(InputEventData eventData)
    {

        if (eventData.MixedRealityInputAction == outputAction) thimbelTouched = false;
    }

    public void OnInputDown(InputEventData eventData)
    {

        if (eventData.MixedRealityInputAction == outputAction) thimbelTouched = true;
    }
    #endregion

    #region Handlers
    protected override void RegisterHandlers()
    {
        CoreServices.InputSystem.RegisterHandler<IMixedRealityInputHandler>(this);
    }

    protected override void UnregisterHandlers()
    {

    }

  
    #endregion
}