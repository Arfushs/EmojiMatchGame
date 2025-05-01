using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LevelListSO levelList;
    public int selectedLevelIndex = 0;

    public LevelData CurrentLevel => levelList.levels[selectedLevelIndex];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void GoToNextLevel()
    {
        selectedLevelIndex++;

        if (selectedLevelIndex >= levelList.levels.Count)
        {
            // Tüm level bitti → Ana menüye dön
            SceneManager.LoadScene("MainMenuScene");
        }
        else
        {
            // Sonraki leveli yükle
            SceneManager.LoadScene("GameScene");
        }
    }


    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

}