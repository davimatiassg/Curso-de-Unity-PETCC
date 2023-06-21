using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    [SerializeField] private int dmg;

    public Rigidbody2D rb;
    public Transform trs;


    public void OnCollisionEnter2D(Collision2D col)
    {
        if(rb.velocity.magnitude >= 1f)
        {
            I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
            if(hit != null) { hit.TakeDmg(1); }
        }
              
    }
}
