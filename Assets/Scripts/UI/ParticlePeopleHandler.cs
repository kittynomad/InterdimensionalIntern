using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePeopleHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    public void SetPopChange(int popChange)
    {
        var emission = ps.emission;
        emission.rateOverTime = popChange;
    }
}
