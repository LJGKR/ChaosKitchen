using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
	public KitchenObejctSO input;
	public KitchenObejctSO output;
	public float fryingTimerMax;
}
