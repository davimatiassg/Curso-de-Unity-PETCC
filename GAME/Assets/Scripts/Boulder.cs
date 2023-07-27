using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField] private int maxDmg = 4;

    public Rigidbody2D rb;
    public Transform trs;

    [SerializeField] AudioClip roll;

    /// Update é chamado uma vez por frame
    public void Update()
    {
        /// Se a pedra tem uma velocidade perceptível
        if (rb.velocity.magnitude > 0.001)
        {
            GetComponent<sfxScript>().playSoundContinuously(roll, 1f, 0.001f/rb.velocity.magnitude);
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        float relVel = col.relativeVelocity.magnitude;
        
        /// Se a velocidade relativo entre o que está sendo colidido e a pedra for grande o bastante
        /// e se a velocidade da pedra for grande o bastante
        if(relVel >= 4f && rb.velocity.magnitude >= 4f)
        {
            I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
            if(hit != null) 
            { 
                /// Aplicando dano no objeto colidido
                hit.TakeHit(Mathf.CeilToInt(maxDmg * Mathf.InverseLerp(4f, 20f, relVel)), col.GetContact(0).point); 
            }
        }
              
    }



}
