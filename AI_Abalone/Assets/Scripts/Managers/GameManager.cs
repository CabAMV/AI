using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameManager que controla el flujo de juego
/// implementa un patron singleton para que sea accesible desde todas las clases del juego
/// </summary>
public class GameManager : Singleton<GameManager>
{
    //Hacer las puntuaciones de jugadores con imagenes
    public AIManager ai;
	public GameObject gameCanvas;
	public GameObject gameOverCanvas;
	public Text gameOverText;

    private bool aiActive;

    override protected void Awake()
    {
        base.Awake();
        aiActive = false;
    }

    public void EndTurn()
    {
        if (IsWinState())
        {
            GameOver();
        }

        else
        {
            ChangeSides();
            if (aiActive)
            {
                ai.Play(PLAYER_COLOR.BLACK);
            }
        }
    }

    public bool getAiActive()
    {
        return aiActive;
    }

    bool IsWinState()
    {
        //completar cuando este el aiboard acabado
        Marble[] marbles = FindObjectsOfType(typeof(Marble)) as Marble[];
        int marbleCounter = 0;
        if (aiActive)
        {
            foreach (Marble white in marbles)
            {
                if(white.isWhite)
                    marbleCounter++;
            }
                
        }
        else
        {
            foreach (Marble black in marbles)
                if (!black.isWhite)
                    marbleCounter++;
        }

		Debug.Log (marbleCounter);

        if (marbleCounter <= 8)
            return true;
        else
            return false;
    }

    public void ChangeSides()
    {
        if (aiActive)
            aiActive = false;
        else
            aiActive = true;
    }

    void GameOver()
    {
		gameCanvas.SetActive (false);
		gameOverCanvas.SetActive (true);

		if (aiActive)
			gameOverText.text = "¡Ha ganado la IA!";
		else
			gameOverText.text = "¡Ha ganado el jugador!";

		aiActive = true;
    }
}
