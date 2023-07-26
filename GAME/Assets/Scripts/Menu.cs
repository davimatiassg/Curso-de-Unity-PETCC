using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField] GameObject optionsMenu;

    private bool menuOpcoesAberto = false;
    UnityEngine.UI.Slider volumeMusicaSlider, volumeEfeitosSlider;

    void Start()
    {
        // Carregar as configurações de volume atuais
        volumeMusicaSlider = optionsMenu.transform.GetChild(0).gameObject.GetComponent<Slider>();
        volumeEfeitosSlider = optionsMenu.transform.GetChild(1).gameObject.GetComponent<Slider>();

        volumeMusicaSlider.value = PlayerPrefs.GetFloat("VolumeMusica", 1f);
        volumeEfeitosSlider.value = PlayerPrefs.GetFloat("VolumeEfeitos", 1f);
    }

    public void OptionsMenu() {
        menuOpcoesAberto = !menuOpcoesAberto;
        optionsMenu.SetActive(menuOpcoesAberto);
    }
    public void SetVolumeMusica(){
        PlayerPrefs.SetFloat("VolumeMusica", volumeMusicaSlider.value);
        PlayerPrefs.Save();
    }
    public void SetVolumeEfeitos(){
        PlayerPrefs.SetFloat("VolumeEfeitos", volumeEfeitosSlider.value);
        PlayerPrefs.Save();
    }

    public void Exit() {
        Application.Quit();
    }

}
