using System;
using UnityEngine;

public class ZobristKeys
{
    protected int[][,] keys; // 31 bits
    protected int boardPositions, numberOfPieces;

    public ZobristKeys (int _boardPositions, int _numberOfPieces)
    {
        System.Random rnd = new System.Random();
        boardPositions = _boardPositions;
        numberOfPieces = _numberOfPieces;

        keys = new int[boardPositions][,];
        keys[0] = new int[5, numberOfPieces];
        keys[1] = new int[6, numberOfPieces];
        keys[2] = new int[7, numberOfPieces];
        keys[3] = new int[8, numberOfPieces];
        keys[4] = new int[9, numberOfPieces];
        keys[5] = new int[8, numberOfPieces];
        keys[6] = new int[7, numberOfPieces];
        keys[7] = new int[6, numberOfPieces];
        keys[8] = new int[5, numberOfPieces];

        for (byte row = 0; row < 9; row++)
        {
            int maxColumn = keys[row].GetLength(0);

            for (byte column = 0; column < maxColumn; column++)
            {
                for (int j = 0; j < numberOfPieces; j++) 
                    keys[row][column,j] = rnd.Next(int.MaxValue);
            }
        }

     }

    public int GetKey(int row,int column, int piece)
    {
        return keys[row][column, piece];
    }

    public void Print ()
    {
        /*string output = "";

        output += "Claves Zobrist:\n";

        for (int i = 0; i < boardPositions; i++)
        {
            for (int j = 0; j < numberOfPieces; j++)
            {
                output += "Position " + Convert.ToString(i).PadLeft(2, '0') + ", Pieza" + j + ": " + Convert.ToString(keys[i, j], 2).PadLeft(32, '0') + "\n";
            }
        }
        Debug.Log(output);*/
    }
}
