using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject howToPlayPanel;

    public void OnPlayClicked()
    {
        GameManager.Instance.selectedLevelIndex = 0;
        SceneManager.LoadScene("GameScene");
    }

    public void OnLevelSelectClicked()
    {
        levelSelectPanel.SetActive(true);
        buttonsPanel.SetActive(false);
    }

    public void OnCloseLevelPanel()
    {
        levelSelectPanel.SetActive(false);
        buttonsPanel.SetActive(true);
    }

    public void OnHowToPlayClicked()
    {
        howToPlayPanel.SetActive(true);
        buttonsPanel.SetActive(false);
        title.SetActive(false);
    }

    public void OnHowToPlayBackClicked()
    {
        howToPlayPanel.SetActive(false);
        buttonsPanel.SetActive(true);
        title.SetActive(true);
    }

    public void OnExitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}