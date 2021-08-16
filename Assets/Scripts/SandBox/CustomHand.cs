using System;
using UnityEngine;
using UnityEngine.Events;

public class CustomHand : MonoBehaviour
{
    public FingerPinch OnIndexPinch = new FingerPinch();
    public FingerPinch OnMiddlePinch = new FingerPinch();

    public OVRHand rightHand { get; private set; } = null;
    public OVRHand leftHand { get; private set; } = null;
    public bool isLeftHand = true;

    private void Awake()
    {
        
        rightHand = GameObject.Find("OVRHandPrefab_Right").GetComponent<OVRHand>();
        leftHand = GameObject.Find("OVRHandPrefab_Left").GetComponent<OVRHand>();

    }

    private void Update()
    {
        if (isLeftHand)
            FingerEvents(leftHand);
        else
            FingerEvents(rightHand);

    }

    public void FingerEvents(OVRHand handedness)
    {
        if (handedness.IsSystemGestureInProgress)
            return;

        if (handedness.GetFingerIsPinching(OVRHand.HandFinger.Index))
            OnIndexPinch.Invoke(this);

        if (handedness.GetFingerIsPinching(OVRHand.HandFinger.Middle))
            OnMiddlePinch.Invoke(this);
    }

    [Serializable]
    public class FingerPinch : UnityEvent<CustomHand> { }
}