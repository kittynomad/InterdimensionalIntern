using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BasicChoice : ScriptableObject
{
    [SerializeField] private string _choiceName;
    [SerializeField] private string _choiceDescription;

    [SerializeField] private StatModifier[] _statModifiers;

    public override string ToString()
    {
        string output = _choiceName + "\n" + _choiceDescription + "\n";
        foreach(StatModifier s in _statModifiers)
        {
            if(!s.HiddenModification)
                output = output + s.StatToModify + " change by " + s.ModificationValue + "\n";
        }
        return output;
    }
}
