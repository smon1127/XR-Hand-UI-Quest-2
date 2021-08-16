using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticTicCollider : MonoBehaviour
{
    Transform scrollObjectPosition;
    public Transform scrollObjectCollection;
    public Transform colliderScaleReference;
    private GameObject hapticTicCollider;


    // Start is called before the first frame update
    void Start()
    {
        hapticTicCollider = GameObject.FindGameObjectWithTag("hapticTicCollider");
        hapticTicCollider.transform.localScale = colliderScaleReference.localScale;
        scrollObjectPosition = hapticTicCollider.transform.parent.transform;
        hapticTicCollider.transform.SetParent(scrollObjectCollection);                        
        
    }

    // Update is called once per frame
    void Update()
    {
        hapticTicCollider.transform.position = scrollObjectPosition.position;
    }
}
