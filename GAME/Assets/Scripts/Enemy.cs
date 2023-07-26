using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, I_HitableObj
{
    public GameObject Player;

    [SerializeField] private int hp = 1;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject deathVFX;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float jump = 25f;
    [SerializeField] private int atkDmg = 1;
    [SerializeField] private float scale = 1f;

    private float disMax = 8.5f;
    private float disMin = 0.5f;
    private float disJump = 1.6f;

    static private float jumpCd = 1.5f;
    private float tJump = 0;
    
    static private float atkCd = 0.25f;
    private float tAtk = 0;

    [SerializeField] private Transform floorChk;
    [SerializeField] private LayerMask solid;

    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource aud;

    [SerializeField] AudioClip stepSound;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip dyingSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();
        Player = GameObject.FindWithTag("Player");
        scale = transform.localScale.x;
    }

    void Update()
    {
        Vector2 pDir = Player.transform.position - transform.position;

        if (Mathf.Abs(pDir.x) > disMin && Mathf.Abs(pDir.x) < disMax)
        {
            /// Persegue o Player
            rb.velocity = new Vector2((Mathf.Sign(pDir.x)) * speed, rb.velocity.y);
            anim.SetBool("Chase", true);
            transform.localScale = new Vector2(-Mathf.Sign(rb.velocity.x) * scale, scale);

            /// Jump
            if (tJump > 0){ tJump -= Time.deltaTime;}
            if (IsOnGround())
            {
                RaycastHit2D ray = Physics2D.Raycast(transform.position, pDir, 1.25f);
                bool obstacleAhead = ray.collider != null;

                if ((Mathf.Abs(pDir.y) > disJump || obstacleAhead) && tJump <= 0)
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

        bool legal = IsOnGround();

    }

    private bool IsOnGround()
    {
        return Physics2D.OverlapCircle(floorChk.position, 0.01f, solid);
    }

    public void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            AttackPlayer();
        }
    }

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
            //Play death animation
            Destroy(this.gameObject, 0.10f);
        }
    }

    public void PlayDeathSound()
    {
        aud.PlayOneShot(dyingSound, 0.5f);
    }

    public void PlayStepSound()
    {
        aud.PlayOneShot(stepSound, 0.5f);
    }

};