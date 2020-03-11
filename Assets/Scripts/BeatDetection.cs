using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatDetection : MonoBehaviour
{
    private float old_average;
    private float average;

    public float epsilon = .25f;
    public int startIndex = 0;
    public int endIndex = 2;

    private bool[] beatBuffer = new bool[3];
    private int beatBufferSize = 0;
    private float beatBufferCooldown = .55f;
    private float beatBufferCD;

    public float cooldown = .45f;
    private float cd;

    private void Awake()
    {
        ClearBeatBuffer();
    }

    void Update()
    {
        if (cd > 0)
            cd -= Time.deltaTime;
        if (beatBufferCD > 0)
            beatBufferCD -= Time.deltaTime;

        CalculateAverage();

        if (average - old_average > epsilon && cd <= 0)
        {
            // Debug.Log("YEET");
            AddBeatToBeatBuffer();

            beatBufferCD = beatBufferCooldown;
            cd = cooldown;
        }

        if (CheckIfBeatBufferFilled())
        {
            // Debug.Log("SUCCESS");
            if (FindObjectOfType<BPerM>().beatTimer > .05f)
            {
                FindObjectOfType<BPerM>().beatTimer = FindObjectOfType<BPerM>().beatInterval;
                // Debug.Log("FIXED BEAT");
            }
            ClearBeatBuffer();
        }

        if (beatBufferCD <= 0)
            ClearBeatBuffer();



    }

    private void CalculateAverage()
    {
        old_average = average;

        average = 0;
        for (int i = startIndex; i < endIndex; i++)
        {
            average += AudioManager.bandBuffer[i];
        }

        average /= (endIndex - startIndex);
    }

    private void AddBeatToBeatBuffer()
    {
        beatBuffer[beatBufferSize] = true;
        beatBufferSize++;
    }

    private void ClearBeatBuffer()
    {
        for (int i = 0; i < beatBuffer.Length; i++)
        {
            beatBuffer[i] = false;
        }
        beatBufferSize = 0;
    }

    private bool CheckIfBeatBufferFilled()
    {
        bool result = true;

        for (int i = 0; i < beatBuffer.Length; i++)
        {
            if (!beatBuffer[i])
                result = false;
        }

        return result;
    }
}
