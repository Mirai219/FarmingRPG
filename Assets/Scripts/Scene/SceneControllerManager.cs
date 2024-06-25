using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControllerManager : SingletonMonobehaviour<SceneControllerManager> {

    private bool isFading = false;
    [SerializeField] private float fadeDuration =1.0f;
    [SerializeField] protected CanvasGroup faderCanvasGroup = null;
    [SerializeField] private Image faderImage = null;

    public SceneName startingSceneName;

    private IEnumerator Start()
    {
        faderImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGroup.alpha = 1f;

        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName.ToString()));

        EventsHandler.SceneEventsHandler.CallAfterSceneLoadEvent();

        SaveLoadManager.Instance.RestoreCurrentSceneData();

        StartCoroutine(Fade(0f));
    }
    public void FadeAndLoadScene(string sceneName,Vector3 spawnPosition)
    {
        if(!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
        }
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition)
    {
        EventsHandler.SceneEventsHandler.CallBeforeSceneUnloadFadeOutEvent();

        yield return StartCoroutine(Fade(1f));

        SaveLoadManager.Instance.StoreCurrentSceneData();

        Player.Instance.gameObject.transform.position = spawnPosition;

        EventsHandler.SceneEventsHandler.CallBeforeSceneUnloadEvent();

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        EventsHandler.SceneEventsHandler.CallAfterSceneLoadEvent();

        SaveLoadManager.Instance.RestoreCurrentSceneData();

        yield return StartCoroutine(Fade(0f));

        EventsHandler.SceneEventsHandler.CallAfterSceneLoadFadeInEvent();


    }

    private IEnumerator Fade(float finalAlpah)
    {
        isFading = true;

        faderCanvasGroup.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpah) / fadeDuration;

        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpah))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha,finalAlpah,fadeSpeed*Time.deltaTime);

            yield return null; 
        }
        
        isFading = false;

        faderCanvasGroup.blocksRaycasts = false;

    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {

        yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);

        Scene newlyLoadedScene= SceneManager.GetSceneAt(SceneManager.sceneCount -1);

        SceneManager.SetActiveScene(newlyLoadedScene);
    }
}
