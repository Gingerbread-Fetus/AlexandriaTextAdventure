using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureGame : MonoBehaviour
{
    [SerializeField] Text textComponent;
    [SerializeField] State startingState;
    Dictionary<string, string> flags;
    
    State state;

    // Start is called before the first frame update
    void Start()
    {
        flags = new Dictionary<string, string>();
        state = startingState;
        textComponent.text = state.GetStateStory();
    }

    // Update is called once per frame
    void Update()
    {
        ManageState();
    }

    private void ManageState()
    {
        string sb = "\n";
        var nextStates = state.GetNextStates();
        for(int index = 0; index < nextStates.Length; index++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + index))
            {
                state = nextStates[index];
                //upon visiting a state, check for and set the necessary flag
                //if the state doesn't set a flag do nothing
                if (String.IsNullOrEmpty(state.GetFlagValue())) {}
                //Next we want to check the requirement that a state that sets a flag, has a key to set it to
                else if (!String.IsNullOrEmpty(state.GetFlagValue()) && !String.IsNullOrEmpty(state.GetFlagKey()))
                {
                    //flags.Add(state.GetFlagKey(), state.GetFlagValue());
                    flags[state.GetFlagKey()] = state.GetFlagValue();
                }
                //Otherwise something has gone wrong
                else
                {
                    Debug.Log("Every state that sets a flag must have a flag to assign it to.");
                }
            }
            //this should only print when the requirement is met
            if (CheckStateFlag(nextStates[index]))
            {
                sb += (index + 1).ToString() + ". " + nextStates[index].GetLinkText() + " \n"; 
            }
            else
            {
                sb += (index + 1).ToString() + ". Requirement: " + nextStates[index].GetFlagValue() + " = " + nextStates[index].CheckRequirementValue() + " not met.\n";
            }
        }
        textComponent.text = state.GetStateStory();
        textComponent.text += sb;
    }

    private bool CheckStateFlag(State testState)
    {
        //Return true if there is no necessary requirement
        if (String.IsNullOrEmpty(testState.CheckRequirementValue())) { return true; }
        //Otherwise it has a requirement, we want to ensure it has a flag key, otherwise print an error.
        if (String.IsNullOrEmpty(testState.GetFlagKey()))
        {
            Debug.Log("ERROR: Every state with a requirement must have a flag key.");
            return false;
        }
        Debug.Log(testState.GetFlagKey() + " " + testState.CheckRequirementValue());
        Debug.Log(flags[testState.GetFlagKey()]);
        return flags[testState.GetFlagKey()].Equals(testState.CheckRequirementValue());
    }
}
