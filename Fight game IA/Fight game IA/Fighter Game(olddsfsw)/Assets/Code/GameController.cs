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

    Movements playerOption = Movements.None;
    Movements IAOption = Movements.None;

    int totalMovements = 0;

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
			         (Input.GetKeyDown (KeyCode.DownArrow) && Input.GetKeyDown (KeyCode.LeftArrow))) // All possible movements
            {
				Debug.Log (IA.PrintTablesData ()); // Prints the table (messages in log)

				// Get movement having in mind the inputs
				if (Input.GetKeyDown (KeyCode.UpArrow) && Input.GetKeyDown (KeyCode.RightArrow))
					playerOption = Movements.AttackMedium;
				else if (Input.GetKeyDown (KeyCode.UpArrow) && Input.GetKeyDown (KeyCode.LeftArrow))
					playerOption = Movements.DefenseMedium;
				else if (Input.GetKeyDown (KeyCode.DownArrow) && Input.GetKeyDown (KeyCode.RightArrow))
					playerOption = Movements.AttackLow;
				else
					playerOption = Movements.DefenseLow;

                totalMovements++;

                // IA movement
                IAOption = IA.nextMove();

				// Activate animations of movements 
				animMan.launchRyuAnim(this.playerOption);
				animMan.launchKenAnim (IAOption);

                MatchStateText.text = "La IA selecciona: " + IAOption + "\n";

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

                MatchStateText.text += "Tasa de victoria: " + (100 * (float)IAWin / totalMovements) + "%\n";
                MatchStateText.text += "Tasa de derrota: " + (100 * (float)playerWin / totalMovements) + "%\n";

                WinStateText.text = "Player: " + playerWin + "   IA: " + IAWin + "\n" + playerOption + " VS " + IAOption;

                // Save the options
                IA.saveMovements(playerOption);
			}
		}
        else if (animMan.getKO()) // Reset the KO characters to idle
        {
            if (Input.GetKeyDown(KeyCode.R))
                animMan.doubleKO(false);
        }
    }

	// The attack must be to a region when is not protected (not counting double KO)
    bool VictoryCondition (Movements moveToCheck, Movements moveToCompare)
    {
		return ((moveToCheck == Movements.AttackLow && moveToCompare == Movements.DefenseMedium) ||
		(moveToCheck == Movements.AttackMedium && moveToCompare == Movements.DefenseLow));
    }
	// Double KO condition
	bool LoseCondition (Movements moveToCheck, Movements moveToCompare)
	{
		return ((moveToCheck == Movements.AttackLow && moveToCompare == Movements.AttackMedium) ||
			(moveToCheck == Movements.AttackMedium && moveToCompare == Movements.AttackLow));
	}
}