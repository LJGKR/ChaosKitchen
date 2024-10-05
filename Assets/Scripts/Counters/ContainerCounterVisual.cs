using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    Animator anim;

	[SerializeField] ContainerCounter containerCounter;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Start()
	{
		containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
	}

	void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
	{
		anim.SetTrigger("OpenClose");
	}
}
