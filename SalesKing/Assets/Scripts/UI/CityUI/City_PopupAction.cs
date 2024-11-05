using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City_PopupAction : MonoBehaviour
{
    void Start()
    {
        
    }

    // 물건을 ~~ 파시면 됩니다 패널 ok 눌렀을 때
    public void OnClickOKStartPanel()
    {
        Managers.Player.MyPlayer.GetComponent<Player>().FreezeAndUnFreezePlayer(false);
    }

}
