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
        [SerializeField]
        private Interactable toggleUserId = null;
        [SerializeField]
        private Interactable toggleRec = null;
        [SerializeField]
        private Interactable toggleAudio = null;
        [SerializeField]
        private Interactable toggleHaptics = null;
        [SerializeField]
        private Interactable togglePassthrough = null;
        [SerializeField]
        private Interactable buttonIteration = null;
        [SerializeField]
        private Interactable toggleScores = null;
        [SerializeField]
        private Interactable buttonExport = null;

        [SerializeField]
        private Interactable buttonShuffle = null;

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

        public ArmUiHandler armUiHandler = null;
        public GameObject environment = null;

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


            userIdText.text = userId.ToString();
            targetObject.SetActive(false);
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
                buttonExport.TriggerOnClick();
            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus) && !inputIsActive)
            {
                buttonShuffle.TriggerOnClick();
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
