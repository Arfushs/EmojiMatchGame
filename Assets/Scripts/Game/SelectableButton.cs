using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ButtonType { Emoji, Word }

public class SelectableButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _bgImage;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _selectedColor = Color.cyan;
    [SerializeField] private Color _matchedColor = Color.green;

    private string _matchKey;
    private ButtonType _type;
    
    private bool _isMatched = false;
    public bool IsMatched => _isMatched;


    public string MatchKey => _matchKey;
    public ButtonType Type => _type;

    public Color SelectedColor => _selectedColor;
    public static Color HoverColor => Color.yellow;
    
    

    public void SetMatchInfo(string matchKey, ButtonType type)
    {
        _matchKey = matchKey;
        _type = type;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isMatched) return; // ✔️ Artık eşleşmişse hiç tepki vermesin

        SelectionManager.Instance.SelectButton(this);
    }

    public void Highlight(bool on)
    {
        if (_isMatched) return;
        _bgImage.color = on ? _selectedColor : _normalColor;
    }

    public void HighlightHover(bool on)
    {
        if (_isMatched) return;
        _bgImage.color = on ? HoverColor : _normalColor;
    }

    public void Disable()
    {
        GetComponent<Button>().interactable = false;
    }

    public void SetMatched(Color matchedColor)
    {
        _isMatched = true;
        _bgImage.color = matchedColor;
        GetComponent<Button>().interactable = false;
    }

    public Vector2 GetCanvasLocalPosition(RectTransform canvas)
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPos, null, out Vector2 localPoint);
        return localPoint;
    }
}