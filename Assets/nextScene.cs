using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string sceneName; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            StartCoroutine(LoadSceneWithDelay(sceneName, 0f));
        }
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay); 
        SceneManager.LoadScene(sceneName); 
    }
}
