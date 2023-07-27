using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField] GameObject optionsMenu;

    private bool menuOpcoesAberto = false;
    UnityEngine.UI.Slider volumeMusicaSlider, volumeEfeitosSlider;

    /// Start é chamado antes do primeiro update de frame
    void Start()
    {
        /// Carregar as configurações de volume atuais
        volumeMusicaSlider = optionsMenu.transform.GetChild(0).gameObject.GetComponent<Slider>();
        volumeEfeitosSlider = optionsMenu.transform.GetChild(1).gameObject.GetComponent<Slider>();

        volumeMusicaSlider.value = PlayerPrefs.GetFloat("VolumeMusica", 1f);
        volumeEfeitosSlider.value = PlayerPrefs.GetFloat("VolumeEfeitos", 1f);
    }

    /// Ativa o menu
    public void OptionsMenu() {
        menuOpcoesAberto = !menuOpcoesAberto;
        optionsMenu.SetActive(menuOpcoesAberto);
    }

    /// Modifica o volume da música de acordo com o slider do menu
    public void SetVolumeMusica(){
        PlayerPrefs.SetFloat("VolumeMusica", volumeMusicaSlider.value);
        PlayerPrefs.Save();
    }

    /// Modifica o volume dos efeitos de acordo com o slider do menu
    public void SetVolumeEfeitos(){
        PlayerPrefs.SetFloat("VolumeEfeitos", volumeEfeitosSlider.value);
        PlayerPrefs.Save();
    }

    /// Fecha o jogo
    public void Exit() {
        Application.Quit();
    }

}
