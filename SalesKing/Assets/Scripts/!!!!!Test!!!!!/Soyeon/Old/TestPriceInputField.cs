using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestPriceInputField : MonoBehaviour
{
    TMP_InputField inputField;
    void Start()
    {
        inputField = this.GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(ValidateInput);
    }

    private void ValidateInput(string input)
    {
        float value;
        if (float.TryParse(input, out value))
        {
            inputField.text = value.ToString("F2");
        }
        else
        {
            inputField.text = "";
        }
    }
}
