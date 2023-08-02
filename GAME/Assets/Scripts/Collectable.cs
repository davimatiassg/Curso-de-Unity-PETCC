using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int scoreBonus = 50;

    void OnTriggerEnter2D(Collider2D collider)
    {
        /// Se está colidindo com o player
        if (collider.gameObject.CompareTag("Player"))
        {
            /// Adicionando os pontos
            collider.gameObject.GetComponent<Player>().Collect(scoreBonus);

            Destroy(this.gameObject);
        }
    }
}
