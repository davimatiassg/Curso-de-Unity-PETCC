using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPausar : MonoBehaviour
{
    [SerializeField] GameObject menuPausa;
    [SerializeField] Button botaoPausa;
    [SerializeField] GameObject menuMusicaSlider;
    [SerializeField] GameObject menuEfeitosSlider;
    [SerializeField] Slider volumeMusicaSlider;
    [SerializeField] Slider volumeEfeitosSlider;
    [SerializeField] AudioSource musica;
    [SerializeField] AudioSource efeitos;

    private bool menuOpcoesAberto = false;

    void Start()
    {
        // Carregar as configurações de volume atuais
        volumeMusicaSlider.value = PlayerPrefs.GetFloat("VolumeMusica", 1f);
        volumeEfeitosSlider.value = PlayerPrefs.GetFloat("VolumeEfeitos", 1f);
    }

    public void PausarJogo(){
        Time.timeScale = 0; 
        botaoPausa.interactable = false;
        menuPausa.SetActive(true);
        menuMusicaSlider.SetActive(false);
        menuEfeitosSlider.SetActive(false);
    }

    public void Continuar(){
        Time.timeScale = 1; 
        botaoPausa.interactable = true;
        menuPausa.SetActive(false);
    }
    public void IrParaMenu(){
        Time.timeScale = 1; 
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void Opcoes(){
        // Toggle menu opções
        menuOpcoesAberto = !menuOpcoesAberto;
        menuMusicaSlider.SetActive(menuOpcoesAberto);
        menuEfeitosSlider.SetActive(menuOpcoesAberto);
    }
    public void SetVolumeMusica(){
        // Atribuir o volume da música de fundo
        musica.volume = volumeMusicaSlider.value;
        // Salvar o volume da música de fundo
        PlayerPrefs.SetFloat("VolumeMusica", volumeMusicaSlider.value);
        PlayerPrefs.Save();
    }
    public void SetVolumeEfeitos(){
        efeitos.volume = volumeEfeitosSlider.value;
        PlayerPrefs.SetFloat("VolumeEfeitos", volumeEfeitosSlider.value);
        PlayerPrefs.Save();
    }
}