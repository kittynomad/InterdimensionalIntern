using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BasicChoice : ScriptableObject
{
    [SerializeField] private string _choiceName;
    [SerializeField] private string _choiceDescription;

    [SerializeField] private StatModifier[] _statModifiers;

    Dictionary<Enums.ModifyableStats, string> statColors = new Dictionary<Enums.ModifyableStats, string>()
    {
        {Enums.ModifyableStats.happiness, "yellow" },
        {Enums.ModifyableStats.happinessGrowth, "orange" },
        {Enums.ModifyableStats.population, "green" },
        {Enums.ModifyableStats.populationGrowth, "green" },
        {Enums.ModifyableStats.resources, "orange" },
        {Enums.ModifyableStats.temperature, "purple" },
        {Enums.ModifyableStats.temperatureGrowth, "purple" },
    };
    [SerializeField] private Animation _choiceAnimation;

    public StatModifier[] StatModifiers { get => _statModifiers; set => _statModifiers = value; }
    public string ChoiceName { get => _choiceName; set => _choiceName = value; }
    public Animation ChoiceAnimation { get => _choiceAnimation; set => _choiceAnimation = value; }

    public override string ToString()
    {
        string output = _choiceName + "\n" + _choiceDescription + "\n";
        foreach(StatModifier s in StatModifiers)
        {
            if(!s.HiddenModification)
                output = output + "<color=" + statColors[s.StatToModify] + ">" + s.StatToModify + "</color> " + s.ModificationToPerform + " " + s.ModificationValue + "\n";
        }
        return output;
    }
}
