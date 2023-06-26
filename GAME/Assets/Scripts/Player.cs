using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, I_HitableObj
{
    [SerializeField] private int Hp = 5;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jump = 20f;

    //Movimentação
    private float hAxis;
    [SerializeField] private LayerMask solid;
    [SerializeField] private Transform floorCheck;
    [SerializeField] private Rigidbody2D rb;

    //Ataque     
    [SerializeField] private Transform sling;
    [SerializeField] private float atkCd = 0.25f;
    [SerializeField] private float atkc = 0;
    [SerializeField] private GameObject pellet;

    //Colecionáveis
    [SerializeField] private Scorer sc;

    //Animação:
    [SerializeField] private Animator anim;

    // Sons
    [SerializeField] AudioClip collectSound;


    /// Métodos da Unity:
    //Start called when the scene is initiated.
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        sc.UpdateLives(Hp);
        sc.AddScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Jump") && IsOnGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }


        if(hAxis != 0)
        {
            Flip();
        }
        

        if(atkc > 0) { atkc -= Time.deltaTime; }
        if (Input.GetButtonDown("Fire1") && atkc <= 0)
        {
            Shoot();
            anim.Play("Attack");
            atkc = atkCd;
        }
        anim.SetBool("OnGround", IsOnGround());
        anim.SetBool("Moving", hAxis != 0);

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(hAxis * speed, rb.velocity.y);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 1f);
        Gizmos.DrawWireSphere(floorCheck.position, 0.2f);
    }


    /// Métodos de controle das ações:
    private bool IsOnGround()
    {
        return Physics2D.OverlapCircle(floorCheck.position, 0.2f, solid);
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(hAxis);
        transform.localScale = scale;
    }

    public void Shoot()
    {
        GameObject shot = Instantiate(pellet);
        Pellet p = shot.GetComponent<Pellet>();
        p.trs.position = (sling.position);
        p.rb.velocity = new Vector2(1*p.speed * transform.localScale.x, 0.2f);
    }

    /// Métodos chamados por outros scripts:

    //Receber dano
    public void TakeHit(int dmg)
    {   
        Hp -= dmg;
        sc.UpdateLives(Hp);
        if(Hp <= 0)
        {
            //Play Death Anim.
            Destroy(this.gameObject, 2f); //provisório
        }
    }

    //Coletar um item
    public void Collect(int scoreBonus = 0){
        // Toca o som de coleta e destroi o objeto
        GetComponent<AudioSource>().PlayOneShot(collectSound);
        sc.AddScore(scoreBonus);

        // Adiciona 50 score por coin coletada


        

    }



}
