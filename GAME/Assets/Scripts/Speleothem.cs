using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speleothem : MonoBehaviour, I_HitableObj
{

    [SerializeField] private GameObject player;
    [SerializeField] private Collider2D HitBox;
    [SerializeField] private Collider2D PlaceBox;
    [SerializeField] private GameObject hitVFX;

    [SerializeField] AudioClip stalactiteFall;

    private Rigidbody2D rb;

    private float timeToLock = 0.25f;

    Vector2 ray_angle = Quaternion.AngleAxis(-20f, Vector3.forward) * Vector2.down ;
    bool alreadyHit = false;
    bool fell = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(!alreadyHit)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, ray_angle, 100f);
            if (ray.collider != null && ray.collider.gameObject == player)
            {
                //Chama o método de receber dano. Acontece que ele simplesmente faz a estalactite cair.
                TakeHit(0, Vector2.zero);
                if (!fell) GetComponent<AudioSource>().PlayOneShot(stalactiteFall);
                fell = true;
            }
        }
        
    }

    void FixedUpdate()
    {
        // Se a estalactite já acertou alguma coisa, trave o objeto no lugar e desative a hit-box de dano.
        if(alreadyHit) 
        { 
            HitBox.enabled = false;
            this.enabled = false; //Desativa este script, para evitar chamadas desnecessárias do Update e Fixed Update.
        }
    }

    // Quando alguma coisa entrar na hitbox de dano
    public void OnTriggerEnter2D(Collider2D col)
    {
        //Verifica se pode causar dano na dita "coisa"
        I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
        //Se puder...
        if(hit != null) 
        {
            if(rb.velocity.y < -1) //Se estiver caindo...
            {
                hit.TakeHit(2, GetComponent<Collider2D>().ClosestPoint(col.bounds.center));     //Causa 2 pontos de dano
                alreadyHit = true;  //Prepara para desativar a hitbox de dano no próximo frame.
                //Repele o objeto acertado para longe
                Rigidbody2D hit_rb = col.attachedRigidbody;
                if(hit_rb != null)
                {
                    float direction = Mathf.Sign(col.transform.position.x - transform.position.x);
                    hit_rb.velocity = new Vector2(direction*5f, 10f);
                }
            }
            else {
                // Não está mais caindo e algo tocou nela. Já pode travar
                rb.constraints = RigidbodyConstraints2D.FreezeAll; 
            }
           
        }
    }

    // Quando alguma coisa entrar na hibox sólida
    public void OnCollisionStay2D(Collision2D col)
    {
        if(timeToLock > 0) { timeToLock -= Time.deltaTime; }
        //Verifica se o objeto que o tocou não é um inimigo, jogador ou algo do gênero
        else if(col.gameObject.GetComponent<I_HitableObj>() == null)
        {
            //Se não for, prepare para travar o objeto no lugar e desativar a hitbox de dano.
            alreadyHit = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll; 
        }
    }
    public void OnCollisionExit2D(Collision2D col)
    {
        timeToLock = 0.5f;
    }

    //Método de receber dano. Simplesmente fará a estalactite cair.
    public void TakeHit(int damage, Vector2 hitPos)
    {
        if(!alreadyHit)
        {
            Instantiate(hitVFX, hitPos, new Quaternion(0, 0, 0, 0));
            PlaceBox.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = new Vector2(0, -12f);
        }
        
    }

}
