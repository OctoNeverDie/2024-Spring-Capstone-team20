using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]
    GameObject ConvoPanel;
    Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            NPC thisNPC = other.GetComponent<NPC>();
            if (thisNPC.currentTalkable == NPCDefine.Talkable.Able)
            {
                Debug.Log("collide with npc");
                ConvoPanel.SetActive(true);
                player.isPlayerLocked = true;
            }
        }
    }
}
