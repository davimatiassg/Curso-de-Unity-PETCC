using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, I_HitableObj
{
    public GameObject Player;

    /// Atributos
    [SerializeField] private int hp = 1;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jump = 25f;
    [SerializeField] private int atkDmg = 1;
    [SerializeField] private float scale = 1f;

    /// Movimentação
    private float disMax = 8.5f; //!< Distância máxima para perseguir o jogador
    private float disMin = 0.5f; //!< Distância mínima para perseguir o jogador
    private float disJump = 1.6f; //!< Distância mínima vertical do jogador para pular

    static private float jumpCd = 1.5f; //!< Tempo mínimo entre pulos
    private float tJump = 0; //!< Cooldown do jumpCd

    [SerializeField] private Transform floorChk;
    [SerializeField] private LayerMask solid;
    
    /// Ataque
    static private float atkCd = 0.25f; //!< Tempo mínimo entre ataques
    private float tAtk = 0; //!< Cooldown do atkCd
    private Rigidbody2D rb;

    /// Animação
    private Animator anim;

    /// Sons
    private AudioSource aud;
    [SerializeField] AudioClip stepSound;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip dyingSound;

    /// Vfx
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject deathVFX;

    /// ------- Métodos da Unity -------

    /// Start é chamado antes do primeiro update de frame
    void Start()
    {
        /// Carregando variáveis
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        Player = GameObject.FindWithTag("Player");
        scale = transform.localScale.x;
    }

    /// Update é chamado uma vez por frame
    void Update()
    {
        /// Direção para/Distância até  o player
        Vector2 pDir = Player.transform.position - transform.position;

        /// Se o player está longe o bastante e perto o bastante para se perseguido
        if (Mathf.Abs(pDir.x) > disMin && Mathf.Abs(pDir.x) < disMax)
        {
            /// Persegue o Player
            rb.velocity = new Vector2((Mathf.Sign(pDir.x)) * speed, rb.velocity.y);
            anim.SetBool("Chase", true);
            transform.localScale = new Vector2(-Mathf.Sign(rb.velocity.x) * scale, scale);

            /// Pulo
            if (tJump > 0){ tJump -= Time.deltaTime;}
            if (IsOnGround())
            {
                RaycastHit2D ray = Physics2D.Raycast(transform.position, pDir, 1.25f);
                bool obstacleAhead = ray.collider != null;

                /// Se o cooldown de pular já acabou e
                /// (tem um obstaculo a frente ou a distância vertical até o player é o suficiente para pular)
                if (tJump <= 0 && (obstacleAhead || Mathf.Abs(pDir.y) > disJump))
                {
                    rb.velocity = new Vector2(rb.velocity.x, jump);
                    tJump = jumpCd;
                }
            }
        }
        else
        {
            /// Fica parado
            rb.velocity = new Vector2(0f, rb.velocity.y);
            anim.SetBool("Chase", false);
        }
    }

    /// ------- Métodos de controle das ações -------

    /// Se está no chão
    private bool IsOnGround()
    {
        return Physics2D.OverlapCircle(floorChk.position, 0.01f, solid);
    }

    /// Se está colidindo
    public void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            AttackPlayer();
        }
    }

    /// Se não está mais colidindo
    public void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            tAtk = atkCd;
        }
    }

    /// Chama o método 'TakeHit' do Player acada 'atkCd' segundos
    void AttackPlayer()
    {
        tAtk -= Time.deltaTime;  
        if (tAtk <= 0)
        {
            Player.GetComponent<Player>().TakeHit(atkDmg, transform.position);
            tAtk = atkCd;
        }   
    }

    /// Leva dano
    public void TakeHit(int damage, Vector2 hitPos)
    {
        if (hp > 0)
        {
            Instantiate(hitVFX, hitPos, new Quaternion(0, 0, 0, 0));
            GetComponent<AudioSource>().PlayOneShot(hurtSound);
            hp -= damage;
        }
        if(hp <= 0)
        {
            Instantiate(deathVFX, transform.position, transform.rotation);

            /// Toca a animação de morte
            Destroy(this.gameObject, 0.10f);
        }
    }

    /// Efeitos sonoros

    public void PlayDeathSound()
    {
        aud.PlayOneShot(dyingSound, 0.5f);
    }

    public void PlayStepSound()
    {
        aud.PlayOneShot(stepSound, 0.5f);
    }

};