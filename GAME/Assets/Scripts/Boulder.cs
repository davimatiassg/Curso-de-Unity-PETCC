using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField] private int dmg;

    public Rigidbody2D rb;
    public Transform trs;

    [SerializeField] AudioClip roll;

    public void Update()
    {
        if (rb.velocity.magnitude > 0)
        {
            //GetComponent<sfxScript>().playSoundContinuously(roll); // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if(rb.velocity.magnitude >= 1f)
        {
            I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
            if(hit != null) { hit.TakeHit(1); }
        }
              
    }



}
