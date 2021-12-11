using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all potentially variable minigame data (damage, duration etc)
/// </summary>
[System.Serializable]
public class MGDTO
{
    public static readonly string[] lesserFeedback = new string[]
{
        "MISSED",
        "DECENT",
        "NICE",
        "GREAT",
        "PERFECT",
};

    public static readonly string[] greaterFeedback = new string[]
{
        "MEDIOCRE",
        "DECENT",
        "NICE",
        "GREAT",
        "FLAWLESS",
};

    public Actor actorInitiated;
    public Actor actorTargeted;
    public float actionCost = 10;
    public float damageMin = 10;
    public float damageMax = 10;
    public string type = "JOB";
    public int maxScore;
    public int frequency = 1;
    public float duration = 3;
    public float startDelay = 0.4f;
    public float endDelay = 0.4f;

    
}
