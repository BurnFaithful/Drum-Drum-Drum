using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoSingleton<GameManager>
{
#if UNITY_EDITOR
    private float elapsedTime;
#endif

    [SerializeField] private bool isAutoMode;
    public bool IsAutoMode { get { return isAutoMode; } private set { isAutoMode = false; } }
    [SerializeField] [ReadOnly] private bool isPause;
    public bool IsPause => isPause;

    public float PlayTime { get; set; }

    public float BarPerSec { get; private set; }
    public float BeatPerSec { get; private set; }
    public float SecPerBar { get; private set; }
    public float SecPerBeat { get; private set; }

    public float PerfectJudgeTime { get; private set; }
    public float GreatJudgeTime { get; private set; }
    public float GoodJudgeTime { get; private set; }

    protected override void Initialize()
    {
        Screen.SetResolution(GlobalDefine.SCREEN_WIDTH, GlobalDefine.SCREEN_HEIGHT, true);
    }

    private void Awake()
    {
        this.Initialize();
    }

    void Start()
    {
#if UNITY_EDITOR
        elapsedTime = 0.0f;
        isAutoMode = false;
#endif

        BeatSetting(GetBPM());
        SoundManager.Instance.CreateSound(LoadManager.Instance.AudioClipData);
        NoteManager.Instance.CreateBGSound(LoadManager.Instance.BGSoundData);
        NoteManager.Instance.CreateNote(LoadManager.Instance.NoteData);

        StartCoroutine(CheckMusicEnd());
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isAutoMode = !isAutoMode;
            if (isAutoMode)
                DebugWrapper.Log("Auto Mode On");
            else
                DebugWrapper.Log("Auto Mode Off");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            isPause = !isPause;
            if (isPause)
            {
                Time.timeScale = 0f;
                DebugWrapper.Log("Game Pause");
            }
            else
            {
                Time.timeScale = 1f;
                DebugWrapper.Log("Game Resume");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DebugWrapper.LogWarning("ESC 키를 통해 실행을 정지했습니다.");
            EditorApplication.isPlaying = false;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            SceneManager.LoadSceneAsync((int)eSceneName.SCENE_RESULT);
        }
    }
#endif

#if UNITY_EDITOR
    private void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;
    }
#endif

    public BMSHeader GetBMSHeader()
    {
        return LoadManager.Instance.Parser.BmsHeader;
    }

    public int GetBPM()
    {
        return GetBMSHeader().info.bpm;
    }

    public void BeatSetting(int bpm)
    {
        // 60BPM에서 초당 마디 = 0.25마디(4개의 노트)
        DebugWrapper.Assert(bpm != 0, "BPM 값이 설정되지 않았습니다.");

        BarPerSec = bpm / (60.0f * GlobalDefine.STD_BEAT);
        SecPerBar = 1 / BarPerSec;
        BeatPerSec = GlobalDefine.BEAT_PER_BAR * BarPerSec;
        SecPerBeat = 1 / BeatPerSec; 

        PerfectJudgeTime = SecPerBeat * 0.5f;
        GreatJudgeTime = SecPerBeat * 1.5f;
        GoodJudgeTime = SecPerBeat * 2.5f;
    }

    IEnumerator CheckMusicEnd()
    {
        if (PlayTime < Mathf.Epsilon)
            Debug.LogError("플레이 타임 설정이 제대로 이루어졌는지 확인이 필요합니다.");

        yield return new WaitForSeconds(PlayTime + 2f);

        if (SingletonData.Instance.resultData.maxCombo == 0)
            SingletonData.Instance.resultData.maxCombo = UIManager.Instance.Combo;

        SceneManager.LoadSceneAsync((int)eSceneName.SCENE_RESULT);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void AllStopCoroutine()
    {
        Application.quitting += StopAllCoroutines;
    }

#if UNITY_EDITOR
    [MenuItem("Test/PlayerPrefs Clear")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    void OnDrawGizmos()
    {
        Handles.Label(new Vector3(8f, 8f, 0f), "Elapsed Time : " + elapsedTime.ToString());
    }
#endif
}
