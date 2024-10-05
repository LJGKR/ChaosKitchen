using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
	Animator anim;

	[SerializeField] CuttingCounter cuttingCounter;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Start()
	{
		cuttingCounter.OnCut += CuttingCounter_OnCut;
	}

	void CuttingCounter_OnCut(object sender, System.EventArgs e) 
	{
		anim.SetTrigger("Cut");
	}
}
