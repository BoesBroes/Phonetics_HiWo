using UnityEngine;
using UnityWebGLMicrophone;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityWebGLMicrophone
{
    public class DisplayMics : MonoBehaviour
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        void Awake()
        {
            Microphone.Init();
            Microphone.QueryAudioInput();
        }
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        void Update()
        {
            Microphone.Update();
        }
#endif


        /// <summary>
        /// Indicates whether microphone is capturing or not
        /// </summary>
        public bool IsRecording
        {
            get
            {
                return _audioClip != null && Microphone.IsRecording(CurrentDeviceName);
            }
        }

        [SerializeField] private int MicrophoneIndex;

        /// <summary>
        /// Sample rate of recorded audio
        /// </summary>
        public int SampleRate { get; private set; }

        /// <summary>
        /// Size of audio frames that are delivered
        /// </summary>
        public int FrameLength { get; private set; }

        /// <summary>
        /// Event where frames of audio are delivered
        /// </summary>
        public event Action<short[]> OnFrameCaptured;

        /// <summary>
        /// Event when audio capture thread stops
        /// </summary>
        public event Action OnRecordingStop;

        /// <summary>
        /// Event when audio capture thread starts
        /// </summary>
        public event Action OnRecordingStart;

        /// <summary>
        /// Available audio recording devices
        /// </summary>
        public List<string> Devices { get; private set; }

        /// <summary>
        /// Index of selected audio recording device
        /// </summary>
        public int CurrentDeviceIndex { get; private set; }

        /// <summary>
        /// Name of selected audio recording device
        /// </summary>
        public string CurrentDeviceName
        {
            get
            {
                if (CurrentDeviceIndex < 0 || CurrentDeviceIndex >= Microphone.devices.Length)
                    return string.Empty;
                return Devices[CurrentDeviceIndex];
            }
        }

        [Header("Voice Detection Settings")]
        [SerializeField, Tooltip("The minimum volume to detect voice input for"), Range(0.0f, 1.0f)]
        private float _minimumSpeakingSampleValue = 0.05f;

        [SerializeField, Tooltip("Time in seconds of detected silence before voice request is sent")]
        private float _silenceTimer = 1.0f;

        [SerializeField, Tooltip("Auto detect speech using the volume threshold.")]
        private bool _autoDetect;

        private float _timeAtSilenceBegan;
        private bool _audioDetected;
        private bool _didDetect;
        private bool _transmit;


        AudioClip _audioClip;
        private event Action RestartRecording;

//        void Awake()
//        {
//            UpdateDevices();
//        }
//#if UNITY_EDITOR
//        void Update()
//        {
//            if (CurrentDeviceIndex != MicrophoneIndex)
//            {
//                ChangeDevice(MicrophoneIndex);
//            }
//        }
//#endif

        /// <summary>
        /// Updates list of available audio devices
        /// </summary>
        public void UpdateDevices()
        {
            Devices = new List<string>();
            foreach (var device in Microphone.devices)
                Devices.Add(device);

            if (Devices == null || Devices.Count == 0)
            {
                CurrentDeviceIndex = -1;
                Debug.LogError("There is no valid recording device connected");
                return;
            }

            CurrentDeviceIndex = MicrophoneIndex;
        }

        /// <summary>
        /// Change audio recording device
        /// </summary>
        /// <param name="deviceIndex">Index of the new audio capture device</param>
        public void ChangeDevice(int deviceIndex)
        {
            if (deviceIndex < 0 || deviceIndex >= Devices.Count)
            {
                Debug.LogError(string.Format("Specified device index {0} is not a valid recording device", deviceIndex));
                return;
            }

            if (IsRecording)
            {
                // one time event to restart recording with the new device 
                // the moment the last session has completed
                RestartRecording += () =>
                {
                    CurrentDeviceIndex = deviceIndex;
                    StartRecording(SampleRate, FrameLength);
                    RestartRecording = null;
                };
                StopRecording();
            }
            else
            {
                CurrentDeviceIndex = deviceIndex;
            }
        }

        /// <summary>
        /// Start recording audio
        /// </summary>
        /// <param name="sampleRate">Sample rate to record at</param>
        /// <param name="frameSize">Size of audio frames to be delivered</param>
        /// <param name="autoDetect">Should the audio continuously record based on the volume</param>
        public void StartRecording(int sampleRate = 16000, int frameSize = 512, bool? autoDetect = null)
        {
            if (autoDetect != null)
            {
                _autoDetect = (bool)autoDetect;
            }

            if (IsRecording)
            {
                // if sample rate or frame size have changed, restart recording
                if (sampleRate != SampleRate || frameSize != FrameLength)
                {
                    RestartRecording += () =>
                    {
                        StartRecording(SampleRate, FrameLength, autoDetect);
                        RestartRecording = null;
                    };
                    StopRecording();
                }

                return;
            }

            SampleRate = sampleRate;
            FrameLength = frameSize;

            //_audioClip = Microphone.Start(CurrentDeviceName, true, 1, sampleRate);

            StartCoroutine(RecordData());
        }

        /// <summary>
        /// Stops recording audio
        /// </summary>
        public void StopRecording()
        {
            if (!IsRecording)
                return;

            Microphone.End(CurrentDeviceName);
            Destroy(_audioClip);
            _audioClip = null;
            _didDetect = false;

            StopCoroutine(RecordData());
        }

        /// <summary>
        /// Loop for buffering incoming audio data and delivering frames
        /// </summary>
        IEnumerator RecordData()
        {
            float[] sampleBuffer = new float[FrameLength];
            int startReadPos = 0;

            if (OnRecordingStart != null)
                OnRecordingStart.Invoke();

            while (IsRecording)
            {
                //int curClipPos = Microphone.GetPosition(CurrentDeviceName);
                //if (curClipPos < startReadPos)
                //    curClipPos += _audioClip.samples;

                //int samplesAvailable = curClipPos - startReadPos;
                //if (samplesAvailable < FrameLength)
                //{
                //    yield return null;
                //    continue;
                //}

                //TEMP!!!
                yield return null;
                continue;

                int endReadPos = startReadPos + FrameLength;
                if (endReadPos > _audioClip.samples)
                {
                    // fragmented read (wraps around to beginning of clip)
                    // read bit at end of clip
                    int numSamplesClipEnd = _audioClip.samples - startReadPos;
                    float[] endClipSamples = new float[numSamplesClipEnd];
                    _audioClip.GetData(endClipSamples, startReadPos);

                    // read bit at start of clip
                    int numSamplesClipStart = endReadPos - _audioClip.samples;
                    float[] startClipSamples = new float[numSamplesClipStart];
                    _audioClip.GetData(startClipSamples, 0);

                    // combine to form full frame
                    Buffer.BlockCopy(endClipSamples, 0, sampleBuffer, 0, numSamplesClipEnd);
                    Buffer.BlockCopy(startClipSamples, 0, sampleBuffer, numSamplesClipEnd, numSamplesClipStart);
                }
                else
                {
                    _audioClip.GetData(sampleBuffer, startReadPos);
                }

                startReadPos = endReadPos % _audioClip.samples;
                if (_autoDetect == false)
                {
                    _transmit = _audioDetected = true;
                }
                else
                {
                    float maxVolume = 0.0f;

                    for (int i = 0; i < sampleBuffer.Length; i++)
                    {
                        if (sampleBuffer[i] > maxVolume)
                        {
                            maxVolume = sampleBuffer[i];
                        }
                    }

                    if (maxVolume >= _minimumSpeakingSampleValue)
                    {
                        _transmit = _audioDetected = true;
                        _timeAtSilenceBegan = Time.time;
                    }
                    else
                    {
                        _transmit = false;

                        if (_audioDetected && Time.time - _timeAtSilenceBegan > _silenceTimer)
                        {
                            _audioDetected = false;
                        }
                    }
                }

                if (_audioDetected)
                {
                    _didDetect = true;
                    // converts to 16-bit int samples
                    short[] pcmBuffer = new short[sampleBuffer.Length];
                    for (int i = 0; i < FrameLength; i++)
                    {
                        pcmBuffer[i] = (short)Math.Floor(sampleBuffer[i] * short.MaxValue);
                    }

                    // raise buffer event
                    if (OnFrameCaptured != null && _transmit)
                        OnFrameCaptured.Invoke(pcmBuffer);
                }
                else
                {
                    if (_didDetect)
                    {
                        if (OnRecordingStop != null)
                            OnRecordingStop.Invoke();
                        _didDetect = false;
                    }
                }
            }


            if (OnRecordingStop != null)
                OnRecordingStop.Invoke();
            if (RestartRecording != null)
                RestartRecording.Invoke();
        }

        void OnGUI()
        {
            GUILayout.BeginVertical(GUILayout.Height(Screen.height));
            GUILayout.FlexibleSpace();

            string[] devices = Microphone.devices;

#if UNITY_WEBGL && !UNITY_EDITOR
            float[] volumes = Microphone.volumes;
#endif

            GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
            GUILayout.FlexibleSpace();
            GUILayout.Label(string.Format("Microphone count={0}", devices.Length));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            for (int index = 0; index < devices.Length; ++index)
            {
                string deviceName = devices[index];
                if (deviceName == null)
                {
                    deviceName = string.Empty;
                }

                GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
                GUILayout.FlexibleSpace();
#if UNITY_WEBGL && !UNITY_EDITOR
                GUILayout.Label(string.Format("Device Name={0} Volume={1}", deviceName, volumes[index]));
#else
                GUILayout.Label(string.Format("Device Name={0}", deviceName));
#endif
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
    }
}
