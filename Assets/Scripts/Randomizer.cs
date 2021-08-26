namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    using Microsoft.MixedReality.Toolkit.Input;
    using Microsoft.MixedReality.Toolkit.UI;
    using Microsoft.MixedReality.Toolkit.Utilities;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class Randomizer : MonoBehaviour
    {

        public Transform gridObjectCollection;
        public List<Transform> targetObjectParent = new List<Transform>();
        public List<Transform> targetObject = new List<Transform>();
        private Transform tempGo;

        public Transform gridObjectCollectionPanel;        
        public List<Transform> targetObjectParentPanel = new List<Transform>();
        public List<Transform> targetObjectPanel = new List<Transform>();
        private Transform tempGoPanel;

        public ScrollingObjectCollection panelScroll;
        public ScrollingObjectCollection worldScroll;
        public ScrollingObjectCollection armScroll;

        public ArmUiHandler armUiHandler = null;
        public ArmSliderHandler armSliderHandler = null;
        public Vector3 targetScrollPosition = new Vector3();
        public int currentTargetIndex;
        public float tempTargetOffset = 0.8f;
        public float targetThreshold = .1f;

        public EvaluationTimer evaluationTimer;
        public MeshRenderer selectBoxQuadMesh;
        public Color selectionCorrectColor = new Vector4(0, 0, 0, 1f);
        public Color selectionIncorrectColor = new Vector4(0, 0, 0, 1f);
        private Color selectionIdleColor = new Vector4();
        public TextMeshPro selectText;        
        private string selectTextIdle = "";    

        public float itemScaleOnTic = 1.5f;
        public float itemScale = 1;
        public float itemScaleFallOff = .7f;
        public bool shuffle = false;
        public int isBeginningOnTop = 0;
        public GameObject buttonSelectIdle = null;
        public GameObject buttonSelectShuffle = null;
        


        void Start()
        {
            selectTextIdle = selectText.text;
            selectionIdleColor = selectBoxQuadMesh.material.color;
            DeclareObjectOrder(gridObjectCollection, targetObjectParent, targetObject);
            DeclareObjectOrder(gridObjectCollectionPanel, targetObjectParentPanel, targetObjectPanel);
            Shuffle();
        }

        public void DeclareObjectOrder(Transform _gridObjectCollection, List<Transform> _targetObjectParent, List<Transform> _targetObject)
        {
            int childCount = 0;
            foreach (Transform child in _gridObjectCollection)
            {             
                _targetObjectParent[childCount] = child;
                _targetObject[childCount] = child.GetChild(0);               
                childCount++;
            }
        }

        private void Update()
        {

            //int childCount = 0;
            //foreach (Transform child in gridObjectCollection)
            //{          
            //    //Do sth with current scroll object
            //    if (childCount == currentScrollIndex)
            //    {
                    
            //    }

            //}

            if (shuffle)
            {
                buttonSelectIdle.SetActive(false);
                buttonSelectShuffle.SetActive(true);
            }else
            {
                buttonSelectIdle.SetActive(true);
                buttonSelectShuffle.SetActive(false);
            }

        }

        public void MoveToTargetIndex()
        {            
            worldScroll.MoveToIndex(currentTargetIndex-1, true);        
        }

        public void Shuffle()
        {
            if (worldScroll.isActiveAndEnabled && panelScroll.isActiveAndEnabled)
            {


                for (int i = 0; i < targetObject.Count; i++)
                {
                    int rnd = Random.Range(i, targetObject.Count);
                    tempGo = targetObject[rnd];
                    targetObject[rnd] = targetObject[i];
                    targetObject[i] = tempGo;

                    targetObject[i].SetParent(targetObjectParent[i]);
                    targetObject[i].localPosition = new Vector3(0, 0, 0);


                    tempGoPanel = targetObjectPanel[rnd];
                    targetObjectPanel[rnd] = targetObjectPanel[i];
                    targetObjectPanel[i] = tempGoPanel;

                    targetObjectPanel[i].SetParent(targetObjectParentPanel[i]);
                    targetObjectPanel[i].localPosition = new Vector3(0, 0, 0);
                }

                isBeginningOnTop = Random.Range(0, 2);
                Debug.Log("targetObjectParent.Count: " + targetObjectParent.Count);

                if (isBeginningOnTop == 0)
                {
                    worldScroll.MoveToIndex(0, true);
                    armScroll.MoveToIndex(0, true);
                    //Define targetObject randomized of last third
                    currentTargetIndex = Random.Range(targetObjectParent.Count / 3 * 2, targetObjectParent.Count);
                }
                else
                {

                    worldScroll.MoveToIndex(targetObjectParent.Count, true);
                    armScroll.MoveToIndex(targetObjectParent.Count, true);
                    //Define targetObject randomized of first third
                    currentTargetIndex = Random.Range(1, targetObjectParent.Count / 3);
                }

                

                //Scroll to y-Position with targetObject in Panel

                targetScrollPosition = new Vector3(0, currentTargetIndex * armUiHandler.pageCellHeight, 0);
                panelScroll.MoveToIndex(currentTargetIndex, true);

                evaluationTimer.StopTimer();
                armUiHandler.draggingCount = 0;
                
            }
        }

        public void SelectTarget()
        {

            
                float currentScrollWithOffset = armUiHandler.currentScrollPosition.y + armUiHandler.pageCellHeight;
                float targetScrollWithOffset = targetScrollPosition.y;
                Debug.Log("currentScrollWithOffset: " + currentScrollWithOffset);
                if (currentScrollWithOffset > (targetScrollWithOffset - targetThreshold) && currentScrollWithOffset < (targetScrollWithOffset + targetThreshold))
                {
                    evaluationTimer.StopTimer();                    
                    StartCoroutine(VisualFeedbackTargetSelection(true));
                    
                }
                else
                {

                    StartCoroutine(VisualFeedbackTargetSelection(false));
                }
            
        }
            
            
            
        

        IEnumerator VisualFeedbackTargetSelection(bool isRight)
        {
            
            if (isRight)
            {
                selectText.text = "Right";
                selectBoxQuadMesh.material.SetFloat("_RimPower", .2f);
                selectBoxQuadMesh.material.SetColor("_RimColor", selectionCorrectColor);
                yield return new WaitForSeconds(0.5f);
                Shuffle();
                yield return new WaitForSeconds(.01f);

            }
            else
            {
                selectText.text = "Wrong";
                selectBoxQuadMesh.material.SetFloat("_RimPower", .2f);
                selectBoxQuadMesh.material.SetColor("_RimColor", selectionIncorrectColor);
                yield return new WaitForSeconds(0.5f);
            }
            
            selectText.text = selectTextIdle;
            selectBoxQuadMesh.material.SetFloat("_RimPower", 7.0f);

        }
    }
}