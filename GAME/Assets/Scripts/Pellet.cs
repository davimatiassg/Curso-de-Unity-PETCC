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
    private bool damaged; //!< Se já deu dano em algo

    public void OnCollisionEnter2D(Collision2D col)
    {
        /// Se está ainda não deu dano e está colidindo com o player
        if(!damaged && col.gameObject.tag != "Player")
        {
            I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();

            if(hit != null)
            { 
                hit.TakeHit(dmg, col.GetContact(0).point); 
                damaged = true;
            }     

            Destroy(this.gameObject, 0.15f);
            
        }
        GetComponent<AudioSource>().PlayOneShot(pelletHitSound);
    }
}
