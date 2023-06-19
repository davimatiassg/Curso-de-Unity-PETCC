using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coletar : MonoBehaviour
{
    [SerializeField] AudioClip somColeta;
    [SerializeField] TMPro.TextMeshProUGUI txtPontos;
    [SerializeField] public GameObject[] vidas;

    void Start() {
        carregarPontos();
        carregarVidas();
    }    
    // Quando o objeto colidir com outro objeto que tenha o componente Collider2D e que tenha a tag "Moeda" 
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Moeda"))
        {
            coletarMoeda(collision.gameObject);
        }
    }
    void coletarMoeda(GameObject moeda){
        // Toca o som de coleta e destroi o objeto
        GetComponent<AudioSource>().PlayOneShot(somColeta);
        Destroy(moeda);

        // Adiciona 50 pontos por moeda coletada
        int pontos = PlayerPrefs.GetInt("Pontos", 0); // 0 é o valor padrão caso não exista a chave "Pontos"
        pontos += 50;
        PlayerPrefs.SetInt("Pontos", pontos);
        PlayerPrefs.Save();

        carregarPontos();
    }

    void carregarPontos(){
        // Atualiza a UI com a quantidade de pontos
        int pontosAtuais = PlayerPrefs.GetInt("Pontos", 0);
        txtPontos.text = "Pontos: " + pontosAtuais.ToString();  
    }

    void carregarVidas(){
        // Atualiza a UI com a quantidade de vidas
        int vidasAtuais = PlayerPrefs.GetInt("Vidas", 5);
        for (int i = 0; i < 5; i++)
        {
            // Se o valor de i for menor que a quantidade de vidas atuais, ativa a imagem da vida
            vidas[i].SetActive(i < vidasAtuais);
        }
    }
}
