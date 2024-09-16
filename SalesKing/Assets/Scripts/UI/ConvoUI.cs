using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ConvoUI : MonoBehaviour
{
    public GameObject TalkOrNotPanel;
    public GameObject ChooseItemPanel;
    public GameObject ConvoPanel;

    public TMP_InputField UserText;
    public GameObject NPCSpeechBubble;
    public TextMeshProUGUI NPCSpeechText; 

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

    public void TalkToNPCBtn()
    {
        string playerDialogue = UserText.text;
        Managers.GPT.TalkToNPC(playerDialogue);
        UserText.text = null;

        // �ӽ÷�.. ��ȭ ���ɸ� �ٷ� ���� ���� ����
        // ���⼭����..
        Managers.GPT.ReceiveNPCAnswer();

        NPCSpeechBubble.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        NPCSpeechBubble.transform.DOScale(1f, 0.5f).SetEase(Ease.InOutBounce).SetUpdate(true);

        

        // �������. 
    }

}

