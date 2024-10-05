using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent //�� Ŭ�������� �������̽��� ����ϸ� �������̽� �ȿ� �ִ� �Լ��� ������ �����ϰԲ� ��
{
	public Transform GetKitchenObjectFollowTransform(); //��ǰ�� ��ġ�� ��ȯ

	public void SetKitchenObject(KitchenObject kitchenObject); //������ Űģ ������Ʈ�� �Ű������� �ѱ� ��ǰ�� �Ҵ�

	public KitchenObject GetKitchenObject();

	public void ClearKitchenObject();

	public bool HasKitchenObject();
}
