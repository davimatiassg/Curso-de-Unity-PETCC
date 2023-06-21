using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, I_HitableObj
{
    private int Hp = 5;

    private float H_axis;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jump = 20f;

    [SerializeField] private float atkCd = 0.25f;
    [SerializeField] private float atkc = 0;
    [SerializeField] private GameObject pellet;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask solid;

    [SerializeField] private Transform floorCheck;
    [SerializeField] private Transform sling;

    // Update is called once per frame
    void Update()
    {
        H_axis = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isOnGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }

        if(H_axis != 0)
        {
            Flip();
        }
        

        if(atkc > 0) { atkc -= Time.deltaTime; }
        if (Input.GetButtonDown("Fire1") && atkc <= 0)
        {
            shoot();
            atkc = atkCd;
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(H_axis * speed, rb.velocity.y);
    }

    private bool isOnGround()
    {
        return Physics2D.OverlapCircle(floorCheck.position, 0.25f, solid);
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(H_axis);
        transform.localScale = scale;
    }

    public void TakeDmg(int dmg)
    {   
        Hp -= dmg;
        Debug.Log("Player: levou dano ;-;");
        if(Hp <= 0)
        {
            //Play Death Anim.
            Destroy(this.gameObject, 2f); //provisório
        }
    }

    public void shoot()
    {
        GameObject shot = Instantiate(pellet);
        Pellet p = shot.GetComponent<Pellet>();
        p.trs.position = transform.TransformPoint(sling.localPosition);
        p.rb.velocity = new Vector2(1*p.speed * transform.localScale.x, 0.2f);
    }

}
