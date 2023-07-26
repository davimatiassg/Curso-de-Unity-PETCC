using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float velocidade = 2f;
    [SerializeField] private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 novaPos = new Vector3(player.position.x, player.position.y + 1f, transform.position.z);
        transform.position = Vector3.Slerp(transform.position, novaPos, velocidade*Time.deltaTime); 
    }
}
