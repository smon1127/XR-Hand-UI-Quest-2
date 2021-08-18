// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;

using TMPro;
using static Microsoft.MixedReality.Toolkit.Experimental.UI.NonNativeKeyboard;

namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Class that initializes the appearance of the features panel according to the toggled states of the associated features
    /// </summary>
    internal class FeaturesPanelKeyboard : MonoBehaviour
    {
        [SerializeField]
        private Interactable toggleUserId = null;
        [SerializeField]
        private Interactable toggleRec = null;
        [SerializeField]
        private Interactable toggleAudio = null;
        [SerializeField]
        private Interactable toggleHaptics = null;
        [SerializeField]
        private Interactable buttonTarget = null;
        [SerializeField]
        private Interactable buttonIteration = null;
        [SerializeField]
        private Interactable toggleScores = null;
        [SerializeField]
        private Interactable buttonExport = null;





        public TextMeshPro userIdText;
        public TextMeshPro iterationText;

        public int userId = 0;
        public bool inputIsActive = false;


        public NonNativeKeyboard nonNativeKeyboard;
        public string inputText = "0";

        private void Start()
        {
            userIdText.text = userId.ToString();
        }


        private void Update()
        {
        
            if (nonNativeKeyboard.isActiveAndEnabled)
            {
                inputIsActive = true;
            }
            else {
                inputIsActive = false;
            }

            Debug.Log("visual keyboard: " + inputIsActive);
            


            
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (inputIsActive)
                {
                    inputText = nonNativeKeyboard.InputField.text;
                    userIdText.text = inputText;
                    userId = int.Parse(inputText);
                    
                }

                if (toggleUserId.IsToggled)
                {
                    toggleUserId.IsToggled = !toggleUserId.IsToggled;
                    nonNativeKeyboard.Close();
                }
                Debug.Log("inputText: " + inputText);
            }

            



            if ((toggleUserId.IsToggled || Input.GetKeyDown(KeyCode.Keypad1)) && !inputIsActive) {                
                nonNativeKeyboard.PresentKeyboard("", LayoutType.Symbol);                         
                toggleUserId.IsToggled = !toggleUserId.IsToggled;
            }

            if (Input.GetKeyDown(KeyCode.Keypad2) && !inputIsActive)
            {
                toggleRec.IsToggled = !toggleRec.IsToggled;
            }

            if (Input.GetKeyDown(KeyCode.Keypad3) && !inputIsActive)
            {
                toggleAudio.IsToggled = !toggleAudio.IsToggled;
            }

            if (Input.GetKeyDown(KeyCode.Keypad4) && !inputIsActive)
            {
                toggleHaptics.IsToggled = !toggleHaptics.IsToggled;
            }

            if (Input.GetKeyDown(KeyCode.Keypad5) && !inputIsActive)
            {
                buttonTarget.SetInputDown();
            }

            if (Input.GetKeyDown(KeyCode.Keypad6) && !inputIsActive)
            {
                buttonIteration.SetInputDown();
            }

            if (Input.GetKeyDown(KeyCode.Keypad7) && !inputIsActive)
            {
                toggleScores.IsToggled = !toggleScores.IsToggled;
            }

            if (Input.GetKeyDown(KeyCode.Keypad8) && !inputIsActive)
            {
                buttonExport.SetInputDown();
            }
        }

    }        
}
