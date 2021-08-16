using System;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardEvents : MonoBehaviour
{
    private KeyboardUnityEvent SpaceBar = new KeyboardUnityEvent();
    public bool switchToggle = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            SpaceBar.Invoke(this);
            SwitchToggle();
        }
    }

    public void SwitchToggle()
    {
        switchToggle = !switchToggle;
    }
}
    [Serializable]
    public class KeyboardUnityEvent : UnityEvent<KeyboardEvents> { }

