using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class EvaluationTimer : MonoBehaviour
{
    public int userId = 0;
    public int iteration = 0;
    public int errorCount = 0;

    public DateTime startTime;
    public DateTime endTime;
    public DateTime firstScrollTime;
    public DateTime targetReachedTime;    
    public TimeSpan duration;
    public string data;

    public bool isTimer = false;
    public bool isMeasuring = false;
    public bool isAudioOn = false;
    public bool isHapticOn = false;
    


    public string OutputFileName { get; } = "XR_HandUI_Taskperformance";


    private void Start()
    {
        startTime = DateTime.Now;
        iteration = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown("space") && !isTimer)
        {
            StartTimer();
        }
        else if (Input.GetKeyDown("space") && isTimer)
        {
            StopTimer();
        }
       

    }

    public void SetUserId(int id)
    {
        userId = id;
    }

    public void StartTimer()
    {
        firstScrollTime = DateTime.Now;
        isTimer = true;
        iteration++;
    }

    public void StopTimer()
    {
        isTimer = false;
        targetReachedTime = DateTime.Now;
        duration = targetReachedTime - firstScrollTime;
        //Debug.Log($"{filename}: {data}");
    }

    void OnApplicationQuit()
    {
        endTime = DateTime.Now;
        WriteData();
    }

    public void WriteData()
    {
        data = String.Format("{0};{1};{2};{3};{4}:{5}:{6};{7};{8};{9};{10};{11};{12};", userId, iteration, firstScrollTime, targetReachedTime, duration.Hours.ToString(), duration.Minutes.ToString(), duration.Seconds.ToString(), isMeasuring.ToString(), isAudioOn.ToString(), isHapticOn.ToString(), errorCount, startTime, endTime);

        var filename = String.Format("{0}.csv", OutputFileName);
        string path = Path.Combine(Application.persistentDataPath, filename);
        TextWriter writer = new StreamWriter(path, true);
        writer.WriteLine(data);
        writer.Close();
    }
}
