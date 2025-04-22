using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIThemeApplier : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private UIColorPalette palette;
    [SerializeField] private Image[] windows;
    [SerializeField] private TMP_Text[] texts;
    [SerializeField] private Button[] buttons;
    [SerializeField] private TMP_Text[] textButtons;

    void Awake()
    {
        ApplyColor();
    }

    void ApplyColor()
    {
        for (int i = 0; i < windows.Length; ++i)
        {
            if (windows[i] != null)
            {
                windows[i].color = palette.WindowColor;
            }
        }

        for (int i = 0; i < texts.Length; ++i)
        {
            if (texts[i] != null)
            {
                texts[i].color = palette.TextColor;
            }
        }

        for (int i = 0; i < buttons.Length; ++i)
        {
            if (buttons[i] != null)
            {
                ColorBlock btnColors = buttons[i].colors;
                btnColors.normalColor = palette.ButtonsColor;
                btnColors.highlightedColor = palette.ButtonsColor * 1.2f;
                btnColors.pressedColor = palette.ButtonsColor * 0.8f;
                btnColors.selectedColor = palette.ButtonsColor;
                buttons[i].colors = btnColors;
            }
        }

        for (int i = 0; i < textButtons.Length; ++i)
        {
            if (textButtons[i] != null)
            {
                textButtons[i].color = palette.TextButtonsColor;
            }
        }
    }
}