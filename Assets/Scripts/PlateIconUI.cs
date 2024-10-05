using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchebObject;
    [SerializeField] private Transform iconTemplate;

	void Awake()
	{
		iconTemplate.gameObject.SetActive(false);
	}

	private void Start()
	{
		plateKitchebObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
	}

	void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
	{
		UpdateVisual();
	}

	void UpdateVisual()
	{
		foreach(Transform child in transform)
		{
			if (child == iconTemplate) continue;
			Destroy(child.gameObject);
		}

		foreach(KitchenObejctSO kitchenObjectSO in plateKitchebObject.GetKitchenObjectSOList())
		{
			Transform iconTransform = Instantiate(iconTemplate, transform);
			iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
			iconTransform.gameObject.SetActive(true);
		}
	}
}
