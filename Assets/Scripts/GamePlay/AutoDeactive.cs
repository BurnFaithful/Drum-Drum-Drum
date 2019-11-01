using System.Collections;
using UnityEngine;

public class AutoDeactive : MonoBehaviour
{
    [SerializeField] private float duration = 0.15f;


    private void OnEnable()
    {
        StartCoroutine(AutoDisable(duration));
    }

    IEnumerator AutoDisable(float time)
    {
        yield return new WaitForSeconds(time);

        this.gameObject.SetActive(false);
    }
}
