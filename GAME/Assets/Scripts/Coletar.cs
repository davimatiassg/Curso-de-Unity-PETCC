using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coletar : MonoBehaviour
{
    [SerializeField] AudioClip somColeta;
    private int qntMoedas = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Moeda"))
        {
            GetComponent<AudioSource>().PlayOneShot(somColeta);
            Destroy(collision.gameObject);
            qntMoedas++;
        }
    }
}
