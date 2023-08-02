using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfall : MonoBehaviour
{
    public int dmg = 2;
    
    [SerializeField] GameObject player;
    private Transform p_trs;
    private Rigidbody2D p_rig;
    private Player p_bhv;

    [SerializeField] private LinkedList<Vector2> recorded = new LinkedList<Vector2>();

    private bool isRecording = false;
    private bool isRewinding = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        p_trs = player.GetComponent<Transform>();
        p_rig = player.GetComponent<Rigidbody2D>();
        p_bhv = player.GetComponent<Player>();
    }

    void Update()
    {

        if(isRewinding)
        {
            if(recorded.Count > 0)
            {
                p_bhv.MakeUnPlayable();
                p_trs.position = recorded.Last.Value;
                recorded.RemoveLast();
            }
            else
            {
                isRewinding = false;
                p_bhv.MakePlayable();
                p_rig.simulated = true;
                Debug.Log("gg");
            }
            
        }
        else if(!p_bhv.IsOnGround())
        {
            isRecording = true;
        }
        else
        {
            recorded.AddLast((Vector2)p_trs.position);
            if(recorded.Count > 5){recorded.RemoveFirst();}
        }

        if(isRecording)
        {
            if(p_bhv.IsOnGround())
            {
                isRecording = false;
                recorded.Clear();
            }
            else
            {
                recorded.AddLast((Vector2)p_trs.position);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject == player)
        {
            p_bhv.TakeHit(dmg, p_trs.position);
            p_rig.velocity = Vector2.zero;
            p_rig.simulated = false;
            isRecording = false;
            isRewinding = true;
            return;
        }

        /// Verifica se pode causar dano na dita "coisa"
        I_HitableObj hit = col.gameObject.GetComponent<I_HitableObj>();

        /// Se puder objeto que entrou na hitbox é capaz de receber dano
        if(hit != null) 
        {
            /// Causa 2 pontos de dano
            hit.TakeHit(dmg*5, GetComponent<Collider2D>().ClosestPoint(col.bounds.center));

            /// Repele o objeto acertado para longe
            Rigidbody2D hit_rb = col.attachedRigidbody;
            if(hit_rb != null)
            {
                float direction = Mathf.Sign(col.transform.position.x - transform.position.x);
                hit_rb.velocity = new Vector2(direction*5f, 10f);
            }
        }
    }

   
}
