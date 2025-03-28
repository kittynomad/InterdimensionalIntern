using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePeopleHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;

    public ParticleSystem Ps { get => ps; set => ps = value; }

    public void SetPopChange(int popChange)
    {
        var emission = ps.emission;
        emission.rateOverTime = popChange;
    }
}
