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


    public void OnCollisionEnter2D(Collision2D col)
    {
        I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
        if(col.gameObject.tag != "Player")
        {
            Destroy(this.gameObject, 0.3f);
            if(hit != null) { hit.TakeHit(dmg); }      
        }

        GetComponent<AudioSource>().PlayOneShot(pelletHitSound);
    }
}
