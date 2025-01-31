/*
 * FileName: ButtonSelect.cs
 * Author: Marlow Greenan
 * CreationDate: 1/31/2025
 */
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public enum SelectType
    {
        ChildSprite,    //Changes the sprite of a child object when selected
        SpriteChange,   //Changes the sprite of the gameObject when selected
        ActivateButton, //Activates button function when selected
    }
    [Header("[R]Selection Settings")]
    [SerializeField] private SelectType _selectType = SelectType.ChildSprite;
    [SerializeField] private Sprite[] _spriteChanges; //[0] = deselected sprite, [1] = selected sprite

    [Header("[O]Child Sprite")]
    [SerializeField] private int _childSprite = 0;
    public void OnSelect(BaseEventData eventData)
    {
        SelectButton();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        switch (_selectType)
        {
            case SelectType.ChildSprite:
                if (_spriteChanges.Length > 0)
                    gameObject.transform.GetChild(0).GetComponent<Image>().sprite = _spriteChanges[0];
                else
                    gameObject.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case SelectType.SpriteChange:
                if (_spriteChanges.Length > 0)
                    gameObject.GetComponent<Image>().sprite = _spriteChanges[0];
                break;
        }
    }
    public void OnDisable()
    {
        switch (_selectType)
        {
            case SelectType.ChildSprite:
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                break;

        }
    }
    public void SelectButton()
    {
        switch (_selectType)
        {
            case SelectType.ChildSprite:
                if (_spriteChanges.Length > 0)
                    gameObject.transform.GetChild(_childSprite).GetComponent<Image>().sprite = _spriteChanges[1];
                else
                    gameObject.transform.GetChild(_childSprite).gameObject.SetActive(true);
                break;
            case SelectType.SpriteChange:
                if (_spriteChanges.Length > 0)
                    gameObject.GetComponent<Image>().sprite = _spriteChanges[1];
                break;
            case SelectType.ActivateButton:
                gameObject.GetComponent<Button>().onClick.Invoke();
                break;
        }
    }
}

