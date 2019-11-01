using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoSingleton<NoteManager> {

    [Serializable]
    public class Line
    {
        public float noteLinePosX;
        public Judge judge;
        public Queue<Note> linearNotes;

        public Line()
        {
            judge = new Judge();
            linearNotes = new Queue<Note>();
        }

        public Note GetFirstNote()
        {
            if (linearNotes.Count != 0)
                return linearNotes.Peek();

            return null;
        }

        public void AutoProcessNote(ref float elapsedTime)
        {
            Note destNote = GetFirstNote();
            if (destNote == null) return;

            if (destNote.JudgeTime - elapsedTime <= 0f)
            {
                destNote.DisableObject();
                linearNotes.Dequeue();
                UIManager.Instance.UpdateRecord(HitState.HIT, JudgeType.PERFECT);
                SingletonData.Instance.resultData.judgeCount[JudgeType.PERFECT]++;
            }
        }

        public void ProcessNote(HitState hitState, ref float elapsedTime)
        {
            Note destNote = GetFirstNote();
            if (destNote == null) return;

            JudgeType judgeResult = judge.Judgement(destNote, ref elapsedTime);

            if (judge.HitState == HitState.IGNORE) return;

            if (judge.HitState == hitState)
            {
                destNote.DisableObject();
                linearNotes.Dequeue();
                UIManager.Instance.UpdateRecord(hitState, judgeResult);
                SingletonData.Instance.resultData.judgeCount[judgeResult]++;
            }
        }
    }
    [SerializeField] private Line[] noteLine;
    [SerializeField] private GameObject[] notePrefab;
    public GameObject[] NotePrefab { get { return notePrefab; } }

    private float elapsedTime;
    public float NoteSpeed { get; set; }

    public delegate void HitBGM(ref float time);

    public static event HitBGM ObjectHitSubject;

    private float[] LineInitializer()
    {
        GameObject linePosParent = GameObject.Find("LineLayout");

        int lineLength = linePosParent.transform.childCount;
        float[] linePos = new float[lineLength];
        for (int i = 0; i < lineLength; ++i)
        {
            linePos[i] = linePosParent.transform.GetChild(i).position.x;
        }
        return linePos;
    }

    protected override void Initialize()
    {
        noteLine = new Line[GlobalDefine.KEY_NUM];
        for (int i = 0; i < noteLine.Length; ++i)
        {
            noteLine[i] = new Line();
            noteLine[i].noteLinePosX = LineInitializer()[i];
        }

        SingletonData.Instance.resultData = new JudgeScore();
    }

    private void Awake()
    {
        DebugWrapper.Assert(notePrefab != null, "노트 프리팹을 설정해주세요.");

        this.Initialize();
    }

    // Use this for initialization
    void Start()
    {
        elapsedTime = 0.0f;
	}

    void FixedUpdate()
    {
        NoteSpeed = GlobalDefine.NOTE_HEIGHT * (Time.fixedDeltaTime * GameManager.Instance.BeatPerSec);
        transform.Translate(Vector2.down * NoteSpeed);

        elapsedTime += Time.fixedDeltaTime;

        ProcessBackgroundAudio();

        if (GameManager.Instance.IsAutoMode)
        {
            AutoMode();
        }
        else
        {
            InputMode();
        }
    }

    public void CreateBGSound(List<BGSound> bgSoundList)
    {
        foreach (BGSound dataSet in bgSoundList)
        {
            dataSet.CalculateJudgeTime(GameManager.Instance.GetBPM());
            ObjectHitSubject += dataSet.Notify;
        }
    }

    public void CreateNote(List<Note> noteList)
    {
        foreach (Note dataSet in noteList)
        {
            float startPos = GlobalDefine.JUDGELINE_POS_Y;

            int lineIndex = 0;
            if (dataSet.Line >= 1 && dataSet.Line <= GlobalDefine.KEY_NUM + 1) // 허용 키 갯수
            {
                if (GlobalDefine.noteLineMatch.TryGetValue(dataSet.Line, out lineIndex))
                {
                    dataSet.CalculateJudgeTime(GameManager.Instance.GetBPM());
                    dataSet.NoteObj = Instantiate(notePrefab[lineIndex % notePrefab.Length]);
                    dataSet.NoteObj.name = "Note_" + dataSet.SoundKey;

                    float disposeX = noteLine[lineIndex].noteLinePosX;
                    float disposeY = startPos + (dataSet.CountBeat() * GlobalDefine.NOTE_HEIGHT * GlobalDefine.STD_BEAT);
                    dataSet.NoteObj.transform.position = new Vector3(disposeX, disposeY, 0f);
                    dataSet.NoteObj.transform.parent = this.transform;

                    noteLine[lineIndex].linearNotes.Enqueue(dataSet);
                }
            }
        }

        // 게임을 끝내고 결과화면으로 넘어갈 때의 타이머 계산용
        GameManager.Instance.PlayTime = noteList.Last().JudgeTime;
    }

    public void AutoMode()
    {
        for (int i = 0; i < noteLine.Length; ++i)
        {
            noteLine[i].AutoProcessNote(ref elapsedTime);
        }
    }

    public void InputMode()
    {
        for (int i = 0; i < noteLine.Length; ++i)
        {
            if (Input.GetKeyDown(InputManager.Instance.KeySetter.keys[i]))
            {
                //DebugWrapper.Log($"{InputManager.Instance.KeySetter.keys[i]} 입력");
                noteLine[i].ProcessNote(HitState.HIT, ref elapsedTime);
            }
            else
            {
                noteLine[i].ProcessNote(HitState.MISS, ref elapsedTime);
            }
        }
    }

    public void ProcessBackgroundAudio()
    {
        ObjectHitSubject?.Invoke(ref elapsedTime);
    }

    // Test
    public void ProcessNote(HitState hitState, JudgeType type)
    {
        if (type == JudgeType.IGNORE) return;

        UIManager.Instance.UpdateRecord(hitState, type);
    }
}
