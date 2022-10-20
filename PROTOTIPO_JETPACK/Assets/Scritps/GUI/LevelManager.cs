using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject plFailed; // painel gameover
    public GameObject plCompleted; // painel n�vel completado
    public GameObject text360; // texto para exibir 360
    public Transform localSpawn; // Local de respawn do jogador


    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        plFailed.SetActive(false);
        plCompleted.SetActive(false);
        text360.SetActive(false);
    }

    /// <summary>
    /// M�todo para habilitar painel de gameover
    /// </summary>
    public void OnFailed()
    {
        Debug.Log("Falhou!");

        plFailed.SetActive(true);
    }

    /// <summary>
    /// M�todo para habilitar painel de n�vel completado
    /// </summary>
    public void OnCompleted()
    {
        Debug.Log("N�vel completado!");
        plCompleted.SetActive(true);
    }

    /// <summary>
    /// M�todo chamado com bot�o na cena para respawn do jogador
    /// </summary>
    public void OnReload()
    {
        Debug.Log("restart");
        StartCoroutine(Respawn());

        // Scene scene = SceneManager.GetActiveScene();
        // SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// Coroutine para respawn do jogador na cena
    /// </summary>
    /// <returns></returns>
    IEnumerator Respawn()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject playerDead = GameObject.FindGameObjectWithTag("PlayerDead");

        if (playerDead != null)
            Destroy(playerDead);

        if (player != null)
            Destroy(player);

        yield return new WaitForSeconds(0.5f);

        Instantiate(playerPrefab, localSpawn.position, Quaternion.identity);
        MyCameraManger.Instance.CameraZoonStart();
    }

    #region Objetos

    /// <summary>
    /// habilita texto 360
    /// </summary>
    public void Active360() => text360.SetActive(true);
    #endregion
}
