using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class GameStartCoundownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

	
	void Start()
	{
		KitchenGameManager.Instance.OnStateChanged += KitchenGameManger_OnStateChanged;

		Hide();
	}

	void Update()
	{
		countdownText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountdownToStartTimer()).ToString();
		//float을 정수형 카운트로 표현하기 위해 변환
	}

	void KitchenGameManger_OnStateChanged(object sender, EventArgs e)
	{
		if (KitchenGameManager.Instance.IsCountdownToStartActive())
		{
			Show();
		}
		else
		{
			Hide();
		}
	}

	void Show()
	{
		transform.gameObject.SetActive(true);
	}

	void Hide()
	{
		transform.gameObject.SetActive(false);
	}
}
