using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[System.Serializable]
public class RequirementPair
{
    [SerializeField]
    string requirement;
    [SerializeField]
    string value;
}

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject
{
    [TextArea(10,14)][SerializeField] string storyText;
    [SerializeField] State[] nextStates;

    //Flags that are set by this state
    [SerializeField]
    public string flagToSet;
    [SerializeField]
    public string[] flagValues;

    //Flags that are required for this state
    [SerializeField]
    public RequirementPair requiredFlags;


    public string GetStateStory()
    {
        return storyText;
    }

    public State[] GetNextStates()
    {
        return nextStates;
    }
}
