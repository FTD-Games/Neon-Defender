using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    // Loading screen for generating dungeon
    public Image loadingBar;
    public TextMeshProUGUI loadingStatus;
    public TextMeshProUGUI loadingHint;
    public GameObject loadingScreen;

    void Awake()
    {
        loadingScreen.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameControl.control.sceneLoader = this;
    }

    public void SetHint(string theHint) => loadingHint.text = theHint;

    public void RefreshStatus(float incProgress)
    {
        loadingBar.fillAmount = incProgress;
        loadingStatus.text = (incProgress * 100.0f).ToString("f0") + " %";
    }

    public void LoadScene(int sceneNr)
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneNow(sceneNr));
    }

    IEnumerator LoadSceneNow(int sceneNr)
    {
        yield break;
    }
}
