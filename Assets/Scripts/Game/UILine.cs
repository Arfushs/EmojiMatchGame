using UnityEngine;

public class UILine : MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void UpdateLine(Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        float distance = direction.magnitude;

        _rectTransform.sizeDelta = new Vector2(distance, 15f); // 5 = kalınlık
        _rectTransform.anchoredPosition = start + direction * 0.5f;
        _rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }
}