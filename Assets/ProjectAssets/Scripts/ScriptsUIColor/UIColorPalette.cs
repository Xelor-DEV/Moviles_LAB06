using UnityEngine;

[CreateAssetMenu(fileName = "UIColorPalette", menuName = "UI/Color Palette")]
public class UIColorPalette : ScriptableObject
{
    [SerializeField] private Color textColor;
    [SerializeField] private Color windowColor;
    [SerializeField] private Color buttonsColor;
    [SerializeField] private Color textButtonsColor;

    public Color TextColor
    {
        get 
        { 
            return textColor; 
        }
        set 
        { 
            textColor = value; 
        }
    }

    public Color WindowColor
    {
        get
        { 
            return windowColor; 
        }
        set 
        { 
            windowColor = value; 
        }
    }

    public Color ButtonsColor
    {
        get
        { 
            return buttonsColor; 
        }
        set 
        { 
            buttonsColor = value; 
        }
    }

    public Color TextButtonsColor
    {
        get 
        { 
            return textButtonsColor; 
        }
        set 
        { 
            textButtonsColor = value; 
        }
    }
}
