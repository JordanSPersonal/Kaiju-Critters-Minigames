using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonBehaviourScript : MonoBehaviour
{
    public List<Sprite> buttonSprites = new List<Sprite>();
    public SpriteRenderer buttonRenderer;
    public ButtonType buttonType;
    public enum ButtonType
    {
        AButton,
        BButton,
        XButton,
        YButton
    }
    private void Start()
    {
        buttonRenderer = GetComponent<SpriteRenderer>();        
    }
    /// <summary>
    /// Changes sprite based on int input, 0 = default, 1 = pressed, 2 = missed
    /// </summary>
    /// <param name="whichSprite"></param>
    public void ChangeSprite(int whichSprite)
    {
        buttonRenderer.sprite = buttonSprites[whichSprite];
    }
}
