using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI myText;
    private Color baseColor;

    private void Start()
    {
        myText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        baseColor = myText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myText.color = Color.cyan;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myText.color = baseColor;
    }
}
