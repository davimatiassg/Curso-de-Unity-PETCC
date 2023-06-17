using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public GameObject Jogador;
    public float Velocidade = 3f;
    public float atkSpd = 5f;
    public int ataque = 1;

    private float disMax = 8f;
    private float disMin = 1.1f;
    private float disAtk = 1.1f;

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

        if (Mathf.Abs(distanciaHorizontal) < disMax && Mathf.Abs(distanciaHorizontal) > disMin)
        {
            /// Persegue o jogador
            rb.velocity = direcao.normalized * Velocidade;
        }
        else
        {
            /// Fica parado
            rb.velocity = Vector2.zero;

            /// Ataca o jogador se eles estiver em alcançe de ataque
            if (Vector2.Distance(transform.position, Jogador.transform.position) <= disAtk)
            {
                AtacarJogador();
            }
        }
    }

    /// Chama o método 'TakeDmg' do jogador acada 'atkSpd' segundos
    private float t = 0f;
    void AtacarJogador()
    {
        if (t >= atkSpd*Time.deltaTime)
        {
            Jogador.GetComponent<Jogador>().TakeDmg(ataque);
            t = 0f;
        }
        t ++;
    }

}
