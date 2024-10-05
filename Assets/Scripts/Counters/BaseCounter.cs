using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
	[SerializeField] Transform counterTopPoint;

	private KitchenObject kitchenObject;

	public virtual void Interact(Player player) { //���� �Լ� override��

    }

	public virtual void InteractSlice(Player player)
	{ 

	}

	public Transform GetKitchenObjectFollowTransform() //��ǰ�� ��ġ�� ��ȯ
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