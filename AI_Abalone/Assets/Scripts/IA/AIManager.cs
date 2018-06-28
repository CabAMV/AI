using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum PLAYER_COLOR
    {
        WHITE,BLACK,NONE
    };


public class AIManager : MonoBehaviour {

    public List<Cell> Cells_Marbles;

    [SerializeField]
    private Board physicBoard;

    //[SerializeField]
    private AIBoard board;

    [SerializeField]
    private MovementHandler movementHandler;

    [SerializeField]
    private PossibleMovesCalculator posibleMovesCal;

    private List<PosibleMove> posibleMoves;
    #region

    /// <summary>
    /// region que separa lo de manu y gabriel de michael
    /// </summary>
    /// 

    public byte MAX_DEPTH = 5;

    private const int INFINITE = 10000;
    private const int MINUS_INFINITE = -10000;

    PLAYER_COLOR activePlayer;

    // PARA FUNCIONAMIENTO CON HASH VALUES
    public ZobristKeys zobristKeys;
    protected TranspositionTable transpositionTable;
    [SerializeField]
    private int hashTableLength = 90000000;
    private int maximumExploredDepth = 0;
    private int globalGuess = INFINITE;
    [SerializeField]
    private int MAX_ITERATIONS = 10;

    void Awake()
    {
        zobristKeys = new ZobristKeys(61, 2);
        //zobristKeys.Print();
        transpositionTable = new TranspositionTable(hashTableLength);
    }

    public void Play(PLAYER_COLOR player)
    {
        this.activePlayer = player;
        ObserveBoard();
        ScoringMove move;
        System.DateTime a = System.DateTime.Now;

        move = MTD(board);
        System.DateTime b = System.DateTime.Now;
        Debug.Log("tiempo del posible moves" + (b - a));
        Debug.Log("Jugador Activo:  " + activePlayer + " Jugada Elegida:  " + move.move.Count + "/     " + move.score);

        Move(move);
    }

    void ObserveBoard()
    {
        board = new AIBoard(physicBoard);

        for (byte row = 0; row < 9; row++)
        {
            int maxColumn = board.supBoard.cells[row].Length;

            for (byte column = 0; column < maxColumn; column++)
            {
                //Text spaceText = buttonList[row, column];
                if (board.supBoard.cells[row][column].Marble == null)
                    board.spaces[row][column] = PLAYER_COLOR.NONE;
                else
                {
                    if (board.supBoard.cells[row][column].Marble.isWhite)
                        board.spaces[row][column] = PLAYER_COLOR.WHITE;
                    else
                        board.spaces[row][column] = PLAYER_COLOR.BLACK;
                }
            }
        }

        board.activePlayer = this.activePlayer;
        board.zobristKeys = this.zobristKeys;
        board.calculateHashValue();
    }

    void Move(ScoringMove scoringMove)
    {
        StartCoroutine(MakeMove(scoringMove));
    }
    private IEnumerator MakeMove(ScoringMove scoringMove)
    {
        yield return StartCoroutine(movementHandler.ExecuteMovement(scoringMove.move));
        GameManager.Instance.EndTurn();
    }

    public ScoringMove MTD(AIBoard board)
    {
        int gamma, guess = globalGuess;
        ScoringMove scoringMove = null;
        maximumExploredDepth = 0;

        string output = "";

        for (byte i = 0; i < MAX_ITERATIONS; i++)
        {
            gamma = guess;
            scoringMove = Test(board, 0, gamma - 1);
            guess = scoringMove.score;
            if (guess == gamma)
            {
                globalGuess = guess;
                output += "guess encontrado en iteracion " + i;
                //MTDPathTest.text = output;
                return scoringMove;
            }
        }

        output += "guess no encontrado";
        globalGuess = guess;
        //MTDPathTest.text = output;
        return scoringMove;
    }

    ScoringMove Test(AIBoard board, byte depth, int gamma)
    {
        // Devuelve el score del tablero y la jugada con la que se llega a él.
        List<MoveMarble> bestMove = new List<MoveMarble>();
        int bestScore = 0;

        ScoringMove scoringMove; // score, movimiento
        AIBoard newBoard;
        Record record;

        if (depth > maximumExploredDepth)
        {
            maximumExploredDepth = depth;
        }

        record = transpositionTable.GetRecord(board.hashValue);

        if (record != null)
        {
            if (record.depth > MAX_DEPTH - depth)
            {
                if (record.minScore > gamma)
                {
                    scoringMove = new ScoringMove(record.minScore, record.bestMove);
                    return scoringMove; 

                }

                if (record.maxScore < gamma)
                {
                    scoringMove = new ScoringMove(record.maxScore, record.bestMove);
                    return scoringMove;

                }
            }
        }
        else
        {
            record = new Record();
            record.hashValue = board.hashValue;
            record.depth = MAX_DEPTH - depth;
            record.minScore = MINUS_INFINITE;
            record.maxScore = INFINITE; 

        }

        // Comprobar si hemos terminado de hacer recursión
        if (board.IsEndOfGame() || depth == MAX_DEPTH)
        {
            if (depth % 2 == 0)
            {
                record.maxScore = board.Evaluate(activePlayer); 

            }
            else
            {
                record.maxScore = -board.Evaluate(activePlayer); 

            }

            record.minScore = record.maxScore;
            transpositionTable.SaveRecord(record);
            
            scoringMove = new ScoringMove(record.maxScore);
        }
        else
        {

            bestScore = MINUS_INFINITE;

            //modificar AIBoard.possibleMoves() para que devuelva los posibles movimientos
            List<MoveMarble>[] possibleMoves;    
            possibleMoves = board.PossibleMoves();
            foreach (List<MoveMarble> move in possibleMoves)
            {
                //newBoard = board.GenerateNewBoardFromMove(move);
                newBoard = board.HashGenerateNewBoardFromMove(move);//cambiar

                // Recursividad
                scoringMove = Test(newBoard, (byte)(depth + 1), -gamma);

                int invertedScore = -scoringMove.score;

                // Actualizar mejor score
                if (invertedScore > bestScore)
                {

                    bestScore = invertedScore;
                    bestMove = move;//cambiar
                    record.bestMove = move;
                }

                if (bestScore < gamma)
                {

                    record.maxScore = bestScore;

                }
                else
                {

                    record.minScore = bestScore;
                }
            }
            transpositionTable.SaveRecord(record);
            scoringMove = new ScoringMove(bestScore, bestMove);
        }
        return scoringMove;
    }

    #endregion
}
