using System;
using UnityEngine;
using UnityEngine.Events;

public class collisionWithObject : MonoBehaviour
{
    public collisionDetection OnCollision = new collisionDetection();

    void OnCollisionEnter(Collision collision)
    {
        OnCollision.Invoke(this);
    }

    [Serializable]
    public class collisionDetection : UnityEvent<collisionWithObject> { }

}
