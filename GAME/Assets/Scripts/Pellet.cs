using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public float speed;
    [SerializeField] private int dmg;

    [SerializeField] AudioClip pelletHitSound;

    public Rigidbody2D rb;
    public Transform trs;
    private bool damaged;


    public void OnCollisionEnter2D(Collision2D col)
    {
        if(!damaged && col.gameObject.tag != "Player")
        {
            I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
            Destroy(this.gameObject, 0.15f);
            if(hit != null) 
            { 
                hit.TakeHit(dmg); 
                damaged = true;
            }     
            
        }
        GetComponent<AudioSource>().PlayOneShot(pelletHitSound);
    }
}
