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
		for(int i=0; i<waitingRecipeSOList.Count; i++) //모든 대기상태의 레시피 개수만큼 리스트 체크
		{
			RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

			if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) //대기중인 레시피의 요구 재료수가 접시에 담긴 재료 리스트의 수와 같다면
			{
				//재료수가 같다 ==> 레시피에 필요한 재료들만 접시에 담겨있다.
				bool plateContentsMatchesRecipe = true;
				foreach(KitchenObejctSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
				{ //레시피에 필요한 재료들 리스트 순회
					bool ingredientFound = false;
					foreach (KitchenObejctSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
					{ //접시에 담긴 재료들 리스트 순회
						if(plateKitchenObjectSO == recipeKitchenObjectSO) //필요한 재료와 접시의 담긴 재료가 같다면
						{
							ingredientFound = true;
							break;
						}
					}
					if (!ingredientFound) //레시피에 필요한 재료를 접시에서 찾지 못했다면
					{
						plateContentsMatchesRecipe = false;
					}
				}

				if (plateContentsMatchesRecipe) //플레이어가 레시피에 맞는 재료들을 잘 가져온 경우
				{
					successRecipeNum++;

					waitingRecipeSOList.RemoveAt(i);

					OnRecipeCompleted?.Invoke(this, EventArgs.Empty);

					return;
				}
			}
		}

		//for문을 통해 모든 주문 대기 레시피들을 다 돌아도 하나의 레시피도 맞게 가져오지 않았을 경우
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
