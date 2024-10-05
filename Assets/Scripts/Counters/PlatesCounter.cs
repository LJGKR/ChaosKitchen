using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
	public event EventHandler OnPlateSpawned;
	public event EventHandler OnPlateRemoved;

	[SerializeField] KitchenObejctSO plateKitchenObjectSO;

    float spawnPlateTimer;
	float spawnPlateTimerMax = 4f;

	private int plateSpawnAmount;
	private int plateSpawnAmountMax = 4;


	void Update()
	{
		spawnPlateTimer += Time.deltaTime;
		if(spawnPlateTimer > spawnPlateTimerMax)
		{
			spawnPlateTimer = 0f;

			if(plateSpawnAmount < plateSpawnAmountMax)
			{
				plateSpawnAmount++;

				OnPlateSpawned?.Invoke(this,EventArgs.Empty);
			}
		}
	}

	public override void Interact(Player player)
	{
		if (!player.HasKitchenObject()) //�÷��̾� �տ� ��ᰡ ���ٸ�
		{
			if(plateSpawnAmount > 0) //���ð� 1���� ������ ���¶��
			{
				plateSpawnAmount--;

				KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

				OnPlateRemoved?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
