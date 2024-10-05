using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
	[SerializeField] KitchenObejctSO kitchenObjectSO;

	public event EventHandler OnPlayerGrabbedObject; //재료를 담는 상자의 애니메이션을 재생시킬 함수를 다룰 이벤트핸들러

	public override void Interact(Player player)
	{
		if (!player.HasKitchenObject()) //플레이어에게 아무것도 없다면
		{
			KitchenObject.SpawnKitchenObject(kitchenObjectSO, player); //해당하는 오브젝트를 소환하고 플레이어의 자식으로 만듦

			OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty); //이벤트 핸들러를 통해 이벤트 재생
			//?를 붙이면 앞에 있는 변수가 비어있나 확인한 후 비어있지 않을 시 뒤에 있는 명령을 실행하고 아니면 하지 않음
			// if(collision != null) 과 같다.
		}
	}
}
