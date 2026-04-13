using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tiles : MonoBehaviour
{
    public int rowNum;
    public int colNum;
    public bool alreadyInstantiated = false;

    public DragAndDrop ThisSwappy;
    
    private void OnMouseDown()
    {
        if (Gamemanager.instance.SettingOpened)
            return;

        if (!Gamemanager.instance.IntrectAble)
            return;

        if (!CheckBoardPartitions())
            return;


        StartCoroutine(DoMove());
        
    }

    public bool CheckBoardPartitions()
    {
        if (!Gamemanager.instance.CheckFourMoves)
        {
            Tiles _Tile = Gamemanager.instance.BoardTiles[rowNum, colNum];

            if (Gamemanager.instance.topLeftBox.Contains(_Tile))
            {
                if (Gamemanager.instance.CurrentPlayer.PartitionAssigned)
                {
                    if (Gamemanager.instance.CurrentPlayer._BoardPartition == BoardPartition.Tleft || Gamemanager.instance.PartitionAssigned[0] == false)
                    {
                        print("yes i want this");
                        return true;
                       
                    }
                    else
                    {
                        print("yes i want this");
                        return false;
                    }
                }
                else
                {
                    if (Gamemanager.instance.PartitionAssigned[0] == true)
                    {
                        return false;
                    }
                    else { return true; }

                }
            }
            else if (Gamemanager.instance.topRightBox.Contains(_Tile))
            {
                if (Gamemanager.instance.CurrentPlayer.PartitionAssigned)
                {
                    if (Gamemanager.instance.CurrentPlayer._BoardPartition == BoardPartition.TRight || Gamemanager.instance.PartitionAssigned[1] == false)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (Gamemanager.instance.PartitionAssigned[1] == true)
                    {
                        return false;
                    }
                    else { return true; }

                }
            }
            else if (Gamemanager.instance.bottomLeftBox.Contains(_Tile))
            {
                if (Gamemanager.instance.CurrentPlayer.PartitionAssigned)
                {
                    if (Gamemanager.instance.CurrentPlayer._BoardPartition == BoardPartition.BLeft || Gamemanager.instance.PartitionAssigned[2] == false)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (Gamemanager.instance.PartitionAssigned[2] == true)
                    {
                        return false;
                    }
                    else { return true; }

                }
            }
            else if (Gamemanager.instance.bottomRightBox.Contains(_Tile))
            {
                if (Gamemanager.instance.CurrentPlayer.PartitionAssigned)
                {
                    if (Gamemanager.instance.CurrentPlayer._BoardPartition == BoardPartition.BRight || Gamemanager.instance.PartitionAssigned[3] == false)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (Gamemanager.instance.PartitionAssigned[3] == true)
                    {
                        return false;
                    }
                    else { return true; }

                }
            }
            else if (Gamemanager.instance.centeredColumnTiles.Contains(_Tile))
            {
                return false;
            }
            else if (Gamemanager.instance.centeredRowTiles.Contains(_Tile))
            {
                return false;
            }
            return false;
        }
        else
        {
            return true;
        }
      
    }
    IEnumerator DoMove()
    {
        if (!alreadyInstantiated)
        {
            if (Gamemanager.instance.CurrentPlayer.AiSwappy == false)
                GameConfigration.instance.PlayerSound(2);

            SpawnSwappy();
            Gamemanager.instance.pendingPlacementComboHighlight = true;
            Gamemanager.instance.CheckCombos();

            Gamemanager.instance.IntrectAble = false;
        }
        yield return new WaitForSeconds(0.5f);
    }
    public void SpawnSwappy()
    {
        StartCoroutine(SpawnASwapy());
    }

    public void DeSpawnSwappy()
    {
        StartCoroutine(DeSpawnASwapy());
    }

    IEnumerator SpawnASwapy()
    {
        if (!Gamemanager.instance.smartAIPhase)
            GameConfigration.instance.PlayerSound(2);


        if (Gamemanager.instance.SelectedSwappy != null)
        {
            Gamemanager.instance.SelectedSwappy.transform.localPosition = new Vector3(0, 0, 1);
            Gamemanager.instance.SelectedSwappy = null;
        }

        alreadyInstantiated = true;

        if (Gamemanager.instance.CurrentPlayer.score == 0)
        {
            Gamemanager.instance.CurrentPlayer.FirstSwappyPlaced = this;
        }

        

        DragAndDrop swappy = Instantiate(Gamemanager.instance.CurrentPlayer.swappyPrefab, transform);
        swappy.transform.localPosition += new Vector3(0, 0, 1);
        ThisSwappy = swappy;
        swappy.MyTile = this;
        Gamemanager.instance.CurrentPlayer.mySwappies.Add(swappy);

        Gamemanager.instance.currentSwappyTile = this;

        if (!Gamemanager.instance.smartAIPhase)
        {
            Tween mytweenint = swappy.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);
            yield return mytweenint.WaitForCompletion();
        }
    }
    IEnumerator DeSpawnASwapy()
    {
        if (!Gamemanager.instance.smartAIPhase)
        {
            Tween mytween = ThisSwappy.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.09f);
            yield return mytween.WaitForCompletion();
        }

        Gamemanager.instance.currentSwappyTile = null;

        SwappyPlayer OtherPlayer = Gamemanager.instance._PlayersList.Find(a => a.mySwappies.Contains(ThisSwappy));

        if (ThisSwappy != null)
        {
            OtherPlayer.mySwappies.Remove(ThisSwappy);
            Destroy(ThisSwappy.gameObject);
        }


        alreadyInstantiated = false;
        ThisSwappy = null;
    }
}
