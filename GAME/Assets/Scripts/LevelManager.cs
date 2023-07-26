using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Animator tanim;
    [SerializeField] private float tduration;

    public void LoadNextLevel(float delay = 0f)
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex +1, delay));
    }

    public void ReloadLevel(float delay = 0f)
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex, delay));
    }

    public void PlayLevel(int buildIndex)
    {
        StartCoroutine(LoadLevel(buildIndex));
    }

    IEnumerator LoadLevel(int buildIndex, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        tanim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(tduration);
        AsyncOperation op = SceneManager.LoadSceneAsync(buildIndex);
        while(!op.isDone)
        {
            yield return null;
        }
    }
}
