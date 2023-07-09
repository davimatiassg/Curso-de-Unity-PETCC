using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Scorer : MonoBehaviour
{
    
    [SerializeField] TMPro.TextMeshProUGUI scoreLabel;

    // Quando o objeto colidir com outro objeto que tenha o componente Collider2D e que tenha a tag "coin" 

    public void UpdateScore(){
        // Atualiza a UI com a quantidade de score
        int scoreCurrent = PlayerPrefs.GetInt("score", 0);
        scoreLabel.text = scoreCurrent.ToString();  
    }

    public void AddScore(int bonus)
    {
        int score = PlayerPrefs.GetInt("score", 0); // 0 é o valor padrão caso não exista a chave "score"
        score += bonus;
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
        UpdateScore();
    }

    public int UpdateLives(int hp){
        // Atualiza a UI com a quantidade de lifes
        for (int i = 0; i < transform.childCount; i++)
        {
            // Se o valor de i for menor que a quantidade de lifes Current, ativa a imagem da vida
            transform.GetChild(i).gameObject.SetActive(i < hp);
        }

        return hp;
    }
}
