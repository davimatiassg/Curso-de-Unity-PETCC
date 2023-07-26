using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private LevelManager Levels;
    [SerializeField] private Animator anim;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            anim.SetTrigger("Open");
            Levels.LoadNextLevel(0.5f);
        }
    }
}
