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
			if(kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO) //�̺�Ʈ�� ���� Űģ������Ʈ�� ����Ʈ�� �ִ� ������Ʈ�� ��ġ�ϸ�
			{
				kitchenObjectSOGameObject.gameObject.SetActive(true); //�ش� ��Ḧ Ȱ��ȭ�Ͽ� ���̰� ó��
			}
		}
	}
}
