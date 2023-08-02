using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaEffects : MonoBehaviour
{
    /// Start é chamado antes do primeiro update de frame
    void Start()
    {
        /// Destruindo o objeto ao fim das partículas
        float duration = GetComponent<ParticleSystem>().main.duration;
        Destroy(this.gameObject, duration);
    }
}
