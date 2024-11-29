using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TipSO", menuName = "SO/TipSO")]
public class TipSO : ScriptableObject
{
    [System.Serializable]
    public class TipSet
    {
        public int npcId;
        public string[] tips = new string[3];
    }

    public List<TipSet> npcTips = new List<TipSet>();
}
