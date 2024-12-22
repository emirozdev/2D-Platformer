using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] float loadDelayTime;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(loadNextLevel());
    }
    IEnumerator loadNextLevel()
    {
        yield return new WaitForSeconds(loadDelayTime);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene +1);
    }
}
