using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Flags;
using System.Text.RegularExpressions;

public class AdventureGame : MonoBehaviour
{
    [SerializeField] TMP_Text textComponent;
    [SerializeField] State startingState;
    [SerializeField] float textSpeed = .1f;
    [SerializeField] Flags flagsData;

    FlagDictionary flags;
    Regex crewNameRegex = new Regex("(%{1}[A-Z]{1}[a-z]*%{1})");
    Regex flavorTextRegex = new Regex("(\\[{1}.*\\]{1})");
    private ScrollRect sr;
    private Coroutine animateText;
    State currentState;
    private string storyText;

    // Start is called before the first frame update
    void Start()
    {
        currentState = startingState;
        textComponent.text = currentState.GetStateStory();
        sr = textComponent.GetComponentInParent<ScrollRect>();
        flags = flagsData.getFlagDictionary();
    }

    // Update is called once per frame
    void Update()
    {
        ManageState();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            textSpeed = textSpeed / 100;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            textSpeed = textSpeed * 100;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            StopCoroutine(animateText);
            textComponent.maxVisibleCharacters = textComponent.text.Length;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            StopAllCoroutines();
            currentState = startingState;
            textComponent.text = currentState.GetStateStory();
            textComponent.maxVisibleCharacters = textComponent.text.Length;
        }
    }

    private void ManageState()
    {
        
        NextStateLink[] nextStates = currentState.GetNextStates();
        for(int index = 0; index < nextStates.Length; index++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + index))
            {
                ChangeState(index);
            }
        }
    }

    public void ChangeState(int nextStateIndex)
    {
        NextStateLink[] nextStates = currentState.GetNextStates();
        sr.normalizedPosition = new Vector2(0, 1);//Sets the scroll rect to the top.
        currentState = nextStates[nextStateIndex].state;

        if (currentState) { }
        else
        {
            textComponent.text = "null";
            textComponent.maxVisibleCharacters = 4;
            return;
        }

        storyText = currentState.GetStateStory();

        SetFlags();

        textComponent.text += "\n";//A line break to seperate the text from the decision links.

        LinkTexts();

        textComponent.text = storyText;

        InsertFlagValues();
        InsertFlavorText();
        //start animating text after setting state.
        animateText = StartCoroutine(AnimateTextCoroutine());
    }
    
    private void InsertFlavorText()
    {
        MatchCollection matches = flavorTextRegex.Matches(textComponent.text);
        textComponent.text = flavorTextRegex.Replace(textComponent.text, ReplaceFlavorHolders);
    }

    private void InsertFlagValues()
    {
        MatchCollection matches = crewNameRegex.Matches(textComponent.text);
        textComponent.text = crewNameRegex.Replace(textComponent.text, ReplaceDelimiters);
    }

    /// <summary>
    /// This is another helper method used as a delegate in inserting flavor text by the crew members.
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    private string ReplaceFlavorHolders(Match m)
    {
        string matchValue = m.Value.Trim(new Char[] { '[', ']'});
        string[] values = matchValue.Split(',');

        string fixedValueOne = values[1].Replace(" ", "");
        string fixedValueTwo = values[2].Replace(" ", "");

        if (flags[fixedValueOne].Equals(fixedValueTwo))
        {
            return values[0]; 
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// This is a helper delegate that allows the above method to replace delimiters with
    /// the desired string. EG: Character names, origins, etc.
    /// </summary>
    private string ReplaceDelimiters(Match m)
    {
        string value = flags[m.Value.Replace("%", "")];
        return value;
    }

    private void SetFlags()
    {
        //upon visiting a state, check for and set the necessary flag if the state doesn't set a flag do nothing
        if (String.IsNullOrEmpty(currentState.GetFlagValue())) { }
        else if (!String.IsNullOrEmpty(currentState.GetFlagValue()) && !String.IsNullOrEmpty(currentState.GetFlagKey()))//If a state sets a flag, must have a key to set it to
        {
            flags[currentState.GetFlagKey()] = currentState.GetFlagValue();
        }
        else
        {
            Debug.Log("Every state that sets a flag must have a flag to assign it to.");
        }
    }

    private void LinkTexts()
    {
        var nextStates = currentState.GetNextStates();
        string LinkText = "";
        LinkText += "\n";//Line break to add space between body and linking texts
        for (int index = 0; index < nextStates.Length; index++)
        {
            if (nextStates[index].state == null || nextStates == null)
            {
                Debug.Log("A state in Next States was not set");
                storyText += "\n ERROR";
            }
            if (CheckStateFlag(nextStates[index].state))
            {
                LinkText += (index + 1).ToString() + ". " + nextStates[index].linkText + " \n";
            }
            else
            {
                LinkText += (index + 1).ToString() + ". Requirement: " + nextStates[index].state.GetFlagValue() + " " + nextStates[index].state.CheckRequirementValue() + " not met.\n";
            } 
        }
        storyText += LinkText;
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
    /// This method will print the story text character by character.
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
            yield return new WaitForSeconds(textSpeed);
        }
    }
}