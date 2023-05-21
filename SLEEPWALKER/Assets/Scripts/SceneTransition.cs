using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    private string sceneToLoad;

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        FadeIn();
    }

    public void FadeIn()
    {
        fadeImage.CrossFadeAlpha(0f, fadeDuration, false);
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        sceneToLoad = sceneName;
        fadeImage.CrossFadeAlpha(1f, fadeDuration, false);
        Invoke(nameof(LoadDelayedScene), fadeDuration);
    }

    private void LoadDelayedScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
