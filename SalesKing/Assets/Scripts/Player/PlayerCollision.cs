using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField]
    GameObject ConvoPanel;
    Player _player;

    void Start()
    {
        _player = GetComponent<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            NPC thisNPC = GetComponent<NPC>();
            if (thisNPC.currentTalkable == NPCDefine.Talkable.Able)
            {
                ConvoPanel.SetActive(true);
                _player.isPlayerLocked = true;
            }
        }
    }
}
