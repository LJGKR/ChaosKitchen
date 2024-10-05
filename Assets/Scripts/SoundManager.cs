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
	//�Լ��� �ҷ��Ë� position�� Camera.main.transform.position�� ���� ī�޶� ��ġ�� ���带 �����Ŵ
}
