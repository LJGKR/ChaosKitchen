using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
	public KitchenObejctSO input;
	public KitchenObejctSO output;
	public int cuttingProgressMax;
}
