using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleData
{
    public string name;
    public int[] timeStamps;
    public bool isStatic;
    public int currentIndex = 0;
    public int numTimeStamps;
}
