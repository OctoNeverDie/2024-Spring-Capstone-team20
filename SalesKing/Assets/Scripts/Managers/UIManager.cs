using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.Json;
using Newtonsoft.Json;
using System;
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


public class UIManager : MonoBehaviour
{
    GameObject myCanvas;
    ConvoUI ui;

    void Start()
    {
        myCanvas = GameObject.Find("Canvas");
        ui = myCanvas.GetComponent<ConvoUI>();
        SetTimeText();
        SetTurnText();
    }

    private void Update()
    {
        SetTimeText();
    }

    public void ShowTalkOrNotPanel()
    {
        ui.TalkOrNotPanel.SetActive(true);
    }

    public void SetNPCAnswerText(string text)
    {
        // 주어진 텍스트의 형식: 
        /*
        string input = @"{
            ""thought"": ""주연님이 계속 인사만 하네요. 뭔가 더 이야기하고 싶어하는 것 같은데, 제안이 필요할까요?"",
            ""reason"": ""일반적인 대화 (affinity: +0), 물건에 대한 정보 없음 (persuasion: +0)"",
            ""emotion"": ""중립"",
            ""suggestedPrice"": ""?"",
            ""reaction"": ""하이하이! 주연님, 혹시 어떤 물건을 판매하고 계신가요? 도움이 될 수 있는 물건이 있다면 듣고 싶어요!""
        }";
        */
        Parser parser = new Parser();
        ConversationData data = parser.ParseInput(text);

        // Now you can access each field like this
        Debug.Log(data.Thought);
        Debug.Log(data.Reason);
        Debug.Log(data.Emotion);
        Debug.Log(data.SuggestedPrice);
        Debug.Log(data.Reaction);

        ui.NPCSpeechText.text = data.Reaction;
        ui.TalkToNPCBtn();
    }

    public void SetStatusText(int suggestedPrice, int persuasion)
    {
        ui.StatusText.text = "Suggested Price: "+suggestedPrice+"\nPersuasion: "+persuasion;
    }

    public void SetTimeText()
    {
        int hour = Managers.Turn.Hour;
        int minute = Managers.Turn.Minute;

        string hour_s;
        string min_s;
        string ampm;

        if(hour < 10) hour_s = "0"+hour;
        else hour_s = hour.ToString();

        if (minute < 10) min_s = "00";
        else min_s = minute/10+"0";

        if (hour < 12) ampm = "AM";
        else ampm = "PM";

        ui.TimeText.text = hour_s+" : "+min_s+" "+ampm;
    }

    public void SetTurnText()
    {
        ui.TurnText.text = Managers.Turn.curTurn+" / 3";
    }

}
