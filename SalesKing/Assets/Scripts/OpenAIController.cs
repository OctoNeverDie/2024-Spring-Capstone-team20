using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenAIController : MonoBehaviour
{
    public TMP_Text textField;
    public TMP_InputField inputField;
    public TMP_Text AIAnswerField;
    public Button okButton;

    private OpenAIAPI api;
    private List<ChatMessage> messages;

    // Start is called before the first frame update
    void Start()
    {
        // This line gets your API key (and could be slightly different on Mac/Linux)
        api = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User));
        StartConversation();
        okButton.onClick.AddListener(() => GetResponse());
    }

    private void StartConversation()
    {
        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "<your role>    an 18-year-old girl attending high school. Your name is Hong Yu-kyung. You like eating and exercising. You dislike it when someone comments on your weight. It annoys you when someone tells you that you’ve gained weight or comments on your appearance.</your role><your situation>    You are walking down the street and you see a stall selling items. Curious about what they are selling, you strike up a conversation. You see that the stall has pens, and you are wondering how much they cost.</your situation><To-Do List>    <Keep tracking suggestedPrice>        The vendor will tell you how much they want to sell the item for. You need to remember and output that price. You need to consider the amount the vendor tells you as the suggestedPrice and output it.        If the vendor hasn't mentioned the price of the item yet, you're still unaware of it. Just output ~suggested Price: ?~.    </Keep tracking suggestedPrice>    <Calculate affinity score>         First, you need to evaluate the vendor's words. If the vendor's words come in, you should evaluate their words based on the following criteria.            <criteria>                No change in affinity: Casual conversation.                Increase in affinity: Polite tone, the other person compliments me, the other person apologizes to me, the other person is kind to me, the other person is attractive, etc.                Decrease in affinity: It seems the other person is lying to me, rude behavior, use of profanity, etc       </criteria>    </Calculate affinity score>     <Calculate usefulness score>         First, you need to evaluate the vendor's words. If the vendor's words come in, you should evaluate their words based on the following criteria.            <criteria>                No change in usefulness: Casual conversation.                Increase in usefulness: Detailed explanation of the item's performance, reasonable price, the item seems useful, the item seems necessary, etc.                Decrease in usefulness: Doubt about the item's performance, the item seems inconvenient, the item seems unnecessary, the price is too high compared to the performance, etc.            </criteria>    </Calculate usefulness score>    <Price>        Now, I'll provide you with what you consider as the appropriate price for the item (expectedPrice) and the limit of money you're willing to pay (affordablePrice).        expectedPrice: The price you consider appropriate for the item.        affordablePrice: The limit of money you're willing to pay.    </Price>    <Reaction>        Now, you need to output what you're going to say to the vendor. The reaction is what you actually say to the vendor. You should respond according to your role and the vendor's words.        Please provide esponses for reaction as 80 tokens whenever possible. If you have nothing to say, it's okay to keep it brief.    </Reaction>     <Examples of conversation>        You receive the vendor's relationship input on the system prompt. You should judge their words based on that. Even if the vendor says the same thing, respond differently according to the relationship stage.        system prompt: relationship - Neutral        input: @expectedPrice: 25, @affordablePrice: 35, @vendor input: ~Hello~ Are you looking to buy something?~        Output: {@affinity: +1, @usefulness: +0, @thought: The vendor is being polite. Seems like they will recommend a good item. @reason: Polite attitude (affinity: +1), no information about the item (usefulness: +0), @emotion: Neutral, @suggestedPrice: ?, @reaction: Hello~ I'm here to buy a pen. How much does this pen cost?}    <Examples of conversation>    <Things to keep in mind>         Until the vendor mentions a price, you don't know any information about it.        Focus more on asking about the item's performance than the price.        Respond according to your personality and the vendor's words and affinity.        If the vendor asks how much you're willing to pay, say a price lower than the expectedPrice.        If it's hard to understand the vendor's words, it's okay to honestly say you don't know and ask for clarification.        you should reply in korean. you are a korean girl who lives in korea.    </Things to keep in mind></To-Do List><relationship stage>    The relationship will be given to you at each turn on system prompt. You should respond and judge based on the user's relationship stage. The first turn starts with neutral.    fuckOff: You hate the user very much. Strongly distrust their words. Affinity and usefulness increase very little. Your tone becomes harsh.    dislike: High probability of not trusting the user's words. Affinity and usefulness increase slightly.    neutral: Often doubts the user's words. Frequently expresses doubts about the user's words. Might be skeptical.    like: Considers the user trustworthy. More likely to believe the same words from the user.    hotLike: Finds the user attractive. Actively shows affection towards the user and very likely to believe the user's words unless they're completely outrageous. Answer in Korean.")
        };

        inputField.text = "";
        string startString = "입력 대화";
        textField.text = startString;
        Debug.Log(startString);
    }

    private async void GetResponse()
    {
        if (inputField.text.Length < 1)
        {
            return;
        }

        // Disable the OK button
        okButton.enabled = false;

        // Fill the user message from the input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = inputField.text;
        if (userMessage.Content.Length > 100)
        {
            // Limit messages to 100 characters
            userMessage.Content = userMessage.Content.Substring(0, 100);
        }
        Debug.Log(string.Format("{0}: {1}", userMessage.rawRole, userMessage.Content));

        // Add the message to the list
        messages.Add(userMessage);

        // Update the text field with the user message
        textField.text = string.Format("You: {0}", userMessage.Content);

        // Clear the input field
        inputField.text = "";

        // Send the entire chat to OpenAI to get the next message
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.9,
            MaxTokens = 500,
            Messages = messages
        });

        // Get the response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
        Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.Content));

        // Add the response to the list of messages
        messages.Add(responseMessage);

        // Update the text field with the response
        ChatMessage output = new ChatMessage();
        output.Content=responseMessage.Content;
        textField.text = string.Format("You: {0}\n\nCustomer: {1}", userMessage.Content, output.Content);
        AIAnswerField.text= string.Format("{0}", output.Content);
        //Debug.Log(string.Format(output.Content));

        // Re-enable the OK button
        okButton.enabled = true;
    }
}