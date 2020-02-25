using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeStamp
{
    private int[] _intTimeStamps;
    private int _numTimeStamps;

    public TimeStamp(string s) {
        _intTimeStamps = mapStringToIntTimeStamps(s);
        _numTimeStamps = _intTimeStamps.Length;
    }

    int[] mapStringToIntTimeStamps(string timeStamps) {
        string[] stringTimeStamps = timeStamps.Split(',');
        int[] intTimeStamps = new int[stringTimeStamps.Length];
        for(int j = 0; j < stringTimeStamps.Length; j++) {
            intTimeStamps[j] = int.Parse(stringTimeStamps[j]);
        }

        return intTimeStamps;
    }

    public int getTimeStamp(int index) {
        if(index < _numTimeStamps) return _intTimeStamps[index];
        return -1;
    }
}
