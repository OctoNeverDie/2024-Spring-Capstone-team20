using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VariableList
{
    public static string S_GPTAnswer { get; set; }

    public static int S_Affinity { get; set; }
    public static int S_Usefulness { get; set; }
    public static int S_AlphaPrice { get; set; }
    public static int S_DefaultPrice { get; set; }


    //4 Things to send-------------------------------
    public static int S_ExpectedPrice { get; set; }
    public static int S_AffordablePrice { get; set; }
    public static string S_Relationship { get; set; }
    public static string S_UserAnswer { get; set; }

    static VariableList()
    {
        S_Affinity = 0;
        S_Usefulness = 0;

        S_DefaultPrice = 10;
        //S_DefaultPrice = (int)Math.Ceiling(S_DefaultPrice * 1.1); //이거는 나중에 
        S_ExpectedPrice = S_DefaultPrice; 
        S_AlphaPrice = 0;
        S_AffordablePrice = S_ExpectedPrice+ S_AlphaPrice;

        S_UserAnswer = "";
        S_Relationship = "neutral";
    }
}
