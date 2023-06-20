using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float velocidade = 2f;
    [SerializeField] private Transform jogador;

    // Update is called once per frame
    void Update()
    {
        Vector3 novaPos = new Vector3(jogador.position.x, jogador.position.y + 1f, transform.position.z);
        transform.position = Vector3.Slerp(transform.position, novaPos, velocidade*Time.deltaTime); 
    }
}
