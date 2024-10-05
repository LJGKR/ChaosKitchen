using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
	public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
	public class OnIngredientAddedEventArgs : EventArgs
	{
		public KitchenObejctSO kitchenObjectSO;
	}


	[SerializeField] private List<KitchenObejctSO> validKitchenObjectSOList; //���ÿ� �ø� �� �ִ� ��� ����Ʈ ex) �߶��� ġ�� -> o , �׳� ġ�� �� -> x

    private List<KitchenObejctSO> kitchenObjectSOList;

	void Awake()
	{
		kitchenObjectSOList = new List<KitchenObejctSO> ();
	}

	public bool TryAddIngredient(KitchenObejctSO kitchenObjectSO)
    {
		if (!validKitchenObjectSOList.Contains(kitchenObjectSO)){
			return false;
		}

		if(kitchenObjectSOList.Contains(kitchenObjectSO)) //�̹� ����Ʈ�� �ش� ��ᰡ �ִ� ��Ȳ�̸�(��ᰡ �ߺ��̶��)
		{
			return false;
		}
		else
		{
			kitchenObjectSOList.Add(kitchenObjectSO);

			OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
			{
				kitchenObjectSO = kitchenObjectSO
			});

			return true;
		}
	}

	public List<KitchenObejctSO> GetKitchenObjectSOList()
	{
		return kitchenObjectSOList;
	}
}
