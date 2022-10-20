using System.Collections;
using UnityEngine;
using Cinemachine;

public class MyCameraManger : Singleton<MyCameraManger>
{
    public float zoomStart, zoomOut;

    private float shakeTimer;
    private float shakeTimerTotal;
    private float startIntenity;

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = zoomStart;
       // cinemachineVirtualCamera.GetComponent<CinemachineConfiner>().InvalidatePathCache();
    }

    /// <summary>
    /// Metodo para tremer a câmera
    /// </summary>
    public void CameraShake(float intensity, float time)
    {
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        startIntenity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startIntenity, 0f, 1f - (shakeTimer / shakeTimerTotal));
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
            CameraZoonStart();


        if (Input.GetKeyDown(KeyCode.D))
            CameraZoonOut();
    }


    /// <summary>
    /// Configura a camerar "camFollow" para seguir o jogador
    /// </summary>
    /// <param name="target"></param>
    public void FollowPlayer(GameObject target)
    {
        cinemachineVirtualCamera.m_Follow = target.transform;
        cinemachineVirtualCamera.m_LookAt = target.transform;
    }

    #region Zoom

    /// <summary>
    /// Método para restaurar o zoom
    /// </summary>
    public void CameraZoonStart()
    {
        StartCoroutine(Zoom(zoomStart, 0.5f)); // driveCam.m_YAxis.Value = 0;
    }

    /// <summary>
    /// Método para aplicar zoom
    /// </summary>
    public void CameraZoonOut()
    {
        StartCoroutine(Zoom(zoomOut, 2f)); // driveCam.m_YAxis.Value = 0;
    }


    // Aplica zoom na camera
    IEnumerator Zoom(float lens, float duracao)
    {
        float elapsed = 0.0f;
        float posAtual = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        while (elapsed <= duracao)
        {
            elapsed += Time.deltaTime;
            //cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp (posAtual, lens, elapsed / duracao);
            cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.SmoothStep (posAtual, lens, elapsed / duracao);
            yield return null;
        }
    }
    #endregion
}
