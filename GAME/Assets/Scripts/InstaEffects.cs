using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaEffects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float duration = GetComponent<ParticleSystem>().main.duration;
        Destroy(this.gameObject, duration);
    }
}
