using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
	[Serializable]
	public struct KitchenObjectSO_GameObject
	{
		public KitchenObejctSO kitchenObjectSO;
		public GameObject gameObject;
	}
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

	void Start()
	{
		plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

		foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList)
		{
				kitchenObjectSOGameObject.gameObject.SetActive(false);
		}
	}

	void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
	{
		foreach(KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList)
		{
			if(kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO) //이벤트로 받은 키친오브젝트가 리스트에 있는 오브젝트와 일치하면
			{
				kitchenObjectSOGameObject.gameObject.SetActive(true); //해당 재료를 활성화하여 보이게 처리
			}
		}
	}
}
