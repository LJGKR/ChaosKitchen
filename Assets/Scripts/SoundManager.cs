using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

	private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
	{
		AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
	}
	//함수를 불러올떄 position에 Camera.main.transform.position을 통해 카메라 위치에 사운드를 재생시킴
}
