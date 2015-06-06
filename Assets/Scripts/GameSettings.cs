using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameSettings
{
    //this holds settings and constants not expected to be changed by the player
    public static float magnetInterval = 0.005f;
    public static float minimumMagnetInterval = 0.005f;
    public static int maximumCharge = 1000;
    public static int maximumMass = 1000;
    public static int maximumVelocity = 50;
    public static int targetFPS = 30;
}
