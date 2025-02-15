using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _choiceTexts;
    [SerializeField] private ChoiceSet _choices;

    public ChoiceSet Choices { get => _choices; set => _choices = value; }

    public void Start()
    {
        for(int i = 0; i < _choices.Choices.Length; i++)
        {
            _choiceTexts[i].text = _choices.Choices[i].ToString();
        }
    }

    public void ApplyChoice(int choiceToApply)
    {
        FindObjectOfType<CivilizationStatsManager>().ApplyChoice(_choices.Choices[choiceToApply]);
    }
}
