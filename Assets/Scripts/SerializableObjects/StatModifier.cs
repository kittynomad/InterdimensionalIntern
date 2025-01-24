using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatModifier
{
    [SerializeField] private Enums.ModifyableStats _statToModify;
    [SerializeField] private float _modificationValue;
}
