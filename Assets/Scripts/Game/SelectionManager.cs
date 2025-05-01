using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DG.Tweening;

public class SelectionManager : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────
    // ▶ Singleton
    // ─────────────────────────────────────────────────────────────
    public static SelectionManager Instance;

    // ─────────────────────────────────────────────────────────────
    // ▶ UI References
    // ─────────────────────────────────────────────────────────────
    [Header("UI References")]
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private UILine linePrefab;
    [SerializeField] private Button hintButton;
    [SerializeField] private Transform lineDrawerContainer;


    // ─────────────────────────────────────────────────────────────
    // ▶ Visual Settings
    // ─────────────────────────────────────────────────────────────
    [Header("Visual Settings")]
    [SerializeField] private Color matchedColor = Color.green;

    // ─────────────────────────────────────────────────────────────
    // ▶ Audio
    // ─────────────────────────────────────────────────────────────
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private AudioClip hintSound;

    // ─────────────────────────────────────────────────────────────
    // ▶ State
    // ─────────────────────────────────────────────────────────────
    private SelectableButton _selectedButton;
    private SelectableButton _hoveredButton;
    private UILine _currentLine;
    
    private int _totalMatchCount = 0;
    private int _currentMatchCount = 0;


    private bool _hintUsed = false;

    public int WrongAttemptCount { get; private set; } = 0;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        _totalMatchCount = GameManager.Instance.CurrentLevel.emojis.Count;
        
    }
    
    private void Update()
    {
        if (_selectedButton != null)
        {
            UpdateLineToCursor();
            HandleHoverDetection();

            if (Input.GetMouseButtonUp(0))
            {
                if (_hoveredButton != null)
                    TryMatch(_selectedButton, _hoveredButton);

                Deselect();
            }
        }
    }

    private void UpdateLineToCursor()
    {
        if (_currentLine == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Input.mousePosition,
            null,
            out Vector2 mousePos
        );

        Vector2 start = _selectedButton.GetCanvasLocalPosition(canvasRect);
        _currentLine.UpdateLine(start, mousePos);
    }

    private void HandleHoverDetection()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        SelectableButton newHovered = null;

        foreach (var result in results)
        {
            var btn = result.gameObject.GetComponent<SelectableButton>();
            if (btn != null && btn != _selectedButton && btn.Type != _selectedButton.Type)
            {
                newHovered = btn;
                break;
            }
        }

        if (_hoveredButton != newHovered)
        {
            _hoveredButton?.HighlightHover(false);
            _hoveredButton = newHovered;
            _hoveredButton?.HighlightHover(true);
        }
    }

    // ─────────────────────────────────────────────────────────────
    // ▶ Public: Seçim Başlat
    // ─────────────────────────────────────────────────────────────
    public void SelectButton(SelectableButton button)
    {
        if (_selectedButton != null) return;

        _selectedButton = button;
        button.Highlight(true);

        _currentLine = Instantiate(linePrefab, lineDrawerContainer);
        _currentLine.GetComponent<Image>().color = button.SelectedColor;

    }

    // ─────────────────────────────────────────────────────────────
    // ▶ Seçimi Sıfırla
    // ─────────────────────────────────────────────────────────────
    private void Deselect()
    {
        _selectedButton?.Highlight(false);
        _selectedButton = null;

        _hoveredButton?.Highlight(false);
        _hoveredButton = null;

        if (_currentLine != null)
            Destroy(_currentLine.gameObject);

        _currentLine = null;
    }

    // ─────────────────────────────────────────────────────────────
    // ▶ Eşleşme Kontrolü
    // ─────────────────────────────────────────────────────────────
    private void TryMatch(SelectableButton a, SelectableButton b)
    {
        if (a.Type == b.Type) return;

        bool matched = a.MatchKey == b.MatchKey;

        if (matched)
        {
            a.Disable();
            b.Disable();

            a.SetMatched(matchedColor);
            b.SetMatched(matchedColor);

            // Kalıcı çizgi
            var line = Instantiate(linePrefab, lineDrawerContainer);
            Vector2 start = a.GetCanvasLocalPosition(canvasRect);
            Vector2 end = b.GetCanvasLocalPosition(canvasRect);
            line.UpdateLine(start, end);
            line.GetComponent<Image>().color = matchedColor;
            
            _currentMatchCount++;

            // 🎯 Tüm eşleşmeler tamamlandı mı?
            if (_currentMatchCount >= _totalMatchCount)
            {
                Invoke(nameof(OnAllMatchesCompleted), 0.5f);
            }
        }
        else
        {
            WrongAttemptCount++;

            if (audioSource && wrongSound)
                audioSource.PlayOneShot(wrongSound);

            a.transform.DOShakePosition(0.5f, strength: new Vector3(25f, 0, 0), vibrato: 10);
            b.transform.DOShakePosition(0.5f, strength: new Vector3(25f, 0, 0), vibrato: 10);
        }
    }
    
    private void OnAllMatchesCompleted()
    {
        int starCount = GameManager.Instance.CurrentLevel.emojis.Count - WrongAttemptCount;
        int levelIndex = GameManager.Instance.selectedLevelIndex;

        // Kaydedilen önceki yıldızdan daha yüksekse güncelle
        int previousStar = PlayerPrefs.GetInt($"LevelStars_{levelIndex}", 0);
        if (starCount > previousStar)
            PlayerPrefs.SetInt($"LevelStars_{levelIndex}", starCount);

        // İşaretle: level geçildi
        PlayerPrefs.SetInt($"LevelPassed_{levelIndex}", 1);

        PlayerPrefs.Save();

        UIManager.Instance.ShowScorePanel(WrongAttemptCount);
    }

    


    // ─────────────────────────────────────────────────────────────
    // ▶ Hint Sistemi
    // ─────────────────────────────────────────────────────────────
    public void OnHintClick()
    {
        if (_hintUsed) return;

        _hintUsed = true;
        WrongAttemptCount++;

        if (hintButton != null)
            hintButton.interactable = false;

        var unmatchedEmojis = new List<SelectableButton>();
        var unmatchedWords = new List<SelectableButton>();

        
        
        foreach (var btn in FindObjectsByType<SelectableButton>(FindObjectsSortMode.None))
        {
            if (!btn.IsMatched)
            {
                if (btn.Type == ButtonType.Emoji)
                    unmatchedEmojis.Add(btn);
                else
                    unmatchedWords.Add(btn);
            }
        }

        unmatchedEmojis.Shuffle();
        unmatchedWords.Shuffle();

        foreach (var emoji in unmatchedEmojis)
        {
            foreach (var word in unmatchedWords)
            {
                if (emoji.MatchKey == word.MatchKey)
                {
                    TryMatch(emoji, word);

                    if (audioSource && hintSound)
                        audioSource.PlayOneShot(hintSound);

                    return;
                }
            }
        }
    }
}
