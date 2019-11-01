using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    private Numberling[] numberlingElements;

    private void Awake()
    {
        numberlingElements = GetComponentsInChildren<Numberling>();
    }
    
    void Start()
    {
        for (JudgeType i = JudgeType.PERFECT; i != JudgeType.IGNORE; ++i)
        {
            numberlingElements[(int)i].RollingNumber(SingletonData.Instance.resultData.judgeCount[i]);
        }
        numberlingElements[4].RollingNumber(SingletonData.Instance.resultData.maxCombo);
    }

    private void Update()
    {
        if (InputManager.Instance.AnyKeyDown())
        {
            SceneManager.LoadSceneAsync((int)eSceneName.SCENE_SELECT);
        }
    }
}
