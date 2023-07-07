using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, I_HitableObj
{
    public GameObject Player;

    [SerializeField] private int hp = 1;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float jump = 25f;
    [SerializeField] private int atkDmg = 1;

    private float disMax = 8f;
    private float disMin = 1.1f;
    private float disJump = 1.6f;

    private float jumpCd = 1.5f;
    private float tJump = 0;
    
    private float atkCd = 1.5f;
    private float tAtk = 1.5f;

    [SerializeField] private Transform floorChk;
    [SerializeField] private LayerMask solid;

    private Rigidbody2D rb;

    [SerializeField] AudioClip stepSound;
    [SerializeField] AudioClip hurtSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 pDir = Player.transform.position - transform.position;

        if (Mathf.Abs(pDir.x) > disMin && Mathf.Abs(pDir.x) < disMax)
        {
            /// Persegue o Player
            rb.velocity = new Vector2((Mathf.Sign(pDir.x)) * speed, rb.velocity.y);
            GetComponent<sfxScript>().playSoundContinuously(stepSound);
            /// Jump
            if (tJump > 0){ tJump -= Time.deltaTime;}
            if (IsOnGround())
            {
                if (pDir.y > disJump && tJump <= 0)
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
        }

    }

    private bool IsOnGround()
    {
        return Physics2D.OverlapCircle(floorChk.position, 0.25f, solid);
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
        if (tAtk<=0)
        {
            Player.GetComponent<Player>().TakeHit(atkDmg);
            tAtk = atkCd;
        }   
    }

    public void TakeHit(int dmg)
    {
        hp -= dmg;
        if(hp <= 0)
        {
            //Play death animation
            Destroy(this.gameObject, 1f);
        }

        GetComponent<AudioSource>().PlayOneShot(hurtSound);
    }

};