using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private AsyncOperation operation;

    private void Start()
    {
        LoadManager.Instance.Parser.ParseBMS(SingletonData.Instance.selectedMusicInfo.bmsFilePath.bmsPath);
        LoadManager.Instance.LoadAudioSource(SingletonData.Instance.selectedMusicInfo.bmsFilePath.bmsParentPath, LoadManager.Instance.SoundData);

        //StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        operation = SceneManager.LoadSceneAsync((int)eSceneName.SCENE_MAINGAME, LoadSceneMode.Single);
        //operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;
        }

        //operation.allowSceneActivation = true;
        yield return null;
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        yield return null;
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return null;
    }
}
