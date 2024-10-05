using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] GameObject stoveOnGameObejct;
    [SerializeField] GameObject Particle;

	void Start()
	{
		stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
	}

	void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
	{
		bool canShow = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried; //두 상태일때만 이펙트가 보이게

		stoveOnGameObejct.SetActive(canShow);
		Particle.SetActive(canShow);
	}
}
