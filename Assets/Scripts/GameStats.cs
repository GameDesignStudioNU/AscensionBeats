using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStats
{
    private static int _maxHeight, _initialHeight;
    private static Vector2 _respawnPlayerPos;
    private static float _respawnEqualizerPos;
    private static float _respawnSongTime;    
    private static float _respawnBeatTimer;    


    public static int MaxHeight { get { return _maxHeight; } set { _maxHeight = value; } }

    public static int InitialHeight { get { return _initialHeight; } set { _initialHeight = value; } }

    // Respawning fields

    public static Vector2 RespawnPlayerPos { get { return _respawnPlayerPos; } set { _respawnPlayerPos = value; } }

    public static float RespawnEqualizerPos { get { return _respawnEqualizerPos; } set { _respawnEqualizerPos = value; } }

    public static float RespawnSongTime { get { return _respawnSongTime; } set { _respawnSongTime = value; } }

    public static float RespawnBeatTimer { get { return _respawnBeatTimer; } set { _respawnBeatTimer = value; } }



}

