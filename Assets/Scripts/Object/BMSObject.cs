[System.Serializable]
public abstract class BMSObject
{
    /// <summary>
    /// 1마디. 박자에 따라 달라짐. 1마디 4비트 표준.
    /// </summary>
    protected int bar;
    /// <summary>
    /// 해당 노트가 첫 비트를 기준으로 얼마나 떨어져있는지. 일종의 offset.
    /// </summary>
    protected int beatLength;
    /// <summary>
    /// 1비트. 노트 1개에 해당
    /// </summary>
    protected float beat;
    /// <summary>
    /// 시작 시간부터 노트가 판정라인에 도달할 때까지의 시간(BPM, BEAT를 통해 계산)
    /// </summary>
    protected float judgeTime;

    public int Bar => bar;
    public int BeatLength => beatLength;
    public float Beat => beat;

    public float JudgeTime { get { return judgeTime; } private set { judgeTime = value; } }

    protected BMSObject() { DebugWrapper.LogWarning($"{this.GetType()} Default Constructor is not available."); }

    protected BMSObject(int bar, float beat)
    {
        this.bar = bar;
        this.beat = beat;
    }

    protected BMSObject(int bar, int beatLength, float beat)
    {
        this.bar = bar;
        this.beatLength = beatLength;
        this.beat = (beat / beatLength) * GlobalDefine.STD_BEAT;
    }

    public virtual float CountBeat()
    {
        int sum = 0;
        for (int i = 0; i < bar; ++i)
        {
            sum += GlobalDefine.STD_BEAT;
        }
        return sum + beat;
    }

    public virtual void CalculateJudgeTime(int bpm)
    {
        float tempBeat = CountBeat();

        float time = (tempBeat / bpm) * 60.0f;
        judgeTime = time;
    }

    public virtual void DisableObject() { }
}
