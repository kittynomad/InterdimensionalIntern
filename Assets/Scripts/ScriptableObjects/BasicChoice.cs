using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BasicChoice : ScriptableObject
{
    [SerializeField] private string _choiceName;
    [SerializeField] private string _choiceDescription;

    [SerializeField] private StatModifier[] _statModifiers;

    Dictionary<Enums.ModifyableStats, string> statColors = new Dictionary<Enums.ModifyableStats, string>();

    public StatModifier[] StatModifiers { get => _statModifiers; set => _statModifiers = value; }
    public string ChoiceName { get => _choiceName; set => _choiceName = value; }

    public override string ToString()
    {
        string output = _choiceName + "\n" + _choiceDescription + "\n";
        foreach(StatModifier s in StatModifiers)
        {
            if(!s.HiddenModification)
                output = output + "<color=green>" + s.StatToModify + "</color> " + s.ModificationToPerform + " " + s.ModificationValue + "\n";
        }
        return output;
    }
}
