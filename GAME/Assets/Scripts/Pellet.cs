using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public float speed;
    [SerializeField] private int dmg;

    public Rigidbody2D rb;
    public Transform trs;


    public void OnCollisionEnter2D(Collision2D col)
    {
        I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
        if(hit != null)
        {
            hit.TakeDmg(dmg);
        }
        Destroy(this.gameObject, 0.3f);
    }
}
