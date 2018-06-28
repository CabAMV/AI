using System;
using System.Collections.Generic;
using System.Text;

// Movements that will be used
public enum Movements { None, AttackMedium, AttackLow, DefenseMedium, DefenseLow };

public class listMovementComparer : IEqualityComparer<List<Movements>>
{
	public bool Equals (List<Movements> list1, List<Movements> list2)
	{
		if (list1.Count != list2.Count)
			return false;

		for (int index = 0; index < list1.Count; index++) 
		{
			if (list1 [index].GetHashCode() != list2 [index].GetHashCode())
				return false;
		}

		return true;
	}

	public int GetHashCode (Movements obj)
	{
		return obj.GetHashCode ();	
	}

	public int GetHashCode (List<Movements> obj)
	{
		return obj.GetHashCode ();	
	}
}

// Helps to the AI to predict the next player movement to counter it
class Predictor
{
    private Dictionary < List<Movements>, DataRecord > data; // Tables with list of the movements of the player
    private System.Random rnd;

    public Predictor()
    {
        rnd = new System.Random();
		data = new Dictionary< List<Movements>, DataRecord >(new listMovementComparer());
        data = new Dictionary<List<Movements>, DataRecord>();
    }

	// Predicts the player's next move
    Movements GetMostLikely(List<Movements> actions)
    {
        DataRecord keyData;
        int highestValue = 0;
        Movements bestAction = Movements.None;

		bool returnValue = data.ContainsKey (actions);

		if (returnValue) // Has information of the serie of player's actions
        {
            keyData = data[actions];

            foreach (Movements action in keyData.counts.Keys)
            {
                if (keyData.counts[action] > highestValue)
                {
                    highestValue = keyData.counts[action];
                    bestAction = action;
                }
            }
        }
        else // Random choice
        {
            int moveNumber = rnd.Next(1, Enum.GetNames(typeof(Movements)).Length);
            bestAction = (Movements)moveNumber;
        }

        return bestAction;
    }

	// Save the player's actions in the table
    public void RegisterSequence(List<Movements> actions)
    {
        List<Movements> key = new List<Movements>(actions);
        key.RemoveAt(key.Count - 1);

        Movements value = actions[actions.Count - 1];

		if (!data.ContainsKey(key)) // Creates new data
        {
            data[key] = new DataRecord();
        }

        DataRecord keyData = data[key];

        if (!keyData.counts.ContainsKey(value)) // If new value
        {
            keyData.counts[value] = 0;
        }

        keyData.counts[value]++;
        keyData.total++;
    }

	// Prints the data of the tables
    public string PrintData()
    {
        string info = "";

        info += "PREDICTOR DATA\n";

        foreach (List<Movements> key in data.Keys)
        {
            foreach (Movements m in key)
                info += m + ", ";

            info += "\n";

            DataRecord keyData = data[key];

            foreach (Movements action in keyData.counts.Keys)
            {
                Console.WriteLine("\t" + action + "--> " + keyData.counts[action]);
                info += "\t" + action + "--> " + keyData.counts[action] + "\n";
            }
        }

        return info;
    }

	// Select the movement that would counter the player's movement
    public Movements counterMovement (List<Movements> actions)
    {
        Movements nextPossibleMovement = GetMostLikely(actions);

        switch (nextPossibleMovement)
        {
            case Movements.AttackMedium:
                return Movements.DefenseMedium;

            case Movements.AttackLow:
                return Movements.DefenseLow;

            case Movements.DefenseMedium:
                return Movements.AttackLow;

            case Movements.DefenseLow:
                return Movements.AttackMedium;
        }

        return Movements.None;
    }
}
