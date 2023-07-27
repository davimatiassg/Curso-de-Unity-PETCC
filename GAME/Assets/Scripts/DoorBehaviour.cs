using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private LevelManager Levels;
    [SerializeField] private Animator anim;

    public void OnTriggerEnter2D(Collider2D col)
    {
        /// Se está colidindo com o player
        if(col.gameObject.tag == "Player")
        {
            /// Ativa a animação "Open"
            anim.SetTrigger("Open");

            /// Carregando um novo nível
            Levels.LoadNextLevel(0.5f);
        }
    }
}
