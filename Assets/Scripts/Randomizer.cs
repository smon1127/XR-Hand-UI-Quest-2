namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
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
        public ScrollingObjectCollection scrollObjectCollectionPanel;
        public ScrollingObjectCollection scrollObjectCollection;
        public ScrollingObjectCollection scrollObjectCollectionArm;
        public Vector3 currentScrollPosition = new Vector3();
        public Vector3 targetScrollPosition = new Vector3();
        public EvaluationTimer evaluationTimer;
        public MeshRenderer selectBoxQuadMesh;
        public Color selectionCorrectColor = new Vector4(0, 0, 0, 1f);
        public Color selectionIncorrectColor = new Vector4(0, 0, 0, 1f);
        private Color selectionIdleColor = new Vector4();
        public TextMeshPro selectText;
        public float targetThreshold = .1f;
        private string selectTextIdle = "";
        public float tempTargetOffset = 0.8f;
        public int currentScrollIndex;
        public int currentTargetIndex;
        public float itemScaleOnTic = 1.5f;
        public float itemScale = 1;
        public float itemScaleFallOff = .7f;
        public bool shuffle = false;
        public GameObject buttonSelectIdle = null;
        public GameObject buttonSelectShuffle = null;
        public ArmSliderHandler armSliderHandler = null;

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

            Debug.Log("c.index: " + currentScrollIndex + "c.scroll: " + currentScrollPosition + "t.scroll: " + targetScrollPosition);

            if (Input.GetKeyDown(KeyCode.Space))
                Shuffle();

            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                MoveToTargetIndex();
            }

            if (scrollObjectCollection.IsDragging)
            {
                currentScrollPosition = scrollObjectCollection.workingScrollerPos;
                scrollObjectCollectionArm.ApplyPosition(currentScrollPosition);

            }
            else if (scrollObjectCollectionArm.IsDragging)
            {
                currentScrollPosition = scrollObjectCollectionArm.workingScrollerPos;
                scrollObjectCollection.ApplyPosition(currentScrollPosition);
            }

            //if (scrollObjectCollectionArm.IsEngaged)
            //{
            //    scrollObjectCollection.ApplyPosition(scrollObjectCollectionArm.workingScrollerPos);
            //}
            //else if (scrollObjectCollection.IsEngaged)
            //{
            //    scrollObjectCollectionArm.ApplyPosition(scrollObjectCollection.workingScrollerPos);
            //}

            currentScrollIndex = (int)(currentScrollPosition.y/scrollObjectCollection.CellHeight);
        }

            public void MoveToTargetIndex()
        {            
                scrollObjectCollection.MoveToIndex(currentTargetIndex, true);          
                
            
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
                targetObject[i].localPosition = new Vector3(0, 0, 0);


                tempGoPanel = targetObjectPanel[rnd];
                targetObjectPanel[rnd] = targetObjectPanel[i];
                targetObjectPanel[i] = tempGoPanel;

                targetObjectPanel[i].SetParent(targetObjectParentPanel[i]);
                targetObjectPanel[i].localPosition = new Vector3(0, 0, 0);
            }
            //Define targetObject randomized of last third
            currentTargetIndex = Random.Range(targetObjectParent.Count / 3 * 2, targetObjectParent.Count);

            //Scroll to y-Position with targetObject in Panel
            float cellHeight = gridObjectCollectionPanel.GetComponent<GridObjectCollection>().CellHeight;
            targetScrollPosition = new Vector3(0,currentTargetIndex* cellHeight,0);
            scrollObjectCollectionPanel.MoveToIndex(currentTargetIndex, true);
        }

        public void SelectTarget()
        {

            if (shuffle)
            {
                Shuffle();
                shuffle = false;
                scrollObjectCollection.MoveToIndex(0, true);
                scrollObjectCollectionArm.MoveToIndex(0, true);
            }
            else
            {
                if (currentScrollPosition.y > (targetScrollPosition.y - targetThreshold) && currentScrollPosition.y < (targetScrollPosition.y + targetThreshold))
                {
                    evaluationTimer.StopTimer();                    
                    StartCoroutine(VisualFeedbackTargetSelection(true));
                    
                }
                else
                {

                    StartCoroutine(VisualFeedbackTargetSelection(false));
                }
            }
        }
            
            
            
        

        IEnumerator VisualFeedbackTargetSelection(bool isRight)
        {
            
            if (isRight)
            {
                selectText.text = "Right";
                selectBoxQuadMesh.material.SetColor("_Color", selectionCorrectColor);
                yield return new WaitForSeconds(0.5f);
                shuffle = true;
                yield return new WaitForSeconds(.01f);

            }
            else
            {
                selectText.text = "Wrong";
                selectBoxQuadMesh.material.SetColor("_Color", selectionIncorrectColor);
                yield return new WaitForSeconds(0.5f);
            }
            
            selectText.text = selectTextIdle;
            selectBoxQuadMesh.material.SetColor("_Color", selectionIdleColor);

        }
    }
}