using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneLoadCameraReset
{
    public int sceneIndex;
    public Vector3 startPosition;
    public float startYRotation;
}

public class SetTrackingType : MonoBehaviour
{
    [SerializeField] SceneLoadCameraReset[] sceneLoadOptions;

    Transform _OVRCameraRig;
    Transform _centreEyeAnchor;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ResetCameraOnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ResetCameraOnSceneLoad;
    }

    private void Awake()
    {
        //XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
    }

    //Helper function to find the correct instances of OVRCameraRig and CentreEyeAnchor
    void FindOVRCameraRig()
    {
        OVRCameraRig ovr = FindObjectOfType<OVRCameraRig>();

        if (ovr)
        {
            _OVRCameraRig = ovr.transform;
            _centreEyeAnchor = ovr.centerEyeAnchor;
        }
        else
        {
            Debug.Log("No OVRCameraRig object found");
        }
    }


    //Calls ResetCamera based on the current scene which was just loaded
    void ResetCameraOnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        FindOVRCameraRig();

        for (int i = 0; i < sceneLoadOptions.Length; i++)
        {
            if (scene.buildIndex == sceneLoadOptions[i].sceneIndex)
            {
                StartCoroutine(ResetCamera(sceneLoadOptions[i].startPosition, sceneLoadOptions[i].startYRotation));
            }
        }
    }

    //Resets the OVRCameraRig's position and Y-axis rotation to help align the player's starting position and view to the target parameters
    IEnumerator ResetCamera(Vector3 targetPosition, float targetYRotation)
    {
        //EditorDebugOffset();

        yield return new WaitForEndOfFrame();

        float currentRotY = _centreEyeAnchor.eulerAngles.y;
        float difference = targetYRotation - currentRotY;
        _OVRCameraRig.Rotate(0, difference, 0);

        Vector3 newPos = new Vector3(targetPosition.x - _centreEyeAnchor.position.x, 0, targetPosition.z - _centreEyeAnchor.position.z);
        _OVRCameraRig.transform.position += newPos;
    }
}