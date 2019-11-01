using UnityEngine;

public enum HitState
{
    MISS = 0,
    HIT,
    IGNORE
}

public enum JudgeType
{
    PERFECT = 0,
    GREAT,
    GOOD,
    MISS,
    IGNORE
}

public class Judge
{
    private HitState hitState;
    public HitState HitState => hitState;

    public JudgeType Judgement(Note destNode, ref float curTime)
    {
        /* 노트가 판정선에 도달하는 시간과 현재 지난 시간의 차이를 비교 */
        /* 차이가 판정값보다 작다면, 판정 처리 */
        float difference = destNode.JudgeTime - curTime;

        if (difference < 0f)
        {
            hitState = HitState.MISS;
            return JudgeType.MISS;
        }
        else if (difference - GameManager.Instance.PerfectJudgeTime <= Mathf.Epsilon)
        {
            hitState = HitState.HIT;
            return JudgeType.PERFECT;
        }
        else if (difference - GameManager.Instance.GreatJudgeTime <= Mathf.Epsilon)
        {
            hitState = HitState.HIT;
            return JudgeType.GREAT;
        }
        else if (difference - GameManager.Instance.GoodJudgeTime <= Mathf.Epsilon)
        {
            hitState = HitState.HIT;
            return JudgeType.GOOD;
        }
        else
        {
            hitState = HitState.IGNORE;
            return JudgeType.IGNORE;
        }
    }
}
