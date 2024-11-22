using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Util 
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static T ExtractValue<T>(string input, string pattern) where T : struct, IConvertible
    {
        var match = Regex.Match(input, pattern);
        if (match.Success)
        {
            string value = match.Groups[1].Value;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (FormatException) 
            { 
                return default(T);//if error, returns 0
            }
        }
        return default;
    }

    public static void ChangeSprite(Image image, Sprite sprite)
    {
        image.sprite = sprite;

        float width = sprite.rect.width;
        float height = sprite.rect.height;
        float ratio = width / height;

        height = image.rectTransform.rect.height;
        width = height * ratio;

        image.rectTransform.sizeDelta = new Vector2(width, height);
    }

    public static string Concat(string pattern, string gptAnswer)
    {
        Match match = Regex.Match(gptAnswer, pattern);

        if (match.Success)
            return match.Groups[1].Value;
        return string.Empty;
    }
}
