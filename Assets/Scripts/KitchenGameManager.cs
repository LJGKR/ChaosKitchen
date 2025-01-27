using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
	public static KitchenGameManager Instance { get; private set; }

	public event EventHandler OnStateChanged;


    enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    State state;
    float waitingToStartTimer = 1f;
    float countdownToStartTimer = 3f;
	float gamePlayingTimer;
    float gamePlayingTimerMax = 180f; //게임 제한 시간

	void Awake()
	{
		Instance = this;
		state = State.WaitingToStart;
	}

	void Update()
	{
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if(waitingToStartTimer < 0f)
                {
                    state = State.CountdownToStart;
					OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
			case State.CountdownToStart:
				countdownToStartTimer -= Time.deltaTime;
				if (countdownToStartTimer < 0f)
				{
					state = State.GamePlaying;
					gamePlayingTimer = gamePlayingTimerMax;
					OnStateChanged?.Invoke(this, EventArgs.Empty);
				}
				break;
			case State.GamePlaying:
				gamePlayingTimer -= Time.deltaTime;
				if (gamePlayingTimer < 0f)
				{
					state = State.GameOver;
					OnStateChanged?.Invoke(this, EventArgs.Empty);
				}
				break;
			case State.GameOver:
				break;
		}
	}

	public bool IsGamePlaying()
	{
		return state == State.GamePlaying;
	}

	public bool IsCountdownToStartActive()
	{
		return state == State.CountdownToStart;
	}

	public float GetCountdownToStartTimer()
	{
		return countdownToStartTimer;
	}

	public bool IsGameOver()
	{
		return state == State.GameOver;
	}

	public float GetGameplayingTimerNormalized()
	{
		return 1 - (gamePlayingTimer / gamePlayingTimerMax);
	}
}
