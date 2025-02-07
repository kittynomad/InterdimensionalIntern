using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _choiceTexts;
    [SerializeField] private ChoiceSet _choices;

    public void Start()
    {
        for(int i = 0; i < _choices.Choices.Length; i++)
        {
            _choiceTexts[i].text = _choices.Choices[i].ToString();
        }
    }
}
