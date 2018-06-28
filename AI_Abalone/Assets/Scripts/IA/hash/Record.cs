using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record
{
    public int hashValue;
    public int minScore;
    public int maxScore;
    public List<MoveMarble> bestMove;
    public int depth;

    public Record()
    {
        hashValue = 0;
        minScore = 0;
        maxScore = 0;
        bestMove = new List<MoveMarble>();
        depth = 0;
    }
}
