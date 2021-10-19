using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//(2018) Made by Tyler Wargo

public class ArduinoOutput : MonoBehaviour
{
	//Acces Serial Script
	public ArduinoSerialConnect arduinoSerialConnect;

	public void Start()
	{
		
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1)){
			arduinoSerialConnect.serial.Write("A");
		}

		if (Input.GetKeyDown(KeyCode.Keypad2))
		{
			arduinoSerialConnect.serial.Write("B");
		}
	}
}