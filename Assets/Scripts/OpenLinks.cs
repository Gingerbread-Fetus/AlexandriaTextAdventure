using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class OpenLinks : MonoBehaviour, IPointerClickHandler {
    [SerializeField] TMP_Text pTextMeshPro;
    [SerializeField] Camera pCamera;
    [SerializeField] AdventureGame gameSession;

    void Awake()
    {
        pTextMeshPro = GetComponent<TextMeshProUGUI>();
        pCamera = Camera.main;
        gameSession = FindObjectOfType<AdventureGame>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click was detected");
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, Input.mousePosition, pCamera);
        if (linkIndex != -1)
        { // was a link clicked?
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            gameSession.ChangeState(linkIndex);
        }
    }

    List<Color32[]> SetLinkToColor(int linkIndex, Color32 color)
    {
        TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];

        var oldVertColors = new List<Color32[]>(); // store the old character colors

        for (int i = 0; i < linkInfo.linkTextLength; i++)
        { // for each character in the link string
            int characterIndex = linkInfo.linkTextfirstCharacterIndex + i; // the character index into the entire text
            var charInfo = pTextMeshPro.textInfo.characterInfo[characterIndex];
            int meshIndex = charInfo.materialReferenceIndex; // Get the index of the material / sub text object used by this character.
            int vertexIndex = charInfo.vertexIndex; // Get the index of the first vertex of this character.

            Color32[] vertexColors = pTextMeshPro.textInfo.meshInfo[meshIndex].colors32; // the colors for this character
            oldVertColors.Add(vertexColors);

            if (charInfo.isVisible)
            {
                vertexColors[vertexIndex + 0] = color;
                vertexColors[vertexIndex + 1] = color;
                vertexColors[vertexIndex + 2] = color;
                vertexColors[vertexIndex + 3] = color;
            }
        }

        // Update Geometry
        pTextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

        return oldVertColors;
    }
}
