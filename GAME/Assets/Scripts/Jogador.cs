using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador : MonoBehaviour, I_ObjAtacavel
{
    private int Hp = 5;

    private float dirHorizontal;
    private float velocidade = 10f;
    private float pulo = 20f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform verificadorDePiso;
    [SerializeField] private LayerMask piso;

    // Update is called once per frame
    void Update()
    {
        dirHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && noChao())
        {
            rb.velocity = new Vector2(rb.velocity.x, pulo);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirHorizontal * velocidade, rb.velocity.y);
    }

    private bool noChao()
    {
        return Physics2D.OverlapCircle(verificadorDePiso.position, 0.25f, piso);
    }

    private void Flip()
    {
        Vector3 escala = transform.localScale;
        escala.x = Mathf.Sign(dirHorizontal);
        transform.localScale = escala;
    }

    public void TakeDmg(int dmg)
    {   
        Hp -= dmg;
        Debug.Log("Player: level dano ;-;");
    }

}
