using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance; // ���ϼ��� ����ȴ�
    public static Managers Instance { get { Init(); return s_instance; } } // ������ �Ŵ����� ����´�

    NPCManager _npc;

    public static NPCManager NPC { get { return Instance._npc; } }

    void Awake()
    {
        Init();

        GameObject npcManager = new GameObject("@NPCManager");

        if (s_instance._npc == null)
        {
            s_instance._npc = npcManager.AddComponent<NPCManager>();
        }
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }
}