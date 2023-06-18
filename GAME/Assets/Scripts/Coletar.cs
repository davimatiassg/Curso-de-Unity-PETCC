using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coletar : MonoBehaviour
{
    [SerializeField] AudioClip somColeta;
    //private int pontos = 0;
    //[SerializeField] TMPro.TextMeshProUGUI txtPontos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Moeda"))
        {
            GetComponent<AudioSource>().PlayOneShot(somColeta);
            Destroy(collision.gameObject);

            int pontos = PlayerPrefs.GetInt("Pontos", 0); // 0 é o valor padrão caso não exista a chave "Pontos"
            pontos += 50;
            PlayerPrefs.SetInt("Pontos", pontos);
            PlayerPrefs.Save();
            
            //txtPontos.text = "Pontos: " + PlayerPrefs.GetInt("Pontos");
        }
    }
}
