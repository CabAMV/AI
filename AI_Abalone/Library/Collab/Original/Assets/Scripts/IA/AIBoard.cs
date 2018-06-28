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
        List<MoveMarble>[] posibleMoves;
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
                    for (int numMarbles = 1; numMarbles < 4; numMarbles++)
                    {
                        for (int directionPosition = 0; directionPosition < 6; directionPosition++)
                        {
                            Direction direction = (Direction)directionPosition;

                            List<PLAYER_COLOR> selectedMarbles = selectMarbles(row, column, numMarbles, activePlayer, direction);
                            //CheckPosibleMoves;
                            List<MoveMarble> moves = new List<MoveMarble>();
                        }
                    }
                }                    
            }
        }                

        //cellsOrdered = cellsSelected;

        //List<MoveMarble> moves = new List<MoveMarble>();
        /*
        Direction direction = (Direction)directionPosition;

        Cell nextCell_0 = (Check_NextCell(cellsOrdered[0], direction) == true) ? Get_NextCell(cellsOrdered[0], direction) : null;
        cellsOrdered = SortCellsSelected(cellsOrdered, nextCell_0);

        for (int indexMove = 0; indexMove < numMarbles; indexMove++)
        {
            isValidMove = SimulateMoveConsequences(indexMove);

            if (!isValidMove)
                break;
        }

        if (isValidMove)
        {
            posibleMoves.Add(new PosibleMove(ChooseTargetCell(), moves, direction));
        }*/

        posibleMoves = null;
        return posibleMoves;//temporal
    }

    private List<PLAYER_COLOR> selectMarbles (int row, int column, int numMarbles, PLAYER_COLOR player, Direction direction)
    {
        List<PLAYER_COLOR> selectedMarbles = new List<PLAYER_COLOR>();

        PLAYER_COLOR actualMarble = spaces[row][column];

        bool cycle = true;
        // NECESITO OTRA IDEA, ESTO ESTA BIEN PERO LOS DATOS INÚTILES
        do
        {
            switch (direction)
            {
                case Direction.Left:
                    if (column - 1 >= 0 && )
                    break;

                case Direction.FrontLeft:
                    nextCell = board.Get_NextCellFrontLeft(cell);
                    break;

                case Direction.FrontRight:
                    nextCell = board.Get_NextCellFrontRight(cell);
                    break;
                case Direction.Right:
                    nextCell = board.Get_NextCellRight(cell);
                    break;

                case Direction.BackRight:
                    nextCell = board.Get_NextCellBackRight(cell);
                    break;

                case Direction.BackLeft:
                    nextCell = board.Get_NextCellBackLeft(cell);
                    break;
                default:
                    actualMarble = null;
                    break;
            }
        }
        while (cycle);
        

        return selectedMarbles;
    }

    private bool Check_NextCell(Cell cell, Direction direction)
    {
        bool isValid = false;

        switch (direction)
        {
            case Direction.Left:
                isValid = board.Check_NextCellLeft(cell);
                break;

            case Direction.FrontLeft:
                isValid = board.Check_NextCellFrontLeft(cell);
                break;

            case Direction.FrontRight:
                isValid = board.Check_NextCellFrontRight(cell);
                break;
            case Direction.Right:
                isValid = board.Check_NextCellRight(cell);
                break;

            case Direction.BackRight:
                isValid = board.Check_NextCellBackRight(cell);
                break;

            case Direction.BackLeft:
                isValid = board.Check_NextCellBackLeft(cell);
                break;
        }

        return isValid;
    }

    public Cell Get_NextCellLeft(Cell currentCell)
    {
        return cells[currentCell.Row][currentCell.Column - 1];
    }

    public Cell Get_NextCellRight(Cell currentCell)
    {
        return cells[currentCell.Row][currentCell.Column + 1];
    }

    /*private void Check_PosibleMoves(int numMarbles)
    {
        for (int i = 0; i < 6; i++)
        {
            bool isValidMove = false;

            //cellsOrdered = cellsSelected;

            List<MoveMarble> moves = new List<MoveMarble>();
            Direction direction = (Direction)i;

            Cell nextCell_0 = (Check_NextCell(cellsOrdered[0], direction) == true) ? Get_NextCell(cellsOrdered[0], direction) : null;
            cellsOrdered = SortCellsSelected(cellsOrdered, nextCell_0);

            for (int indexMove = 0; indexMove < numMarbles; indexMove++)
            {
                isValidMove = SimulateMoveConsequences(indexMove);

                if (!isValidMove)
                    break;
            }

            if (isValidMove)
            {
                posibleMoves.Add(new PosibleMove(ChooseTargetCell(), moves, direction));
            }
        }
    }*/

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
