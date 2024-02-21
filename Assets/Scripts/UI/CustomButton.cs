using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CustomButton : MonoBehaviour, ISelectHandler, ISubmitHandler, IDeselectHandler
{
    [SerializeField] protected bool muteOnSubMenuStart;
    [SerializeField] protected HomeMenu menuManager;
    [SerializeField] protected TMP_Text buttonText;
    protected AudioManager audioManager;
    protected Color selectedTextColor;
    protected Color originalTextColor;
    protected const string hexColorSelected = "5A007B";
    protected const string hexColorOriginal = "DE2900";

    public void OnDeselect(BaseEventData eventData)
    {
        buttonText.color = originalTextColor;
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        if (audioManager != null && !muteOnSubMenuStart)
            audioManager.PlaySound(SoundKey.ButtonPressed);
        else if (!muteOnSubMenuStart)
            buttonText.color = selectedTextColor;
        else if (muteOnSubMenuStart)
            muteOnSubMenuStart = false;
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        if (audioManager != null)
            audioManager.PlaySound(SoundKey.ButtonSelected);
    }


    protected virtual void Start()
    {
        audioManager = menuManager.audioManager;
        originalTextColor = CalculateHexColor(hexColorOriginal);
        selectedTextColor = CalculateHexColor(hexColorSelected);
    }

    private Color CalculateHexColor(string hexColor)
    {
        Color color;
        string alpha, red, green, blue;
        if (hexColorSelected.Length >= 6)
        {
            red = hexColor.Substring(0, 2);
            green = hexColor.Substring(2, 2);
            blue = hexColor.Substring(4, 2);
            if (hexColor.Length >= 8)
                alpha = hexColor.Substring(6, 2);
            else
                alpha = "FF";

            color = new Color((int.Parse(red, NumberStyles.HexNumber) / 255f),
            (int.Parse(green, NumberStyles.HexNumber) / 255f),
            (int.Parse(blue, NumberStyles.HexNumber) / 255f),
            (int.Parse(alpha, NumberStyles.HexNumber) / 255f));
            return color;
        }
        else
        {
            return Color.white;
        }
    }
}