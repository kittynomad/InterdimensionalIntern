using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatWithAssociatedValue
{
    [SerializeField] private Enums.ModifyableStats _stat;
    [SerializeField] private float _value;

    public Enums.ModifyableStats Stat { get => _stat; set => _stat = value; }
    public float Value { get => _value; set => _value = value; }
}
