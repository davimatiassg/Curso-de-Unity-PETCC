using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndGameScore : MonoBehaviour
{
    
    [SerializeField] TMPro.TextMeshProUGUI scoreLabel; //!< Elemento da UI que mostra o score
    
    void Start(){
        /// Atualiza a UI com a quantidade de score
        int scoreCurrent = PlayerPrefs.GetInt("score", 0);
        scoreLabel.text = scoreCurrent.ToString();  
    }
}
