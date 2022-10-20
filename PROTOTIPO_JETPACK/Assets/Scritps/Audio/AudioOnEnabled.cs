using UnityEngine;

public class AudioOnEnabled : MonoBehaviour
{
	public string clip = "nomeAudio";

	private void Start()
	{
		AudioManager.main.PlayMusic(clip);
	}
}
