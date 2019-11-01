using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    private Image blinkTarget;
    private bool isBlinkOn;
    public bool IsBlinkOn
    {
        get { return isBlinkOn; }
        set
        {
            isBlinkOn = value;
            if (isBlinkOn)
                blinkTarget.enabled = true;
            else
                blinkTarget.enabled = false;
        }
    }
    [SerializeField] private bool isProcessing;
    [SerializeField] private float interval;

    private void Awake()
    {
        blinkTarget = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DebugWrapper.Assert(blinkTarget != null, $"'{this.gameObject}'는 blinkTarget이 없습니다.");

        isProcessing = true;
        interval = 1f;
        StartCoroutine(BlinkProcess(interval));
    }

    IEnumerator BlinkProcess(float interval, bool isProcessing = true)
    {
        while (isProcessing)
        {
            yield return new WaitForSeconds(interval);

            IsBlinkOn = !IsBlinkOn;
        }
    }
}
