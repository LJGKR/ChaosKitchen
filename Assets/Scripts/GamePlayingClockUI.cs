using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image gamePlayingTimerImage;

	void Update()
	{
		gamePlayingTimerImage.fillAmount = KitchenGameManager.Instance.GetGameplayingTimerNormalized();
	}
}
