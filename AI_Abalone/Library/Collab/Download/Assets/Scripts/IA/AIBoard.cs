using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct keyposition
{
    public int row;
    public int column;
    public PLAYER_COLOR changedTo;

    public keyposition(int _row, int _column, PLAYER_COLOR _changedTo)
    {
        row = _row;
        column = _column;
        changedTo = _changedTo;
    }

}

public class AIBoard
{

    public Board supBoard;

    public PLAYER_COLOR[][] spaces;
    public PLAYER_COLOR activePlayer;

    public byte rows, columns;

    private byte[][] evaluationMatrix;

    public ZobristKeys zobristKeys;
    public int hashValue;

    public AIBoard(Board board)
    {

        supBoard = board;
        rows = 9;
        spaces = new PLAYER_COLOR[9][];
        spaces[0] = new PLAYER_COLOR[5];
        spaces[1] = new PLAYER_COLOR[6];
        spaces[2] = new PLAYER_COLOR[7];
        spaces[3] = new PLAYER_COLOR[8];
        spaces[4] = new PLAYER_COLOR[9];
        spaces[5] = new PLAYER_COLOR[8];
        spaces[6] = new PLAYER_COLOR[7];
        spaces[7] = new PLAYER_COLOR[6];
        spaces[8] = new PLAYER_COLOR[5];


        evaluationMatrix = new byte[9][];
        evaluationMatrix[0] = new byte[] { 0, 0, 0, 0, 0 };
        evaluationMatrix[1] = new byte[] { 0, 1, 1, 1, 1, 0 };
        evaluationMatrix[2] = new byte[] { 0, 1, 3, 3, 3, 1, 0 };
        evaluationMatrix[3] = new byte[] { 0, 1, 3, 5, 5, 3, 1, 0 };
        evaluationMatrix[4] = new byte[] { 0, 1, 3, 5, 7, 5, 3, 1, 0 };
        evaluationMatrix[5] = new byte[] { 0, 1, 3, 5, 5, 3, 1, 0 };
        evaluationMatrix[6] = new byte[] { 0, 1, 3, 3, 3, 1, 0 };
        evaluationMatrix[7] = new byte[] { 0, 1, 1, 1, 1, 0 };
        evaluationMatrix[8] = new byte[] { 0, 0, 0, 0, 0 };

    }

    public int Evaluate(PLAYER_COLOR player)
    {
        if (IsWinningPosition(player))
        {
            return 1000;
        }
        if (IsWinningPosition(Opponent(player)))
        {
            return -1000;
        }

        int evaluationSum = 0;

        // canicas del AI en el medio
        for (byte row = 0; row < rows; row++)
        {
            int maxcolumn = spaces[row].Length;

            for (byte column = 0; column < maxcolumn; column++)
            {
                if (spaces[row][column] == player)
                    evaluationSum += evaluationMatrix[row][column];
                else if (spaces[row][column] == Opponent(player))
                    evaluationSum -= evaluationMatrix[row][column];
                if (isHexagon(row, column))
                    evaluationSum += 10;
            }
        }

        // Número de canicas amigas y enemigas



        

        
        Debug.Log(evaluationSum);


        return evaluationSum;
    }

    public List<MoveMarble>[] PossibleMoves()  //modificar para sacar la lista de posibles movimientos 
    {
        /*int[] moves;
        int count = 0;

        for (byte column = 0; column < columns; column++)
        {
            if (IsEmptySpace(0, column)) count++;
        }

        moves = new int[count];

        count = 0;
        for (byte column = 0; column < columns; column++)
        {
            if (IsEmptySpace(0, column))
            {
                moves[count] = column;
                count++;
            }
        }*/

        return new List<MoveMarble>[0];//temporal
    }

    PLAYER_COLOR Opponent(PLAYER_COLOR player)
    {
        if (player == PLAYER_COLOR.WHITE)
        {
            return PLAYER_COLOR.BLACK;
        }
        else
        {
            return PLAYER_COLOR.WHITE;
        }
    }

