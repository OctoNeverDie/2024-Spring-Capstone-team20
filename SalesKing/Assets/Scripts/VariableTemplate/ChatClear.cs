using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatClear : MonoBehaviour
{
    public void OnClick()
    {
        TemplateSend templateSend = new TemplateSend();
        templateSend.EndGPT();
    }
}
