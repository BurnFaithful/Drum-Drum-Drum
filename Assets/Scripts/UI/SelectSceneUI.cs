using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SelectSceneUI : MonoBehaviour
{
    [SerializeField] private RectTransform musicContent;
    [SerializeField] private GameObject arrow_down;
    [SerializeField] private GameObject arrow_up;
    
    private float scrollSpeed;
    private bool isScroll;
    private float interval;
    
    private MusicList musicList;

    public delegate void InputArrowKey(eDirection dir);
    public delegate void InputArrowKeyWithAction(eDirection dir, Action action);
    public delegate void InputArrowKeyWithPrevNextAction(eDirection dir, Action prevAction, Action nextAction);

    public static event InputArrowKeyWithPrevNextAction ChangeMusicSubject;
    public static event InputArrowKey ChangeLevelSubject;

    private void Awake()
    {
        musicList = new MusicList();
        ChangeMusicSubject += musicList.Notify;
    }

    // Start is called before the first frame update
    void Start()
    {
        scrollSpeed = 100f;
        isScroll = false;

        interval = GlobalDefine.SCREEN_HEIGHT + musicContent.GetComponent<VerticalLayoutGroup>().spacing;

        BMSFileSystem.Instance.AllDirectoryRecursiveFileFind();
        List<MusicInfo> musicInfoList = BMSFileSystem.Instance.FindMusicInfoList;
        foreach (MusicInfo info in musicInfoList)
        {
            ChangeLevelSubject += info.Notify;
            this.AddMusicInfo(info);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isScroll) return;
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeMusicSubject?.Invoke(eDirection.UP,
                null,
                () =>
                {
                    if (musicList.SelectedMusicIndex == 0)
                        arrow_up.SetActive(false);
                    else if (musicList.SelectedMusicIndex == musicList.MusicListLength() - 2)
                        arrow_down.SetActive(true);
                    Scroll(musicList.SelectedMusicIndex);
                });
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeMusicSubject?.Invoke(eDirection.DOWN,
                null,
                () =>
                {
                    if (musicList.SelectedMusicIndex == musicList.MusicListLength() - 1)
                        arrow_down.SetActive(false);
                    else if (musicList.SelectedMusicIndex == 1)
                        arrow_up.SetActive(true);
                    Scroll(musicList.SelectedMusicIndex);
                });
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeLevelSubject?.Invoke(eDirection.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeLevelSubject?.Invoke(eDirection.RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            MusicInfo selectedMusic = musicList.GetSelectedMusic();
            SingletonData.Instance.selectedMusicInfo = selectedMusic.BmsFileList[selectedMusic.SelectedLevelIndex];
            SceneManager.LoadSceneAsync((int)eSceneName.SCENE_LOADING);
        }
    }

    private void OnDestroy()
    {
        ChangeMusicSubject -= musicList.Notify;
    }

    public void Scroll(int selectedMusicInex)
    {
        StartCoroutine(CoScroll(selectedMusicInex));
    }

    IEnumerator CoScroll(int selectedMusicIndex)
    {
        float cursorPosY = interval * selectedMusicIndex;
        do
        {
            musicContent.anchoredPosition = new Vector2(
                musicContent.anchoredPosition.x,
                Mathf.MoveTowards(musicContent.anchoredPosition.y, cursorPosY, Time.deltaTime * scrollSpeed));
        } while (Util.FDistance(musicContent.anchoredPosition.y, cursorPosY) > Mathf.Epsilon);

        isScroll = false;
        yield return isScroll;
    }

    public void AddMusicInfo(MusicInfo info)
    {
        musicList.AddMusic(info);
        AddMusicUIElement(info);
    }

    public void AddMusicUIElement(MusicInfo info)
    {
        GameObject musicObj = new GameObject(info.MusicTitle);
        musicObj.transform.parent = musicContent.transform;
        musicObj.transform.localPosition = Vector3.zero;
        musicObj.transform.localScale = Vector3.one;

        BMSMusicInfo selectedMusicInfo = info.BmsFileList[info.SelectedLevelIndex];
        TextMeshProUGUI musicText = musicObj.AddComponent<TextMeshProUGUI>();
        musicText.text = $"Title : {info.MusicTitle}\nGenre : {selectedMusicInfo.genre}\nArtist : {selectedMusicInfo.artistName}\nBPM : {selectedMusicInfo.bpm}\n";
        musicText.fontSize = 60;
        musicText.alignment = TextAlignmentOptions.CenterGeoAligned;

        musicObj.GetComponent<RectTransform>().sizeDelta = new Vector2(700, GlobalDefine.SCREEN_HEIGHT);
    }
}
