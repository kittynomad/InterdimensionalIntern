using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _choiceOneText;
    [SerializeField] private TextMeshProUGUI _choiceTwoText;
    [SerializeField] private BasicChoice[] _choices;

    public void Start()
    {
        _choiceOneText.text = _choices[0].ToString();
        _choiceTwoText.text = _choices[1].ToString();
    }
}
