using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predictor : MonoBehaviour
{
    private Dictionary<string, DataRecord> data;
    private string possibleActions;

    public Predictor(string _possibleActions)
    {
        possibleActions = _possibleActions;
        data = new Dictionary<string, DataRecord>();
    }

    public string GetMostLikely(string actions)
    {
        DataRecord keyData;
        int highestValue = 0;
        char bestAction = ' ';

        if (data.ContainsKey(actions))
        {
            keyData = data[actions];
 
            foreach (char action in keyData.counts.Keys)
            {
                if (keyData.counts[action] > highestValue)
                {
                    highestValue = keyData.counts[action];
                    bestAction = action;
                }
            }
        }
        else
        {
           bestAction = possibleActions[Random.Range(0,possibleActions.Length)];
        }

        return bestAction + "";
    }

    public void RegisterSequence(string actions)
    {
        string key = actions.Substring(0, actions.Length - 1);
        char value = actions[actions.Length - 1];

        if (!data.ContainsKey(key))
        {
            data[key] = new DataRecord();
        }

        DataRecord keyData = data[key];

        if (!keyData.counts.ContainsKey(value))
        {
            keyData.counts[value] = 0;
        }

        keyData.counts[value]++;
        keyData.total++;
    }


    public string counterMovement(string eleccionesPredecir)
    {
        string option = GetMostLikely(eleccionesPredecir);

		if (option == "1")
            return "3";
		if (option == "2")
            return "4";
		if (option == "3")
            return "2";
		if (option == "4")
            return "1";

        return "1";
    }

    public string PrintData()
    {
        string info = "";

        info += "PREDICTOR DATA\n";

        foreach (string key in data.Keys)
        {
            info += key + "\n";

            DataRecord keyData = data[key];
            foreach (char action in keyData.counts.Keys)
            {
                info += "\t" + action + " -> " + keyData.counts[action];
            }
        }

        return info;
    }
}
