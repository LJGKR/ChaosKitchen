using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

	private List<GameObject> plateVisualGameObjectList;

	void Awake()
	{
		plateVisualGameObjectList = new List<GameObject>();
	}

	void Start()
	{
		platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
		platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
	}

	void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
	{
		Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

		float plateOffsetY = .1f;
		plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

		plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
	}

	void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
	{
		GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1]; //맨 위에 생성된 접시를 지정하여
		plateVisualGameObjectList.Remove(plateGameObject); //리스트에서 제거하고 파괴
		Destroy(plateGameObject);
	}
}
