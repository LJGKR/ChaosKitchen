using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
	public event EventHandler OnRecipeSpawned;
	public event EventHandler OnRecipeCompleted;

	public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    List<RecipeSO> waitingRecipeSOList;
    float spawnRecipeTimer;
    float spawnRecipeTimerMax = 4f;
	int waitingRecipeMax = 4;
	int successRecipeNum;

	void Awake()
	{
		Instance = this;

		waitingRecipeSOList = new List<RecipeSO>();
	}

	void Update()
	{
		spawnRecipeTimer -= Time.deltaTime;
		if(spawnRecipeTimer <= 0f)
		{
			spawnRecipeTimer = spawnRecipeTimerMax;

			if(waitingRecipeSOList.Count < waitingRecipeMax)
			{
				RecipeSO waitingRecipsSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
				waitingRecipeSOList.Add(waitingRecipsSO);

				OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
	{
		for(int i=0; i<waitingRecipeSOList.Count; i++) //��� �������� ������ ������ŭ ����Ʈ üũ
		{
			RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

			if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) //������� �������� �䱸 ������ ���ÿ� ��� ��� ����Ʈ�� ���� ���ٸ�
			{
				//������ ���� ==> �����ǿ� �ʿ��� ���鸸 ���ÿ� ����ִ�.
				bool plateContentsMatchesRecipe = true;
				foreach(KitchenObejctSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
				{ //�����ǿ� �ʿ��� ���� ����Ʈ ��ȸ
					bool ingredientFound = false;
					foreach (KitchenObejctSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
					{ //���ÿ� ��� ���� ����Ʈ ��ȸ
						if(plateKitchenObjectSO == recipeKitchenObjectSO) //�ʿ��� ���� ������ ��� ��ᰡ ���ٸ�
						{
							ingredientFound = true;
							break;
						}
					}
					if (!ingredientFound) //�����ǿ� �ʿ��� ��Ḧ ���ÿ��� ã�� ���ߴٸ�
					{
						plateContentsMatchesRecipe = false;
					}
				}

				if (plateContentsMatchesRecipe) //�÷��̾ �����ǿ� �´� ������ �� ������ ���
				{
					successRecipeNum++;

					waitingRecipeSOList.RemoveAt(i);

					OnRecipeCompleted?.Invoke(this, EventArgs.Empty);

					return;
				}
			}
		}

		//for���� ���� ��� �ֹ� ��� �����ǵ��� �� ���Ƶ� �ϳ��� �����ǵ� �°� �������� �ʾ��� ���
	}

	public List<RecipeSO> GetWaitingRecipeSOList()
	{
		return waitingRecipeSOList;
	}

	public int GetSuccessRecipeNum()
	{
		return successRecipeNum;
	}
}
