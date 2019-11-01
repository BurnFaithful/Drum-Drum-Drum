using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Numberling : MonoBehaviour
{
    public Action numberlingPrevAction;

    [SerializeField][ReadOnly] private GameObject parentPanel;
    public GameObject ParentPanel => parentPanel;
    [SerializeField][ReadOnly] private Transform[] numImage;

    private Queue<int> numberlingQueue;
    public Queue<int> NumberlingQueue => numberlingQueue;

    // Start is called before the first frame update
    void Awake()
    {
        parentPanel = transform.parent.gameObject;

        int numberLength = transform.childCount;
        numImage = new Transform[numberLength];
        for (int i = 0; i < numberLength; ++i)
        {
            numImage[i] = transform.GetChild(numberLength - i - 1);
        }

        numberlingQueue = new Queue<int>();
    }

    public void SetNumber(ref int number, bool isFill = false)
    {
        numberlingPrevAction?.Invoke();

        Util.CalculateDigit(ref numberlingQueue, number);
        if (numberlingQueue != null)
        {
            int digit = 0;
            while (numberlingQueue.Count > 0)
            {
                if (!isFill) numImage[digit].gameObject.SetActive(true);
                numImage[digit].GetComponent<Image>().sprite = LoadManager.Instance.NumberSprites[numberlingQueue.Dequeue()];
                digit++;
            }
            if (!isFill)
            {
                for (int i = digit; i < numImage.Length; ++i)
                    numImage[i].gameObject.SetActive(false);
            }
        }
    }

    public void RollingNumber(int number, float duration = 1f)
    {
        numberlingPrevAction?.Invoke();

        StartCoroutine(CoRollingNumber(number, duration));
    }

    IEnumerator CoRollingNumber(int number, float duration)
    {
        int rollingNum = 0;
        float rollSpeed = duration / number;
        int increase = Mathf.CeilToInt(Time.deltaTime / rollSpeed);

        while (rollingNum < number)
        {
            rollingNum += increase;
            if (rollingNum > number)
                rollingNum = number;
            SetNumber(ref rollingNum, true);

            yield return new WaitForFixedUpdate();
        }
    }
}
