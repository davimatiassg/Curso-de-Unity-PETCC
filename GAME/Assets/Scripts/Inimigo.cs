using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public GameObject Player;
    private float velocidade = 3f;
    private float pulo = 25f;
    private int ataque = 1;

    private float disMax = 8f;
    private float disMin = 1.1f;
    private float disAtk = 1.1f;
    private float disPulo = 1.6f;

    private float puloFreq = 1.5f;
    private float tPulo = 0f;
    
    private float atkCd = 1.5f;
    private float tAtk = 0f;

    [SerializeField] private Transform verificadorDePiso;
    [SerializeField] private LayerMask piso;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 dirPerseguir = Player.transform.position - transform.position;
        float disHorizontal = dirPerseguir.x;
        float disVertical = dirPerseguir.y;

        /// Define a componente vertical como zero para o objeto não voar
        Vector2 dirPerseguirHorizontal = new Vector2 (dirPerseguir.x, 0f);

        float disPlayer = Vector2.Distance(transform.position, Player.transform.position);

        if (Mathf.Abs(disHorizontal) > disMin && Mathf.Abs(disHorizontal) < disMax)
        {
            /// Persegue o Player
            rb.velocity = new Vector2((dirPerseguirHorizontal.normalized * velocidade).x, rb.velocity.y);
        }
        else
        {
            /// Fica parado
            rb.velocity = new Vector2(0f, rb.velocity.y);

            /// Ataca o Player se eles estiver em alcançe de ataque
            AtacarPlayer();
        }

        /// Pulo
        if (disVertical > disPulo && isOnGround())
        {
            if (tPulo >= puloFreq/Time.deltaTime)
            {
                rb.velocity = new Vector2(rb.velocity.x, pulo);
                tPulo = 0f;
            }
            tPulo ++;
        }
        else {
            tPulo = 0f;
        }

    }

    private bool isOnGround()
    {
        return Physics2D.OverlapCircle(verificadorDePiso.position, 0.25f, piso);
    }


    /// Chama o método 'TakeDmg' do Player acada 'atkCd' segundos
    void AtacarPlayer()
    {
        tAtk -= Time.deltaTime;  
        if (tAtk<=0)
        {
            Player.GetComponent<Player>().TakeDmg(ataque);
            tAtk = atkCd;
        }   
    }

};