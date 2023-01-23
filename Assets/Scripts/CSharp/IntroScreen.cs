using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour
{
    public float introLength = 5f;
    private AsyncOperation sceneload;

    private void Start()
    {
        StartCoroutine(nextSceneTimer());
        sceneload = SceneManager.LoadSceneAsync(1);
        sceneload.allowSceneActivation = false;
    }

    IEnumerator nextSceneTimer()
    {
        yield return new WaitForSeconds(introLength);
        sceneload.allowSceneActivation = true;
        yield return null;
    }
}
