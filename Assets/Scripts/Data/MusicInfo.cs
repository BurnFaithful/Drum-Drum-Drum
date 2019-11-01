using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicInfo
{
    private string musicTitle;
    private List<BMSMusicInfo> bmsFileList;
    private int selectedLevelIndex;
    private GameObject musicListObj;
    
    public string MusicTitle { get { return musicTitle; } set { musicTitle = value; } }
    public List<BMSMusicInfo> BmsFileList { get { return bmsFileList; } private set { bmsFileList = value; } }
    public int SelectedLevelIndex { get { return selectedLevelIndex; } set { selectedLevelIndex = value; } }

    public MusicInfo()
    {
        this.musicTitle = "None Title";
        this.bmsFileList = new List<BMSMusicInfo>();
        selectedLevelIndex = 0;
    }

    public MusicInfo(string title)
    {
        this.musicTitle = title;
        this.bmsFileList = new List<BMSMusicInfo>();
        selectedLevelIndex = 0;
    }

    public void Notify(eDirection dir)
    {
        if (dir.Equals(eDirection.LEFT))
        {
            if (selectedLevelIndex > 0)
                selectedLevelIndex--;
        }
        else if (dir.Equals(eDirection.RIGHT))
        {
            if (selectedLevelIndex < bmsFileList.Count - 1)
                selectedLevelIndex++;
        }
    }
}

public class MusicList
{
    private List<MusicInfo> musicInfoList;
    public int SelectedMusicIndex { get; private set; }

    public MusicList()
    {
        musicInfoList = new List<MusicInfo>();
        SelectedMusicIndex = 0;
    }

    public MusicList(int startIndex)
    {
        musicInfoList = new List<MusicInfo>();
        SelectedMusicIndex = startIndex;
    }

    public MusicInfo GetSelectedMusic()
    {
        return musicInfoList[SelectedMusicIndex];
    }

    public int MusicListLength()
    {
        return musicInfoList.Count;
    }

    public void AddMusic(MusicInfo music)
    {
        musicInfoList.Add(music);
    }

    public void Notify(eDirection dir, Action prevAction = null, Action nextAction = null)
    {
        if (dir.Equals(eDirection.UP))
        {
            if (SelectedMusicIndex > 0)
            {
                prevAction?.Invoke();
                SelectedMusicIndex--;
                nextAction?.Invoke();
            }
        }
        else if (dir.Equals(eDirection.DOWN))
        {
            if (SelectedMusicIndex < musicInfoList.Count - 1)
            {
                prevAction?.Invoke();
                SelectedMusicIndex++;
                nextAction?.Invoke();
            }
        }
    }
}
