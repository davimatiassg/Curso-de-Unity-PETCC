using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private BoxCollider2D col;
    [SerializeField] private SpriteRenderer spr;
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
    private float extrSpeed;

    /// Start é chamado antes do primeiro update de frame
    void Start()
    {
        /// Carregando variáveis
        col = GetComponent<BoxCollider2D>();
        aud = lavaTop.gameObject.GetComponent<AudioSource>();
        trs = GetComponent<Transform>();
        eff = GetComponent<BuoyancyEffector2D>();

        /// Posicionando elementos da Lava
        lavaTop.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(spr.size.x, 0.5f);
        minDim = trs.position;
        extrSpeed = Vector2.Distance(maxDim, minDim)*2/lavaTime;
        lavaTop.position = trs.position + Vector3.right*spr.gameObject.transform.localPosition.x + Vector3.up * 0.25f;
        eff.surfaceLevel = 1;
        
        /// Configurando alcance do áudio
        aud.minDistance = spr.size.x;
        aud.maxDistance = aud.minDistance + 5f;

        if(lavaWait * lavaTime != 0){ StartCoroutine(Countdown()); }
        
    }

    /// Update é chamado uma vez por frame
    void Update()
    {
        if(isExtruding) { ExtrudeLava(maxDim); }
        else { ExtrudeLava(minDim); }
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
        yield return new WaitForSeconds(lavaWait+lavaTime);
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
    void ExtrudeLava(Vector2 targetDim)
    {
        if(Mathf.Abs(trs.position.y - targetDim.y) > 0.2f)
        {
            /// Alterando a altura da lava, guardando a variação na altura da coluna.
            trs.position = Vector2.MoveTowards(trs.position, targetDim, Time.deltaTime * extrSpeed);
        }
        
    }


    /// Quando algo entrar no alcance:
    public void OnTriggerStay2D(Collider2D col)
    {
        I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
        if(hit != null) { 

            /// Aplicando dano no que foi colidido
            hit.TakeHit(3, GetComponent<Collider2D>().ClosestPoint(col.bounds.center));
            Rigidbody2D hit_rb = col.attachedRigidbody;
            /// Jogando o que foi colidido para cima
            if(hit_rb != null)
            {
                hit_rb.AddForce(Vector2.up);
            }
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if(!trs)
        {
            trs = GetComponent<Transform>();
        }
        spr.gameObject.transform.localPosition = new Vector2 (spr.gameObject.transform.localPosition.x, -spr.size.y/2);
        /// Posicionando elementos da Lava
        lavaTop.gameObject.GetComponent<SpriteRenderer>().size = new Vector2(spr.size.x, 0.5f);
        lavaTop.position = trs.position + Vector3.right*spr.gameObject.transform.localPosition.x + Vector3.up * 0.25f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(maxDim + (Vector2)spr.gameObject.transform.localPosition, spr.size);
    }

}
