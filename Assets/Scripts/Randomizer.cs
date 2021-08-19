using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    public List<Transform> targetObjectParent = new List<Transform>();
    public List<Transform> targetObject = new List<Transform>();
    private Transform tempGO;
    public Transform gridObjectCollection;
    private Transform tempGo;

    void Start()
    {        
        int childCount = 0;
        foreach (Transform child in gridObjectCollection)
        {
            targetObjectParent[childCount] = child;
            targetObject[childCount] = child.GetChild(0);
            childCount++;
        }  
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Shuffle();
    }

 

    public void Shuffle()
    {
        for (int i = 0; i < targetObject.Count; i++)
        {
            int rnd = Random.Range(i, targetObject.Count);
            tempGo = targetObject[rnd];
            targetObject[rnd] = targetObject[i];
            targetObject[i] = tempGo;
            targetObject[i].SetParent(targetObjectParent[i]);
            targetObject[i].localPosition = new Vector3(0,0,0);
        }
    }
}
