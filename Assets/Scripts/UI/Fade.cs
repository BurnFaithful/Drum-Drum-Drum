using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private Image fadePanel;
    private float fadeDuration;

    private void Awake()
    {
        //DebugWrapper.Assert(fadePanel);
        fadePanel = GetComponent<Image>();
    }

    private void Start()
    {
        fadeDuration = 2f;
        fadePanel.raycastTarget = false;
        
        FadeIn(fadeDuration);
    }

    public void FadeIn(float duration)
    {
        fadePanel.CrossFadeAlpha(0f, duration, false);
    }

    public void FadeOut(float duration)
    {
        fadePanel.CrossFadeAlpha(255f, duration, false);
    }
}
