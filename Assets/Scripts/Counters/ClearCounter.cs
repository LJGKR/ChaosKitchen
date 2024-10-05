using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] KitchenObejctSO kitchenObjectSO;


	public override void Interact(Player player)
    {
        if(!HasKitchenObject()) //���� ��ǰ�� ���ٸ�
        {
            if (player.HasKitchenObject()) //�÷��̾ ��ǰ�� �ű���ִٸ�
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) //�÷��̾ ����ִ� ��ǰ�� ���ö�� + out�� ���� ���� �÷��̾��� ��ǰ�� ������ ������
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) { //�÷���Ʈ ������Ʈ�� �ִ� ����Ʈ�� ī���Ϳ� �ִ� ��Ḧ �߰�
						GetKitchenObject().DestroySelf(); //���� ��� �ı� 
					}
                }
                else        //�̹� ī���� ���� ��ǰ�� ������ �÷��̾ ��� �ִ� ��ǰ�� ���ð� �ƴ϶� �ٸ� �����
                {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)) //ī���Ϳ� �ִ°� ���ö��
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) //�÷��̾ ����ִ� ��ᰡ ������ �ִ� �����
                        {
							player.GetKitchenObject().DestroySelf();
						}

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
}
