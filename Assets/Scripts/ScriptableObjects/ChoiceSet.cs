using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ChoiceSet : ScriptableObject
{
    [SerializeField] private string _choiceSetName;
    [SerializeField] private Enums.CivilizationPhase _choicePhase;
    [SerializeField] private BasicChoice[] _choices;
}
