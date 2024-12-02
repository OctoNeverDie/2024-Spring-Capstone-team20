using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightSO", menuName = "SO/LightSO")]
public class LightSO : ScriptableObject
{
    [System.Serializable]
    public class LightSet
    {
        public Define.LightType lightType;
        public float intensity;
        public string Color;
    }

    [System.Serializable]
    public class EmotionLightSet
    {
        public Define.Emotion emotion;
        public List<LightSet> lightSets;
    }

    public List<EmotionLightSet> emotionLightSets = new List<EmotionLightSet>();

    public Dictionary<Define.Emotion, List<LightSet>> dictEmoLight;

    private void OnEnable()
    {
        dictEmoLight = new Dictionary<Define.Emotion, List<LightSet>>();
        foreach (var item in emotionLightSets)
        {
            if (!dictEmoLight.ContainsKey(item.emotion))
            {
                dictEmoLight.Add(item.emotion, item.lightSets);
            }
            else
            {
                Debug.LogWarning($"Duplicate emotion key detected: {item.emotion}");
            }
        }
    }

    public List<LightSet> GetLightSets(Define.Emotion emotion)
    {
        if (dictEmoLight.TryGetValue(emotion, out var sets))
        {
            return sets;
        }
        return null; // Or return an empty list, based on your needs
    }
}
