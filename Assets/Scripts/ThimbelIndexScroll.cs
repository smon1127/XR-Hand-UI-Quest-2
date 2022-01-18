using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThimbelIndexScroll : InputSystemGlobalHandlerListener, IMixedRealityInputHandler
{

    Transform indexMiddleJoint = null;
    //Transform referenceObject;
    Transform leftThumbTip = null;

    [Range(-0.07f, 0.07f)]
    public float rootPos = 0.0f;

    Vector3 prevPos = new Vector3(0,0,0);


    public Transform sliderUI;
    public Transform sliderKnob;

    public bool travelMode = false;
    public bool resetRoot = false;
    bool isFirstTouch = true;

    
    public float travaledDistance = 0f;

    [Range(0f, 2f)]
    public float speed = 1f;
    

    // Start is called before the first frame update
    void Start()
    {
        //indexMiddleJoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //indexMiddleJoint.SetActive(false);

        //referenceObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //leftThumbTip = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //indexMiddleJoint.transform.position = new Vector3(0, 0, 0);
        //indexMiddleJoint.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        //referenceObject.transform.position = new Vector3(0, 0, 0);
        //referenceObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        //leftThumbTip.transform.position = new Vector3(0, 0, 0);
        //leftThumbTip.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        IMixedRealityHandJointService handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        if (handJointService != null)
        {
            if (handJointService != null)
            {
                leftThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Left);
                indexMiddleJoint = handJointService.RequestJointTransform(TrackedHandJoint.IndexMiddleJoint, Handedness.Left);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Get hand Element Rotations and Positions

        IMixedRealityHandJointService handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        if (handJointService != null)
        {
            if (handJointService != null)
            {
                leftThumbTip = handJointService.RequestJointTransform(TrackedHandJoint.ThumbTip, Handedness.Left);
                indexMiddleJoint = handJointService.RequestJointTransform(TrackedHandJoint.IndexMiddleJoint, Handedness.Left);
            }
        }

        //referenceObject.transform.localPosition = indexMiddleJoint.transform.position + new Vector3(leftThumbTip.transform.position.x+offset, 0, 0);

        sliderUI.position = indexMiddleJoint.position;
        sliderKnob.localPosition = new Vector3(travaledDistance* speed, 0, 0);
    }

    private void FixedUpdate()
    {
        if (travelMode)
        {
            sliderUI.gameObject.SetActive(true);
            if (isFirstTouch)
            {
                prevPos = leftThumbTip.transform.localPosition;
                isFirstTouch = false;                
            }

            
                travaledDistance = Mathf.Lerp(prevPos.x, leftThumbTip.transform.localPosition.x, 1);


            //Debug.Log("prevPos: " + prevPos.x + " thPos: " + leftThumbTip.transform.localPosition.x + " trDis: " + travaledDistance);

        }
        else
        {
            sliderUI.gameObject.SetActive(false);
        }
    }

  
    public void OnInputUp(InputEventData eventData)
    {
        isFirstTouch = true;
        
        if(resetRoot) travaledDistance = rootPos;
        if (eventData.InputSource.SourceName == "GenericJoystickController Controller")
            travelMode = false;

        
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (eventData.InputSource.SourceName == "GenericJoystickController Controller")
        {
            travelMode = true;
            
        }
            

        

    }

    protected override void RegisterHandlers()
    {
        CoreServices.InputSystem.RegisterHandler<IMixedRealityInputHandler>(this);
    }

    protected override void UnregisterHandlers()
    {

    }
}
