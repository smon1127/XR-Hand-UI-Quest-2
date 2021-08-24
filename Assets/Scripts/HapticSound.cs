
namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    using Microsoft.MixedReality.Toolkit.UI;
    using Syntacts;
    using System.Collections;
    using UnityEngine;

    public class HapticSound : MonoBehaviour
    {

       
        public string[] hapticFile = new string[] { "SquareTic", "SquareTac", "SquareToc", "", "", "", "", "", "", "" };
        public bool[] customHaptic = new bool[] { true, true, true, true, true, true, true, true, true, true };
        public Vector3[] customHapticDesign = new[] { new Vector3(defaultFreq, defaultDuration, defaultPitch), new Vector3(defaultFreq, defaultDuration, defaultPitch), new Vector3(defaultFreq, defaultDuration, defaultPitch), new Vector3(defaultFreq, defaultDuration, defaultPitch), new Vector3(defaultFreq, defaultDuration, defaultPitch), new Vector3(defaultFreq, defaultDuration, defaultPitch), new Vector3(defaultFreq, defaultDuration, defaultPitch), new Vector3(defaultFreq, defaultDuration, defaultPitch), new Vector3(defaultFreq, defaultDuration, defaultPitch), new Vector3(defaultFreq, defaultDuration, defaultPitch) };
        public string[] soundFile = new string[] { "", "", "", "", "", "", "", "", "" };
        public AudioClip[] soundPattern;
        public AudioClip[] hapticPattern;


        [Tooltip("Lowest Frequency (Hz) for haptic Feedback when scrolling")]
        public float scrollLowPitch = .75f;
        [Tooltip("Highest Frequency (Hz) for haptic Feedback when scrolling")]
        public float scrollHighPitch = 1.25f;



        [Header("Tick Notch Sounds")]

        [SerializeField]
        [Tooltip("Whether to play 'tick tick' sounds as the slider passes notches")]
        public bool playTickSounds = true;
        public bool hapticThroughSyntacts = false;

        [SerializeField]
        public float minSecondsBetweenTicks = 0.01f;

        private const int defaultFreq = 150;
        private const float defaultDuration = 0.01f;
        private const float defaultPitch = 1.0f;
        private AudioSource soundAudioSource = null;
        private AudioSource hapticAudioSource = null;
        private float hapticPitch;

        public int playHapticSoundPattern = 0;
        public ArmUiHandler armUiHandler;
        public ArmSliderHandler armSliderHandler;
        public ScrollingObjectCollection armScroll;

        [Header("Info")]
        [ReadOnly] public int hapticsChannel = 1;
        [ReadOnly] public int soundChannel = 0;
        [ReadOnly] public SyntactsHub syntactsHub;



        // Start is called before the first frame update
        void Start()
        {
            //syntactsHub.session.Play(hapticFileChannel, new Sine(velocityFreq) * new Sine(5));
            // rb = GetComponent<Rigidbody>();
            //rb.isKinematic = false;

            if (!hapticThroughSyntacts)
            {
                if (soundAudioSource == null)
                {
                    soundAudioSource = gameObject.AddComponent<AudioSource>();
                    soundAudioSource.panStereo = -1;

                }

                if (hapticAudioSource == null)
                {
                    hapticAudioSource = gameObject.AddComponent<AudioSource>();
                    hapticAudioSource.panStereo = 1;
                }
            }
        }



        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                playHapticSoundPattern = 0;
            }

            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                playHapticSoundPattern = 1;
            }

            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                playHapticSoundPattern = 2;
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                playHapticSoundPattern = 3;
            }

            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                playHapticSoundPattern = 4;
            }

            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                playHapticSoundPattern = 5;
            }

            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                playHapticSoundPattern = 6;
            }

            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                playHapticSoundPattern = 7;
            }

            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                playHapticSoundPattern = 8;
            }

            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                playHapticSoundPattern = 9;
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                PressKeyHapticSample(playHapticSoundPattern);
            }



            //if (Input.GetKeyDown(KeyCode.KeypadEnter))
            //{
            //    if (rb.isKinematic)
            //    {
            //        rb.isKinematic = false;
            //    }
            //    else
            //    {
            //        rb.isKinematic = true;
            //    }
            //    Debug.Log("Kinematic " + rb.isKinematic);
            //}


        }
  
        public void UpdatePitch(int numPad, float pitch)
        {
            customHapticDesign[numPad].z = pitch;
            //Debug.Log("customHapticDesign[numPad].x: " + customHapticDesign[numPad].z);
        }

        public void PressKeyHapticSample(int numPad)
        {
            

            if (hapticThroughSyntacts)
            {

                syntactsHub.session.SetPitch(hapticsChannel, customHapticDesign[numPad].z);
                syntactsHub.session.SetPitch(soundChannel, customHapticDesign[numPad].z);
                //Debug.Log("Enter haptics");

                if (armUiHandler.isHaptic)
                {
                    if (customHaptic[numPad])
                    {
                        //Debug.Log("Enter custom haptics");
                        Signal customHaptic = new Sine(customHapticDesign[numPad].x) * new ASR(customHapticDesign[numPad].y, customHapticDesign[numPad].y, customHapticDesign[numPad].y);
                        syntactsHub.session.Play(hapticsChannel, customHaptic);
                        //Debug.Log("customHaptic" + customHapticDesign[numPad].ToString("F2"));
                        //Debug.Log("Play haptic: " + hapticsChannel + customHaptic);
                    }
                    else
                    {
                        //Debug.Log("soundFile: " + soundFile[numPad] + ", hapticFile:" + hapticFile[numPad]);

                        Signal hapticSig;
                        if (Syntacts.Library.LoadSignal(out hapticSig, hapticFile[numPad]))
                        {
                            syntactsHub.session.Play(hapticsChannel, hapticSig);
                            //Debug.Log("Play haptic: " + hapticsChannel + customHaptic);
                        }
                        else if (hapticFile[numPad] == "")
                            return;
                        else
                            Debug.LogError("Failed to load haptic signal " + hapticFile[numPad]);
                    }
                }

                if (armUiHandler.isAudio)
                {

                    Signal soundSig;
                    if (Syntacts.Library.LoadSignal(out soundSig, soundFile[numPad]))
                    {
                        //Debug.Log("Enter soundfile");
                        syntactsHub.session.Play(soundChannel, soundSig);
                        //Debug.Log("Play sound: " + soundChannel + soundSig);
                    }
                    else if (soundFile[numPad] == "")
                        return;
                    else
                        Debug.LogError("Failed to load sound signal " + soundFile[numPad]);
                }
            }else
            {

                //Debug.Log("I'm here: HapticSound no syntacts");
                if (soundAudioSource.isActiveAndEnabled && hapticAudioSource.isActiveAndEnabled)
                {
                    //hapticPattern = haptic.playHapticSoundPattern; 
                    StartCoroutine(SoundHaptic(numPad));
                }
            }
        }

        IEnumerator SoundHaptic(int numPadEnum)
        {
            float normal = Mathf.Lerp(0, armSliderHandler.sliderHeight, armScroll.workingScrollerPos.y);
            hapticPitch = Mathf.Lerp(scrollLowPitch, scrollHighPitch, normal);


            if (armUiHandler.isHaptic)
            {
                //Debug.Log("I'm here: Play Haptic");
                hapticAudioSource.PlayOneShot(hapticPattern[numPadEnum]);
                hapticAudioSource.pitch = hapticPitch;
            }                

            yield return new WaitForSeconds(armUiHandler.hapticLatencyAudioOffset);

            if (armUiHandler.isAudio)
            {
                //Debug.Log("I'm here: Play Audio");
                soundAudioSource.PlayOneShot(soundPattern[numPadEnum]);
                soundAudioSource.pitch = hapticPitch;
            }
                
        }
    }

    //void OnGUI()
    //{
    //    if (Event.current.isKey && Event.current.type == EventType.KeyDown)
    //    {
    //        Debug.Log(Event.current.keyCode);
    //    }
    //}
}