using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, I_HitableObj
{
    /// Atributos
    [SerializeField] private int Hp = 5;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jump = 20f;
    [SerializeField] private float inv_time = 3f; //!< Tempo de invencibilidade em segundos
    private float invt = 0; //!< Cool down do tempo de invencibilidade

    /// Estado
    private bool playable = true;

    /// Movimentação
    private float hAxis;
    [SerializeField] private LayerMask solid;
    [SerializeField] private Transform floorCheck;
    private Rigidbody2D rb;

    /// Ataque     
    [SerializeField] private Transform sling;
    [SerializeField] private float atkCd = 0.25f;
    [SerializeField] private float atkc = 0;
    [SerializeField] private GameObject pellet;

    /// Colecionáveis
    [SerializeField] private Scorer sc;

    /// Animação
    private Animator anim;
    private SpriteRenderer spr;

    /// Sons
    [SerializeField] AudioClip collectSound;
    [SerializeField] AudioClip stepSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip pelletSound;
    AudioSource aud;
    int justJumped = 0;

    /// Vfx
    [SerializeField] private GameObject hitVFX;

    /// ------- Métodos da Unity -------

    /// Start é chamado antes do primeiro update de frame
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        spr = this.gameObject.GetComponent<SpriteRenderer>();
        aud = this.gameObject.GetComponent<AudioSource>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        sc.UpdateLives(Hp);
        sc.AddScore(0);
    }

    /// Update é chamado uma vez por frame
    void Update()
    {
        /// Se o jogador estiver podendo se mover:
        if(playable)
        {
            /// Recebe o input do usuário e vira o player na direção certa
            hAxis = Input.GetAxisRaw("Horizontal");
            if(hAxis != 0)
            {
                Flip();
            }
            
            /// Pulo
            if (Input.GetButton("Jump") && IsOnGround())
            {
                rb.velocity = new Vector2(rb.velocity.x, jump);
                if (justJumped == 0)
                { 
                    /// Só tocando o som em um novo pulo
                    aud.PlayOneShot(jumpSound);
                    justJumped ++;
                }
                justJumped ++;
            }
            if (justJumped > 0) justJumped --;

            /// Ataque
            if(atkc > 0) { atkc -= Time.deltaTime; }
            if (Input.GetButtonDown("Fire1") && atkc <= 0)
            {
                Shoot();
                anim.Play("Attack");
                atkc = atkCd;
            }

            /// Invencibilidade após receber dano
            if(invt > 0)
            { 
                invt -= Time.deltaTime; 
                spr.color = Color.white * (Mathf.PingPong(15*Time.time, 1) + 0.3f);
                if(invt <= 0) { spr.color = Color.white; }
            }

            /// Animação
            anim.SetBool("OnGround", IsOnGround());
            anim.SetBool("Moving", hAxis != 0);
        }

    }

    /// Fixed Update é chamado em intervalos de tempo constantes
    private void FixedUpdate()
    {
        /// Movimentação
        if(playable)
        {
            rb.velocity = new Vector2(hAxis * speed, rb.velocity.y);
        }
    }

    /// Linhas de auxílio (somente no editor do Unity)
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 1f);
        Gizmos.DrawWireSphere(floorCheck.position, 0.2f);
    }


    /// ------- Métodos de controle das ações -------

    /// Se o jogador está no chão
    private bool IsOnGround()
    {
        return Physics2D.OverlapCircle(floorCheck.position, 0.2f, solid);
    }

    /// Virar o player para a direção certa
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(hAxis);
        transform.localScale = scale;
    }

    /// Disparar um projétil
    public void Shoot()
    {
        GameObject shot = Instantiate(pellet);
        Pellet p = shot.GetComponent<Pellet>();
        p.trs.position = (sling.position);
        p.rb.velocity = new Vector2(1*p.speed * transform.localScale.x, 0.2f);

        aud.PlayOneShot(pelletSound);
    }

    /// ------- Métodos chamados por outros scripts -------

    /// Receber dano
    public void TakeHit(int damage, Vector2 hitPos)
    {   
        /// Se o jogador está jogável e não invencível
        if(playable && invt <= 0)
        {
            /// Deixando o jogador injogavel e invencível
            invt = inv_time;
            playable = false;
            Instantiate(hitVFX, transform);
            rb.velocity -= hitPos - (Vector2)transform.position;

            /// Recebendo dano
            Hp -= damage;
            sc.UpdateLives(Hp);

            /// Morrendo :(
            if(Hp <= 0)
            {
                playable = false;
                spr.enabled = false;
                GameObject.FindWithTag("SceneLoader").GetComponent<LevelManager>().ReloadLevel(1f);
            }

            /// Indicando o dano por uma animação em um efeito sonoro
            anim.Play("Hurt");
            aud.PlayOneShot(hurtSound);

        }
        
    }

    /// Coletar um item
    public void Collect(int scoreBonus = 0) {
        /// Toca o som de coleta e adiciona os pontos ao score
        aud.PlayOneShot(collectSound);
        sc.AddScore(scoreBonus);
    }

    /// Entregar o controle do player de volta ao usuário.
    public void makePlayable(){
        playable = true;
    }


    /// Efeitos sonoros
    public void playStepSound()
    {
        aud.PlayOneShot(stepSound);
    }

}
