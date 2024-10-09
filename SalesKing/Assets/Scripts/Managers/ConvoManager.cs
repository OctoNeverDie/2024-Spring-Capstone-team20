using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Text.Json;
using Newtonsoft.Json;
using System.Linq;
public class ConvoManager : MonoBehaviour
{
    public void ConvoStarted()
    {
        Managers.Time.StopAndRestartTime(true);
        Managers.Cam.SwitchToDialogueCam();
        Managers.UI.ShowTalkOrNotPanel();
    }

    public void ConvoFinished()
    {
        Managers.Time.StopAndRestartTime(false);
        Managers.Player.MyPlayer.GetComponent<Player>().PlayerExitConvo();
        Managers.NPC.curTalkingNPC.GetComponent<NPC>().currentTalkable = NPCDefine.Talkable.Not;
        Managers.NPC.curTalkingNPC.GetComponent<NPC>().myCanvas.SetActive(false);
        Managers.NPC.curTalkingNPC = null;
        Managers.Cam.SwitchToFirstPersonCam();
        Managers.Player.MyPlayer.GetComponent<Player>().PlayerBody.SetActive(true);
        Managers.Turn.AddTurnAndCheckTalkTurn();
    }
}
