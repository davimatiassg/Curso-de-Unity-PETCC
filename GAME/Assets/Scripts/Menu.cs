using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [SerializeField] GameObject optionsMenu;

    private bool menuOpcoesAberto = false;

    void Start()
    {
        // Carregar as configurações de volume atuais
        Slider volumeMusicaSlider = optionsMenu.transform.GetChild(0).gameObject.GetComponent<Slider>();
        Slider volumeEfeitosSlider = optionsMenu.transform.GetChild(1).gameObject.GetComponent<Slider>();

        volumeMusicaSlider.value = PlayerPrefs.GetFloat("VolumeMusica", 1f);
        volumeEfeitosSlider.value = PlayerPrefs.GetFloat("VolumeEfeitos", 1f);
    }

    public void Play() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Jogo");
    }

    public void OptionsMenu() {
        menuOpcoesAberto = !menuOpcoesAberto;
        optionsMenu.SetActive(menuOpcoesAberto);
    }

    public void Exit() {
        Debug.Log("NUNNCAAAAAAAAAA !!!");
        Application.Quit();
    }

}
