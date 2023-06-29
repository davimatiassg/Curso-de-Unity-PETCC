using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private BoxCollider2D col;

    static public float tJatoIntervalo = 4f;
    static private float tJato = 2f;
    private float jatoIntervaloCd = tJatoIntervalo;
    private float jatoCd = tJato;

    private float altura;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        altura = transform.localScale.y;
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

                    col.size = new Vector2(1f, 1f + jatoH);
                    transform.localScale = new Vector2(transform.localScale.x, altura * (1f + jatoH));

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
        if(hit != null) { hit.TakeHit(2); }
    }

}
