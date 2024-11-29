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

    public Dictionary<Define.Emotion, List<LightSet>> dictEmoLight = new Dictionary<Define.Emotion, List<LightSet>>();
}
