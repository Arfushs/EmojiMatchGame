using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class GameSceneUIController : MonoBehaviour
    {
        [SerializeField] private Transform emojiContainer;
        [SerializeField] private Transform wordContainer;
        [SerializeField] private GameObject emojiButtonPrefab;
        [SerializeField] private GameObject wordButtonPrefab;

        private void Start()
        {
            LoadLevel();
        }

        private void LoadLevel()
        {
            var level = GameManager.Instance.CurrentLevel;

            List<string> emojis = new(level.emojis);
            List<string> words = new(level.words);

            // Karıştırmak için index eşleşmelerini ayrı tutalım
            List<(string emoji, string word, string matchKey)> pairs = new();

            for (int i = 0; i < emojis.Count; i++)
            {
                string matchKey = $"match_{i}";
                pairs.Add((emojis[i], words[i], matchKey));
            }

            // Shuffle list (emoji ve word sıraları bağımsız olmalı)
            var shuffledEmojiPairs = new List<(string emoji, string matchKey)>();
            var shuffledWordPairs = new List<(string word, string matchKey)>();

            foreach (var pair in pairs)
            {
                shuffledEmojiPairs.Add((pair.emoji, pair.matchKey));
                shuffledWordPairs.Add((pair.word, pair.matchKey));
            }

            shuffledEmojiPairs.Shuffle();
            shuffledWordPairs.Shuffle();

            // Spawn emojiler
            foreach (var (emoji, key) in shuffledEmojiPairs)
            {
                GameObject btn = Instantiate(emojiButtonPrefab, emojiContainer);
                var sel = btn.GetComponent<SelectableButton>();
                sel.SetMatchInfo(key, ButtonType.Emoji);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = emoji;
            }

            // Spawn kelimeler
            foreach (var (word, key) in shuffledWordPairs)
            {
                GameObject btn = Instantiate(wordButtonPrefab, wordContainer);
                var sel = btn.GetComponent<SelectableButton>();
                sel.SetMatchInfo(key, ButtonType.Word);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = word.FirstCharacterToUpper();
            }
        }

    }
}