using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Text.Json;
using Newtonsoft.Json;
using System.Linq;

public class ConversationData
{
    public string Thought { get; set; }
    public string Reason { get; set; }
    public string Emotion { get; set; }
    public string SuggestedPrice { get; set; }
    public string Reaction { get; set; }
}

public class Parser
{
    public ConversationData ParseInput(string input)
    {
        // Split the input by lines and remove unnecessary characters
        var lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(line => line.Trim().TrimEnd(','));

        // Create a dictionary to store key-value pairs
        var dataDictionary = new Dictionary<string, string>();

        foreach (var line in lines)
        {
            // Split each line by the first colon, extracting key and value
            var parts = line.Split(new[] { ':' }, 2);

            if (parts.Length == 2)
            {
                string key = parts[0].Trim();
                string value = parts[1].Trim();

                // Add the key-value pair to the dictionary
                dataDictionary[key] = value;
            }
        }

        // Create and populate ConversationData from the dictionary
        var data = new ConversationData
        {
            Thought = dataDictionary.GetValueOrDefault("thought"),
            Reason = dataDictionary.GetValueOrDefault("reason"),
            Emotion = dataDictionary.GetValueOrDefault("emotion"),
            SuggestedPrice = dataDictionary.GetValueOrDefault("suggestedPrice"),
            Reaction = dataDictionary.GetValueOrDefault("reaction")
        };

        return data;
    }
}


public class ConvoManager : MonoBehaviour
{
    public void ConvoStarted()
    {
        Managers.Time.StopAndRestartTime(true);
        Managers.Cam.SwitchToDialogueCam();
        Managers.UI.ShowTalkOrNotPanel();
    }

    public void ParseNPCAnswer(string input)
    {
        Parser parser = new Parser();
        ConversationData data = parser.ParseInput(input);

        Debug.Log(data.Thought);
        Debug.Log(data.Reason);
        Debug.Log(data.Emotion);
        Debug.Log(data.SuggestedPrice);
        Debug.Log(data.Reaction);

        Managers.UI.SetNPCAnswerText(data.Reaction);
        Managers.UI.SetStatusText(data.Thought, data.Reason, data.Emotion, data.SuggestedPrice);
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
    }
}
