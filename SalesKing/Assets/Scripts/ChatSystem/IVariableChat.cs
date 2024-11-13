using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVariableChat
{
    public void UserInput(string type, string user_input);
    public void GptOutput(string type, string gpt_output);
    private struct GptResult { };
}