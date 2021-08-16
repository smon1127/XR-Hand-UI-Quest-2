namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    using Microsoft.MixedReality.Toolkit.UI;
    using UnityEngine;

    public class ArmSliderSoundValue : MonoBehaviour
    {
        ScrollingObjectCollection armContainer;

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

        [Range(0, 0.5f)]
        [SerializeField]
        private float tickEvery = 0.1f;

        [SerializeField]
        private float startPitch = 0.75f;

        [SerializeField]
        private float endPitch = 1.25f;

        [SerializeField]
        private float minSecondsBetweenTicks = 0.1f;


        // Play sound when passing through slider notches
        public float accumulatedDeltaSliderValue = 0;
        private float lastSoundPlayTime;
        public bool touch = false;
        private AudioSource passNotchAudioSource = null;

        private HapticSound haptic;
        private float hapticPitch;
        public int hapticPattern = 4;
        Vector3 startPosition;

        

        // Start is called before the first frame update
        void Start()
        {
            haptic = GameObject.FindGameObjectWithTag("SyntactsHub").GetComponent<HapticSound>();
            hapticPitch = haptic.customHapticDesign[hapticPattern].x;

            if (passNotchAudioSource == null)
            {
                passNotchAudioSource = gameObject.AddComponent<AudioSource>();
                if (haptic.soundChannel == 0)
                    passNotchAudioSource.panStereo = -1;

                if (haptic.soundChannel == 1)
                    passNotchAudioSource.panStereo = 1;
            }
            startPosition = armContainer.ScrollContainerPosition;

            
        }

        // Update is called once per frame
        void Update()
        {        

            if (armContainer.IsDragging)
            {
                if (playTickSounds && passNotchAudioSource != null && passNotchSound != null)
                {                   
                    float delta = armContainer.ScrollContainerPosition.y - startPosition.y;
                    accumulatedDeltaSliderValue += Mathf.Abs(delta);
                    float now = Time.timeSinceLevelLoad;
                    Debug.Log("startPosition.y" + startPosition.y + " armContainer.localPosition.y: " + armContainer.ScrollContainerPosition.y + " accumulatedDeltaSliderValue: " + accumulatedDeltaSliderValue);

                    if (accumulatedDeltaSliderValue > tickEvery && now - lastSoundPlayTime > minSecondsBetweenTicks)
                    {
                        
                        passNotchAudioSource.pitch = Mathf.Lerp(startPitch, endPitch, armContainer.ScrollContainerPosition.y);
                        hapticPitch = Mathf.Lerp(haptic.scrollLowPitch, haptic.scrollHighPitch, armContainer.ScrollContainerPosition.y);

                        if (passNotchAudioSource.isActiveAndEnabled)
                        {
                            passNotchAudioSource.PlayOneShot(passNotchSound);
                            haptic.PressKeyHapticSample(hapticPattern);
                            haptic.UpdatePitch(hapticPattern, hapticPitch);                                
                        }
                        accumulatedDeltaSliderValue = 0;
                        lastSoundPlayTime = now;
                    }                 
                    
                }
            }             
        }

        public void SetContainerPositionOnTouch() {
            startPosition = armContainer.ScrollContainerPosition;
        }

        public void SetTouch(bool isTouch)
        {
            touch = isTouch;
        }






    }
}