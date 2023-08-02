using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private BoxCollider2D col;
    private SpriteRenderer spr;
    private AudioSource aud;
    private Transform trs;
    private BuoyancyEffector2D eff;

    
    public float lavaWait = 4f; //!< Tempo entre as paredes de lava
    public float lavaTime = 2f; //!< Tempo de duração da parede de lava

    [SerializeField] private Vector2 maxDim = Vector2.one;
    [SerializeField] private Vector2 minDim = Vector2.zero;

    [SerializeField] private Transform lavaTop;
    [SerializeField] AudioClip extrLava;

    private bool isExtruding = false;
    private Vector2 targetDim;
    private float extrSpeed;

    /// Start é chamado antes do primeiro update de frame
    void Start()
    {
        /// Carregando variáveis
        col = GetComponent<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();
        aud = lavaTop.gameObject.GetComponent<AudioSource>();
        trs = GetComponent<Transform>();
        eff = GetComponent<BuoyancyEffector2D>();

        /// Posicionando elementos da Lava
        lavaTop.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(spr.size.x, 0.5f);
        lavaTop.localPosition = Vector2.up * (0.25f + (spr.size.y/2));
        minDim = spr.size;
        targetDim = maxDim;
        extrSpeed = Vector2.Distance(maxDim, minDim)*2/lavaTime;
        eff.surfaceLevel = spr.size.y/2;
        
        /// Configurando alcance do áudio
        aud.minDistance = spr.size.x;
        aud.maxDistance = aud.minDistance + 5f;

        StartCoroutine(Countdown());
    }

    /// Update é chamado uma vez por frame
    void Update()
    {
        if(isExtruding) { ExtrudeLava(); }
    }

    /// Muda o valor de isExtruding quando precisar iniciar ou interromper a extrusão
    IEnumerator Countdown()
    {
        while(true)
        {
            if(isExtruding) { yield return WaitForExtrude(); }
            else { yield return WaitForLava(); }
        }
    }

    /// Aguarda o tempo de recarga da extrusão para iniciar uma nova.
    IEnumerator WaitForLava()
    {
        yield return new WaitForSeconds(lavaWait);
        aud.PlayOneShot(extrLava);
        isExtruding = true;
    }

    /// Aguarda o tempo de extrusão para encerra-la.
    IEnumerator WaitForExtrude()
    {
        yield return new WaitForSeconds(lavaTime);
        isExtruding = false;
    }


    /// Movimenta a Lava 
    void ExtrudeLava()
    {
        /// Se chegou ao tamanho-alvo
        if(spr.size == targetDim)
        {
            /// Troque o tamanho-alvo para o estado anterior.
            if(targetDim == maxDim){targetDim = minDim;}
            else {targetDim = maxDim;}
        }

        /// Alterando o tamanho da lava, guardando a variação na altura da coluna.
        float pDelta = -spr.size.y; 
        spr.size = Vector2.MoveTowards(spr.size, targetDim, extrSpeed*Time.deltaTime); /// Alterando o tamanho da lava
        pDelta += spr.size.y;
        pDelta = pDelta/2;

        /// Alterando posições da lava e do topo da lava
        trs.position += Vector3.up*pDelta; 
        lavaTop.localPosition = Vector2.up * (0.25f + (spr.size.y * 0.5f));
        eff.surfaceLevel += pDelta;
        /// Som da lava
        aud.minDistance = spr.size.x +10f;
        aud.maxDistance = aud.minDistance +5f;
    }


    /// Quando algo entrar no alcance:
    public void OnTriggerEnter2D(Collider2D col)
    {
        I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
        if(hit != null) { 

            /// Aplicando dano no que foi colidido
            hit.TakeHit(3, GetComponent<Collider2D>().ClosestPoint(col.bounds.center));
            Rigidbody2D hit_rb = col.attachedRigidbody;
            /*
            /// Jogando o que foi colidido para cima
            if(hit_rb != null)
            {
                Debug.Log("wowo");
                Debug.Log(col.gameObject);
                hit_rb.velocity = new Vector2(0, 30f);
            }*/
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if(!trs)
        {
            spr = GetComponent<SpriteRenderer>();
            trs = GetComponent<Transform>();
        }
        /// Posicionando elementos da Lava
        lavaTop.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(spr.size.x, 0.5f);
        lavaTop.localPosition = Vector2.up * (0.25f + (spr.size.y/2));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(trs.position + Vector3.up * Mathf.Abs(maxDim.y - spr.size.y)/2 , maxDim);
    }

}
