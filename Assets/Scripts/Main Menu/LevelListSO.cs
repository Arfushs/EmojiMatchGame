using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelData
{
    public List<string> emojis;
    public List<string> words;
}

[CreateAssetMenu(fileName = "LevelList", menuName = "Game/LevelList")]
public class LevelListSO : ScriptableObject
{
    public List<LevelData> levels = new List<LevelData>();
    
}