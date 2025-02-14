using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatWithAssociatedValue
{
    [SerializeField] private Enums.ModifyableStats _stat;
    [SerializeField] private float _value;
}
