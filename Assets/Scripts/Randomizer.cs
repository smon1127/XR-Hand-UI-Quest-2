namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    using Microsoft.MixedReality.Toolkit.UI;
    using System.Collections.Generic;
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

        public int currentTargetCount;

        void Start()
        {
            DeclareObjectOrder(gridObjectCollection, targetObjectParent, targetObject);
            DeclareObjectOrder(gridObjectCollectionPanel, targetObjectParentPanel, targetObjectPanel);
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
                targetObject[i].localPosition = new Vector3(0, 0, 0);


                tempGoPanel = targetObjectPanel[rnd];
                targetObjectPanel[rnd] = targetObjectPanel[i];
                targetObjectPanel[i] = tempGoPanel;

                targetObjectPanel[i].SetParent(targetObjectParentPanel[i]);
                targetObjectPanel[i].localPosition = new Vector3(0, 0, 0);
            }
            //Define targetObject randomized of last third
            currentTargetCount = Random.Range(targetObjectParent.Count / 3 * 2, targetObjectParent.Count);

            //Scroll to y-Position with targetObject in Panel
            float cellHeight = scrollObjectCollectionPanel.CellHeight;
            Vector3 targetScrollPosition = new Vector3(0,currentTargetCount* cellHeight,0);
            scrollObjectCollectionPanel.ApplyPosition(targetScrollPosition);
        }
    }
}