using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AdventureGame : MonoBehaviour
{
    [SerializeField] TMP_Text textComponent;
    [SerializeField] State startingState;
    [SerializeField] float TextSpeed = .4f;
    Dictionary<string, string> flags;
    
    State state;

    // Start is called before the first frame update
    void Start()
    {
        flags = new Dictionary<string, string>();
        state = startingState;
        textComponent.text = state.GetStateStory();
        //textComponent.maxVisibleCharacters = 0;
        //StartCoroutine(AnimateTextCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        ManageState();
    }

    private void ManageState()
    {
        string storyText;
        NextStateLink[] nextStates = state.GetNextStates();
        for(int index = 0; index < nextStates.Length; index++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + index))
            {
                state = nextStates[index].state;
                if (state) { }
                else
                {
                    textComponent.text = "null";
                    textComponent.maxVisibleCharacters = 4;
                    return;
                }
                storyText = state.GetStateStory();
                //upon visiting a state, check for and set the necessary flag if the state doesn't set a flag do nothing
                if (String.IsNullOrEmpty(state.GetFlagValue())) { }
                else if (!String.IsNullOrEmpty(state.GetFlagValue()) && !String.IsNullOrEmpty(state.GetFlagKey()))//If a state sets a flag, must have a key to set it to
                {
                    flags[state.GetFlagKey()] = state.GetFlagValue();
                }
                else
                {
                    Debug.Log("Every state that sets a flag must have a flag to assign it to.");
                }

                textComponent.text += "\n";//A line break to seperate the text from the decision links.

                storyText += LinkTexts();
                textComponent.text = storyText;
                //start animating text after setting state.
                StartCoroutine(AnimateTextCoroutine());
            }
        }
    }

    private string LinkTexts()
    {
        var nextStates = state.GetNextStates();
        string storyText = "";
        for (int index = 0; index < nextStates.Length; index++)
        {
            if (nextStates[index].state == null || nextStates == null)
            {
                Debug.LogError("A state in Next States was not set");
                return "ERROR";
            }
            if (CheckStateFlag(nextStates[index].state))
            {
                storyText += (index + 1).ToString() + ". " + nextStates[index].linkText + " \n";
            }
            else
            {
                storyText += (index + 1).ToString() + ". Requirement: " + nextStates[index].state.GetFlagValue() + " = " + nextStates[index].state.CheckRequirementValue() + " not met.\n";
            } 
        }
        return storyText;
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
        return flags[testState.GetFlagKey()].Equals(testState.CheckRequirementValue());
    }

    /// <summary>
    /// This method will print the story text character by character. Then append the choices array onto the end.
    /// </summary>
    /// <param name="LinkTextString">The connecting choices for this story state.</param>
    private IEnumerator AnimateTextCoroutine()
    {
        string storytext = textComponent.text;
        int i = 0;
        while (i < storytext.Length)
        {
            textComponent.maxVisibleCharacters = i;
            i++;
            yield return new WaitForSeconds(TextSpeed);
        }
    }
}