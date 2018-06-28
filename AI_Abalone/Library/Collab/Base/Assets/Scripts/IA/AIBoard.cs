using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            }
        }

        // Número de canicas amigas y enemigas

        // Canicas agrupadas

        //Posiciones de ataque enemigas y aliadas



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
        int[] positionsRow = new int[moves.Count];
        int[] positionsColumn = new int[moves.Count];

        int posIndex = 0;
        int piece, zobristKey;

        foreach (MoveMarble move in moves)
        {
            bool white = move.CurrentCell.Marble.isWhite;

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
                        if (white)
                            spaces[row][column] = PLAYER_COLOR.WHITE;
                        else
                            spaces[row][column] = PLAYER_COLOR.BLACK;

                        positionsRow[posIndex] = row;
                        positionsColumn[posIndex] = column;
                        posIndex++;
                    }

                }
            }
        }

        //Se modifica mas de una posicion al mismo tiempo asi que hay que hacer un bucle
        for (int i=0;i<positionsRow.Length ;i++)
        {
            if (player == PLAYER_COLOR.BLACK)
            {
                piece = 0;
            }
            else
            {
                piece = 1;
            }
            zobristKey = zobristKeys.GetKey(positionsRow[i],positionsColumn[i], piece);

            hashValue ^= zobristKey; 
        }
        /*
        int filledRow = 0, position, piece, zobristKey;

        for (int row = rows - 1; row >= 0; row--)
        {
            if (IsEmptySpace(row, column))
            {
                spaces[row][column] = player;
                filledRow = row;
                break;
            }
        }

        position = filledRow * columns + column;
        if (player == PLAYER_COLOR.BLACK)
        {
            piece = 0;
        }
        else
        {
            piece = 1;
        }

        zobristKey = zobristKeys.GetKey(position, piece);

        hashValue ^= zobristKey;*/
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
                    }
                    else
                    {
                        piece = 1;
                    }
                    zobristKey = zobristKeys.GetKey(row,column, piece);
                    hashValue ^= zobristKey;
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
}
