using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] KitchenObejctSO kitchenObejctSO;

    IKitchenObjectParent kitchenObjectParent;

    public KitchenObejctSO GetKitchenObjectSO()
    {
        return kitchenObejctSO;

	}

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) //매개변수의 자식으로 자신을 넘김
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        if (kitchenObjectParent.HasKitchenObject()) //이미 매개변수인 카운터에게 물품이 있다면
        {
            Debug.Log("Counter already has a kitchenObject!");
        }

        this.kitchenObjectParent = kitchenObjectParent;
        kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
        //키친 오브젝트의 부모와 카운터를 생성된 위치를 자식으로 둔 카운터로 정하는 함수
    }

    public IKitchenObjectParent GetKitchenObjectParent() //물품 자신을 포함하는 부모 오브젝트 반환
    {
        return kitchenObjectParent;
    }

	public void DestroySelf()
	{
        kitchenObjectParent.ClearKitchenObject() ;
		Destroy(gameObject);
	}

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject) //이 오브젝트가 접시라면
        {
            plateKitchenObject = this as PlateKitchenObject; //매개변수에 자기자신을 접시 오브젝트로서 넘겨줌
            return true;
        }
        else
        {
            plateKitchenObject = null;
			return false;
        }
    }

    public static KitchenObject SpawnKitchenObject(KitchenObejctSO kitchenObejctSO, IKitchenObjectParent kitchenObjectParent) 
    {
		//메모리에 올려 어디서든 접근 가능하게 하고, 매개변수를 통해 원하는 키친 오브젝트의 프리팹을 소환해서 매개변수인 kitchenObjectParent의 자식으로 설정할 수 있는 함수
		Transform kitchenObejctTransform = Instantiate(kitchenObejctSO.prefab);

        KitchenObject kitchenObject = kitchenObejctTransform.GetComponent<KitchenObject>();
		kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
	}
}
