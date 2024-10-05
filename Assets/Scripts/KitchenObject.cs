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

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) //�Ű������� �ڽ����� �ڽ��� �ѱ�
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        if (kitchenObjectParent.HasKitchenObject()) //�̹� �Ű������� ī���Ϳ��� ��ǰ�� �ִٸ�
        {
            Debug.Log("Counter already has a kitchenObject!");
        }

        this.kitchenObjectParent = kitchenObjectParent;
        kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
        //Űģ ������Ʈ�� �θ�� ī���͸� ������ ��ġ�� �ڽ����� �� ī���ͷ� ���ϴ� �Լ�
    }

    public IKitchenObjectParent GetKitchenObjectParent() //��ǰ �ڽ��� �����ϴ� �θ� ������Ʈ ��ȯ
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
        if(this is PlateKitchenObject) //�� ������Ʈ�� ���ö��
        {
            plateKitchenObject = this as PlateKitchenObject; //�Ű������� �ڱ��ڽ��� ���� ������Ʈ�μ� �Ѱ���
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
		//�޸𸮿� �÷� ��𼭵� ���� �����ϰ� �ϰ�, �Ű������� ���� ���ϴ� Űģ ������Ʈ�� �������� ��ȯ�ؼ� �Ű������� kitchenObjectParent�� �ڽ����� ������ �� �ִ� �Լ�
		Transform kitchenObejctTransform = Instantiate(kitchenObejctSO.prefab);

        KitchenObject kitchenObject = kitchenObejctTransform.GetComponent<KitchenObject>();
		kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
	}
}
