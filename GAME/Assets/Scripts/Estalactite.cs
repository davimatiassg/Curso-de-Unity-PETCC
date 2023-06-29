using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estalactite : MonoBehaviour
{

    [SerializeField] GameObject player;
    Rigidbody2D rb;

    private bool caiu = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 angulo = Quaternion.AngleAxis(-20f, Vector3.forward) * Vector2.down;
        RaycastHit2D raio = Physics2D.Raycast(transform.position, angulo, 100f);

        if (raio.collider != null && raio.collider.gameObject == player)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.velocity = new Vector2(0, -12f);
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if(!caiu && rb.velocity.magnitude >= 1f)
        {
            I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
            if(hit != null) { hit.TakeHit(2); }
        }
        caiu = true;
    }

}
