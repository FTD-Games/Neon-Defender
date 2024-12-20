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

    private void SetHint(string theHint) => loadingHint.text = theHint;

    private void RefreshStatus(float incProgress)
    {
        loadingBar.fillAmount = incProgress;
        loadingStatus.text = $"{incProgress * 100.0f:f0} %";
    }

    public void LoadScene(int sceneNr, string hint)
    {
        SetHint(hint);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneNow(sceneNr));
    }

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
