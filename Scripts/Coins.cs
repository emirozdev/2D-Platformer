using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    GameSession gameSession;
    AudioManager audioManager;
    bool wasCollected = false;
    private void Start()
    {
        audioManager= GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player"&& !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().collectCoin();
            audioManager.playSFX(audioManager.coin);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
