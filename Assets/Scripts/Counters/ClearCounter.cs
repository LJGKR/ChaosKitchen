using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] KitchenObejctSO kitchenObjectSO;


	public override void Interact(Player player)
    {
        if(!HasKitchenObject()) //위에 물품이 없다면
        {
            if (player.HasKitchenObject()) //플레이어가 물품을 옮기고있다면
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) //플레이어가 들고있는 물품이 접시라면 + out을 통해 비교한 플레이어의 물품을 밖으로 가져옴
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) { //플레이트 오브젝트에 있는 리스트에 카운터에 있는 재료를 추가
						GetKitchenObject().DestroySelf(); //얻은 재료 파괴 
					}
                }
                else        //이미 카운터 위에 물품이 있지만 플레이어가 들고 있는 물품이 접시가 아니라 다른 재료라면
                {
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)) //카운터에 있는게 접시라면
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) //플레이어가 들고있는 재료가 넣을수 있는 재료라면
                        {
							player.GetKitchenObject().DestroySelf();
						}

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
}
