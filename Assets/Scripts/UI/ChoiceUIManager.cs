using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _choiceTexts;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private ChoiceSet _choices;
    [SerializeField] private bool _retroMode = false;
    [SerializeField] private float _textSpeed;

    private bool isTyping;

    Dictionary<string, string> commands = new Dictionary<string, string>();



    public void Start()
    {
        commands.Add("help", "help");
        commands.Add("choiceLookup", "?");
        //foreach (TextMeshProUGUI t in _choiceTexts)
        //t.text = "";
        for (int i = 0; i < _choices.Choices.Length; i++)
        {
            if (!_retroMode)
                _choiceTexts[i].text = _choices.Choices[i].ToString();
            else
            {
                _choiceTexts[0].text = _choiceTexts[0].text + "\n" + _choices.Choices[i].ToString();
                _inputField.Select();
            }

        }
        StartCoroutine(TypeChoices(_choiceTexts[0].text));
    }

    public void ApplyChoice(int choiceToApply)
    {
        
        try
        {
            FindObjectOfType<CivilizationStatsManager>().ApplyChoice(_choices.Choices[choiceToApply]);
            StartCoroutine(TypeAdditional("\nchose a thing"));
        }
        catch
        {
            StartCoroutine(TypeAdditional("\nInvalid choice!"));
            _inputField.Select();
        }
        
    }

    public void ApplyChoice()
    {
        try
        {
            string t = _inputField.text;

            InputTextHandler(t);
            
        }
        catch
        {
            Debug.LogError("attempt to convert string to int failed!");
        }
    }

    private void InputTextHandler(string t)
    {
        //add entered text to output text
        _choiceTexts[0].maxVisibleCharacters += t.Length;
        _choiceTexts[0].text += "\n>" + t;

        if (t.Substring(t.Length - 1).Equals(commands["choiceLookup"]))
        {
            try
            {
                int i = System.Convert.ToInt32(t.Substring(0, 1));
                print(_choices.Choices[i].ToString());
                StopAllCoroutines();
                StartCoroutine(TypeAdditional("\n"+_choices.Choices[i].ToString()));
                return;
            }
            catch
            {
                InvalidInputHandler();
                return;
            }
        }
        int inputAsInt = -1;
        try
        {
            inputAsInt = System.Convert.ToInt32(t);
            ApplyChoice(inputAsInt);
        }
        catch
        {
            InvalidInputHandler();
        }
        
        
    }

    private void InvalidInputHandler()
    {
        StopAllCoroutines();
        StartCoroutine(TypeAdditional("\nInvalid command!"));
    }

    IEnumerator TypeChoices(string sentence)
    {
        _inputField.text = "";
        isTyping = true;

        _choiceTexts[0].maxVisibleCharacters = 0;
        _choiceTexts[0].text = sentence;
        char[] sentenceCharArray = sentence.ToCharArray();

        for (int i = 0; i < sentenceCharArray.Length; i++)
        {
            char letter = sentenceCharArray[i];

            _choiceTexts[0].maxVisibleCharacters++;


            //wait pre-specified time until printing the next letter
            yield return new WaitForSeconds(_textSpeed);
        }
    }

    IEnumerator TypeAdditional(string sentence)
    {
        _inputField.text = "";
        isTyping = true;

        char[] oldSentenceCharArray = _choiceTexts[0].text.ToCharArray();

        _choiceTexts[0].maxVisibleCharacters = oldSentenceCharArray.Length;
        _choiceTexts[0].text = _choiceTexts[0].text + sentence;

        char[] sentenceCharArray = sentence.ToCharArray();

        for (int i = 0; i < sentenceCharArray.Length; i++)
        {
            char letter = sentenceCharArray[i];

            _choiceTexts[0].maxVisibleCharacters++;


            //wait pre-specified time until printing the next letter
            yield return new WaitForSeconds(_textSpeed);
        }
    }
}