using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{
	public KitchenObejctSO input;
	public KitchenObejctSO output;
	public float burningTimerMax;
}
