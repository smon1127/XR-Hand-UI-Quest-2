namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    using UnityEngine;

    public class SandBox : MonoBehaviour
    {

        //ArmUiHandler p;


        public Transform armContainer;
        [Header("Audio Clips")]
        [SerializeField]
        [Tooltip("Sound to play when interaction with slider starts")]
        public AudioClip interactionStartSound = null;
        [SerializeField]
        [Tooltip("Sound to play when interaction with slider ends")]
        public AudioClip interactionEndSound = null;

        [Header("Tick Notch Sounds")]

        [SerializeField]
        [Tooltip("Whether to play 'tick tick' sounds as the slider passes notches")]
        private bool playTickSounds = true;

        [SerializeField]
        [Tooltip("Sound to play when slider passes a notch")]
        private AudioClip passNotchSound = null;

        [Range(0, 1)]
        [SerializeField]
        private float tickEvery = 0.34f;

        [SerializeField]
        private float startPitch = 0.75f;

        [SerializeField]
        private float endPitch = 1.25f;

        [SerializeField]
        private float minSecondsBetweenTicks = 0.01f;


        // Play sound when passing through slider notches
        public float accumulatedDeltaSliderValue = 0;
        private float lastSoundPlayTime;

        private AudioSource passNotchAudioSource = null;

        private HapticSound haptic;
        private float hapticPitch;
        public int hapticPattern = 0;

        public Transform target;
        public bool touch = false;
        Vector3 startPosition;
        Vector3 normalizeDirection;

        [Range(0.0f, 2.0f)]
        public float speed = .01f;

        public int tic = 0;

        

        // Start is called before the first frame update
        void Start()
        {
            //float aValue = 130;
            //float normal = Mathf.InverseLerp(100, 150, aValue);
            //float bValue = Mathf.Lerp(.5f, 1, normal);
            //string path = @"D:\Unity\Unity Frameworks\MRTK\XR Handmenus\Assets\InputRecordings" + "inputhands";
            //p = gameObject.GetComponent<ArmUiHandler>();

            if (passNotchAudioSource == null)
            {
                passNotchAudioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            }

            startPosition = armContainer.position;
            normalizeDirection = (target.position - transform.position).normalized;
            //Debug.Log("path: " + path);


            haptic = GameObject.FindGameObjectWithTag("SyntactsHub").GetComponent<HapticSound>();
            hapticPitch = haptic.customHapticDesign[hapticPattern].x;

        }

        // Update is called once per frame
        void Update()
        {
            //p.ToggleIsSliding(true);

            //Distance between two Objects  
            //float dist = Vector3.Distance(other.localPosition, transform.localPosition);

            //Move Object via time
            



            armContainer.position += normalizeDirection * speed * Time.deltaTime;
            
                

            //MoveObject via Slider
            //armContainer.position = new Vector3(0,0,speed);

            if (touch)
            {
                
                float delta = armContainer.position.z - startPosition.z;
                accumulatedDeltaSliderValue += Mathf.Abs(delta);
                float now = Time.timeSinceLevelLoad;              

                if (accumulatedDeltaSliderValue > tickEvery && now - lastSoundPlayTime > minSecondsBetweenTicks)
                {
                    Debug.Log("startPosition.z" + startPosition.z + " armContainer.position.z: " + armContainer.position.z);
                    //Debug.Log("Tick");

                    passNotchAudioSource.pitch = Mathf.Lerp(startPitch, endPitch, armContainer.position.z);
                    hapticPitch = Mathf.Lerp(haptic.scrollLowPitch, haptic.scrollHighPitch, armContainer.position.z);
                    

                    if (passNotchAudioSource.isActiveAndEnabled)
                    {
                        tic++;
                        accumulatedDeltaSliderValue = 0;
                        lastSoundPlayTime = now;
                        passNotchAudioSource.PlayOneShot(passNotchSound);
                        haptic.PressKeyHapticSample(hapticPattern);
                        haptic.UpdatePitch(hapticPattern, hapticPitch);
                    }
                    
                }
                    
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
                speed = -speed;

            if (Input.GetKeyDown("space"))
            {
                if (touch)
                {
                    SetTouch(false);
                }
                else { 
                    SetContainerPositionOnTouch();
                    SetTouch(true);
                }
            }


       
            
        }

        public void SetContainerPositionOnTouch() {
            startPosition = armContainer.position;
            Debug.Log("startPosition.z: " + startPosition.z);
        }

        public void SetTouch(bool isTouch)
        {
            touch = isTouch;
        }






    }
}