using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatModifier
{
    [SerializeField] private Enums.ModifyableStats _statToModify;
    [SerializeField] private float _modificationValue;
    [SerializeField] private bool _hiddenModification;

    public Enums.ModifyableStats StatToModify { get => _statToModify; set => _statToModify = value; }
    public float ModificationValue { get => _modificationValue; set => _modificationValue = value; }
    public bool HiddenModification { get => _hiddenModification; set => _hiddenModification = value; }
}
