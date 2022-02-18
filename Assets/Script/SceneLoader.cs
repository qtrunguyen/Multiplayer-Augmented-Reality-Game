using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public string sceneNameToBeLoaded;
    public void LoadScene(string _sceneName)
    {
        sceneNameToBeLoaded = _sceneName;

        StartCoroutine(InitializeSceneLoading());
    }

    IEnumerator InitializeSceneLoading()
    {
        //First, load the loading scene
        yield return SceneManager.LoadSceneAsync("Scene_Loading");

        //Load the actual scene
        StartCoroutine(LoadActualScene());
    }

    IEnumerator LoadActualScene()
    {
        var asyncSceneLoading = SceneManager.LoadSceneAsync(sceneNameToBeLoaded);
        
        //this stop the scene from displaying when it's still loading
        asyncSceneLoading.allowSceneActivation = false;

        while (!asyncSceneLoading.isDone)
        {
            Debug.Log(asyncSceneLoading.progress);
            if (asyncSceneLoading.progress >= 0.9f)
            {
                //Finally show the scene
                asyncSceneLoading.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
