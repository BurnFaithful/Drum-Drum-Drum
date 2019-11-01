using System;
using UnityEngine;

public class Note : BMSObject
{
    public int Line { get; set; }
    public string SoundKey { get; set; }
    public Judge JudgeObj { get; set; }
    public GameObject NoteObj { get; set; }

    public Note() : base() { }
    public Note(int bar, int beatLength, float beat, string soundKey) : base(bar, beatLength, beat)
    {
        JudgeObj = new Judge();
        this.SoundKey = soundKey;
    }

    public Note(int bar, int beatLength, float beat, int line, string soundKey) : base(bar, beatLength, beat)
    {
        JudgeObj = new Judge();
        this.Line = line;
        this.SoundKey = soundKey;
    }

    public Note(int bar, int beatLength, float beat, string soundKey, float judgeTime) : base(bar, beatLength, beat)
    {
        JudgeObj = new Judge();
        this.SoundKey = soundKey;
    }

    public Note(int bar, int beatLength, float beat, int line, string soundKey, float judgeTime) : base(bar, beatLength, beat)
    {
        JudgeObj = new Judge();
        this.Line = line;
        this.SoundKey = soundKey;
    }

    public override void DisableObject()
    {
        SoundManager.Instance.PlayOneShotSound(SoundKey);
        NoteObj.SetActive(false);
    }
}