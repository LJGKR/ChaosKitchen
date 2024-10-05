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
	public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged; //�������̽��� ���� ���� �̺�Ʈ�ڵ鷯

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
		if (HasKitchenObject()) //���� ��ᰡ �÷����ִٸ�
		{
			switch (state)
			{
				case State.Idle:
					break;
				case State.Frying:
					fryingTimer += Time.deltaTime; //Ÿ�̸Ӹ� ������Ű��

					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
					{
						progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
					});

					if (fryingTimer > fryingRecipeSO.fryingTimerMax) //���½ð��� �ѱ�� ������Ʈ�� �ı��ϰ� ������� ����
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
					burningTimer += Time.deltaTime; //Ÿ�̸Ӹ� ������Ű��

					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
					{
						progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
					});

					if (burningTimer > burningRecipeSO.burningTimerMax) //���½ð��� �ѱ�� ������Ʈ�� �ı��ϰ� ������� ����
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
		if (!HasKitchenObject()) //���� ��ǰ�� ���ٸ�
		{
			if (player.HasKitchenObject()) //�÷��̾ ��ǰ�� �ű���ִٸ�
			{
				if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
				{
					//�÷��̾ ����ִ� ��ᰡ ���� �����ǰ� �ִ� ����� ���� �� ����
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
				//�÷��̾ �ƹ��͵� ������� �ʴٸ�
			}
		}
		else //�̹� ī���� ���� ��ǰ�� �ִٸ�
		{
			if (player.HasKitchenObject()) //�÷��̾ ��ǰ�� �ű�� �ִٸ�
			{
				if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) //�÷��̾ ����ִ� ��ǰ�� ���ö�� + out�� ���� ���� �÷��̾��� ��ǰ�� ������ ������
				{
					if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
					{ //�÷���Ʈ ������Ʈ�� �ִ� ����Ʈ�� ī���Ϳ� �ִ� ��Ḧ �߰�
						GetKitchenObject().DestroySelf(); //���� ��� �ı� 

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
				//�÷��̾ �ƹ��͵� ������� �ʴٸ� ī���� �� ��Ḧ �÷��̾�Է� �ű�
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

	bool HasRecipeWithInput(KitchenObejctSO input) //�������� ����� �����ǰ� �����ϴ��� Ȯ���ϴ� �����Լ�
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
			if (fryingRecipeSO.input == input) //�Ѱܹ��� ��� SO�� ������ ���� �����Ǿȿ� �ִ� input�� ��ġ�ϸ� �ش� ������ ��Ḧ ����
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
			if (burningRecipeSO.input == input) //�Ѱܹ��� ��� SO�� ������ ���� �����Ǿȿ� �ִ� input�� ��ġ�ϸ� �ش� ������ ��Ḧ ����
			{
				return burningRecipeSO;
			}
		}
		return null;
	}
}
