using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
	[SerializeField] GameObject hasProgressGameObject;
    [SerializeField] Image barImage;

	IHasProgress hasProgress;
	void Start()
	{
		hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

		hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
		barImage.fillAmount = 0;

		Hide();
	}

	void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
	{
		barImage.fillAmount = e.progressNormalized;

		if(e.progressNormalized == 0f || e.progressNormalized == 1f)
		{
			Hide();
		}
		else
		{
			Show();
		}
	}

	void Show()
	{
		gameObject.SetActive(true);
	}

	void Hide()
	{
		gameObject.SetActive(false);
	}
}
