using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, I_HitableObj
{
    // Atributos
    [SerializeField] private int Hp = 5;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jump = 20f;
    [SerializeField] private float inv_time = 3f;
    private float invt = 0;

    // Estado:
    private bool playable = true;

    // Movimentação
    private float hAxis;
    [SerializeField] private LayerMask solid;
    [SerializeField] private Transform floorCheck;
    [SerializeField] private Rigidbody2D rb;

    // Ataque     
    [SerializeField] private Transform sling;
    [SerializeField] private float atkCd = 0.25f;
    [SerializeField] private float atkc = 0;
    [SerializeField] private GameObject pellet;

    // Colecionáveis
    [SerializeField] private Scorer sc;

    // Animação:
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spr;
    // Sons
    [SerializeField] AudioClip collectSound;
    [SerializeField] AudioClip stepSound;
    
    // Mds o_o
    public struct audioTuple
    {
        public float time;
        public AudioClip clip;

        public audioTuple(float time_,AudioClip clip_)
        {
            time = time_;
            clip = clip_;
        }
    }
    public List<audioTuple> audioQueue = new List<audioTuple>();


    /// Métodos da Unity:
    //Start called when the scene is initiated.
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        spr = this.gameObject.GetComponent<SpriteRenderer>();
        sc.UpdateLives(Hp);
        sc.AddScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        // Se o jogador estiver podendo se mover:
        if(playable)
        {
            //Recebe o input do usuário e vira o player na direção certa
            hAxis = Input.GetAxisRaw("Horizontal");
            if(hAxis != 0)
            {
                Flip();
            }
            
            //Pulo
            if (Input.GetButton("Jump") && IsOnGround())
            {
                rb.velocity = new Vector2(rb.velocity.x, jump);
            }

            //Ataque
            if(atkc > 0) { atkc -= Time.deltaTime; }
            if (Input.GetButtonDown("Fire1") && atkc <= 0)
            {
                Shoot();
                anim.Play("Attack");
                atkc = atkCd;
            }

            //Invencibilidade após receber dano
            if(invt > 0)
            { 
                invt -= Time.deltaTime; 
                spr.color = Color.white * (Mathf.PingPong(15*Time.time, 1) + 0.3f);
                if(invt <= 0) { spr.color = Color.white; }
            }

            //Animação
            anim.SetBool("OnGround", IsOnGround());
            anim.SetBool("Moving", hAxis != 0);
        }

    }

    private void FixedUpdate()
    {
        //Movimentação
        if(playable)
        {
            rb.velocity = new Vector2(hAxis * speed, rb.velocity.y);
            if (Input.GetAxisRaw("Horizontal") != 0) playSoundContinuously(stepSound);
        }
    }

    //Linhas de auxílio (somente no editor do Unity)
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 1f);
        Gizmos.DrawWireSphere(floorCheck.position, 0.2f);
    }


    /// ** Métodos de controle das ações:

    // Se o jogador está no chão
    private bool IsOnGround()
    {
        return Physics2D.OverlapCircle(floorCheck.position, 0.2f, solid);
    }

    // Virar o player para a direção certa
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(hAxis);
        transform.localScale = scale;
    }

    // Disparar um projétil
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
        if(playable && invt <= 0)
        {
            invt = inv_time;
            playable = false;
            anim.Play("Hurt");
            Hp -= dmg;
            sc.UpdateLives(Hp);
            if(Hp <= 0)
            {
                //Play Death Anim.
                Destroy(this.gameObject, 2f); //provisório
            }
        }
        
    }

    //Coletar um item
    public void Collect(int scoreBonus = 0){
        // Toca o som de coleta e destroi o objeto
        GetComponent<AudioSource>().PlayOneShot(collectSound);
        sc.AddScore(scoreBonus);
    }

    //Entregar o controle do player de volta ao usuário.
    public void makePlayable(){
        playable = true;
    }


    public void playSoundContinuously(AudioClip clip)
    {
        bool newClip = true;

        for (int i = 0;i < audioQueue.Count;i ++)
        {
            if (audioQueue[i].clip == clip)
            {
                newClip = false;
            }

            if (audioQueue[i].time <= 0)
            { // O tempo restante do clip acabou -> removendo o clip da lista
                audioQueue.RemoveAt(i);
                i --;
            }
            else { // Diminuindo o tempo restante
                audioQueue[i] = new audioTuple(audioQueue[i].time - Time.deltaTime, audioQueue[i].clip);
            }
        }

        if (newClip)
        {
            audioQueue.Add(new audioTuple(clip.length, clip));
            GetComponent<AudioSource>().PlayOneShot(clip);
        }

    }


}
