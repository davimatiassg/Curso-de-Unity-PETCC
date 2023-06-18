using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadosUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI txtPontos;
    [SerializeField] public GameObject[] vidas;

    void Start()
    {
        // Para testar o sistema de vidas (remover depois)
        PlayerPrefs.SetInt("Vidas", 4); 
        PlayerPrefs.Save();
    }

    void Update()
    {
        // Atualiza a quantidade de pontos
        int pontosAtuais = PlayerPrefs.GetInt("Pontos", 0);
        txtPontos.text = "Pontos: " + pontosAtuais.ToString();

        // Atualiza a quantidade de vidas
        int vidasAtuais = PlayerPrefs.GetInt("Vidas", 3);
        for (int i = 0; i <= vidasAtuais; i++)
        {
            vidas[i].SetActive(i < vidasAtuais);
        }
    }
}
