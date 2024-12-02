using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryNpcSO", menuName = "SO/StoryNpcSO")]
public class StoryNpcSO : ScriptableObject
{
    [System.Serializable]
    public class StoryNpcSet
    {
        public List<int> npc_IDs = new List<int>();
    }

    public List<StoryNpcSet> storyNpcs = new List<StoryNpcSet>();

    private void OnEnable()
    {
    }
}
