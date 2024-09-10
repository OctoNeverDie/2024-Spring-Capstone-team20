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

    }

    public void NoTalkBtn()
    {
        Managers.Turn.ConvoFinished();
        TalkOrNotPanel.SetActive(false);
    }
}

