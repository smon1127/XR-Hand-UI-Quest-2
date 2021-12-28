
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class ThimbelTest : MonoBehaviour
{    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log("button 1: " + Input.GetKey(KeyCode.JoystickButton1) + ", Axis 1: " + Input.GetAxis("AXIS_1"));
            Debug.Log("button 2: " + Input.GetKey(KeyCode.JoystickButton2) + ", Axis 2: " + Input.GetAxis("AXIS_2"));           

        }
    }
}
