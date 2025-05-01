using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private TextMeshProUGUI wrongText;
    [SerializeField] private TextMeshProUGUI starText;
    
    [SerializeField] private GameObject pausePanel;
    private void Awake()
    {
        Instance = this;
    }

    public void ShowScorePanel(int wrongCount)
    {
        scorePanel.SetActive(true);
        gamePanel.SetActive(false);
        wrongText.text = $"Hatalar: {wrongCount}";
        starText.text = $" {CalculateStars(wrongCount)}";
    }

    private int CalculateStars(int wrong)
    {
        return Mathf.Clamp(GameManager.Instance.CurrentLevel.emojis.Count - wrong, 0, GameManager.Instance.CurrentLevel.emojis.Count);
    }
    
    public void OnNextLevelClicked()
    {
        GameManager.Instance.GoToNextLevel();
    }

    public void OnMainMenuClicked()
    {
        GameManager.Instance.GoToMainMenu();
    }

    public void OnPauseButtonClicked()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnResumeButtonClicked()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

}