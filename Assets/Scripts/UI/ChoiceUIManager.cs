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

    public void Start()
    {
        foreach (TextMeshProUGUI t in _choiceTexts)
            t.text = "";
        for(int i = 0; i < _choices.Choices.Length; i++)
        {
            if(!_retroMode)
                _choiceTexts[i].text = _choices.Choices[i].ToString();
            else
            {
                _choiceTexts[0].text = _choiceTexts[0].text + "\n" + _choices.Choices[i].ToString();
                _inputField.Select();
            }
                
        }
    }

    public void ApplyChoice(int choiceToApply)
    {
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
}