    public bool IsEndOfGame()
    {
        if (IsWinningPosition(PLAYER_COLOR.WHITE))
        {
            return true;
        }
        else if (IsWinningPosition(PLAYER_COLOR.BLACK))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public bool IsWinningPosition(PLAYER_COLOR player)
    {
        int numberMarbles = 0;

        for (byte row = 0; row < rows; row++)
        {
            int maxcolumn = spaces[row].Length;

            for (byte column = 0; column < maxcolumn; column++)
            {
                if (spaces[row][column] == Opponent(player))
                    numberMarbles++;
            }
        }

        if (numberMarbles <= 8)
            return true;
        else
            return false;
    }

    public AIBoard GenerateNewBoardFromMove(List<MoveMarble> move)
    {
        AIBoard newBoard = this.DuplicateBoard();
        newBoard.Move(move, activePlayer);
        newBoard.activePlayer = Opponent(newBoard.activePlayer);
        return newBoard;
    }

    public AIBoard HashGenerateNewBoardFromMove(List<MoveMarble> move)
    {
        AIBoard newBoard = this.DuplicateBoard();
        newBoard.zobristKeys = this.zobristKeys;
        newBoard.HashMove(move, activePlayer);
        newBoard.activePlayer = Opponent(newBoard.activePlayer);
        return newBoard;
    }

    public AIBoard DuplicateBoard()
    {
        AIBoard newBoard = new AIBoard(this.supBoard);

        for (byte row = 0; row < rows; row++)
        {
            int maxcolumn = spaces[row].Length;

            for (byte column = 0; column < maxcolumn; column++)
            {
                newBoard.spaces[row][column] = this.spaces[row][column];
            }
        }

        newBoard.activePlayer = this.activePlayer;
        return newBoard;
    }

    public bool IsEmptySpace(int row, int column)
    {
        if (spaces[row][column] == PLAYER_COLOR.NONE) return true;
        else return false;
    }

    public void Move(List<MoveMarble> moves, PLAYER_COLOR player)   //modificar todo el cuerpo
    {

        foreach (MoveMarble move in moves)
        {
            bool white= move.CurrentCell.Marble.isWhite;
            for (byte row = 0; row < rows; row++)
            {
                int maxcolumn = spaces[row].Length;

                for (byte column = 0; column < maxcolumn; column++)
                {
                    if (move.CurrentCell.Row == row && move.CurrentCell.Column == column && move.hasToDestroy)
                        spaces[row][column] = PLAYER_COLOR.NONE;
                    else if (move.CurrentCell.Row == row && move.CurrentCell.Column == column && spaces[row][column] != PLAYER_COLOR.NONE)
                        spaces[row][column] = PLAYER_COLOR.NONE;
                    else if (move.NextCell != null && move.NextCell.Row == row && move.NextCell.Column == column && spaces[row][column] == PLAYER_COLOR.NONE)
                    {
                        if (white)
                            spaces[row][column] = PLAYER_COLOR.WHITE;
                        else
                            spaces[row][column] = PLAYER_COLOR.BLACK;
                    }
                }
            }
        }
        
    }

    public void HashMove(List<MoveMarble> moves, PLAYER_COLOR player) //modificar todo el cuerpo
    {

        int piece, zobristKey;

        List<keyposition> rowCols=new List<keyposition>();

        foreach (MoveMarble move in moves)
        {
            PLAYER_COLOR current;
            PLAYER_COLOR next;

            if (!move.CurrentCell.Marble)
                current = PLAYER_COLOR.NONE;
            else if (move.CurrentCell.Marble.isWhite)
                current = PLAYER_COLOR.WHITE;
            else
                current = PLAYER_COLOR.BLACK;

            if (!move.CurrentCell.Marble)
                next = PLAYER_COLOR.NONE;
            else if (move.CurrentCell.Marble.isWhite)
                next = PLAYER_COLOR.WHITE;
            else
                next = PLAYER_COLOR.BLACK;

            

            for (byte row = 0; row < rows; row++)
            {
                int maxcolumn = spaces[row].Length;

                for (byte column = 0; column < maxcolumn; column++)
                {
                    if (move.CurrentCell.Row == row && move.CurrentCell.Column == column && move.hasToDestroy)
                        spaces[row][column] = PLAYER_COLOR.NONE;
                    else if (move.CurrentCell.Row == row && move.CurrentCell.Column == column && spaces[row][column] == player)
                        spaces[row][column] = PLAYER_COLOR.NONE;
                    else if (move.NextCell != null && move.NextCell.Row == row && move.NextCell.Column == column && spaces[row][column] == PLAYER_COLOR.NONE)
                    {
                        if (current == PLAYER_COLOR.WHITE && (next == PLAYER_COLOR.BLACK || next == PLAYER_COLOR.NONE))
                        {
                            spaces[row][column] = PLAYER_COLOR.WHITE;
                            rowCols.Add(new keyposition(row, column, spaces[row][column]));
                        }

                        else if (current == PLAYER_COLOR.BLACK && (next == PLAYER_COLOR.WHITE || next == PLAYER_COLOR.NONE))
                        {
                            spaces[row][column] = PLAYER_COLOR.BLACK;
                            rowCols.Add(new keyposition(row, column, spaces[row][column]));
                        }

                        else if (current == next)
                        {
                            spaces[row][column] = current;
                        }
                    }
                }
            }

            
        }
        rowCols.Add(new keyposition(moves[moves.Count-1].CurrentCell.Row, moves[moves.Count - 1].CurrentCell.Column,activePlayer));
        
        //Se modifica mas de una posicion al mismo tiempo asi que hay que hacer un bucle
        for (int i=0;i<rowCols.Count ;i++)
        {
            if (rowCols[i].changedTo == PLAYER_COLOR.BLACK)
                piece = 0;
            else
                piece = 1;

            zobristKey = zobristKeys.GetKey((int)rowCols[i].row, (int)rowCols[i].column,piece);

            hashValue ^= zobristKey; 
        }

    }

    public void calculateHashValue()
    {
        int piece;
        int zobristKey;

        hashValue = 0;

        for (int row = 0; row < 9; row++)
        {
            int maxColumn = this.supBoard.cells[row].Length;

            for (int column = 0; column < maxColumn; column++)
            {
                if (!IsEmptySpace(row, column))
                {

                    if (spaces[row][column] == PLAYER_COLOR.BLACK)
                    {
                        piece = 0;
                        zobristKey = zobristKeys.GetKey(row, column, piece);
                        hashValue ^= zobristKey;
                    }
                    else if(spaces[row][column] == PLAYER_COLOR.WHITE)
                    {
                        piece = 1;
                        zobristKey = zobristKeys.GetKey(row, column, piece);
                        hashValue ^= zobristKey;
                    }

                    
                }
            }
        }
        PrintHash();
    }

    void PrintHash()
    {
        string output = "";
        output += "Valor Hash del Tablero: " + hashValue + " // " + System.Convert.ToString(hashValue, 2).PadLeft(32, '0');
        Debug.Log(output);
    }

    #region

    private bool isHexagon(int row, int column)
    {
        int counter = 0;

        if (Check_NextCellLeft(row, column))
        {
            if (NextCellLeft(row, column))
                counter++;
        }
        if (Check_NextCellRight(row, column))
        {
            if (NextCellRight(row, column))
                counter++;
        }
        if (Check_NextCellFrontLeft(row, column))
        {
            if (NextCellFrontLeft_IsSameColor(row, column))
                counter++;
        }
        if (Check_NextCellFrontRight(row, column))
        {
            if (NextCellFrontRight_IsSameColor(row, column))
                counter++;
        }
        if (Check_NextCellBackLeft(row, column))
        {
            if (NextCellBackLeft_IsSameColor(row, column))
                counter++;
        }
        if (Check_NextCellBackRight(row, column))
        {
            if (NextCellBackRight_IsSameColor(row, column))
                counter++;
        }

        if (counter >= 6)
            return true;
        else
            return false;

        
    }


    public bool NextCellLeft(int row, int column)
    {
        if (spaces[row][column] == spaces[row][column - 1])
            return true;
        else
            return false;
    }

    public bool NextCellRight(int row, int column)
    {
        if (spaces[row][column] == spaces[row][column + 1])
            return true;
        else
            return false;
    }


    public bool NextCellFrontLeft_IsSameColor(int row, int column)
    {
        if (row < 4)
        {
            if (spaces[row][column] == spaces[row + 1][column])
                return true;
            else
                return false;
        }
        else
        {
            if (spaces[row][column] == spaces[row + 1][column - 1])
                return true;
            else
                return false;
        }
    }

    public bool NextCellFrontRight_IsSameColor(int row, int column)
    {
        if (row < 4)
        {
            if (spaces[row][column] == spaces[row + 1][column + 1])
                return true;
            else
                return false;
        }
        else
        {

            if (spaces[row][column] == spaces[row + 1][column])
                return true;
            else
                return false;
        }
    }

    public bool NextCellBackLeft_IsSameColor(int row, int column)
    {
        if (row < 5)
        {
            if (spaces[row][column] == spaces[row - 1][column - 1])
                return true;
            else
                return false;
        }
        else
        {
            if (spaces[row][column] == spaces[row - 1][column])
                return true;
            else
                return false;
        }
    }

    public bool NextCellBackRight_IsSameColor(int row, int column)
    {
        if (row < 5)
        {
            if (spaces[row][column] == spaces[row - 1][column])
                return true;
            else
                return false;
        }
        else
        {
            if (spaces[row][column] == spaces[row - 1][column + 1])
                return true;
            else
                return false;
        }
    }

    //------------CHECKS----------------

    public bool Check_NextCellLeft(int row, int column)
    {
        if (column > 0)
            return true;
        else
            return false;
    }

    public bool Check_NextCellRight(int row, int column)
    {
        if (column < (spaces[row].Length - 1))
            return true;
        else
            return false;
    }

    public bool Check_NextCellFrontLeft(int row, int column)
    {
        if (row < 4)
        {
            return true;
        }
        else if (column > 0 && row < 8)
        {
            return true;
        }

        return false;
    }

    public bool Check_NextCellFrontRight(int row, int column)
    {
        if (row < 4)
        {
            return true;
        }
        else if (column < (spaces[row].Length - 1) && row < 8)
        {
            return true;
        }
        return false;
    }

    public bool Check_NextCellBackLeft(int row, int column)
    {
        if (row > 4)
        {
            return true;
        }
        else if (column > 0 && row > 0)
        {
            return true;
        }
        return false;
    }

    public bool Check_NextCellBackRight(int row, int column)
    {
        if (row > 4)
        {
            return true;
        }
        else if (column < (spaces[row].Length - 1) && row > 0)
        {
            return true;
        }
        return false;
    }

    #endregion

}
