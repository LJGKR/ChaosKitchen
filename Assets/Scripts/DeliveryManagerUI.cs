using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

	void Awake()
	{
		recipeTemplate.gameObject.SetActive(false);
	}

	void Start()
	{
		DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
		DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;

		UpdateVisual();
	}

	void DeliveryManager_OnRecipeCompleted(object sender,EventArgs e)
	{
		UpdateVisual();
	}

	void DeliveryManager_OnRecipeSpawned(object sender, EventArgs e)
	{
		UpdateVisual();
	}

	void UpdateVisual()
	{
		foreach(Transform child in container)
		{
			if (child == recipeTemplate) continue; //���� �˻��߿� �ֹ� �����ǰ� �ִ� ���ø��� ã����
			Destroy(child.gameObject);
		}

		foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
		{
			Transform recipeTransform = Instantiate(recipeTemplate, container);
			recipeTransform.gameObject.SetActive(true);
			recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
		}
	}
}