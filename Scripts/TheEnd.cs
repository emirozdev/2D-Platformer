using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TheEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(theEnd());
    }
    IEnumerator theEnd()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(0);
    }

    
}
