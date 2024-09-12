using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoUI : MonoBehaviour
{
    public GameObject TalkOrNotPanel;
    public GameObject ChooseItemPanel;
    public GameObject ConvoPanel;

    public void YesTalkBtn()
    {
        ChooseItemPanel.SetActive(true);
        TalkOrNotPanel.SetActive(false);
    }

    public void NoTalkBtn()
    {
        Managers.Turn.ConvoFinished();
        TalkOrNotPanel.SetActive(false);
        ConvoPanel.SetActive(false);
    }

    public void SelectItemBtn()
    {
        ChooseItemPanel.SetActive(false);
        ConvoPanel.SetActive(true);
    }
}

