

namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    using System.Collections.Generic;
    using UnityEngine.Events;
    using UnityEngine;
    using System;
    using UnityEngine.XR;
    using Microsoft.MixedReality.Toolkit.UI;
    using Microsoft.MixedReality.Toolkit.Input;
    using Microsoft.MixedReality.Toolkit.Utilities;
    using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
    using System.Collections;
    using TMPro;

    public class ArmUiHandler : MonoBehaviour
    {


        //+++++++
        //Range Variables 
        //+++++++

        [Range(-0.01f, 0.03f)]
        public float uiOffsetPositionY = 0.0f;

        [Range(-0.12f, 0.04f)]
        public float uiOffsetPositionX = -0.04f;

        [Range(-0.12f, 0.04f)]
        public float uiOffsetPositionZ = 0f;

        [Range(0.25f, 0.75f)]
        public float uiScale = 0.6f;

        [Range(45.0f, 105.0f)]
        public float uiOffsetRotationX = 55.0f;


        public bool isSwitchToggled = true;


        [Range(-.05f, .05f)]
        public float togglePositionX = 0f;

        [Range(-.05f, .05f)]
        public float togglePositionY = 0f;

        [Range(-.05f, .05f)]
        public float togglePositionZ = 0f;

        public Vector3 toggleRotation = new Vector3(0, 0, 0);
        public Vector3 toggleRotationParent = new Vector3(0, 0, 0);

        [Range(-.05f, .05f)]
        public float togglePositionPalmUpX = 0f;

        [Range(-.05f, .05f)]
        public float togglePositionPalmUpY = 0f;

        [Range(-.05f, .05f)]
        public float togglePositionPalmUpZ = 0f;


        [Range(-0.1f, 0.1f)]
        public float hapticLatencyPositionOffset = 0.013f;
        [Range(0f, 1f)]
        public float hapticLatencyAudioOffset = 0f;
        [Range(0.01f, 0.1f)]
        public float sphereSizeCalibrate = 0.04f;
        public float sphereSizeIdle = 0.4f;
        private float sphereSize;

        public int hoverItemTierPage = 3;
        public int armHapticPattern = 1;

     

        public Vector3 worldUIStartPosition = new Vector3(0,0.1f,0.2f);
        public Vector3 panelStartPosition = new Vector3(0f, 0.3f, 0f);


        public Color calibrationColor = new Vector4(0, 0, 0, 1);
        public Color hoverColor = new Vector4(0, 0, 0, 1);
        public Color defaultColor = new Vector4(0, 0, 0, 1);
        public Color scrollArowIsDraggingColor = new Vector4(1, 1, 1, 1);
        public Color scrollArowIdleColor = new Vector4(1, 1, 1, .5f);
        public Color hoverBackplateColor = new Vector4(0,0,0, 1f);
        public Color idleBackplateColor = new Vector4(0, 0, 0, 1f);
        //public Color armHoverColor = new Vector4(0.3160377f, 0.6499509f, 1, 1);
        //public Color armIdleColor = new Vector4(0, 0, 0, 1);
        public bool debug = false;
        
        public bool isPalmUp = false;
        public bool leftIndexInCalibrationZone = false;
        public bool rightIndexInCalibrationZone = false;
        public bool menuIsLeft = false;
        public bool rightIsDominant = true;
        public bool isHaptic = true;
        public bool isAudio = true;
        public bool isCalibrating = false;
        public bool toggleFeaturePanelVisibility = false;
        public bool armButtonsPressed = false;
        public bool leftHandIsTracked = false;
        public bool rightHandIsTracked = false;

        //+++++++
        //Public Variables 
        //+++++++






        public TextMeshPro scrollIconUpText;
        public TextMeshPro scrollIconDownText;
        public MeshRenderer scrollBackplateArmQuad;
        public TextMeshPro scrollIconUpText2;
        public TextMeshPro scrollIconDownText2;
        public MeshRenderer scrollBackplateWorldQuad;
        public GameObject calibrateSphere;
        public Transform elbow;
        public Transform leftWrist;
        public Transform rightWrist;
        public Transform rightIndex;
        public Transform leftIndex;
        public Transform rightThumb;
        public Transform leftThumb;
        public Transform thumb;
        public Transform wrist;

        public Transform uiAnchor;
        public Transform uiOffset;

        public GameObject[] debugCubes = new GameObject[4];

        public PinchSlider pinchSliderSwitch = null;
        public ScrollingObjectCollection armScroll;
        public ScrollingObjectCollection worldScroll;
        public Randomizer randomizer;
        public Transform worldGridObject;
        public Transform armGridObject;
        public GameObject menuToggle;
        public GameObject scrollMenu;
        public GameObject mainMenuLeft;
        public GameObject mainMenuRight;
        public GameObject calibrationButton;
        public Interactable circularButtonInteractable;
        public RadialViewAnchor radialViewAnchor;
        public GameObject toggleFeaturePanel;
        public SolverHandler anchorSolvers;
        public GameObject anchors;
        public ArmSliderHandler armSliderHandler = null;
        public Interactable buttonSelect;
        public MeshRenderer scrollBackplateQuad;

        //private InputDevice LeftController = new InputDevice();
        //private InputDevice RightController = new InputDevice();
        //private Vector3 RightHandPos;
        //private float RightHandTrigger;
        //private Vector3 LeftHandPos;
        //private float LeftHandTrigger;
        //private Quaternion EyeRotation;
        //private Vector3 EyePos;
        private bool firstTime;
        private float calibrateSpherePosXRight;
        private float calibrateSpherePosXLeft;

        //+++++++
        //Private Variables 
        //+++++++
      
        private List<InputDevice> Devices = new List<InputDevice>();
        private GameObject rightHand;
        private GameObject leftHand;
        private GameObject currentWorldItem;
        private bool readyForCalibration = true;


        void TrackingLost()
        {
            Debug.Log("********************* TrackingLost **************************");
        }

        // Start is called before the first frame update
        void Start()
        {
            
            InputDevices.GetDevices(Devices);
            foreach (var dev in Devices)
            {
                Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", dev.name, dev.isValid.ToString()));
            }

            Debug.Log("----------------------------------- " + Devices.Count.ToString());        

            calibrateSpherePosXRight = -calibrateSphere.transform.position.x;
            calibrateSpherePosXLeft = calibrateSphere.transform.position.x;
            sphereSize = sphereSizeIdle;
            anchorSolvers.AdditionalOffset = worldUIStartPosition;
            toggleFeaturePanel.transform.localPosition = panelStartPosition;

            if (rightIsDominant)
            {
                calibrateSphere.transform.position = new Vector3(calibrateSpherePosXLeft, calibrateSphere.transform.position.y, calibrateSphere.transform.position.z);
            }
            else
            {
                calibrateSphere.transform.position = new Vector3(calibrateSpherePosXRight, calibrateSphere.transform.position.y, calibrateSphere.transform.position.z);
            }

            IMixedRealityHandJointService handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
            if (handJointService != null)
            {
                leftHandIsTracked = handJointService.IsHandTracked(Handedness.Left);
                rightHandIsTracked = handJointService.IsHandTracked(Handedness.Left);
            }

            if (debug)
            {
                pinchSliderSwitch.SliderValue = 1;
                radialViewAnchor.IsAppActive(true);
                firstTime = false;
            }
            else
            {                
                FlipToggleScale(false);
                uiOffset.gameObject.SetActive(false);              
                menuToggle.SetActive(false);
                firstTime = true;
                isSwitchToggled = true;
                radialViewAnchor.IsAppActive(false);
            }
            

        }


        // Update is called once per frame
        void Update()
        {

            



            //// If MRTK is in the process of disabling all services, then do not create a new camera 
            //if (!MixedRealityToolkit.IsDisablingAllServices)
            //{
            //    Debug.LogWarning("No cameras found. Creating a \"MainCamera\".");
            //    mainCamera = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener)) { tag = "MainCamera" }.GetComponent<Camera>();
            //}



            //if (rightIndexInCalibrationZone || leftIndexInCalibrationZone)
            //    StartCoroutine(CalibrateArms());

            if (rightIndexInCalibrationZone || leftIndexInCalibrationZone)
                readyForCalibration = true;

            if (rightIndexInCalibrationZone)
                menuIsLeft = true;

            if (leftIndexInCalibrationZone)
                menuIsLeft = false;

            if (readyForCalibration)
            {
                calibrationButton.SetActive(true);
            }
            else
            {
                calibrationButton.SetActive(false);
            }

       
            calibrateSphere.transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);


            //Hover (toggle) item on worldScroll with buttons (Do not delete!!!)
            //int childCountWorld = 0;
            //foreach (Transform childWorld in worldGridObject)
            //{
            //    childCountWorld++;            

            //    if (childCountWorld == (worldScroll.FirstVisibleCellIndex + hoverItemTierPage))
            //    {
            //        childWorld.GetComponent<Interactable>().IsToggled = true;
            //        if (armButtonsPressed)
            //            childWorld.GetComponent<Interactable>().SetInputDown();
            //        else
            //            childWorld.GetComponent<Interactable>().SetInputUp();
            //    }
            //    else
            //    {                    
            //        childWorld.GetComponent<Interactable>().IsToggled = false;
            //    }
            //}

            //Simulate Select button press on arm button press
            
          

            //foreach (Transform childArm in armGridObject)
            //{                             
            //    if (childArm.gameObject.GetComponent<Interactable>().HasPress)
            //    {
            //        buttonSelect.SetInputDown();
            //        randomizer.SelectTarget();
            //        Debug.Log("Armbutton pressed");
            //    }                   
            //    else
            //    {
            //        buttonSelect.SetInputUp();
            //    }                 

            //}

        

            if (toggleFeaturePanelVisibility)
            {
                toggleFeaturePanel.SetActive(true);
            }
            else
            {
                toggleFeaturePanel.SetActive(false);
            }


            IMixedRealityHandJointService handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
            if (handJointService != null)
              {


                //+++++++
                //Scroll Mechanics
                //+++++++  

                if (armScroll.IsEngaged)
                {
                    worldScroll.ApplyPosition(armScroll.workingScrollerPos);    
                }
                else if (worldScroll.IsEngaged)
                {     
                    armScroll.ApplyPosition(worldScroll.workingScrollerPos);   
                }

                //if (armScroll.IsEngaged && armScroll.TryGetPointerPositionOnPlane(out Vector3 currentPointerPos))
                //{
                //    worldScroll.ApplyPosition(armScroll.workingScrollerPos);
                //}
                //else if ((armScroll.CurrentVelocityState != ScrollingObjectCollection.VelocityState.None
                //      || armScroll.previousVelocityState != ScrollingObjectCollection.VelocityState.None)
                //      && armScroll.CurrentVelocityState != ScrollingObjectCollection.VelocityState.Animating) // Prevent the Animation coroutine from being overridden
                //{
                //    // We're not engaged, so handle any not touching behavior
                //    worldScroll.HandleVelocityFalloff();

                //    // Apply our position
                //    worldScroll.ApplyPosition(armScroll.workingScrollerPos);
                //}

                //if (worldScroll.IsEngaged)
                //{                    
                //    armScroll.ApplyPosition(worldScroll.workingScrollerPos);                    
                //}


                //ArticulatedHandDefinition bla = bla.IsPinching;

                if (handJointService != null)
                {
                    leftHandIsTracked = handJointService.IsHandTracked(Handedness.Left);
                    rightHandIsTracked = handJointService.IsHandTracked(Handedness.Left);

                    rightIndex.position = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right).position;
                    rightIndex.eulerAngles = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Right).eulerAngles;

                    rightThumb.position = handJointService.RequestJointTransform(TrackedHandJoint.ThumbMetacarpalJoint, Handedness.Right).position;
                    rightThumb.eulerAngles = handJointService.RequestJointTransform(TrackedHandJoint.ThumbMetacarpalJoint, Handedness.Right).eulerAngles;

                    rightWrist.position = handJointService.RequestJointTransform(TrackedHandJoint.Wrist, Handedness.Right).position;
                    rightWrist.eulerAngles = handJointService.RequestJointTransform(TrackedHandJoint.Wrist, Handedness.Right).eulerAngles;

                    leftIndex.position = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left).position;
                    leftIndex.eulerAngles = handJointService.RequestJointTransform(TrackedHandJoint.IndexTip, Handedness.Left).eulerAngles;

                    leftThumb.position = handJointService.RequestJointTransform(TrackedHandJoint.ThumbMetacarpalJoint, Handedness.Left).position;
                    leftThumb.eulerAngles = handJointService.RequestJointTransform(TrackedHandJoint.ThumbMetacarpalJoint, Handedness.Left).eulerAngles;

                    leftWrist.position = handJointService.RequestJointTransform(TrackedHandJoint.Wrist, Handedness.Left).position;
                    leftWrist.eulerAngles = handJointService.RequestJointTransform(TrackedHandJoint.Wrist, Handedness.Left).eulerAngles;
                }
       


                


                //Positioning UI
                uiOffset.localPosition = new Vector3(uiOffsetPositionZ, uiOffsetPositionY, uiOffsetPositionX);
                uiOffset.localScale = new Vector3(uiScale, uiScale, uiScale);

                if (menuIsLeft)
                {
                    thumb.position = leftThumb.position;
                    thumb.eulerAngles = leftThumb.eulerAngles;
                    wrist.position = leftWrist.position;
                    wrist.eulerAngles = leftWrist.eulerAngles;
                    uiOffset.localRotation = Quaternion.Euler(uiOffsetRotationX, -90, 0);
                    uiOffset.localPosition = new Vector3(uiOffsetPositionZ, uiOffsetPositionY, uiOffsetPositionX);
                    
                }
                else
                {
                    thumb.position = rightThumb.position;
                    thumb.eulerAngles = rightThumb.eulerAngles;
                    wrist.position = rightWrist.position;
                    wrist.eulerAngles = rightWrist.eulerAngles;
                    float normal = Mathf.Lerp(60, 140, Mathf.InverseLerp(105, 45, uiOffsetRotationX));
                    uiOffset.localRotation = Quaternion.Euler(normal, -90, 0);
                    uiOffset.localPosition = new Vector3(uiOffsetPositionZ, (float)(uiOffsetPositionY * 1.1), uiOffsetPositionX);
                    //if (!isPalmUp)
                    //{
                    //    menuToggle.transform.localPosition = new Vector3(-togglePositionX, (float)(togglePositionY * 1.1), togglePositionZ);
                    //    float normalToggle = Mathf.Lerp(60, 140, Mathf.InverseLerp(105, 45, toggleRotationParent.x));
                    //    menuToggle.transform.localEulerAngles = new Vector3(toggleRotationParent.x, toggleRotationParent.y, toggleRotationParent.z - 180);
                    //    menuToggle.transform.GetChild(0).transform.localEulerAngles = toggleRotation;
                    //}

                }

                if (!isPalmUp)
                {
                    menuToggle.transform.localPosition = new Vector3(togglePositionX, togglePositionY, togglePositionZ);
                    menuToggle.transform.localEulerAngles = toggleRotationParent;
                    menuToggle.transform.GetChild(0).transform.localEulerAngles = toggleRotation;
                }

                elbow.LookAt(wrist);

                if (armSliderHandler.isTic)
                {
                    scrollBackplateQuad.material.SetFloat("_BorderWidth", 0.4f);
                }
                else
                {
                    scrollBackplateQuad.material.SetFloat("_BorderWidth", 0.2f);
                }


                if (armScroll.IsEngaged || worldScroll.IsEngaged)
                {
                    scrollIconUpText.color = scrollArowIsDraggingColor;
                    scrollIconDownText.color = scrollArowIsDraggingColor;
                    scrollBackplateWorldQuad.material.SetColor("_Color", hoverBackplateColor);
                    scrollIconUpText2.color = scrollArowIsDraggingColor;
                    scrollIconDownText2.color = scrollArowIsDraggingColor;
                    scrollBackplateWorldQuad.material.SetColor("_Color", hoverBackplateColor);
                } else
                {
                    scrollIconUpText.color = scrollArowIdleColor;
                    scrollIconDownText.color = scrollArowIdleColor;
                    //scrollBackplateArmQuad.material.SetColor("_Color", idleBackplateColor);
                    scrollIconUpText2.color = scrollArowIdleColor;
                    scrollIconDownText2.color = scrollArowIdleColor;
                    scrollBackplateWorldQuad.material.SetColor("_Color", idleBackplateColor);
                }

                if (isSwitchToggled)
                {                  
                    
                    scrollMenu.SetActive(true);
                    mainMenuLeft.SetActive(false);
                    mainMenuRight.SetActive(false);
                    circularButtonInteractable.IsToggled = true;
                }
                else
                {
                    scrollMenu.SetActive(false);

                    if (menuIsLeft)
                    {
                        mainMenuLeft.SetActive(true);
                        mainMenuRight.SetActive(false);
                    }else
                    {
                        mainMenuLeft.SetActive(false);
                        mainMenuRight.SetActive(true);
                    }
                       

                    circularButtonInteractable.IsToggled = false;
                }
                if (!isCalibrating)
                {
                    if (leftIndexInCalibrationZone || rightIndexInCalibrationZone)
                    {
                        calibrateSphere.GetComponent<MeshRenderer>().material.SetColor("_RimColor", hoverColor);
                    }
                    else
                    {
                        calibrateSphere.GetComponent<MeshRenderer>().material.SetColor("_RimColor", defaultColor);
                    }
                }
                

                //+++++++
                //Debug Area
                //+++++++



                    if (debug)
                {
                    uiOffset.gameObject.SetActive(false);
                    calibrateSphere.GetComponent<MeshRenderer>().enabled = true;

                    FlipToggleScale(false);

                    foreach (GameObject cube in debugCubes)
                    {
                        cube.GetComponent<MeshRenderer>().enabled = true;
                        //cube.transform.localScale = new Vector3(debugCubeScale, debugCubeScale, debugCubeScale);
                    }
                    //Debug.Log("rightIndex: " + rightIndex);
                    //Debug.Log("leftWrist: " + leftWrist);
                    //Debug.Log(" RPos: " + RightHandPos.ToString()+" RTRig: "+RightHandTrigger.ToString());
                    //Debug.Log(" LPos: " + LeftHandPos.ToString() + " LTRig: " + LeftHandTrigger.ToString());
                    //Debug.Log(" EPos: " + EyePos.ToString() + " ERot: " + EyeRotation.ToString() + " T: " + EyePos.y.ToString());
                }
                else
                {
                    foreach (GameObject cube in debugCubes)
                    {
                        cube.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }
            else
                Debug.LogError("Couldn't find OVRHandPrefab_Left or OVRHandPrefab_Right. Please connect your Headset");
        }
        

        public void ToggleSwitch()
        {
            if (pinchSliderSwitch.SliderValue < 1)
            {
                isSwitchToggled = false;
                radialViewAnchor.IsAppActive(false);
            }
            else if (pinchSliderSwitch.SliderValue > 0)
            {
                isSwitchToggled = true;
                radialViewAnchor.IsAppActive(true);
            }
        }

        
        public void ToggleRightIsDominant()
        {
            rightIsDominant = !rightIsDominant;

            if (rightIsDominant)
            {
                calibrateSphere.transform.position = new Vector3(calibrateSpherePosXLeft, calibrateSphere.transform.position.y, calibrateSphere.transform.position.z);
            }
            else
            {
                calibrateSphere.transform.position = new Vector3(calibrateSpherePosXRight, calibrateSphere.transform.position.y, calibrateSphere.transform.position.z);
            }
            RecalibrateArm();
        }

        public void IsArmButtonsPressed(bool isPressed)
        {
            armButtonsPressed = isPressed;
        }
 

        public void FlipToggleScale(bool isUp)
        {
            isPalmUp = isUp;

            if (isPalmUp)
            {
                menuToggle.transform.localScale = new Vector3(1, 1, 1);
                menuToggle.transform.parent.transform.localScale = new Vector3(1, 1, 1);
                menuToggle.transform.GetChild(0).transform.localPosition = new Vector3(togglePositionPalmUpX, togglePositionPalmUpY, togglePositionPalmUpZ);
                menuToggle.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, 0, 0);
            }

            if (!isPalmUp)
            {
                menuToggle.transform.localScale = new Vector3(1, 1, 1);
                menuToggle.transform.parent.transform.localScale = new Vector3(1, 1, 1);
                menuToggle.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);

            }

            //Debug.Log("toggleScale: " + toggleScale);
        }

        public void IndexInCalibrationZone(bool menuIsLeft, bool isRight)
        {
            if (leftIndexInCalibrationZone && isRight)
            {
                rightIndexInCalibrationZone = isRight;
            }else if (rightIndexInCalibrationZone && menuIsLeft)
            {
                leftIndexInCalibrationZone = menuIsLeft;
            }else {
                leftIndexInCalibrationZone = menuIsLeft;
                rightIndexInCalibrationZone = isRight;
            }        
        }


        public void ScrollByTier(int amount)
        {
            worldScroll.MoveToIndex(amount, true);
        }
        //public void ToggleDebugSwitch()
        //{
        //    if (pinchSliderSwitch.SliderValue < 1)
        //    {

        //        debug = false;
        //    }
        //    else if (pinchSliderSwitch.SliderValue > 0)
        //    {

        //        debug = true;
        //    }
        //}

        //public void ToggleDebugSpeech()
        //{
        //    if (debug)
        //    {
        //        pinchSliderSwitch.SliderValue = 0;
        //        debug = false;
        //    }
        //    else
        //    {
        //        pinchSliderSwitch.SliderValue = 1;
        //        debug = true;
        //    }
        //}

        public void WriteInDebug(string log)
        {
            Debug.Log(log);
        }



        //Transform GetChildWithName(Transform obj, string name)
        //{
        //    Transform trans = obj.transform;
        //    Transform childTrans = trans.Find(name);
        //    if (childTrans != null)
        //    {
        //        return childTrans.transform;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //disable Arm and enable calibration Zone
        public void RecalibrateArm()
        {
            uiOffset.gameObject.SetActive(false);
            //menuToggle.SetActive(false);
            //sphereSize = sphereSizeIdle;
        }


        public void ActivateMenus()
        {
            uiOffset.gameObject.SetActive(true);
            //menuToggle.SetActive(true);
            circularButtonInteractable.IsToggled = true;
            radialViewAnchor.IsAppActive(true);
            //sphereSize = sphereSizeCalibrate;
            toggleFeaturePanelVisibility = true;
        }

        public void StartCalibration()
        {
            StartCoroutine(CalibrateArms());
        }

        IEnumerator CalibrateArms()
        {         

            ActivateMenus();
            toggleFeaturePanel.GetComponent<FeaturesPanelKeyboard>().ToggleOptions(2);
            isCalibrating = true;
            if (isCalibrating)
            {
                elbow.position = calibrateSphere.transform.position;
                calibrateSphere.GetComponent<MeshRenderer>().material.SetColor("_RimColor", calibrationColor);
                yield return new WaitForSeconds(0.5f);
                isCalibrating = false;
                readyForCalibration = false;
            }
            toggleFeaturePanel.GetComponent<FeaturesPanelKeyboard>().ToggleOptions(2);

        }
    }
}