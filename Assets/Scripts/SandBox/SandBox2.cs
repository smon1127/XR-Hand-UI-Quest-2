using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    public class SandBox2 : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            AudioSource sc = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}