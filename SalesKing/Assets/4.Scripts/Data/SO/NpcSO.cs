using UnityEngine;


[CreateAssetMenu(fileName = "NpcSO", menuName = "SO/NpcSO")]
public class NpcSO : ScriptableObject
{
    public NpcInfo npcInfo;

    public void Initialize(NpcInfo data)
    {
        npcInfo = new NpcInfo(data);
    }
}