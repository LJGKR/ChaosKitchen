using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;

	void Start()
	{
		KitchenGameManager.Instance.OnStateChanged += KitchenGameManger_OnStateChanged;

		Hide();
	}

	void KitchenGameManger_OnStateChanged(object sender, EventArgs e)
	{
		if (KitchenGameManager.Instance.IsGameOver())
		{
			Show();

			recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessRecipeNum().ToString();
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
