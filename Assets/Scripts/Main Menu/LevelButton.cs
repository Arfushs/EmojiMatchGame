using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private TextMeshProUGUI _starText;
    

    private int _levelIndex;

    public void Setup(int levelIndex, bool isUnlocked, int stars)
    {
        _levelIndex = levelIndex;
        _label.text = "Level " + (levelIndex + 1);
        _starText.text = $"{stars}";
        
        GetComponent<Button>().interactable = isUnlocked;
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GameManager.Instance.selectedLevelIndex = _levelIndex;
        SceneManager.LoadScene("GameScene");
    }
}