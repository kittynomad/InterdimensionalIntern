using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ChoiceSet : ScriptableObject
{
    [SerializeField] private string _choiceSetName;
    [SerializeField] private Enums.CivilizationPhase _choicePhase;
    [SerializeField] private BasicChoice[] _choices; //set of choices available in this set
    [SerializeField] [Range(0.0f, 100.0f)] private float _weight = 1f;

    public string ChoiceSetName { get => _choiceSetName; set => _choiceSetName = value; }
    public Enums.CivilizationPhase ChoicePhase { get => _choicePhase; set => _choicePhase = value; }
    public BasicChoice[] Choices { get => _choices; set => _choices = value; }
    public float Weight { get => _weight; set => _weight = value; }
}
