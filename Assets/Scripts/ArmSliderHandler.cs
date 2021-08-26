namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    using Microsoft.MixedReality.Toolkit.UI;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class ArmSliderHandler : MonoBehaviour
    {
        public ArmUiHandler armUiHandler;
        Rigidbody bodyCollide = null;

        private float lastSoundPlayTime;

        //[SerializeField]
        //private float startPitch = 0.75f;

        //[SerializeField]
        //private float endPitch = 1.25f;



        public HapticSound hapticSound;
        private float hapticPitch;

        public RectTransform scrollbarReferenceRect;
        public RectTransform scrollbarVerticalRect;
        private float scrollbarVerticalRectIdle;
        public float scrollbarHoverWidth;
        private float scrollbarOffsetX;
        private float scrollbarOffsetXIdle;

        private GameObject scrollingGameObject;
        public Transform localGridObjectCollection = null;
        public Transform referenceGridObjectCollection = null;
        private ScrollingObjectCollection localScroll;
        private ScrollingObjectCollection referenceScroll;
        public Transform colliderScaleReference;
        public bool isTic = false;
        public List<GameObject> hapticTicCollider = new List<GameObject>();
        public List<Transform> scrollObjectPosition;
        string hapticTicColliderTag = "hapticTicCollider";
        string quadTag = "quad";
        

        int armSelectionCount = 0;
        int sliderSize;
        int tiersPerPage;
        float pageCellHeight;
        float pageCellWidth;
        public float sliderHeight;
        float absolutSliderHeight;


        
        //InteractableProfileItem profileItem;
        //public GameObject frontPlateTarget = null;
        //public List<Theme> frontPlateTheme = null;

        // Start is called before the first frame update
        

        private void Start()
        {
            
            //profileItem.Target = frontPlateTarget;
            //profileItem.Themes = frontPlateTheme;

            scrollingGameObject = gameObject.transform.parent.gameObject;
            //IsHover(false);
            localScroll = scrollingGameObject.GetComponent<ScrollingObjectCollection>();
            referenceScroll = referenceGridObjectCollection.transform.parent.transform.parent.gameObject.GetComponent<ScrollingObjectCollection>();
            localScroll.MaskEnabled = true;

            

            sliderSize = localGridObjectCollection.transform.childCount;
            tiersPerPage = localScroll.TiersPerPage;
            pageCellHeight = localScroll.CellHeight;
            pageCellWidth = localScroll.CellWidth;
            sliderHeight = (sliderSize * pageCellHeight) - (tiersPerPage * pageCellHeight);
            absolutSliderHeight = sliderSize * pageCellHeight;

            
            scrollbarVerticalRectIdle = scrollbarVerticalRect.rect.width;
            scrollbarOffsetXIdle = scrollbarVerticalRect.anchoredPosition.x;
            scrollbarOffsetX = scrollbarOffsetXIdle - ((scrollbarHoverWidth - scrollbarVerticalRectIdle) / 2);
            scrollbarVerticalRect.anchoredPosition = new Vector3(scrollbarOffsetX, 0, 0);

            //yield return null;
            scrollbarReferenceRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, absolutSliderHeight * 1000);
            scrollbarReferenceRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pageCellWidth * 1000);
            
            scrollbarVerticalRect.gameObject.GetComponent<Scrollbar>().size = armUiHandler.scrollBarSize;

            //select each haptics tic collider from childs     

            foreach (Transform child in localGridObjectCollection)
            {
                foreach (Transform childOfChild in child)
                {
                    if (childOfChild.tag == hapticTicColliderTag)
                        hapticTicCollider.Add(childOfChild.gameObject);
                }
            }

            scrollObjectPosition = new List<Transform>(new Transform[hapticTicCollider.Count]);

            //Debug.Log("hapticTicCollider.Count: " + hapticTicCollider.Count);


            for (int i = 0; i < hapticTicCollider.Count; i++)
            {
                //set position and scale of local parent 
                hapticTicCollider[i].transform.localScale = colliderScaleReference.localScale;
                scrollObjectPosition[i] = hapticTicCollider[i].transform.parent.transform;

                //move afterwards to parent "scrollingObjectCollection" to avoid scroll script to disable collider
                hapticTicCollider[i].transform.SetParent(scrollingGameObject.transform);
            }

            //Set scale of haptic Tic Main Collider to size of reference
            gameObject.transform.localScale = colliderScaleReference.localScale;

            //Get position of first slider element
            Transform firstObject = localGridObjectCollection.GetChild(0);

            //Set position of collider to first childs position
            gameObject.transform.position = firstObject.position;

            //Offset position y of collider
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localScale.x/2, gameObject.transform.localScale.y / 2 + armUiHandler.hapticLatencyPositionOffset, 0);




            if (bodyCollide == null)
            {
                bodyCollide = gameObject.AddComponent<Rigidbody>();
                bodyCollide.isKinematic = true;
            }            

            
        }
       

        void Update()
        {

            scrollbarReferenceRect.localPosition = new Vector3(0, localScroll.ScrollContainerPosition.y * 1000, 0);

            if (localScroll.IsEngaged || referenceScroll.IsEngaged)
            {
                scrollbarVerticalRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollbarHoverWidth);
                scrollbarVerticalRect.anchoredPosition = new Vector3(scrollbarOffsetX, 0, 0);
            }
            else
            {
                scrollbarVerticalRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollbarVerticalRectIdle);
                scrollbarVerticalRect.anchoredPosition = new Vector3(scrollbarOffsetXIdle, 0, 0);
            }

            for (int i = 0; i < hapticTicCollider.Count; i++)
            {
                hapticTicCollider[i].transform.position = scrollObjectPosition[i].position;
            }
     
            OnTriggerEnter(gameObject.GetComponent<BoxCollider>());
        }


        //public void IsHover(bool hover)
        //{
        //    foreach (Transform childButton in localGridObjectCollection)
        //    {
        //        //Debug.Log("childs of : " + localGridObjectCollection);
        //        foreach (Transform childOfChildButton in childButton)
        //        {
        //            //Debug.Log("childs of : " + localGridObjectCollection);
        //            if (childOfChildButton.CompareTag(quadTag))
        //            {
        //                Debug.Log("Quad Found");

        //                if(hover)                          
        //                    childOfChildButton.GetComponent<MeshRenderer>().material.SetColor("_Color", hoverColor);
        //                else
        //                    childOfChildButton.GetComponent<MeshRenderer>().material.SetColor("_Color", defaultColor);

        //            }

        //        }
        //    }
        //}

        public void OnTriggerEnter(Collider other)
        {


            if (other.gameObject.tag == "hapticTicCollider")
            {
                PlayHapticSoundOnCollision();
                
                StartCoroutine(TicDuration());
                //Debug.Log("I'm here: Collision");
            }

        }

        IEnumerator TicDuration()
        {
            isTic = true;
            yield return new WaitForSeconds(.001f);
            isTic = false;
        }

        public void IsScrollStart(bool isStart)
        {
            if (isStart)
            {
                Debug.Assert(localScroll != null, "Scroll view needs to be defined before using jump to top or buttom.");
                localScroll.MoveToIndex(0);
            }
            else
            {
                localScroll.MoveToIndex(sliderSize);
            }

        } 

        public void PlayHapticSoundOnCollision()
        {
            
            float now = Time.timeSinceLevelLoad;
            if (localScroll.IsDragging && now - lastSoundPlayTime > hapticSound.minSecondsBetweenTicks)
            {
                //Debug.Log("I'm here: localScroll.IsDragging");

                if (hapticSound.playTickSounds)
                {

                    float normal = Mathf.Lerp(0, sliderHeight, localScroll.workingScrollerPos.y);

                    //Debug.Log("a: " + currentPosition + " s: " + sliderHeight + " normal:" + normal);

                    hapticPitch = Mathf.Lerp(hapticSound.scrollLowPitch, hapticSound.scrollHighPitch, normal);
                    hapticSound.PressKeyHapticSample(hapticSound.playHapticSoundPattern);
                    hapticSound.UpdatePitch(hapticSound.playHapticSoundPattern, hapticPitch);
                    //Debug.Log("I'm here: Passed Notch");                    
                }

                lastSoundPlayTime = now;
            }
        }
    }
}
   