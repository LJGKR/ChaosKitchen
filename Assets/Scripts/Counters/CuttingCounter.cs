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
		if (!HasKitchenObject()) //���� ��ǰ�� ���ٸ�
		{
			if (player.HasKitchenObject()) //�÷��̾ ��ǰ�� �ű���ִٸ�
			{
				if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
				{
					//�÷��̾ ����ִ� ��ᰡ �ڸ��� �����ǰ� �ִ� ����� ���� �� ����
					player.GetKitchenObject().SetKitchenObjectParent(this);
					cuttingProgress = 0;

					CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOInput(GetKitchenObject().GetKitchenObjectSO());
					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
					{
						progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
						//int���� ������ �Ҽ����� �߻��� �� �ֱ⿡ float ������ ����
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
					}
				}
			}
			else
			{
				//�÷��̾ �ƹ��͵� ������� �ʴٸ�
				GetKitchenObject().SetKitchenObjectParent(player);
			}
		}
	}

	public override void InteractSlice(Player player)
	{
		if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
		{
			//��ᰡ ���� �ִٸ� && �÷����ִ� ��ᰡ �ڸ� �� �ִ� ����� �ڸ���
			cuttingProgress++;

			OnCut?.Invoke(this, EventArgs.Empty);

			CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOInput(GetKitchenObject().GetKitchenObjectSO());

			OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
			{
				progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
				//int���� ������ �Ҽ����� �߻��� �� �ֱ⿡ float ������ ����
			});

			if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) //�ƽ� Ŀ�� Ƚ���� �Ǳ� ������ ����
            {
				KitchenObejctSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());//�÷��� ����� SO�����͸� �ҷ��� �ƿ�ǲ�� ����

				GetKitchenObject().DestroySelf();

				KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this); //�Ű������� �߶��� ������Ʈ�� ��ȯ�ϰ� �ڽ����� ����� �Լ�
			}
		}
	}

	bool HasRecipeWithInput(KitchenObejctSO input) //�������� ����� �����ǰ� �����ϴ��� Ȯ���ϴ� �����Լ�
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
			if (cuttingRecipeSO.input == input) //�Ѱܹ��� ��� SO�� ������ ���� �����Ǿȿ� �ִ� input�� ��ġ�ϸ� �ش� ������ ��Ḧ ����
			{
				return cuttingRecipeSO;
			}
		}
		return null;
	}
}
