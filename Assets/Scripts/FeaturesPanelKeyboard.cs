// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using static Microsoft.MixedReality.Toolkit.Experimental.UI.NonNativeKeyboardCustom;

namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    using Microsoft.MixedReality.Toolkit.Input;
    using UnityEngine;

    /// <summary>
    /// Class that initializes the appearance of the features panel according to the toggled states of the associated features
    /// </summary>
    public class FeaturesPanelKeyboard : MonoBehaviour
    {

        public Interactable toggleUserId = null;

        public Interactable toggleRec = null;

        public Interactable toggleAudio = null;

        public Interactable toggleHaptics = null;

        public Interactable togglePassthrough = null;

        public Interactable buttonIteration = null;

        public Interactable toggleScores = null;

        public Interactable toggleHome = null;

        public Interactable toggleSettings = null;

        public GameObject settingsPanel = null;
        public GameObject homePanel = null;


        public Interactable buttonShuffle = null;

        public TextMeshPro userIdText;
        public TextMeshPro iterationText;

        public int userId = 0;
        public bool inputIsActive = false;

        public bool isRec = false;
        public bool isAudio = false;
        public bool isHaptic = false;
        public bool isSetting = true;

        public NonNativeKeyboardCustom nonNativeKeyboard;
        public GameObject targetObject;
        public string inputText = "0";

        public EvaluationTimer evaluationTimer;
        public int iteration = 0;

        public ArmUiHandler armUiHandler = null;
        public GameObject environment = null;
        public GameObject anchors = null;
        public GameObject calibrateArea = null;
        private bool firstTime = true;

        private GameObject localAvatar = null;
        private GameObject cameraRig = null;
        private OVRPassthroughLayer passthroughLayer = null;
        public OVROverlay.OverlayType AROverlay = OVROverlay.OverlayType.Overlay;
        public OVROverlay.OverlayType VRUnderlay = OVROverlay.OverlayType.Underlay;

        private void Start()
        {
            localAvatar = GameObject.Find("MRTK-Quest_LocalAvatar(Clone)");
            cameraRig = GameObject.Find("MRTK-Quest_OVRCameraRig(Clone)");
            passthroughLayer = cameraRig.GetComponent<OVRPassthroughLayer>();

            togglePassthrough.IsToggled = armUiHandler.isPassthrough;
            userIdText.text = userId.ToString();
            targetObject.SetActive(false);
            gameObject.SetActive(true);
            anchors.SetActive(false);
            firstTime = true;
        }

        private void Update()
        {

            MixedRealityInputSystemProfile inputSystemProfile = CoreServices.InputSystem?.InputSystemProfile;
            if (inputSystemProfile == null)
            {
                return;
            }

            MixedRealityHandTrackingProfile handTrackingProfile = inputSystemProfile.HandTrackingProfile;
            if (handTrackingProfile != null)
            {
                handTrackingProfile.EnableHandMeshVisualization = !togglePassthrough.IsToggled;

            }


            if (togglePassthrough.IsToggled)
            {
                //Passthrouh on
                passthroughLayer.overlayType = AROverlay;
                passthroughLayer.textureOpacity = .2f;
                environment.SetActive(false);
            }
            else
            {
                //Passthrouh off
                passthroughLayer.overlayType = VRUnderlay;
                environment.SetActive(true);
                passthroughLayer.textureOpacity = .2f;
            }


            localAvatar.SetActive(!togglePassthrough.IsToggled);
            if(armUiHandler != null)
            {
                armUiHandler.isHaptic = toggleHaptics.IsToggled;
                armUiHandler.isAudio = toggleAudio.IsToggled;
            }
           

            if (nonNativeKeyboard.isActiveAndEnabled)
            {
                inputIsActive = true;
                targetObject.SetActive(false);
            }
            else
            {
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


            if (Input.GetKeyDown(KeyCode.Keypad1) && !inputIsActive)
            {
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
                togglePassthrough.IsToggled = !togglePassthrough.IsToggled;
            }

            if (Input.GetKeyDown(KeyCode.Keypad6) && !inputIsActive)
            {
                buttonIteration.TriggerOnClick();
            }

            if (Input.GetKeyDown(KeyCode.Keypad7) && !inputIsActive)
            {
                toggleScores.IsToggled = !toggleScores.IsToggled;
            }

            if (Input.GetKeyDown(KeyCode.Keypad8) && !inputIsActive)
            {
            
            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus) && !inputIsActive)
            {
                buttonShuffle.TriggerOnClick();
            }
        }
    
        public void ToggleOptions(int buttonNum)
        {
 
            switch (buttonNum)
            {
                //Home active
                case 1:
                    homePanel.SetActive(true);
                    settingsPanel.SetActive(false);
                    toggleSettings.IsToggled = false;
                    anchors.SetActive(false);
                    calibrateArea.SetActive(false);
                    break;
                //Settings active
                case 2:
                    homePanel.SetActive(false);
                    settingsPanel.SetActive(true);
                    toggleHome.IsToggled = false;
                    if (firstTime)
                    {                       
                        anchors.SetActive(false);                                                
                        firstTime = false;
                    }
                    else
                    {
                        anchors.SetActive(true);
                    }
                    
                    calibrateArea.SetActive(true);
                    break;
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
