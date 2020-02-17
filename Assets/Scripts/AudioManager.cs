using System;
using UnityEngine.Audio;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public float scaleFactor = 10f;
    public static float[] samples = new float[512];
    public static float[] freqBands = new float[8];
    public static float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];
    public static AudioManager instance;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    void GetSpectrumAudioData()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void GetFrequencyBands()
    {
        int count = 0;

        for(int i = 0; i < 8; i++) {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2,i) * 2;

            if(i == 7) {
                sampleCount += 2; // Since sum of 2^i from 0 <= i <= 7 is only 510 and we have 512 samples
            }

            for(int j = 0; j < sampleCount; j++) {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            freqBands[i] = average * scaleFactor;
        }
    }

    void BandBuffer()
    {
        for(int i = 0; i < 8; i++) {
            if(freqBands[i] > bandBuffer[i]) {
                bandBuffer[i] = freqBands[i];
                bufferDecrease[i] = 0.005f;
            }
            if(freqBands[i] < bandBuffer[i]) {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.Play();
    }

    void Update()
    {
        GetSpectrumAudioData();
        GetFrequencyBands();
        BandBuffer();
    }

    // public void Play(string name) 
    // {
    //     Song s = Array.Find(songs, song => song.name == name);
    //     if(s == null)
    //         return;
    //     s.source.Play();
    // }
}
