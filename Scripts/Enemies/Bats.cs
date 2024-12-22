using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bats : MonoBehaviour
{
    GameObject player;
    [SerializeField] float batSpeed;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        // Objenin oyuncunun pozisyonuna ilerlemesini saðlar.
        transform.position = Vector2.MoveTowards(transform.position,player.transform.position,batSpeed*Time.deltaTime);
    }

}
