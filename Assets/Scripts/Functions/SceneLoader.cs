using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private bool _activationNextScene;
    public Image loadingBar;
    public TextMeshProUGUI loadingStatus;
    public TextMeshProUGUI loadingHint;
    public GameObject loadingScreen;

    private void Start()
    {
        loadingScreen.SetActive(false);
        GameControl.control.SceneLoader = this;
    }

    /// <summary>
    /// Sets custom text to the loading screen, while loading.
    /// </summary>
    private void SetHint(string theHint) => loadingHint.text = theHint;

    /// <summary>
    /// Refreshs the status like: 5 %
    /// </summary>
    /// <param name="incProgress">The input progress is multiplied by 100 for percent. 0.5 -> 50 %</param>
    private void RefreshStatus(float incProgress)
    {
        loadingBar.fillAmount = incProgress;
        loadingStatus.text = $"{incProgress * 100.0f:f0} %";
    }

    /// <summary>
    /// Loads the target scene with a custom displayed hint. Also shows status progress automatically.
    /// </summary>
    public void LoadScene(int sceneNr, string hint)
    {
        SetHint(hint);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneNow(sceneNr));
    }

    /// <summary>
    /// Loads the scene async with controlling the status progress and progress bar.
    /// On finish loading the scene is automatically switched.
    /// </summary>
    IEnumerator LoadSceneNow(int sceneNr)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNr);
        operation.allowSceneActivation = false;
        RefreshStatus(0f);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            RefreshStatus(progress);
            if (progress >= 0.9f && !_activationNextScene)
                _activationNextScene = true;
            if (_activationNextScene)
            {
                yield return new WaitForSeconds(1f); // This line fakes a loading time from 1 secs, can be removed on release
                operation.allowSceneActivation = _activationNextScene;
            }
            yield return null;
        }
    }
}
