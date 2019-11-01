using UnityEngine;

public class BGSound : BMSObject
{
    private string soundKey;
    public string SoundKey => soundKey;

    public BGSound() : base() { }
    public BGSound(int bar, int beatLength, float beat, string soundKey) : base(bar, beatLength, beat)
    {
        this.soundKey = soundKey;
    }

    public override void DisableObject()
    {
        NoteManager.ObjectHitSubject -= this.Notify;
        SoundManager.Instance.PlayOneShotSound(soundKey);
    }

    public void Notify(ref float elapsedTime)
    {
        if (judgeTime - elapsedTime <= Mathf.Epsilon)
        {
            DisableObject();
        }
    }
}

