using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private BoxCollider2D col;
    private SpriteRenderer spr;
    private AudioSource aud;
    [SerializeField] private Transform lavaTop;

    public float tExtrWait = 4f;
    private float extrWaitCd = 4f;

    public float tExtr = 2f;
    private float extrCd = 2f;

    private float altura;

    [SerializeField] AudioClip extrLava;
    bool extrLavaOn = false;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();
        aud = GetComponent<AudioSource>();
        lavaTop.gameObject.GetComponent<SpriteRenderer>().size = Vector2.right * spr.size.x + Vector2.up * 0.5f;
        altura = spr.size.y;
        lavaTop.localPosition = Vector2.up * (0.25f + (spr.size.y * 0.5f));
        aud.minDistance = spr.size.x;
        aud.maxDistance = aud.minDistance +5f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (extrWaitCd < 0 && extrCd < 0)
        {
            // O extr de lava parou; resetar os timers
            extrWaitCd = tExtrWait;
            extrCd = tExtr;
            extrLavaOn = false;
        }
        else {
            if (extrWaitCd < 0)
            {
                // Acabou o cooldown; lançando o extr de lava
                if (extrCd >= 0)
                {
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 
                    if (!extrLavaOn) aud.PlayOneShot(extrLava);  // TORNAR LOCAL !!!!!!!!!!!
                    extrLavaOn = true;


                    float r = extrCd/tExtr;
                    float extrH = 10f*(r < 0.5 ? r : 1 - r); 
                    
                    spr.size = new Vector2(spr.size.x, altura * (1f + extrH));
                    lavaTop.localPosition = Vector2.up * (0.25f + (spr.size.y * 0.5f));

                    extrCd -= Time.deltaTime;
                    aud.minDistance = spr.size.x;
                    aud.maxDistance = aud.minDistance +5f;
                }
            }
            else {
                extrWaitCd -= Time.deltaTime;
            }

        }

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();
        if(hit != null) { 
            hit.TakeHit(2);
            Rigidbody2D hit_rb = col.attachedRigidbody;
            if(hit_rb != null)
            {
                hit_rb.velocity = new Vector2(0, 30f);
            }
        }
    }

}
