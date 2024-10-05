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


	[SerializeField] private List<KitchenObejctSO> validKitchenObjectSOList; //접시에 올릴 수 있는 재료 리스트 ex) 잘라진 치즈 -> o , 그냥 치즈 블럭 -> x

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

		if(kitchenObjectSOList.Contains(kitchenObjectSO)) //이미 리스트에 해당 재료가 있는 상황이면(재료가 중복이라면)
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
