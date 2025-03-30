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
    private CivilizationStatsManager csm;
    private int secretFinders = 0;

    Dictionary<string, string> commands = new Dictionary<string, string>() 
    {
        {"choiceLookup", "?"},
        {"userInputIndicator", ">"},
        {"checkCivStats", "?civStats" },
    };

    Dictionary<string, System.Action> commandActions = new Dictionary<string, System.Action>();


    public ChoiceSet Choices { get => _choices; set => _choices = value; }

    public void Start()
    {
        SetUpCommands();
        csm = FindObjectOfType<CivilizationStatsManager>();
        sm = FindObjectOfType<StageManager>();
        _choiceTexts[0].text = "";
        _choiceTexts[0].maxVisibleCharacters = 0;
        StartCoroutine(TypeAdditional("type help for commands"));
        
        
    }

    //commands have to be set up during runtime which is why commandActions isn't instatiated w/ these entries
    private void SetUpCommands()
    {
        commandActions.Add("help", DisplayCommands);
        commandActions.Add("?civStats", DisplayCurrentStats);
        commandActions.Add("bored", DisplayWhatToDo);
        commandActions.Add("bored1", DisplayWhatToDo);
        commandActions.Add("bored2", InputTipsCommand);
        commandActions.Add("funCommand", DisplayFunCommand);
        commandActions.Add("animal", DisplayAnimal);
        commandActions.Add("credits", DisplayCredits);
        commandActions.Add("frown", FrownCommand);
        commandActions.Add("rapture", RaptureCommand);
        commandActions.Add("waste", WasteCommand);
        commandActions.Add("cow", WarmCommand);
        commandActions.Add("wasd", WASDCommand);
        commandActions.Add("qwerty", QWERTYCommand);
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
                _choiceTexts[0].text = _choiceTexts[0].text + "\n" + i + ": " + _choices.Choices[i].ToString();
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
        _choiceTexts[0].text += "\n" + commands["userInputIndicator"] + t + "\n";

        try
        {
            if (commandActions.ContainsKey(t)) //display commands
            {
                commandActions[t]();
                return;
            }
        }
        catch
        {

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

    #region commandFunctions

    private void InvalidInputHandler()
    {
        StopAllCoroutines();
        StartCoroutine(TypeAdditional("\nInvalid command!\nType \"help\" for list of commands"));
    }

    private void DisplayCommands()
    {
        StartCoroutine(TypeAdditional("\nType choice num. + \"?\" to display choice info \n(i.e. \"0?\" for choice 0)"
            +"\nType choice number to choose that option\n(i.e. type \"1\" for choice 1)"
            +"\nType ?civStats to see exact current stats of civilization"
            +"\nIf you have idle hands, you could try typing 'bored'...? "
            +"\n(NOTE: all commands are case sensitive)"));
    }

    private void DisplayCurrentStats()
    {
        StartCoroutine(TypeAdditional(csm.ToString()));
    }

    private void DisplayWhatToDo()
    {
        string s = "\nNO IDLE HANDS, INTERN!\n" 
            +"----------------------------------\n"
            +"If you're out of things to do, try entering random words/phrases into the terminal.\n"
            +"There are several hidden commands, which do a variety of random things.\n"
            +"If you find a 'funCommand', write it down on a sticky note for future interns!" +
            "\n(bored 1/2)";

        StartCoroutine(TypeAdditional(s));
    }

    private void DisplayFunCommand()
    {
        secretFinders++;
        string s = "CONGRATS, INTERN! YOU FOUND A HIDDEN COMMAND!"
            + "\nYou are Intern #" + secretFinders + " to use this command.";
        StartCoroutine(TypeAdditional(s));
    }

    private void DisplayAnimal()
    {
        string[] animals = { "Dog!\nDog says 'Woof, Woof!'", "Cat!\nCat says 'Meow, Meow!'", "Bird!\nBird says 'Tweet, Tweet!'" };

        string s = "TODAY'S ANIMAL IS: " + animals[(int)Random.Range(0, animals.Length)];

        StartCoroutine(TypeAdditional(s));
    }

    private void DisplayCredits()
    {
        string s = "CREDITS WIP";
        StartCoroutine(TypeAdditional(s));
    }

    private void FrownCommand()
    {
        csm.Happiness -= 1;
        string s = "Someone, somewhere, is now having a very bad day...\n(Happiness lowered by 1%)";
        StartCoroutine(TypeAdditional(s));
    }

    private void RaptureCommand()
    {
        csm.Population -= 1;
        string s = "One of your citizens have just been raptured!\n(Population lowered by 1)";
        StartCoroutine(TypeAdditional(s));
    }

    private void WasteCommand()
    {
        csm.Resources -= 1;
        string s = "Did someone's house just get robbed??\n(Resources lowered by 1)";
        StartCoroutine(TypeAdditional(s));
    }

    private void WarmCommand()
    {
        csm.Temperature += 1;
        string s = "A cow somewhere is... oh...\n(Temperature increased by 1Z)";

        StartCoroutine(TypeAdditional(s));
    }

    private void WASDCommand()
    {
        string s = "!!WARNING!! GAMER DETECTED!\nAPPROPRIATE AUTHORITIES HAVE BEEN NOTIFIED";
        StartCoroutine(TypeAdditional(s));
    }

    private void QWERTYCommand()
    {
        string s = "Running out of ideas?\n";
        StartCoroutine(TypeAdditional(s));
    }

    private void InputTipsCommand()
    {
        string s = "*Most commands use camelCase capitalization. (no spaces, second word onward capitalized" +
            "\n*Most commands are case sensitive, so keep the above tip in mind." +
            "\n*Observe your surroundings for inspiration for words/phrases to try." +
            "\n*All commands/output will be interrupted when a choice is ready to be made." +
            "\n(bored 2/2)";
        StartCoroutine(TypeAdditional(s));
    }

    private void StaticClearCommand()
    {
        string s = "Whoops, spaced out for a moment there.\nSystem Restored";
        StartCoroutine(TypeAdditional(s));
    }

    #endregion

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