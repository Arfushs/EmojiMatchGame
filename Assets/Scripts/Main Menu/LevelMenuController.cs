using UnityEngine;

public class LevelMenuController : MonoBehaviour
{
    [SerializeField] private Transform levelContainer;
    [SerializeField] private GameObject levelButtonPrefab;

    private void Start()
    {
        for (int i = 0; i < GameManager.Instance.levelList.levels.Count; i++)
        {
            int stars = PlayerPrefs.GetInt("LevelStars_" + i, 0);
            bool isUnlocked = i == 0 || PlayerPrefs.GetInt("LevelStars_" + (i - 1), 0) > 0;

            GameObject buttonObj = Instantiate(levelButtonPrefab, levelContainer);
            var levelBtn = buttonObj.GetComponent<LevelButton>();
            levelBtn.Setup(i, isUnlocked, stars);
        }
    }

}