using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls the elements of the scene and the game itself
class GameController : MonoBehaviour 
{
	// Shows data of the matches
    public Text WinStateText;
    public Text MatchStateText;

    int playerWin = 0, IAWin = 0;
    int IAAccuracy = 0;

    string playerOption = "";
    string IAOption = "";

    int totalchar = 0;

	private AnimatorManager animMan;
    private IAController IA;

	void Start()
	{
		animMan = this.GetComponent<AnimatorManager>();
        IA = this.GetComponent<IAController>();
	}

    void Update()
    {	
		if (animMan.isPlayingAnim () && !animMan.getKO() ) // Both characters must be in idle
		{
			if ((Input.GetKeyDown (KeyCode.UpArrow) && Input.GetKeyDown (KeyCode.RightArrow)) ||
			         (Input.GetKeyDown (KeyCode.UpArrow) && Input.GetKeyDown (KeyCode.LeftArrow)) ||
			         (Input.GetKeyDown (KeyCode.DownArrow) && Input.GetKeyDown (KeyCode.RightArrow)) ||
			         (Input.GetKeyDown (KeyCode.DownArrow) && Input.GetKeyDown (KeyCode.LeftArrow))) // All possible char
            {
				Debug.Log (IA.PrintTablesData ()); // Prints the table (messages in log)

				// Get movement having in mind the inputs
				if (Input.GetKeyDown (KeyCode.UpArrow) && Input.GetKeyDown (KeyCode.RightArrow))
					playerOption = "1";
				else if (Input.GetKeyDown (KeyCode.UpArrow) && Input.GetKeyDown (KeyCode.LeftArrow))
					playerOption = "3";
				else if (Input.GetKeyDown (KeyCode.DownArrow) && Input.GetKeyDown (KeyCode.RightArrow))
					playerOption = "2";
				else
					playerOption = "4";

                totalchar++;

                // IA movement
                IAOption = IA.nextMove();

				// Activate animations of char 
				animMan.launchRyuAnim(this.playerOption);
				animMan.launchKenAnim (IAOption);

                MatchStateText.text = "La IA selecciona: " + IAOption + "\n";

                // If the IA predicts the movement correctly
                if (reverseMove(IAOption) == playerOption)
                    IAAccuracy++;

                // Victory condition IA
                if (VictoryCondition (IAOption, playerOption))
                {
					// Ken
					animMan.launchRyuKO(true);
                    MatchStateText.text += "¡La IA ha ganado!\n";
                    IAWin++;
				} 
				// Victory condition Player
				else if (VictoryCondition (playerOption, IAOption))
                {
                    // Ryu
                    animMan.launchKenKO(true);
                    MatchStateText.text += "¡La IA ha perdido!\n";
                    playerWin++;
                } 
				// Double KO
				else if (LoseCondition (IAOption, playerOption)) 
				{
					animMan.doubleKO (true);
                    MatchStateText.text += "¡Doble KO!\n";
                } 
				else // Tie
				{
                    MatchStateText.text += "¡Empate!\n";
                }

                MatchStateText.text += "Tasa de detección victoriosa: " + (100 * (float)IAAccuracy / totalchar) + "%\n";

                WinStateText.text = "Player: " + playerWin + "   IA: " + IAWin + "\n";
                WinStateText.text += selectedMove(playerOption) + " VS " + selectedMove(IAOption);

                // Save the options
                IA.savechar(playerOption);
			}
		}
        else if (animMan.getKO()) // Reset the KO characters to idle
        {
            if (Input.GetKeyDown(KeyCode.R))
                animMan.doubleKO(false);
        }
    }

    string selectedMove (string move)
    {
        switch (move)
        {
            case "1":
                return "Attack";
            case "2":
                return "AttackLow";
            case "3":
                return "Defense";
            case "4":
                return "DefenseLow";
            default:
                return "Error";
        }
    }

    string reverseMove (string move)
    {
        switch (move)
        {
            case "1":
                return "4";
            case "2":
                return "3";
            case "3":
                return "1";
            case "4":
                return "2";
            default:
                return "1";
        }
    }

	// The attack must be to a region when is not protected (not counting double KO)
    bool VictoryCondition (string moveToCheck, string moveToCompare)
    {
		return ((moveToCheck == "2" && moveToCompare == "3") ||
		(moveToCheck == "1" && moveToCompare == "4"));
    }
	// Double KO condition
	bool LoseCondition (string moveToCheck, string moveToCompare)
	{
		return ((moveToCheck == "2" && moveToCompare == "1") ||
			(moveToCheck == "1" && moveToCompare == "2"));
	}
}