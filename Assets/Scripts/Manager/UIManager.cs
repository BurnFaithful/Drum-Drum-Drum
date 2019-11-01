using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JudgeScore
{
    public int maxCombo;
    public int finalScore;
    public Dictionary<JudgeType, int> judgeCount;

    public JudgeScore()
    {
        this.maxCombo = 0;
        this.finalScore = 0;
        this.judgeCount = new Dictionary<JudgeType, int>();
        for (JudgeType i = JudgeType.PERFECT; i != JudgeType.IGNORE; ++i)
        {
            judgeCount[i] = 0;
        }
    }

    public JudgeScore(int maxCombo, int finalScore)
    {
        this.maxCombo = maxCombo;
        this.finalScore = finalScore;
        this.judgeCount = new Dictionary<JudgeType, int>();
        for (JudgeType i = JudgeType.PERFECT; i != JudgeType.IGNORE; ++i)
        {
            judgeCount[i] = 0;
        }
    }

    /// <summary>
    /// PerfectCnt, GreatCnt, GoodCnt, MissCnt 4개의 파라미터를 설정하고 싶은 갯수만큼 순서대로 설정.
    /// 설정되지 않은 값은 0으로 초기화.
    /// </summary>
    /// <param name="maxCombo"></param>
    /// <param name="finalScore"></param>
    /// <param name="judgeValue"></param>
    public JudgeScore(int maxCombo, int finalScore, params int[] judgeValue)
    {
        this.maxCombo = maxCombo;
        this.finalScore = finalScore;
        this.judgeCount = new Dictionary<JudgeType, int>();
        for (JudgeType i = JudgeType.PERFECT; i != JudgeType.IGNORE; ++i)
        {
            if ((int)i <= judgeValue.Length - 1)
                judgeCount[i] = judgeValue[(int)i];
            else
                judgeCount[i] = 0;
        }
    }
}

public class UIManager : MonoSingleton<UIManager>
{
    //--------------- UI Object
    [SerializeField] private GameObject judgePanel;
    private Numberling numberling;

    public Sprite[] judgeTextSpriteArr;

    //--------------- Variable
    private int combo;
    public int Combo { get { return combo; } set { combo = value; } }
    private int score;
    public int Score { get { return score; } set { score = value; } }

    private void JudgeUIIntialize()
    {
        judgePanel = GameObject.Find("JudgePanel");
        numberling = GetComponentInChildren<Numberling>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    protected override void Initialize()
    {
        JudgeUIIntialize();
    }

    private void Awake()
    {
        DebugWrapper.Assert(judgeTextSpriteArr != null, this.GetType() + " : Where is Judge Text Sprite");

        this.Initialize();

        numberling.numberlingPrevAction = () =>
        {
            numberling.ParentPanel.transform.localScale = Vector2.zero;
            numberling.ParentPanel.transform.DOScale(Vector2.one, 0.1f).SetEase(Ease.OutQuint);
        };
    }

    public void UpdateRecord(HitState hitState, JudgeType type)
    {
        if (hitState.Equals(HitState.HIT))
        {
            combo++;
            ShowCombo();
        }
        else
        {
            if (combo > SingletonData.Instance.resultData.maxCombo)
                SingletonData.Instance.resultData.maxCombo = combo;
            combo = 0;
        }
        ShowJudge(type);
    }

    private void ShowCombo()
    {
        numberling.SetNumber(ref combo);
        numberling.ParentPanel.SetActive(true);
    }

    private void ShowJudge(JudgeType type)
    {
        judgePanel.GetComponentInChildren<Image>().sprite = judgeTextSpriteArr[(int)type];
        judgePanel.transform.localScale = Vector2.zero;
        judgePanel.transform.DOScale(Vector2.one, 0.1f).SetEase(Ease.OutQuint);
        judgePanel.SetActive(true);
    }
}
