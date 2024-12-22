using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives;
    [SerializeField] public int playerCoins;
    [SerializeField] TextMeshProUGUI playerLivesText;
    [SerializeField] TextMeshProUGUI playerCoinsText;

    private void Awake()
    {

        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        playerCoinsText.text=playerCoins.ToString();
        playerLivesText.text=playerLives.ToString();
    }

    public void playerLivesProcess()
    {
        if (playerLives > 1)
        {
            takeLife();
        }
        else
        {
            StartCoroutine(gameOver());
        }


    }

    IEnumerator gameOver()
    {

        Debug.Log("Game Over");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(8);
        Destroy(gameObject);


    }
    public void collectCoin()
    {
        playerCoins++;
        playerCoinsText.text = playerCoins.ToString();

    }

    public void takeLife()
    {
        playerLives--;
        playerLivesText.text = playerLives.ToString();

    }
    
    public void buyHeart()
    {
        if (playerCoins > 2)
        {
            playerLives++;
            playerCoins = playerCoins - 3;
            playerCoinsText.text = playerCoins.ToString();
            playerLivesText.text = playerLives.ToString();
        }
        else
        {
            Debug.Log("Yetersiz coin");
        }

    }
}
