using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcLookSO", menuName = "SO/NpcLookSO")]
public class NpcLookSO : ScriptableObject
{
    [System.Serializable]
    public class LookSet
    {
        public int npcId;
        public GameObject look;
        public Sprite npcProfileImg;
    }

    public List<LookSet> npcLooks = new List<LookSet>();
}
