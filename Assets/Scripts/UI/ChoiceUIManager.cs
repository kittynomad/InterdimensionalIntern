using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceUIManager : MonoBehaviour
{
    [SerializeField] private CivilizationStatsManager _statsManager;

    [SerializeField] private TextMeshProUGUI[] _choiceTexts;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private ChoiceSet _choices;
    [SerializeField] private bool _retroMode = false;
    [SerializeField] private float _textSpeed;

    private bool isTyping;
    private StageManager sm;

    Dictionary<string, string> commands = new Dictionary<string, string>();


    public ChoiceSet Choices { get => _choices; set => _choices = value; }

    public void Start()
    {
        commands.Add("help", "help");
        commands.Add("choiceLookup", "?");
        commands.Add("userInputIndicator", ">");
        sm = FindObjectOfType<StageManager>();
        _choiceTexts[0].text = "";
        _choiceTexts[0].maxVisibleCharacters = 0;
        StartCoroutine(TypeAdditional("type help for commands"));
        
        
    }
    private void OnEnable()
    {
        _statsManager.PopUpManager.PopUpCanvas.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        _statsManager.PopUpManager.PopUpCanvas.gameObject.SetActive(true);
    }

    public void DisplayNewChoices()
    {
        print(sm.Stages[sm.CurStage].ChoiceSets.Length);
        _choices = GetNextChoiceSet();
        foreach (TextMeshProUGUI t in _choiceTexts)
            t.text = "";

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
        StartCoroutine(TypeChoices(_choiceTexts[0].text + "\ntype \"help\" for a list of commands"));
    }

    public void ApplyChoice(int choiceToApply)
    {
        
        try
        {
            
            StartCoroutine(TypeAdditional("\nchose a thing", choiceToApply));
        }
        catch
        {
            StartCoroutine(TypeAdditional("\nInvalid choice!"));
            
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
        //_choiceTexts[0].maxVisibleCharacters += (t.Length + commands["userInputIndicator"].Length + 1);
        _choiceTexts[0].text += "\n" + commands["userInputIndicator"] + t;

        if(t.Equals(commands["help"])) //display commands
        {
            DisplayCommands();
            return;
        }

        if (t.Substring(t.Length - 1).Equals(commands["choiceLookup"])) //display info about a choice
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
        try //choose an option
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
        StartCoroutine(TypeAdditional("\nInvalid command!\nType \"help\" for list of commands"));
    }

    private void DisplayCommands()
    {
        StartCoroutine(TypeAdditional("\nType choice num. + \"?\" to display choice info \n(i.e. \"0?\" for choice 0)"
            +"\nType choice number to choose that option\n(i.e. type \"1\" for choice 1)"));
    }

    private ChoiceSet GetNextChoiceSet(bool weighted = true)
    {
        if (!weighted) //return truly random choiceSet if not accounting for weight
            return sm.Stages[sm.CurStage].ChoiceSets[(int)Random.Range(0, sm.Stages[sm.CurStage].ChoiceSets.Length)];

        float totalWeight = 0f;
        CivStage stage = sm.Stages[sm.CurStage];

        foreach (ChoiceSet c in stage.ChoiceSets)
            totalWeight += c.Weight;

        for(int i = 0; i < stage.ChoiceSets.Length - 1; i++)
        {
            if(Random.Range(0, totalWeight) < stage.ChoiceSets[i].Weight)
            {
                return stage.ChoiceSets[i];
            }

            totalWeight -= stage.ChoiceSets[i + 1].Weight;
        }

        return stage.ChoiceSets[stage.ChoiceSets.Length - 1];
    }

    IEnumerator TypeChoices(string sentence)
    {
        _inputField.DeactivateInputField();
        _inputField.text = "";
        isTyping = true;

        _choiceTexts[0].maxVisibleCharacters = 0;
        _choiceTexts[0].text = sentence;
        char[] sentenceCharArray = sentence.ToCharArray();

        int length = GetStringLengthWithoutRichText(_choiceTexts[0].text);

        for (int i = 0; i < length; i++)
        {
            
            _choiceTexts[0].maxVisibleCharacters++;

            //wait pre-specified time until printing the next letter
            yield return new WaitForSeconds(_textSpeed);
        }
        _inputField.ActivateInputField();
        _inputField.Select();
    }

    IEnumerator TypeAdditional(string sentence, int choiceToApply = -1)
    {
        _inputField.DeactivateInputField();
        _inputField.text = "";
        isTyping = true;
        bool isTag = false;

        char[] oldSentenceCharArray = _choiceTexts[0].text.ToCharArray();

        _choiceTexts[0].maxVisibleCharacters = GetStringLengthWithoutRichText(_choiceTexts[0].text);
        int oldLength = _choiceTexts[0].maxVisibleCharacters;
        _choiceTexts[0].text = _choiceTexts[0].text + sentence;

        char[] sentenceCharArray = sentence.ToCharArray();

        int length = GetStringLengthWithoutRichText(_choiceTexts[0].text);

        for (int i = oldLength; i < length; i++)
        {
            //char letter = sentenceCharArray[i];

            _choiceTexts[0].maxVisibleCharacters++;

            //wait pre-specified time until printing the next letter
            yield return new WaitForSeconds(_textSpeed);
        }


        if (choiceToApply != -1)
        {
            yield return new WaitForSeconds(1f);
            FindObjectOfType<CivilizationStatsManager>().ApplyChoice(_choices.Choices[choiceToApply]);
            _choices = null;
            FindObjectOfType<StageManager>().OnChoice();
        }
        _inputField.ActivateInputField();
        _inputField.Select();
    }

    private int GetStringLengthWithoutRichText(string t)
    {
        int output = 0;
        bool isTag = false;

        char[] sentenceCharArray = t.ToCharArray();

        for(int i = 0; i < sentenceCharArray.Length; i++)
        {
            char letter = sentenceCharArray[i];

            if (letter.ToString().Equals("<"))
                isTag = true;
            if (letter.ToString().Equals(">"))
            {
                isTag = false;
                output += 1;
                continue;
            }
            if (!isTag)
                output++;
        }

        Debug.Log("Total Length: " + sentenceCharArray.Length
            + "\nCalculated Length: " + output);

        return output;

    }

}