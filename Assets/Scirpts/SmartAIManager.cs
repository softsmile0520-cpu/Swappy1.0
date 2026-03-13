using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using DG.Tweening;

public class SmartAIManager : MonoBehaviour
{
    public static SmartAIManager instance;

    [SerializeField]
    public List<Moves> AIMoves = new List<Moves>(3); 

    public int AiScoreCount = 0;
    public int LastComboCounter = 0;

    public SwappyPlayer TurnPlayer;
    public int n = 0;
    private void Awake()
    {
        instance = this;
    }
    public void SmartAI()
    {
        
        if (Gamemanager.instance.CurrentPlayer.AiSwappy)
        {
            for (int i = 0; i < AIMoves.Count; i++)
            {
                EmptyMove(AIMoves[i]);
            }
            LastComboCounter = 0;

            TurnPlayer = Gamemanager.instance.CurrentPlayer;
            StartCoroutine(AiCalculation());

        }
    }

    public IEnumerator AiCalculation()
    {
        if ((TurnPlayer.score >= 9) || (TurnPlayer.ForcedAi))
        {
            Gamemanager.instance.smartAIPhase = true;

            for (int row = 0; row < Gamemanager.instance.B_Size; row++)
            {
                for (int col = 0; col < Gamemanager.instance.B_Size; col++)
                {
                    Tiles TheTile = Gamemanager.instance.BoardTiles[row, col];

                    if (TheTile.CheckBoardPartitions())
                    {

                        if (TheTile.alreadyInstantiated)
                        {
                            if (TurnPlayer.mySwappies.Contains(TheTile.ThisSwappy))
                            {
                                Gamemanager.instance.FillAdjacentTiles(TheTile);
                                Gamemanager.instance.FillSwapableTiles();

                                DragAndDrop SwappyToSwapFrom = null;
                                DragAndDrop SwapableSwappy = null;

                                for (int i = 0; i < Gamemanager.instance.SwapableSwapies.Count; i++)
                                {
                                    AiScoreCount = 0;


                                    SwappyToSwapFrom = TheTile.ThisSwappy;
                                    SwapableSwappy = Gamemanager.instance.SwapableSwapies[i];


                                    Gamemanager.instance.SwapTheSwapies(SwappyToSwapFrom, SwapableSwappy);

                                    //yield return new WaitForSeconds(0.1f);

                                    Gamemanager.instance.CheckCombos();

                                    yield return new WaitForEndOfFrame();

                                    Gamemanager.instance.SwapTheSwapies(SwappyToSwapFrom, SwapableSwappy);
                                    //yield return new WaitForSeconds(0.1f);

                                    if (AiScoreCount == 0 && Gamemanager.instance.PossiblePositions() == 0)
                                    {
                                        AiScoreCount = 1;
                                    }
                                    ChooseMove(SwappyToSwapFrom, SwapableSwappy, null, AiScoreCount, true);

                                }
                            }
                        }
                    }
                }
            }
            for (int row = 0; row < Gamemanager.instance.B_Size; row++)
            {
                for (int col = 0; col < Gamemanager.instance.B_Size; col++)
                {
                    Tiles TheTile = Gamemanager.instance.BoardTiles[row, col];

                    if (TheTile.CheckBoardPartitions())
                    {
                        if (!TheTile.alreadyInstantiated)
                        {
                            TheTile.SpawnSwappy();

                            //yield return new WaitForSeconds(0.1f);

                            Gamemanager.instance.CheckCombos();

                            yield return new WaitForEndOfFrame();

                            TheTile.DeSpawnSwappy();
                            //yield return new WaitForSeconds(0.1f);

                            if (AiScoreCount == 0)
                            {
                                AiScoreCount = 1;
                            }

                            ChooseMove(null, null, TheTile, AiScoreCount, false);

                        }
                    }
                }
            }

            yield return new WaitForEndOfFrame();

            DoAiAction();
        }
        else
        {
            Gamemanager.instance.smartAIPhase = false;

            if (TurnPlayer.score == 0)
            {
                Tiles TheTile = FinalTile();
                print(TheTile);

                TheTile.SpawnSwappy();
                yield return new WaitForSeconds(1f);
                Gamemanager.instance.NextTurn();



            }
            else
            {
                if (Gamemanager.instance.FirstAIShapes(TurnPlayer.FirstSwappyPlaced, true))
                {
                    Gamemanager.instance.GetEmptyAdjacent();
                    if (Gamemanager.instance.EmptyAdjacentTiles.Count > 0)
                    {
                        int ShapeRand = Random.Range(0, Gamemanager.instance.EmptyAdjacentTiles.Count);
                        Gamemanager.instance.EmptyAdjacentTiles[ShapeRand].SpawnSwappy();
                        Gamemanager.instance.CheckCombos();
                    }
                    else
                    {
                        ChangeFirst();
                    }
                }
                else if (Gamemanager.instance.FirstAIShapes(TurnPlayer.FirstSwappyPlaced, false))
                {
                    Gamemanager.instance.GetEmptyAdjacent();
                    if (Gamemanager.instance.EmptyAdjacentTiles.Count > 0)
                    {
                        int ShapeRand = Random.Range(0, Gamemanager.instance.EmptyAdjacentTiles.Count);
                        Gamemanager.instance.EmptyAdjacentTiles[ShapeRand].SpawnSwappy();
                        Gamemanager.instance.CheckCombos();
                    }
                    else
                    {
                        ChangeFirst();
                    }
                }
                else
                {
                    ChangeFirst();
                }
            }
        }
    }
    public Tiles TheTile;
    public Tiles FinalTile()
    {
        int RandRow = Random.Range(0, Gamemanager.instance.B_Size);
        int RandCol = Random.Range(0, Gamemanager.instance.B_Size);

        TheTile = Gamemanager.instance.BoardTiles[RandRow, RandCol];

        if (TheTile.CheckBoardPartitions())
        {
            if (CheckPartitionSides(TheTile))
            {
                return TheTile;
            }
            else
            {
                return FinalTile();
            }

        }
        else
        {
            return FinalTile();
        }
    }

