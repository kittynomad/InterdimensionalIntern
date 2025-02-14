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

    public void Start()
    {
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
        StartCoroutine(TypeAdditional("\nchose a thing"));
        FindObjectOfType<CivilizationStatsManager>().ApplyChoice(_choices.Choices[choiceToApply]);
    }

    public void ApplyChoice()
    {
        try
        {
            string t = _inputField.text;
            ApplyChoice(System.Convert.ToInt32(t));
            _inputField.text = "";
        }
        catch
        {
            Debug.LogError("attempt to convert string to int failed!");
        }
    }

    IEnumerator TypeChoices(string sentence)
    {
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