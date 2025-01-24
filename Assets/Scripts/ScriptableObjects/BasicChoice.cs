using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BasicChoice : ScriptableObject
{
    [SerializeField] private string _choiceName;
    [SerializeField] private string _choiceDescription;

    [SerializeField] private StatModifier[] _statModifiers;
}
