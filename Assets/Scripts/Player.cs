using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IKitchenObjectParent
{
	public static Player Instance { get; private set; }

	public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

	public class OnSelectedCounterChangedEventArgs : EventArgs
	{
		public BaseCounter e_selectedCounter;
	}

    [SerializeField] float speed;
    [SerializeField] float rotateSpeed;
    [SerializeField] LayerMask countersLayerMask;
    [SerializeField] Transform kitchenObjectHoldPoint;

    Vector3 moveVec;
    Vector3 lastInteractDir;
    bool isWalking;
    Animator anim;
    float interactDistance = 2f;
	BaseCounter selectedCounter;
	KitchenObject kitchenObject;

	void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		if(Instance != null )
		{
			Debug.LogError("There is more than one Player instance");
		}
		Instance = this;
	}

	void FixedUpdate()
    {
        Move();
        Turn();
		Interact();

	}

	void Move()
	{
		//x = Input.GetAxisRaw("Horizontal");
		//z = Input.GetAxisRaw("Vertical");
        // moveVec = new Vector3(x, 0, z).normalized;
        //InputSysyem 적용 전 플레이어 이동 로직

        transform.position += moveVec * speed * Time.fixedDeltaTime;

        isWalking = moveVec.magnitude > 0;

        anim.SetBool("IsWalking", isWalking);


        //인터렉트를 위한 코드
		if (moveVec != Vector3.zero) //움직이는 중이라면
		{
			lastInteractDir = moveVec; //움직이던 방향을 저장
		}

	}

	void Turn()
    {
        transform.forward = Vector3.Slerp(transform.forward, moveVec, rotateSpeed * Time.fixedDeltaTime);
    }

	void Interact()
	{
		if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
		{
			if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) //getcomponenet하여 비어있나 확인하는 코드와 동일
			{
				if (baseCounter != selectedCounter)
				{
					SetSelectedCounter(baseCounter);
				}
			}
			else
			{
				SetSelectedCounter(null);
			}
		}
		else
		{
			SetSelectedCounter(null);
		}
	}

    void OnMove(InputValue value) //인풋시스템을 이용한 플레이어 이동
    {
        moveVec = new Vector3((value.Get<Vector2>().x),0f,(value.Get<Vector2>().y));
    }

    void OnInteract()
    {
		if (!KitchenGameManager.Instance.IsGamePlaying()) return;

		if (selectedCounter != null)
		{
			selectedCounter.Interact(this);
		}
	}

	void OnSlice()
	{
		if (!KitchenGameManager.Instance.IsGamePlaying()) return;

		if (selectedCounter != null)
		{
			selectedCounter.InteractSlice(this);
		}
	}

	void SetSelectedCounter(BaseCounter selectedCounter)
	{
		this.selectedCounter = selectedCounter;

		OnSelectedCounterChanged.Invoke(this, new OnSelectedCounterChangedEventArgs
		{
			e_selectedCounter = selectedCounter
		}); ;
	}

	public Transform GetKitchenObjectFollowTransform() //물품의 위치를 반환
	{
		return kitchenObjectHoldPoint;
	}

	public void SetKitchenObject(KitchenObject kitchenObject)
	{
		this.kitchenObject = kitchenObject;
	}

	public KitchenObject GetKitchenObject()
	{
		return kitchenObject;
	}

	public void ClearKitchenObject()
	{
		kitchenObject = null;
	}

	public bool HasKitchenObject()
	{
		return kitchenObject != null;
	}
}
