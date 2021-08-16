using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.Events;
using UnityEngine.XR;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;



public class HeadRig : MonoBehaviour
{
    private InputDevice LeftController = new InputDevice();
    private InputDevice RightController = new InputDevice();
    private List<InputDevice> Devices = new List<InputDevice>();
    private Vector3 RightHandPos;
    private float RightHandTrigger;

    private Vector3 LeftHandPos;
    private float LeftHandTrigger;

    private Vector3 EyePos;
    private Quaternion EyeRotation;

    Transform elbow;
    Transform upperArm;
    Transform forearm;
    Transform hand;



    Vector3 fingerPointer;
    GameObject sphere;
    public GameObject skeleton;
    public bool debug = true;

    void trackingLost()
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

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        skeleton = GameObject.Find("OVRHandPrefab_Right");
        elbow = this.GetComponent<InverseKinematics>().elbow.transform;
        upperArm = this.GetComponent<InverseKinematics>().upperArm.transform;
        forearm = this.GetComponent<InverseKinematics>().forearm.transform;
        hand = this.GetComponent<InverseKinematics>().hand.transform;
    }

    // Update is called once per frame
    void Update()
    {
        InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.CenterEye, Devices);
        if (Devices.Count > 0)
        {
            Devices[Devices.Count - 1].TryGetFeatureValue(CommonUsages.centerEyePosition, out EyePos);
            Devices[Devices.Count - 1].TryGetFeatureValue(CommonUsages.centerEyeRotation, out EyeRotation);
        }


        InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, Devices);
        if (Devices.Count > 0)
        {
            Devices[Devices.Count - 1].TryGetFeatureValue(CommonUsages.devicePosition, out RightHandPos);
            Devices[Devices.Count - 1].TryGetFeatureValue(CommonUsages.trigger, out RightHandTrigger);
            if (RightHandTrigger > 0.01) Devices[Devices.Count - 1].SendHapticImpulse(0, RightHandTrigger, 0.05f);

        }

        InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, Devices);
        if (Devices.Count > 0)
        {
            Devices[Devices.Count - 1].TryGetFeatureValue(CommonUsages.devicePosition, out LeftHandPos);
            Devices[Devices.Count - 1].TryGetFeatureValue(CommonUsages.trigger, out LeftHandTrigger);
            if (LeftHandTrigger > 0.01) Devices[Devices.Count - 1].SendHapticImpulse(0, LeftHandTrigger, 0.05f);

        }
        
        

        // Add collider to tip of index finger
        foreach (OVRBone bone in skeleton.GetComponent<OVRSkeleton>().Bones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
            {
                fingerPointer = bone.Transform.position;
                sphere.transform.localScale = Vector3.one * 0.02f;
                if (debug)
                sphere.transform.position = fingerPointer;
            }
        }

        //Debug.Log(" RPos: " + RightHandPos.ToString()+" RTRig: "+RightHandTrigger.ToString());
        //Debug.Log(" LPos: " + LeftHandPos.ToString() + " LTRig: " + LeftHandTrigger.ToString());
        //Debug.Log(" EPos: " + EyePos.ToString() + " ERot: " + EyeRotation.ToString() + " T: " + EyePos.y.ToString());

        transform.position = EyePos;   
     

    }




    public void CalibrateIK(String armPart)
    {       
        if (armPart == "hand")
            hand.position = fingerPointer;

        if (armPart == "forearm")
            forearm.position = fingerPointer;

        if (armPart == "upperArm")
            upperArm.position = fingerPointer;

        if (armPart == "elbow")
            elbow.position = fingerPointer;
    }
}