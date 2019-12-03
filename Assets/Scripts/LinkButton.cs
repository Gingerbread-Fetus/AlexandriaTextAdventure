using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LinkButton : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] State linkedState; //Serialized for debugging
    AdventureGame game;

    void Start()
    {
        game = FindObjectOfType<AdventureGame>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        game.ChangeState(linkedState);
    }

    public void SetLinkedState(State state)
    {
        linkedState = state;
    }
    
    private void SetText(string newText)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.text = newText;
    }
}
