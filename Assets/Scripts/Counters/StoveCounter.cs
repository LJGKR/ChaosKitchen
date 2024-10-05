using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using static CuttingCounter;
using static IHasProgress;

public class StoveCounter : BaseCounter, IHasProgress
{
	public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged; //인터페이스로 인한 구현 이벤트핸들러

	public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
	public class OnStateChangedEventArgs : EventArgs
	{
		public State state;
	}

	public enum State //state machine
	{
		Idle,
		Frying,
		Fried,
		Burned,
	}
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

	State state;
	float fryingTimer;
	float burningTimer;
	FryingRecipeSO fryingRecipeSO;
	BurningRecipeSO burningRecipeSO;

	void Start()
	{
		state = State.Idle;
	}

	void Update()
	{
		if (HasKitchenObject()) //위에 재료가 올려져있다면
		{
			switch (state)
			{
				case State.Idle:
					break;
				case State.Frying:
					fryingTimer += Time.deltaTime; //타이머를 증가시키고

					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
					{
						progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
					});

					if (fryingTimer > fryingRecipeSO.fryingTimerMax) //굽는시간을 넘기면 오브젝트를 파괴하고 결과물을 생성
					{
						GetKitchenObject().DestroySelf();

						KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

						state = State.Fried;
						burningTimer = 0f;

						burningRecipeSO = GetBurningRecipeSOInput(GetKitchenObject().GetKitchenObjectSO());

						OnStateChanged?.Invoke(this,new OnStateChangedEventArgs{
							state = state
						});

						OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
						{
							progressNormalized = 0f
						});
					}
					break;
				case State.Fried:
					burningTimer += Time.deltaTime; //타이머를 증가시키고

					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
					{
						progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
					});

					if (burningTimer > burningRecipeSO.burningTimerMax) //굽는시간을 넘기면 오브젝트를 파괴하고 결과물을 생성
					{
						GetKitchenObject().DestroySelf();

						KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

						state = State.Burned;

						OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
						{
							state = state
						});

						OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
						{
							progressNormalized = 0f
						});
					}
					break;
				case State.Burned:
					break;
			}
		}
	}

	public override void Interact(Player player)
	{
		if (!HasKitchenObject()) //위에 물품이 없다면
		{
			if (player.HasKitchenObject()) //플레이어가 물품을 옮기고있다면
			{
				if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
				{
					//플레이어가 들고있는 재료가 굽는 레시피가 있는 재료라면 놓을 수 있음
					player.GetKitchenObject().SetKitchenObjectParent(this);

					fryingRecipeSO = GetFryingRecipeSOInput(GetKitchenObject().GetKitchenObjectSO());

					state = State.Frying;
					fryingTimer = 0f;

					OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
					{
						state = state
					});

					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
					{
						progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
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

						state = State.Idle;

						OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
						{
							state = state
						});

						OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
						{
							progressNormalized = 0f
						});
					}
				}
			}
			else
			{
				//플레이어가 아무것도 들고있지 않다면 카운터 위 재료를 플레이어에게로 옮김
				GetKitchenObject().SetKitchenObjectParent(player);

				state = State.Idle;

				OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
				{
					state = state
				});

				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
				{
					progressNormalized = 0f
				});
			}
		}
	}

	bool HasRecipeWithInput(KitchenObejctSO input) //넣으려는 재료의 레시피가 존재하는지 확인하는 리턴함수
	{
		FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOInput(input);
		return fryingRecipeSO != null;
	}

	private KitchenObejctSO GetOutputForInput(KitchenObejctSO input)
	{
		FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOInput(input);

		if (fryingRecipeSO != null)
		{
			return fryingRecipeSO.output;
		}
		else
		{
			return null;
		}
	}

	FryingRecipeSO GetFryingRecipeSOInput(KitchenObejctSO input)
	{
		foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
		{
			if (fryingRecipeSO.input == input) //넘겨받은 재료 SO가 포문을 돌며 레시피안에 있는 input과 일치하면 해당 레시피 재료를 리턴
			{
				return fryingRecipeSO;
			}
		}
		return null;
	}

	BurningRecipeSO GetBurningRecipeSOInput(KitchenObejctSO input)
	{
		foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
		{
			if (burningRecipeSO.input == input) //넘겨받은 재료 SO가 포문을 돌며 레시피안에 있는 input과 일치하면 해당 레시피 재료를 리턴
			{
				return burningRecipeSO;
			}
		}
		return null;
	}
}
