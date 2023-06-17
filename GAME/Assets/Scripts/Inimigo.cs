using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public GameObject Jogador;
    public float Velocidade = 3f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 direcao = (Vector2)Jogador.transform.position - (Vector2)transform.position;
        direcao = new Vector2(direcao.x, 0f); // Define a componente vertical como zero
        float distanciaHorizontal = direcao.x;

        Quaternion novaRotacao = Quaternion.LookRotation(Vector3.forward, direcao);
        rb.MoveRotation(novaRotacao.eulerAngles.z);

        if (Mathf.Abs(distanciaHorizontal) < 8 && Mathf.Abs(distanciaHorizontal) > 1.1)
        {
            rb.velocity = direcao.normalized * Velocidade;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
