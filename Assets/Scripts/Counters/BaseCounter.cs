using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
	[SerializeField] Transform counterTopPoint;

	private KitchenObject kitchenObject;

	public virtual void Interact(Player player) { //가상 함수 override용

    }

	public virtual void InteractSlice(Player player)
	{ 

	}

	public Transform GetKitchenObjectFollowTransform() //물품의 위치를 반환
	{
		return counterTopPoint;
	}

	public void SetKitchenObject(KitchenObject kitchenObject)
	{
		this.kitchenObject = kitchenObject;
	}

	public KitchenObject GetKitchenObject()
	{
		return kitchenObject;
	}

	public void ClearKitchenObject()
	{
		kitchenObject = null;
	}

	public bool HasKitchenObject()
	{
		return kitchenObject != null;
	}
}
