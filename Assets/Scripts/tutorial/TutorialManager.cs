using System;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private RectTransform hand1;
    [SerializeField] private RectTransform hand2;
    [SerializeField] private UILine _line1;
    [SerializeField] private UILine _line2;
    [SerializeField] private RectTransform startPos1;
    [SerializeField] private RectTransform startPos2;
    [SerializeField] private RectTransform canvas;

    private void Update()
    {
        _line1.UpdateLine(GetCanvasLocalPosition(canvas,startPos1),GetCanvasLocalPosition(canvas,hand1));
        _line2.UpdateLine(GetCanvasLocalPosition(canvas,startPos2),GetCanvasLocalPosition(canvas,hand2));
    }
    
    public Vector2 GetCanvasLocalPosition(RectTransform cnv, Transform obj)
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, obj.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cnv, screenPos, null, out Vector2 localPoint);
        return localPoint;
    }
}
