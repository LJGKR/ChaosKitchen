using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
	[SerializeField] KitchenObejctSO kitchenObjectSO;

	public event EventHandler OnPlayerGrabbedObject; //��Ḧ ��� ������ �ִϸ��̼��� �����ų �Լ��� �ٷ� �̺�Ʈ�ڵ鷯

	public override void Interact(Player player)
	{
		if (!player.HasKitchenObject()) //�÷��̾�� �ƹ��͵� ���ٸ�
		{
			KitchenObject.SpawnKitchenObject(kitchenObjectSO, player); //�ش��ϴ� ������Ʈ�� ��ȯ�ϰ� �÷��̾��� �ڽ����� ����

			OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty); //�̺�Ʈ �ڵ鷯�� ���� �̺�Ʈ ���
			//?�� ���̸� �տ� �ִ� ������ ����ֳ� Ȯ���� �� ������� ���� �� �ڿ� �ִ� ����� �����ϰ� �ƴϸ� ���� ����
			// if(collision != null) �� ����.
		}
	}
}
