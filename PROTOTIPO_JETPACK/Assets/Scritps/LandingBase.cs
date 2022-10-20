using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingBase : MonoBehaviour
{
    public float timeLanding;
    public GameObject progressBar;
    public GameObject bar;

    bool isLanding;
    Vector3 localScale;
    BoxCollider2D bx;

    // Start is called before the first frame update
    void Start()
    {
        bx = GetComponent<BoxCollider2D>();
        localScale = bar.transform.localScale;
        isLanding = false;
    }

    /// <summary>
    /// Método chamado quando o jogodor pousa na base
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Landing" && !isLanding)
        {
            localScale.x = 0f;
            progressBar.SetActive(true);
            StartCoroutine(Loading());
        }
    }

    /// <summary>
    /// Método chamado quando o jogador sai da base
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        progressBar.SetActive(false);
        StopAllCoroutines();
    }

    /// <summary>
    /// Coroutine para calcular progresso
    /// </summary>
    /// <returns></returns>
    IEnumerator Loading()
    {
        float time = 0;

        while (localScale.x < 1f)
        {
            if (time < timeLanding)
            {
                time += Time.deltaTime;
                float progress = Mathf.Clamp01(time / 3.0f);
                localScale.x = progress;
                bar.transform.localScale = localScale;
            }

            yield return null;
        }

        Posed();
    }

    /// <summary>
    /// Método chamado quando o jogodor fica tempo suficiente na base
    /// </summary>
    void Posed()
    {
        isLanding = true;
        progressBar.SetActive(false);
        
        if (LevelManager.instance != null)
            LevelManager.instance.OnCompleted();

        Debug.Log("Ganhou!");
    }
}
