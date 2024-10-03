using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVariableChat
{
    public void Input(string user_input);
    public void Output(string gpt_output);
}
