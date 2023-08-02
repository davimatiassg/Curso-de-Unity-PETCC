using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speleothem : MonoBehaviour, I_HitableObj
{

    public int dmg = 2;

    [SerializeField] private GameObject player;
    [SerializeField] private Collider2D HitBox;
    [SerializeField] private CapsuleCollider2D HurtBox;
    [SerializeField] private BoxCollider2D SolidBox;
    [SerializeField] private GameObject hitVFX; //!< Efeito visual do hit
    [SerializeField] private Transform sensor;
    [SerializeField] private float sensorRange;

    [SerializeField] AudioClip stalactiteFall;

    private Rigidbody2D rb;

    private Vector2 ray_angle;

    bool isFalling = false;                //!< Se a estalactite já colidiu com alguma coisa

    void Start()
    {
        /// Carregando variáveis
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        player = GameObject.FindWithTag("Player");
        ray_angle = sensor.rotation * Vector2.down;
    }

    void FixedUpdate()
    {
        if(!isFalling)
        {
            RaycastHit2D ray = Physics2D.Raycast(sensor.position, ray_angle, sensorRange, Physics2D.AllLayers);

            /// Se o player colidiu com o raycast
            if (ray.collider != null && ray.collider.gameObject == player)
            {
                /// Chama o método responsável por fazer a estalactite cair.
                Fall();
            }
        }
    }

    /// Quando alguma coisa entrar na hitbox de dano
    void OnTriggerEnter2D(Collider2D col)
    {
        /// Verifica se pode causar dano na dita "coisa"
        I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();

        /// Se puder objeto que entrou na hitbox é capaz de receber dano
        if(hit != null) 
        {
            /// Causa 2 pontos de dano
            hit.TakeHit(2, GetComponent<Collider2D>().ClosestPoint(col.bounds.center));

            /// Repele o objeto acertado para longe
            Rigidbody2D hit_rb = col.attachedRigidbody;
            if(hit_rb != null)
            {
                float direction = Mathf.Sign(col.transform.position.x - transform.position.x);
                hit_rb.velocity = new Vector2(direction*5f, 10f);
            }
        }
    }

    /// Quando alguma coisa entrar na hibox sólida
    void OnCollisionStay2D(Collision2D col)
    {
        /// Verifica se o objeto que o tocou é um objeto do cenário
        if(col.gameObject.tag == "backgroundObj")
        {
            
            /// Usando OverlapPoint para verificar se a colisão ocorre na parte de baixo do espeleotema
            Collider2D[] overlaps = Physics2D.OverlapPointAll(transform.position + (Vector3.down * 0.02f));
            foreach(Collider2D c in overlaps)
            {
                if(c.gameObject == col.gameObject)
                {
                    /// Fixando o espeleotema ao objeto do cenário 
                    Destroy(rb);
                    transform.parent = col.transform;

                    /// Desativando a hitbox de dano e esse script.
                    HitBox.enabled = false;
                    this.enabled = false;
                    return;
                }
            }
        }
        else
        {
            Fall();
        }
    }
    /// Método de receber dano. Simplesmente fará a estalactite cair.
    public void TakeHit(int damage, Vector2 hitPos)
    {
        Instantiate(hitVFX, hitPos, new Quaternion(0, 0, 0, 0));
        Fall();
    }

    /// Método para fazer o espeleotema cair.
    private void Fall()
    {
        if(!isFalling)
        {
            GetComponent<AudioSource>().PlayOneShot(stalactiteFall);
            Vector2 Fx_pos = Vector2.up * (transform.position.y + SolidBox.offset.y + SolidBox.size.y/2);
            Instantiate(hitVFX, Fx_pos, new Quaternion(0, 0, 0, 0));

            HurtBox.enabled = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.velocity = Vector2.up * 3;
            
            isFalling = true;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(sensor.position, sensor.position + (sensor.rotation * Vector3.down) * sensorRange);
    }

}
