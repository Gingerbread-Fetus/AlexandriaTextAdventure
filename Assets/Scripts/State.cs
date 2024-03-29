﻿using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[System.Serializable]
public class FlagRequirement
{
    //Every state that has a requirement requires a flag to check in the flags dictionary
    public string flagKey;
    //This is the variable used to set the flag in the adventure game class. This of course is different than the key required for the state to be read.
    public string flagValue;
    //This is the requirement for this state to be shown, likewise, every state with a requirement value requires a key to check in the flags dictionary
    public string requiredValue;
    
}

[System.Serializable]
public class NextStateLink
{
    public State state;//the state in question
    public string linkText;//The text printed when it is linked to.
}

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject
{
    [TextArea(10,14)][SerializeField] string storyText;
    [SerializeField]NextStateLink[] nextStates;
    [SerializeField]FlagRequirement flagRequirement;

    public string GetStateStory()
    {
        return storyText;
    }

    public NextStateLink[] GetNextStates()
    {
        return nextStates;
    }

    public string CheckRequirementValue()
    {
        return flagRequirement.requiredValue;
    }

    public string GetFlagKey()
    {
        return flagRequirement.flagKey;
    }

    public string GetFlagValue()
    {
        return flagRequirement.flagValue;
    }
}