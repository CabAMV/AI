using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct keyposition
{
    public int row;
    public int column;
    public PLAYER_COLOR changedTo;
    public Direction direction;

    public keyposition(int _row, int _column, PLAYER_COLOR _changedTo)
    {
        row = _row;
        column = _column;
        changedTo = _changedTo;
        direction = 0;
    }
    public keyposition(int _row, int _column, PLAYER_COLOR _changedTo, Direction _direction)
    {
        row = _row;
        column = _column;
        changedTo = _changedTo;
        direction = _direction;
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

    public keyposition possibleMarble;

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
        evaluationMatrix[1] = new byte[] { 0, 3, 3, 3, 3, 0 };
        evaluationMatrix[2] = new byte[] { 0, 3, 5, 5, 5, 3, 0 };
        evaluationMatrix[3] = new byte[] { 0, 3, 5, 7, 7, 5, 3, 0 };
        evaluationMatrix[4] = new byte[] { 0, 3, 5, 7, 9, 7, 5, 3, 0 };
        evaluationMatrix[5] = new byte[] { 0, 3, 5, 7, 7, 5, 3, 0 };
        evaluationMatrix[6] = new byte[] { 0, 3, 5, 5, 5, 3, 0 };
        evaluationMatrix[7] = new byte[] { 0, 3, 3, 3, 3, 0 };
        evaluationMatrix[8] = new byte[] { 0, 0, 0, 0, 0 };

    }

    public int Evaluate(PLAYER_COLOR player)
    {
        if (IsWinningPosition(player))
        {
            return 100000;
        }
        if (IsWinningPosition(Opponent(player)))
        {
            return -100000;
        }

        int evaluationSum = 0;

        int numberMarblesOpponents = 0;
		int numberMarblesPlayer = 0;

        // canicas del AI en el medio
        for (byte row = 0; row < rows; row++)
        {
            int maxcolumn = spaces[row].Length;

            for (byte column = 0; column < maxcolumn; column++)
            {
				if (spaces [row] [column] == player) 
				{
					evaluationSum += evaluationMatrix [row] [column];
					numberMarblesPlayer++;
				}
                else if (spaces[row][column] == Opponent(player))
                {
                    evaluationSum -= evaluationMatrix[row][column] * 5;
                    numberMarblesOpponents++;
                }
                if (isHexagon(row, column))
                    evaluationSum += 1;
            }
        }

		evaluationSum += (14 - numberMarblesOpponents) * 100;
		evaluationSum -= (14 - numberMarblesPlayer) * 3;

        // Número de canicas amigas y enemigas
        // Cuento las canicas de los enemigos y resto del numero total posible
        // Si cuanto menos enemigos más puntos se puede condierar como haber atacado?

        return evaluationSum;
    }

    public List<MoveMarble>[] PossibleMoves()  //modificar para sacar la lista de posibles movimientos 
    {
        List<List<MoveMarble>> aux = new List<List<MoveMarble>>();

        MoveMarble first = null;
        MoveMarble second = null;
        MoveMarble third = null;

        MoveMarble firstEnemy = null;
        MoveMarble secondEnemy = null;

        //Direction direction = Direction.Left;

        //cellsSelected = _cellsSelected;
        //isWhite = _isWhite;
        for (byte row = 0; row < rows; row++)
        {
            int maxcolumn = spaces[row].Length;

            for (byte column = 0; column < maxcolumn; column++)
            {
                if (spaces[row][column] == activePlayer)
                {
                    for (int direction = 0; direction < 6; direction++)
                    {
						first = null;
						second = null;
						third = null;

						firstEnemy = null;
						secondEnemy = null;

                        if (checkNextCell(row, column, (Direction)direction))
                        {
                            //Si hay un espacio vacio
                            if (NextCellColor(row, column, (Direction)direction) == PLAYER_COLOR.NONE)
                            {
                                //primera canica
                                List<MoveMarble> temp = new List<MoveMarble>();
                                first = new MoveMarble(supBoard.cells[row][column], supBoard.cells[possibleMarble.row][possibleMarble.column]);
                                temp.Add(first);
                                aux.Add(temp);
                                //segunda canica
                                if (checkNextCell(row, column, oppositeDirection((Direction)direction)))
                                {
                                    if (NextCellColor(row, column, oppositeDirection((Direction)direction)) == activePlayer)
                                    {
                                        temp = new List<MoveMarble>();
                                        second = new MoveMarble(supBoard.cells[possibleMarble.row][possibleMarble.column], first.CurrentCell);
                                        temp.Add(first);
                                        temp.Add(second);
                                        aux.Add(temp);
                                        //tercera canica
                                        if (checkNextCell(second.CurrentCell.Row, second.CurrentCell.Column, oppositeDirection((Direction)direction)))
                                        {
                                            if (NextCellColor(second.CurrentCell.Row, second.CurrentCell.Column, oppositeDirection((Direction)direction)) == activePlayer)
                                            {
                                                temp = new List<MoveMarble>();
                                                third = new MoveMarble(supBoard.cells[possibleMarble.row][possibleMarble.column], second.CurrentCell);
                                                temp.Add(first);
                                                temp.Add(second);
                                                temp.Add(third);
                                                aux.Add(temp);


                                            }
                                        }
                                    }
                                }
                            }

                            //Si hay enemigos delante
                            else if (NextCellColor(row, column, (Direction)direction) == Opponent(activePlayer))
                            {
                                //primera canica
                                first = new MoveMarble(supBoard.cells[row][column], supBoard.cells[possibleMarble.row][possibleMarble.column]);

                                //primer enemy
                                if (checkNextCell(possibleMarble.row, possibleMarble.column, (Direction)direction))
                                    NextCellColor(possibleMarble.row, possibleMarble.column, (Direction)direction);
                                firstEnemy = new MoveMarble(first.NextCell, supBoard.cells[possibleMarble.row][possibleMarble.column]);

                                //segunda canica
                                if (checkNextCell(row, column, oppositeDirection((Direction)direction)))
                                {
                                    if (NextCellColor(row, column, oppositeDirection((Direction)direction)) == activePlayer)
                                    {
                                        second = new MoveMarble(supBoard.cells[possibleMarble.row][possibleMarble.column], first.CurrentCell);

                                        if (checkNextCell(firstEnemy.CurrentCell.Row, firstEnemy.CurrentCell.Column, (Direction)direction))
                                        {
                                            if (NextCellColor(firstEnemy.CurrentCell.Row, firstEnemy.CurrentCell.Column, (Direction)direction) == Opponent(activePlayer))
                                            {
                                                secondEnemy = new MoveMarble(firstEnemy.NextCell, supBoard.cells[possibleMarble.row][possibleMarble.column]);
                                            }
                                        }

                                        //tercera canica
                                        if (checkNextCell(second.CurrentCell.Row, second.CurrentCell.Column, oppositeDirection((Direction)direction)))
                                        {
                                            if (NextCellColor(second.CurrentCell.Row, second.CurrentCell.Column, oppositeDirection((Direction)direction)) == activePlayer)
                                            {
                                                third = new MoveMarble(supBoard.cells[possibleMarble.row][possibleMarble.column], second.CurrentCell);
                                            }
                                        }
                                    }
                                }

                                List<MoveMarble> temp = new List<MoveMarble>();
                                if (second != null)
                                {
                                    //mover un enemigo
                                    if (secondEnemy == null)
                                    {
                                        if (checkNextCell(firstEnemy.CurrentCell.Row, firstEnemy.CurrentCell.Column, (Direction)direction))
                                        {
                                            if (NextCellColor(firstEnemy.CurrentCell.Row, firstEnemy.CurrentCell.Column, (Direction)direction) == PLAYER_COLOR.NONE)
                                            {
                                                temp = new List<MoveMarble>();
                                                temp.Add(firstEnemy);
                                                temp.Add(first);
                                                temp.Add(second);
                                                aux.Add(temp);
                                               
                                                if (third != null)
                                                {
                                                    temp = new List<MoveMarble>();
                                                    temp.Add(firstEnemy);
                                                    temp.Add(first);
                                                    temp.Add(second);
                                                    temp.Add(third);
                                                    aux.Add(temp);
                                                }
                                            }
                                        }
                                        else // Eliminar enemigo
                                        {
                                            firstEnemy = new MoveMarble(firstEnemy.CurrentCell, true);
                                            temp = new List<MoveMarble>();
                                            temp.Add(firstEnemy);
                                            temp.Add(first);
                                            temp.Add(second);
											aux.Add (temp);

                                            if (third != null)
                                            {
                                                temp = new List<MoveMarble>();
                                                temp.Add(firstEnemy);
                                                temp.Add(first);
                                                temp.Add(second);
                                                temp.Add(third);
                                                aux.Add(temp);
                                            }

                                        }
                                    }
                                    //mover dos enemigos
                                    else
                                    {
										if (checkNextCell(secondEnemy.CurrentCell.Row, secondEnemy.CurrentCell.Column, (Direction)direction))
                                        {
											if (NextCellColor(secondEnemy.CurrentCell.Row, secondEnemy.CurrentCell.Column, (Direction)direction) == PLAYER_COLOR.NONE)
                                            {

                                                if (third != null)
                                                {
                                                    temp = new List<MoveMarble>();
                                                    temp.Add(secondEnemy);
                                                    temp.Add(firstEnemy);
                                                    temp.Add(first);
                                                    temp.Add(second);
                                                    temp.Add(third);
                                                    aux.Add(temp);
                                                }
                                            }
                                        }
                                        else // Eliminar enemigo
                                        {
                                            if (third != null)
                                            {
                                                secondEnemy = new MoveMarble(secondEnemy.CurrentCell, true);
                                                temp = new List<MoveMarble>();
                                                temp.Add(secondEnemy);
                                                temp.Add(firstEnemy);
                                                temp.Add(first);
                                                temp.Add(second);
                                                temp.Add(third);
                                                aux.Add(temp);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //List<List<MoveMarble>> aux = new List<List<MoveMarble>>();
        List<MoveMarble>[] posibleMoves = new List<MoveMarble>[aux.Count];

        for (int i = 0; i < aux.Count; i++)
            posibleMoves[i] = aux[i];

        return posibleMoves;
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
            bool white = move.CurrentCell.Marble.isWhite;
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

        List<keyposition> rowCols = new List<keyposition>();

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
        rowCols.Add(new keyposition(moves[moves.Count - 1].CurrentCell.Row, moves[moves.Count - 1].CurrentCell.Column, activePlayer));

        //Se modifica mas de una posicion al mismo tiempo asi que hay que hacer un bucle
        for (int i = 0; i < rowCols.Count; i++)
        {
            if (rowCols[i].changedTo == PLAYER_COLOR.BLACK)
                piece = 0;
            else
                piece = 1;

            zobristKey = zobristKeys.GetKey((int)rowCols[i].row, (int)rowCols[i].column, piece);

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
                    else if (spaces[row][column] == PLAYER_COLOR.WHITE)
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

    public Direction oppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return Direction.Right;

            case Direction.FrontLeft:
                return Direction.BackRight;

            case Direction.FrontRight:
                return Direction.BackLeft;

            case Direction.Right:
                return Direction.Left;

            case Direction.BackRight:
                return Direction.FrontLeft;

            case Direction.BackLeft:
                return Direction.FrontRight;

            default:
                return 0;
        }
    }
		
    public PLAYER_COLOR NextCellColor(int row, int column, Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                possibleMarble = new keyposition(row, column - 1, PLAYER_COLOR.NONE);
                return spaces[row][column - 1];

            case Direction.FrontLeft:
                if (row < 4)
                {
                    possibleMarble = new keyposition(row + 1, column, PLAYER_COLOR.NONE);
                    return spaces[row + 1][column];
                }
                else
                {
                    possibleMarble = new keyposition(row + 1, column - 1, PLAYER_COLOR.NONE);
                    return spaces[row + 1][column - 1];
                }


            case Direction.FrontRight:
                if (row < 4)
                {
                    possibleMarble = new keyposition(row + 1, column + 1, PLAYER_COLOR.NONE);
                    return spaces[row + 1][column + 1];
                }
                else
                {
                    possibleMarble = new keyposition(row + 1, column, PLAYER_COLOR.NONE);
                    return spaces[row + 1][column];
                }

            case Direction.Right:
                possibleMarble = new keyposition(row, column + 1, PLAYER_COLOR.NONE);
                return spaces[row][column + 1];

            case Direction.BackRight:
                if (row < 5)
                {
                    possibleMarble = new keyposition(row - 1, column, PLAYER_COLOR.NONE);
                    return spaces[row - 1][column];
                }
                else
                {
                    possibleMarble = new keyposition(row - 1, column + 1, PLAYER_COLOR.NONE);
                    return spaces[row - 1][column + 1];
                }

            case Direction.BackLeft:
                if (row < 5)
                {
                    possibleMarble = new keyposition(row - 1, column - 1, PLAYER_COLOR.NONE);
                    return spaces[row - 1][column - 1];
                }
                else
                {
                    possibleMarble = new keyposition(row - 1, column, PLAYER_COLOR.NONE);
                    return spaces[row - 1][column];
                }

            default:
                return PLAYER_COLOR.NONE;
        }
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
		
    public bool checkNextCell(int row, int column, Direction direction)
    {
        switch (direction)
        {
			case Direction.Left:
				return Check_NextCellLeft (row, column);

			case Direction.FrontLeft:
				return Check_NextCellFrontLeft (row, column);

			case Direction.FrontRight:
				return Check_NextCellFrontRight (row, column);

			case Direction.Right:
				return Check_NextCellRight (row, column);

			case Direction.BackRight:
				return Check_NextCellBackRight (row, column);

			case Direction.BackLeft:
				return Check_NextCellBackLeft (row, column);
               
            default:
                return false;
        }
    }

    #endregion

}
