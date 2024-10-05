using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent //각 클래스마다 인터페이스를 상속하면 인터페이스 안에 있는 함수를 강제로 구현하게끔 함
{
	public Transform GetKitchenObjectFollowTransform(); //물품의 위치를 반환

	public void SetKitchenObject(KitchenObject kitchenObject); //각자의 키친 오브젝트에 매개변수로 넘긴 물품을 할당

	public KitchenObject GetKitchenObject();

	public void ClearKitchenObject();

	public bool HasKitchenObject();
}
