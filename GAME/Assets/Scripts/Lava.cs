using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private BoxCollider2D col;
    private SpriteRenderer spr;
    [SerializeField] private Transform lavaTop;

    static public float tJatoIntervalo = 4f;
    static private float tJato = 2f;
    private float jatoIntervaloCd = tJatoIntervalo;
    private float jatoCd = tJato;

    private float altura;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        spr = GetComponent<SpriteRenderer>();
        lavaTop.gameObject.GetComponent<SpriteRenderer>().size = Vector2.right * spr.size.x + Vector2.up * 0.5f;
        altura = spr.size.y;
        lavaTop.localPosition = Vector2.up * (0.25f + (spr.size.y * 0.5f));
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jatoIntervaloCd < 0 && jatoCd < 0)
        {
            // O jato de lava parou; resetar os timers
            jatoIntervaloCd = tJatoIntervalo;
            jatoCd = tJato;
        }
        else {
            if (jatoIntervaloCd < 0)
            {
                // Acabou o cooldown; lançando o jato de lava
                if (jatoCd >= 0)
                {
                    float r = jatoCd/tJato;
                    float jatoH = 10f*(r < 0.5 ? r : 1 - r);

                    
                    
                    spr.size = new Vector2(spr.size.x, altura * (1f + jatoH));
                    lavaTop.localPosition = Vector2.up * (0.25f + (spr.size.y * 0.5f));

                    jatoCd -= Time.deltaTime;
                }
            }
            else {
                jatoIntervaloCd -= Time.deltaTime;
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
