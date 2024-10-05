using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CuttingCounter : BaseCounter, IHasProgress
{

	public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

	public event EventHandler OnCut;

	[SerializeField] CuttingRecipeSO[] cuttingRecipeSOArray;

	int cuttingProgress;


	public override void Interact(Player player)
	{
		if (!HasKitchenObject()) //위에 물품이 없다면
		{
			if (player.HasKitchenObject()) //플레이어가 물품을 옮기고있다면
			{
				if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
				{
					//플레이어가 들고있는 재료가 자르는 레시피가 있는 재료라면 놓을 수 있음
					player.GetKitchenObject().SetKitchenObjectParent(this);
					cuttingProgress = 0;

					CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOInput(GetKitchenObject().GetKitchenObjectSO());
					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
					{
						progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
						//int끼리 나누면 소수점이 발생할 수 있기에 float 변수로 받음
					});
				}
			}
			else
			{
				//플레이어가 아무것도 들고있지 않다면
			}
		}
		else //이미 카운터 위에 물품이 있다면
		{
			if (player.HasKitchenObject()) //플레이어가 물품을 옮기고 있다면
			{
				if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) //플레이어가 들고있는 물품이 접시라면 + out을 통해 비교한 플레이어의 물품을 밖으로 가져옴
				{
					if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
					{ //플레이트 오브젝트에 있는 리스트에 카운터에 있는 재료를 추가
						GetKitchenObject().DestroySelf(); //얻은 재료 파괴 
					}
				}
			}
			else
			{
				//플레이어가 아무것도 들고있지 않다면
				GetKitchenObject().SetKitchenObjectParent(player);
			}
		}
	}

	public override void InteractSlice(Player player)
	{
		if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
		{
			//재료가 위에 있다면 && 올려져있는 재료가 자를 수 있는 재료라면 자르기
			cuttingProgress++;

			OnCut?.Invoke(this, EventArgs.Empty);

			CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOInput(GetKitchenObject().GetKitchenObjectSO());

			OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
			{
				progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
				//int끼리 나누면 소수점이 발생할 수 있기에 float 변수로 받음
			});

			if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) //맥스 커팅 횟수가 되기 전까지 실행
            {
				KitchenObejctSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());//올려진 재료의 SO데이터를 불러와 아웃풋에 저장

				GetKitchenObject().DestroySelf();

				KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this); //매개변수인 잘라진 오브젝트를 소환하고 자식으로 만드는 함수
			}
		}
	}

	bool HasRecipeWithInput(KitchenObejctSO input) //넣으려는 재료의 레시피가 존재하는지 확인하는 리턴함수
	{
		CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOInput(input);
		return cuttingRecipeSO != null;
	}

	private KitchenObejctSO GetOutputForInput(KitchenObejctSO input)
	{
		CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOInput(input);

		if(cuttingRecipeSO != null)
		{
			return cuttingRecipeSO.output;
		}
		else
		{
			return null;
		}
	}

	CuttingRecipeSO GetCuttingRecipeSOInput(KitchenObejctSO input)
	{
		foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
		{
			if (cuttingRecipeSO.input == input) //넘겨받은 재료 SO가 포문을 돌며 레시피안에 있는 input과 일치하면 해당 레시피 재료를 리턴
			{
				return cuttingRecipeSO;
			}
		}
		return null;
	}
}
