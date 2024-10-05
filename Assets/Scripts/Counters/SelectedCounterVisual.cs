using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] BaseCounter baseCounter;
    [SerializeField] GameObject[] visualGameObjects;
    void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.e_selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

	void Show()
    {
        foreach(GameObject gameObject in visualGameObjects)
        {
			gameObject.SetActive(true);

		}
    }
    
    void Hide()
    {
		foreach (GameObject gameObject in visualGameObjects)
		{
			gameObject.SetActive(false);

		}
	}
}
