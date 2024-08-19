using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimTest : MonoBehaviour
{
    NPCMove move;

    void Start()
    {
        move = GetComponent<NPCMove>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.SlightlyPositive);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Positive);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.VeryPositive);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.SlightlyNegative);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Negative);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.VeryNegative);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            move.PlayRandomNPCAnim(NPCDefine.AnimType.Attack);
        }
    }

}
