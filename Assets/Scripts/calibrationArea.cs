
namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    using UnityEngine;
    using Microsoft.MixedReality.Toolkit.UI;

    public class calibrationArea : MonoBehaviour
    {

        public ArmUiHandler handler;
        private Rigidbody calibRb = null;

        // Start is called before the first frame update
        void Start()
        {            
            if (calibRb == null)
            {
                calibRb = gameObject.AddComponent<Rigidbody>();
                calibRb.isKinematic = true;
            }

        }


 

        private void OnTriggerEnter(Collider other)
        {
         
            if (other.CompareTag("leftIndex"))
            {
                //Debug.Log("Index enter Zone");
                handler.IndexInCalibrationZone(true, false);
            }
            if (other.CompareTag("rightIndex"))
            {
                //Debug.Log("Index enter Zone");
                handler.IndexInCalibrationZone(false, true);
            }

            if (other.CompareTag("rightIndex") && other.CompareTag("leftIndex"))
            {
                //Debug.Log("Index enter Zone");
                handler.IndexInCalibrationZone(true, true);
            }

        }


        private void OnTriggerExit(Collider other)
        {
           
            if (other.CompareTag("leftIndex"))
            {
                //Debug.Log("Index enter Zone");
                handler.IndexInCalibrationZone(false, false);
            }
            if (other.CompareTag("rightIndex"))
            {
                //Debug.Log("Index enter Zone");
                handler.IndexInCalibrationZone(false, false);
            }
        }


    }
}