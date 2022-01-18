namespace Microsoft.MixedReality.Toolkit.Input
{
    using UnityEngine;
    using Microsoft.MixedReality.Toolkit;
    using Microsoft.MixedReality.Toolkit.Utilities;
    using System.Collections.Generic;
    using System;

    public class ThimbelDataProvider : InputSystemGlobalHandlerListener, IMixedRealityInputHandler
    {

        [Header("MRTK Input Actions")]
        [SerializeField] private MixedRealityInputAction outputAction;
        public MixedRealityInputAction inputAction;
        public Handedness handedness;

        //public InputSystemGlobalHandlerListener InputSystemGlobalHandlerListener;
        public bool thimbelTouched = false;
        public KeyCode[] btn = new KeyCode[] { KeyCode.JoystickButton0, KeyCode.JoystickButton1, KeyCode.JoystickButton2 };
        public bool debugMode = false;


        [Flags]
        public enum ActiveThimbel
        {
            thimbel0 = 1,
            thimbel1 = 2,
            thimbel2 = 4,
            //thimbel0AndThimbel1 = thimbel0 | thimbel1
        }
        public ActiveThimbel activeThimbel;

        private void Update()
        {

           
            //var pointers = new HashSet<IMixedRealityPointer>();

            // Find all valid pointers
            //foreach (var inputSource in CoreServices.InputSystem.DetectedInputSources)
            //{
            //    foreach (var pointer in inputSource.Pointers)
            //    {
            //        if (pointer.IsInteractionEnabled && !pointers.Contains(pointer))
            //        {
            //            pointers.Add(pointer);
            //            Debug.Log("pointer: " + pointer);
            //        }
            //    }
            //}


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

            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                handedness ^= Handedness.Left;
                
            }

            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                handedness ^= Handedness.Right;
            }

            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                handedness ^= Handedness.Both;
            }


            if (handedness == Handedness.Right)
            {
                SetHandRayEnabled(false, Handedness.Left);
                SetHandRayEnabled(true, Handedness.Right);
            }
            else if (handedness == Handedness.Left)
            {
                SetHandRayEnabled(true, Handedness.Left);
                SetHandRayEnabled(false, Handedness.Right);
            }
            else if (handedness == Handedness.Both)
            {
                SetHandRayEnabled(true, Handedness.Both);
            }
            else if (handedness == Handedness.None)
            {
                SetHandRayEnabled(false, Handedness.Both);
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
                        //Debug.Log("Input down with Thimbel Flag: " + (int)activeThimbel);
                        //Debug.Log(controller.InputSource + " Handedness: " + handedness);

                    }
                    else
                    {
                        CoreServices.InputSystem?.RaiseOnInputUp(controller.InputSource, Handedness.Any, inputAction);
                        //Debug.Log("RaiseOnInputUp");
                    }
                }
            }
        }

        public void SetHandRayEnabled(bool isEnabled, Handedness handedness)
        {
            PointerUtils.SetHandRayPointerBehavior(isEnabled ? PointerBehavior.Default : PointerBehavior.AlwaysOff, handedness);
            PointerUtils.SetHandGrabPointerBehavior(isEnabled ? PointerBehavior.Default : PointerBehavior.AlwaysOff, handedness);
            PointerUtils.SetHandPokePointerBehavior(isEnabled ? PointerBehavior.Default : PointerBehavior.AlwaysOff, handedness);
        }

        #region On Input Up/Down
        public void OnInputUp(InputEventData eventData)
        {

            if (eventData.MixedRealityInputAction == outputAction) thimbelTouched = false;
        }

        public void OnInputDown(InputEventData eventData)
        {

            if (eventData.MixedRealityInputAction == outputAction) thimbelTouched = true;
            

            if (eventData.InputSource.SourceName == "GenericJoystickController Controller" || debugMode)
               {
                    
                    Debug.Log("Handedness: " + eventData.Handedness);
                    Debug.Log("SourceId: " + eventData.InputSource.SourceId);
                    Debug.Log("SourceName: " + eventData.InputSource.SourceName);
                    Debug.Log("SourceType: " + eventData.InputSource.SourceType);
                    Debug.Log("InputAction: " + eventData.MixedRealityInputAction.Description);
                    //Debug.Log("used: " + eventData.used);
                    //Debug.Log("selectedObject: " + eventData.selectedObject);
               }
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
}