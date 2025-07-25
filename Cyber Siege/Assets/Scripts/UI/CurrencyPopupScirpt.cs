using System;
using TMPro;
using UnityEngine;

public class CurrencyPopupScirpt : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float timeUntilDisappear = 1f;
    [SerializeField] private float fadeSpeed = 3f;
    [SerializeField] private float moveSpeedY = 20f;

    private TextMeshProUGUI myLabel;
    private Color textColour;

    private void Awake()
    {
        myLabel = GetComponentInChildren<TextMeshProUGUI>();
        textColour = myLabel.color;
    }

    private void Update()
    {
        // move upwards
        transform.position += new Vector3(0, moveSpeedY) * Time.deltaTime;

        timeUntilDisappear -= Time.deltaTime;
        // Start disappearing
        if (timeUntilDisappear < 0)
        {
            // Fade out
            textColour.a -= fadeSpeed * Time.deltaTime;
            myLabel.color = textColour;
            // After fully fading
            if (textColour.a < 0)
            {
                Destroy(this);
            }
        }
    }

    public void SetLabel(int amt)
    {
        string res = "";
        if (amt < 0)
        {
            // Make Colour Red
            textColour = Color.red;
            res += "-";
        }
        else
        {
            // Make Colour Green
            textColour = Color.green;
            res += "+";
        }
        res += $"${Math.Abs(amt)}";
        myLabel.color = textColour;
        myLabel.text = res;
    }
}
