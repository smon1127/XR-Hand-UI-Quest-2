// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.UI
{
    /// <summary>
    /// Class that initializes the appearance of the features panel according to the toggled states of the associated features
    /// </summary>
    public class FeaturesHapticPanelVisuals : MonoBehaviour
    {
        [SerializeField]
        private Interactable hapticButton = null;
        [SerializeField]
        private Interactable audioButton = null;
        [SerializeField]
        private Interactable handMeshButton = null;
        [SerializeField]
        private Interactable passthroughButton = null;

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
                handTrackingProfile.EnableHandMeshVisualization = handMeshButton.IsToggled;
                
            }


            

            if (passthroughButton.IsToggled)
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
                
                       
            localAvatar.SetActive(handMeshButton.IsToggled);
            armUiHandler.isHaptic = hapticButton.IsToggled;
            armUiHandler.isAudio = audioButton.IsToggled;           

        }

        public void ToggleHandsWhenFalse()
        {
            if (!passthroughButton.IsToggled)
                handMeshButton.IsToggled = true;
        }
    }
}
