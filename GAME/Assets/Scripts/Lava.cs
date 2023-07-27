using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private BoxCollider2D col;
    private SpriteRenderer spr;
    private AudioSource aud;
    [SerializeField] private Transform lavaTop;

    static public float tUntilActive = 4f; //!< Tempo entre as paredes de lava
    private float untilActiveCd = tUntilActive; //!< Cooldown do tUntilActive

    static public float tLavaWall = 2f; //!< Tempo de duração da parede de lava
    private float lavaWallCd = tLavaWall; //!< Cooldown do tLavaWall

    private float altura; //!< Altura da parede de lava

    [SerializeField] AudioClip extrLava;
    bool extrLavaOn = false;

    /// Start é chamado antes do primeiro update de frame
    void Start()
    {
        /// Carregando variáveis
        col = GetComponent<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();
        aud = lavaTop.gameObject.GetComponent<AudioSource>();
        lavaTop.gameObject.GetComponent<SpriteRenderer>().size = Vector2.right * spr.size.x + Vector2.up * 0.5f;
        altura = spr.size.y;
        lavaTop.localPosition = Vector2.up * (0.25f + (spr.size.y * 0.5f));
        aud.minDistance = spr.size.x;
        aud.maxDistance = aud.minDistance + 5f;
    }

    /// Update é chamado uma vez por frame
    void Update()
    {
        /// Se a parede de lava já se abaixou
        if (untilActiveCd < 0 && lavaWallCd < 0)
        {
            /// Resetar os timers
            untilActiveCd = tUntilActive;
            lavaWallCd = tLavaWall;
            extrLavaOn = false;
        }
        else {

            /// Se chegou a hora de uma nova parede de lava
            if (untilActiveCd < 0)
            {
                /// Enquanto a parede de lava estiver de pé
                if (lavaWallCd >= 0)
                {
                    /// Som da lava
                    if (!extrLavaOn) aud.PlayOneShot(extrLava);
                    extrLavaOn = true;

                    /// ------- Alterando a altura da parede de lava -------

                    float r = lavaWallCd/tLavaWall; //!<  1 - (tempoDecorrido ÷ tempoTotal)

                    /* Se o tempo estiver menor que a metade da duração total: suba,
                    caso contrário: desça. */
                    float extrH = 10f*(r < 0.5 ? r : 1 - r);
                    
                    spr.size = new Vector2(spr.size.x, altura * (1f + extrH)); /// alterando o tamanho do sprite
                    lavaTop.localPosition = Vector2.up * (0.25f + (spr.size.y * 0.5f));

                    /// Distância mínima e máxima para emitir o som
                    aud.minDistance = spr.size.x +10f;
                    aud.maxDistance = aud.minDistance +5f;

                    lavaWallCd -= Time.deltaTime;
                }
            }
            else {
                untilActiveCd -= Time.deltaTime;
            }

        }

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
        if(hit != null) { 

            /// Aplicando dano no que foi colidido
            hit.TakeHit(3, GetComponent<Collider2D>().ClosestPoint(col.bounds.center));
            Rigidbody2D hit_rb = col.attachedRigidbody;

            /// Jogando o que foi colidido para cima
            if(hit_rb != null)
            {
                hit_rb.velocity = new Vector2(0, 30f);
            }
        }
    }

}