    bool CheckPartitionSides(Tiles _tile)
    {
        Gamemanager.instance.FillAdjacentTiles(_tile);

        List<Tiles> TempTiles = new List<Tiles>();

        if (Gamemanager.instance.topLeftBox.Contains(TheTile))
            Gamemanager.instance.CurrentPlayer._BoardPartition = BoardPartition.Tleft;
        else if (Gamemanager.instance.topRightBox.Contains(TheTile))
            Gamemanager.instance.CurrentPlayer._BoardPartition = BoardPartition.TRight;
        else if (Gamemanager.instance.bottomLeftBox.Contains(TheTile))
            Gamemanager.instance.CurrentPlayer._BoardPartition = BoardPartition.BLeft;
        else if (Gamemanager.instance.bottomRightBox.Contains(TheTile))
            Gamemanager.instance.CurrentPlayer._BoardPartition = BoardPartition.BRight;


        switch (Gamemanager.instance.CurrentPlayer._BoardPartition)
        {
            case BoardPartition.Tleft:
                foreach (Tiles tile in Gamemanager.instance.topLeftBox) 
                {
                    TempTiles.Add(tile);
                }

                break;

            case BoardPartition.TRight:
                foreach (Tiles tile in Gamemanager.instance.topRightBox)
                {
                    TempTiles.Add(tile);
                }

                break;

            case BoardPartition.BLeft:
                foreach (Tiles tile in Gamemanager.instance.bottomLeftBox)
                {
                    TempTiles.Add(tile);
                }

                break;

            case BoardPartition.BRight:
                foreach (Tiles tile in Gamemanager.instance.bottomRightBox)
                {
                    TempTiles.Add(tile);
                }

                break;
        }

        if (Gamemanager.instance.AdjacentTiles.Count == 9)
        {

            foreach (var tile in Gamemanager.instance.AdjacentTiles)
            {
                if (!TempTiles.Contains(tile))
                {
                    return false;
                }
            }
            return true;

        }

        return false;
    }
    void ChangeFirst()
    {
        n = TurnPlayer.mySwappies.IndexOf(TurnPlayer.FirstSwappyPlaced.ThisSwappy);
        n++;
        if (n == TurnPlayer.mySwappies.Count)
        {
            n = 0;
            TurnPlayer.ForcedAi = true;
        }
        else
        {
            TurnPlayer.FirstSwappyPlaced = TurnPlayer.mySwappies[n].MyTile;
        }
        StartCoroutine(AiCalculation());
    }

    void EmptyMove(Moves move)
    {
        move.Empty = true;
        move.Points = 0;
        move.Swapping = false;
        move.TileToSpawn = null;
        move.SwappyToSwapFrom = null;
        move.SwappyToSwapWith = null;
    }

    void ChooseMove(DragAndDrop SwappyA, DragAndDrop SwappyB, Tiles TileA, int point, bool swap)
    {
        if (point > LastComboCounter)
        {
            LastComboCounter = point;

            Moves CurrentMove = new Moves();

            CurrentMove.Empty = false;
            CurrentMove.Points = point;
            CurrentMove.Swapping = swap;
            CurrentMove.TileToSpawn = TileA;
            CurrentMove.SwappyToSwapFrom = SwappyA;
            CurrentMove.SwappyToSwapWith = SwappyB;

            for (int i = 0; i < AIMoves.Count; i++)
            {
                if (AIMoves[i].Empty)
                {
                    AIMoves[i] = CurrentMove;
                }
            }
            AIMoves[2] = AIMoves[1];
            AIMoves[1] = AIMoves[0];
            AIMoves[0] = CurrentMove;
        }
    }

    void DoAiAction()
    {
        Moves ToDoMove;
        Gamemanager.instance.smartAIPhase = false;
        if (TurnPlayer.mySwappies.Count <= 12)
        {
            ToDoMove = AIMoves[0];
        }
        else
        {
            ToDoMove = AIMoves[(int)TurnPlayer.PlayerDifficulty];
        }

        if (ToDoMove.Swapping)
        {
            if (ToDoMove.SwappyToSwapFrom != null && ToDoMove.SwappyToSwapWith != null)
                Gamemanager.instance.SwapTheSwapies(ToDoMove.SwappyToSwapFrom, ToDoMove.SwappyToSwapWith);
            else
                DoMoveException();
        }
        else
        {
            ToDoMove.TileToSpawn.SpawnSwappy();
        }

        Gamemanager.instance.CheckCombos();

    }
    public void DoMoveException()
    {
        Gamemanager.instance.FillAdjacentTiles(TurnPlayer.mySwappies[0].MyTile);
        Gamemanager.instance.FillSwapableTiles();
        Gamemanager.instance.SwapTheSwapies(TurnPlayer.mySwappies[0], Gamemanager.instance.SwapableSwapies[0]);
    }

}

[System.Serializable]
public class Moves
{
    public bool Empty = true;
    public int Points = 0;
    public bool Swapping = false;
    public Tiles TileToSpawn;
    public DragAndDrop SwappyToSwapFrom;
    public DragAndDrop SwappyToSwapWith;
}