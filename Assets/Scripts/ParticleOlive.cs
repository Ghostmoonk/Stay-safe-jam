using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOlive : MonoBehaviour
{
    [SerializeField] ParticleSystem trail;

    public void PlayParticle()
    {
        trail.Play();
    }
}
