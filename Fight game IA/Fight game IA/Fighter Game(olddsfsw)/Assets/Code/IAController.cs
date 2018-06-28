using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IA controller
public class IAController : MonoBehaviour
{
    [SerializeField]
    int windowSize = 3; // Number of movements to use as prediction

    Predictor predictor;

    List<Movements> totalChoices;
    List<Movements> choicesToPredict;
    List<Movements> choicesToRegister;

    // Use this for initialization
    void Start ()
    {
        predictor = new Predictor();
        totalChoices = new List<Movements>();
        choicesToPredict = new List<Movements>();
        choicesToRegister = new List<Movements>();
    }

	// Print the table's data
    public string PrintTablesData()
    {
        return predictor.PrintData();
    }

	// Takes the move that wpuld counter the player's movement if its prediction is true
    public Movements nextMove ()
    {
        return predictor.counterMovement(choicesToPredict);
    }

	// Save the player's movements for data collecting
    public void saveMovements (Movements playerOption)
    {
        totalChoices.Add(playerOption);
        choicesToPredict.Add(playerOption);
        choicesToRegister.Add(playerOption);

        if (choicesToPredict.Count > windowSize - 1)
            choicesToPredict.RemoveAt(0);

		if (choicesToRegister.Count > windowSize - 1) 
		{
			if (choicesToRegister.Count > windowSize)
				choicesToRegister.RemoveAt(0);

			predictor.RegisterSequence(choicesToRegister);
		}
			

        /*if (choicesToRegister.Count > windowSize)
        {
            choicesToRegister.RemoveAt(0);
            //predictor.RegisterSequence(choicesToRegister);
        }*/
    }
}
