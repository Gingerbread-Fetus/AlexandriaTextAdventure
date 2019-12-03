using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    [SerializeField] LinkButton buttonPrefab;

    public void ClearPanel()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddButton(NextStateLink linkedState)
    {
        LinkButton linkButton = Instantiate(buttonPrefab, transform) as LinkButton;
        var text = linkButton.GetComponentInChildren<TextMeshProUGUI>();
        linkButton.SetLinkedState(linkedState.state);
        text.text = linkedState.linkText;
    }
}
