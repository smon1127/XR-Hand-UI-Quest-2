using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHandler : MonoBehaviour
{
    public string PathRecording = "D:\'Unity\'Unity Frameworks\'MRTK\'XR Handmenus\'Assets\'InputRecordings";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setPath(string path)
    {
        path = PathRecording;
    }
   }
