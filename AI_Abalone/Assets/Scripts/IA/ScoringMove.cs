using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoringMove  {

    public int score;
    //public int move;
    public List<MoveMarble> move;

    public ScoringMove(int _score)  //añadir lista de fichar y direccion en la que mover dichas fichas
    {
        score = _score;
        move = new List<MoveMarble>();
    }
    public ScoringMove(int _score, List<MoveMarble> _mov)  //añadir lista de fichar y direccion en la que mover dichas fichas
    {
        score = _score;
        move = _mov;
    }
}
