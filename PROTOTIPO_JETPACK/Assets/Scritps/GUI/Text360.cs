using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text360 : MonoBehaviour
{
    public float tweenTime;

    private void OnEnable()
    {
        Tween();
    }

    private void Tween()
    {
        LeanTween.cancel(gameObject);

        transform.localScale = Vector3.one;

        LeanTween.scale(gameObject, Vector3.one * 1.8f, tweenTime).setEasePunch();//=> { gameObject.SetActive(false)};

        Invoke("Deactivate", 2f);

        //   gameObject.SetActive(false);
    }

    private void Deactivate() => gameObject.SetActive(false);
}
