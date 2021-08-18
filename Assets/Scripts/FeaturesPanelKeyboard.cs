// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using static Microsoft.MixedReality.Toolkit.Experimental.UI.NonNativeKeyboardCustom;

namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    using UnityEngine;

    /// <summary>
    /// Class that initializes the appearance of the features panel according to the toggled states of the associated features
    /// </summary>
    public class FeaturesPanelKeyboard : MonoBehaviour
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

        public bool isRec = false;
        public bool isAudio = false;
        public bool isHaptic = false;

        public NonNativeKeyboardCustom nonNativeKeyboard;
        public GameObject targetObject;
        public string inputText = "0";

        public EvaluationTimer evaluationTimer;
        public int iteration = 0;

        private void Start()
        {
            userIdText.text = userId.ToString();
            targetObject.SetActive(false);
        }

        private void Update()
        {        
            if (nonNativeKeyboard.isActiveAndEnabled)
            {
                inputIsActive = true;
                targetObject.SetActive(false);
            }
            else {
                inputIsActive = false;
                targetObject.SetActive(true);
            }

            isRec = toggleRec.IsToggled;
            isAudio = toggleAudio.IsToggled;
            isHaptic = toggleHaptics.IsToggled;

            Debug.Log("visual keyboard: " + inputIsActive);
            
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnSubmit();
            }

            iteration = evaluationTimer.iteration;
            iterationText.text = iteration.ToString();


            if (Input.GetKeyDown(KeyCode.Keypad1) && !inputIsActive) {                
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
        public void OnSubmit()
        {
            if (inputIsActive)
            {
                inputText = nonNativeKeyboard.InputField.text;
                userIdText.text = inputText;
                if (inputText != "")
                    userId = int.Parse(inputText);
            }

            if (toggleUserId.IsToggled)
            {
                toggleUserId.IsToggled = !toggleUserId.IsToggled;
                nonNativeKeyboard.Close();
            }
            Debug.Log("inputText: " + inputText);
        }

        public void ToggleUserId()
        {
            if (toggleUserId.IsToggled)
            {
                nonNativeKeyboard.PresentKeyboard("", LayoutType.Symbol);
            }
            else
            {
                nonNativeKeyboard.Close();
            }
        }

    }        
}
