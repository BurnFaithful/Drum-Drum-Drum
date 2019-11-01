using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject bgPanel;

    // Start is called before the first frame update
    void Start()
    {
        //bgPanel.transform.DOShakePosition(10f, 5);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            OnClickStart();
        else if (Input.GetKeyDown(KeyCode.S))
            OnClickExit();
    }

    public void OnClickStart()
    {
        SceneManager.LoadSceneAsync((int)eSceneName.SCENE_SELECT);
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
