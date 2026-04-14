using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public int playerStartingIndex = 0;

    public bool smartAIPhase = false;

    public int B_Size = 11;

    public bool SettingOpened = false;

   

    [SerializeField]
    Tiles tilePrefab;

    GameObject Board;
     
    public int moves = 0;
    public bool L1 = false;
    public bool L2 = false;
    public bool L3 = false;
    public bool L4 = false;

    public List<GameObject> SwappingLineShape;

    public GameObject TimerPrefab;
    public GameObject timerView;
    public GameObject PopUpPrefab;

    public int TotalCombos = 0;

    public List<GameObject> ComboShapes;

    [Header("Placement combo highlight")]
    [Tooltip("Yellow ring shown on the tile where you placed a token when that placement completes a combo. Import your JPG as Sprite (2D).")]
    public Sprite placementComboYellowSprite;

    [Tooltip("How long the yellow overlay stays visible (seconds).")]
    public float placementComboYellowDuration = 0.9f;

    [Tooltip("Local Z in front of the placed token (token is at z≈1).")]
    public float placementComboYellowLocalZ = 1.15f;

    [Tooltip("Uniform scale for the yellow sprite (1 = fit sprite bounds to token size).")]
    public float placementComboYellowScale = 1f;

    [Header("Combo spawn highlight (green)")]
    [Tooltip("Green ring on each tile where a new token is created by the combo. Import your green JPG as Sprite (2D).")]
    public Sprite comboSpawnGreenSprite;

    [Tooltip("How long each green overlay stays visible (seconds).")]
    public float comboSpawnGreenDuration = 0.9f;

    [Tooltip("Local Z in front of the new token (token spawns at z≈1).")]
    public float comboSpawnGreenLocalZ = 1.18f;

    [Tooltip("Uniform scale for the green sprite.")]
    public float comboSpawnGreenScale = 1f;

    [Tooltip("Sorting order for green overlays (above yellow if same tile).")]
    public int comboSpawnGreenSortingOrder = 51;

    /// <summary>Set true only when <see cref="CheckCombos"/> is triggered from empty-tile placement (<see cref="Tiles.DoMove"/>).</summary>
    public bool pendingPlacementComboHighlight;

    /// <summary>Set when <see cref="CheckCombos"/> runs after swapping your swappy with an opponent's; <see cref="swapHighlightPartnerTile"/> is your tile before the swap (the other tile is <see cref="currentSwappyTile"/> after <see cref="SwapTheSwapies"/>).</summary>
    bool pendingSwapWithOpponentHighlight;
    Tiles swapHighlightPartnerTile;

    public Tiles[,] BoardTiles = new Tiles[11, 11];

    public List<Tiles> TilesToSpawn = new List<Tiles>();
    public List<Tiles> TilesToDeSpawn = new List<Tiles>();

    public List<Tiles> EmptyAdjacentTiles = new List<Tiles>();
    public List<Tiles> AdjacentTiles = new List<Tiles>();
    public List<DragAndDrop> SwapableSwapies = new List<DragAndDrop>();

    public List<SwappyPlayer> _PlayersList = new List<SwappyPlayer>();
    public SwappyPlayer CurrentPlayer = null;
    public DragAndDrop SelectedSwappy;
    public int CurrentPlayerIndex = 0;
    public Tiles currentSwappyTile;

    public int MovesCount = 0;

    public int CurrentComboCount = 0;
    public int TotalDead;

    public bool IntrectAble;

    bool ComboChecker;

    public int maxComboSwappies;

    public bool GameOver = false;

    public Sprite GoldMedal;
    public Sprite SilverMedal;
    public Sprite BronzeMedal;

    public List<Tiles> topLeftBox = new List<Tiles>();
    public List<Tiles> topRightBox = new List<Tiles>();
    public List<Tiles> bottomLeftBox = new List<Tiles>();
    public List<Tiles> bottomRightBox = new List<Tiles>();

    public List<Tiles> centeredColumnTiles = new List<Tiles>();  
    public List<Tiles> centeredRowTiles = new List<Tiles>();     

    public List<GameObject> Boxcolors = new List<GameObject>();


    public int TotalLeft;


    public List<bool> PartitionAssigned;
    private void Awake()
    {
        instance = this;

        _PlayersList = GameConfigration.instance._SwappyPlayer;

        SetPlayers();
    }
    private void Start()
    {
        Time.timeScale = 1;
        GameConfigration.instance.BGSoundPlayer(0);
        CreateBoard();
        Time.timeScale = 0;
        PreGamePanel.ShowUI();
        StartGame();
    }

    void CreateBoard()
    {
        Board = new GameObject("BoardTiles");
        BoardTiles = new Tiles[B_Size, B_Size];

        int middleRow = B_Size / 2;
        int middleCol = B_Size / 2;

        for (int row = 0; row < B_Size; row++)
        {
            for (int col = 0; col < B_Size; col++)
            {
                Tiles ThisTile = Instantiate(tilePrefab);

                ThisTile.transform.position = new Vector3(-col, -row, 0);
                ThisTile.name = "Tile_" + row + "_" + col;
                ThisTile.rowNum = row;
                ThisTile.colNum = col;
                ThisTile.transform.parent = Board.transform;
                BoardTiles[row, col] = ThisTile;

                // Add tiles to the centered row list
                if (row == middleRow)
                {
                    centeredRowTiles.Add(ThisTile);
                    ThisTile.GetComponent<Renderer>().material.color = Color.gray;
                }

                // Add tiles to the centered column list
                if (col == middleCol)
                {
                    centeredColumnTiles.Add(ThisTile);
                    ThisTile.GetComponent<Renderer>().material.color = Color.gray;
                }

                // Determine which box the tile belongs to and color it
                if (row < middleRow && col < middleCol) // Top-left box
                {
                    ThisTile.GetComponent<Renderer>().material.color = Color.red;
                    topLeftBox.Add(ThisTile);
                }
                else if (row < middleRow && col > middleCol) // Top-right box
                {
                    ThisTile.GetComponent<Renderer>().material.color = Color.blue;
                    topRightBox.Add(ThisTile);
                }
                else if (row > middleRow && col < middleCol) // Bottom-left box
                {
                    ThisTile.GetComponent<Renderer>().material.color = Color.green;
                    bottomLeftBox.Add(ThisTile);
                }
                else if (row > middleRow && col > middleCol) // Bottom-right box
                {
                    ThisTile.GetComponent<Renderer>().material.color = Color.yellow;
                    bottomRightBox.Add(ThisTile);
                }
            }
        }
    }
    void StartGame()
    {
        CurrentPlayerIndex = 0;
        CurrentPlayer = _PlayersList[CurrentPlayerIndex];
        if (CurrentPlayer.AiSwappy)
        {
            IntrectAble = false;
            SmartAIManager.instance.SmartAI();
        }
        else
        {
            IntrectAble = true;
        }
    }

    public void SetPlayers()
    {
        List<int> SwapiesIndexes = new List<int>();
        for (int i = 0; i < GameConfigration.instance.Swappies.Count; i++)
        {
            SwapiesIndexes.Add(i);
        }
        SwapiesIndexes.Remove(GameConfigration.instance.currentSwappyIndex);

        for (int i = 0; i < _PlayersList.Count; i++)
        {
            _PlayersList[i].swappyPrefab = GameConfigration.instance.AvailableSwappies[i];
            if (_PlayersList[i].AiSwappy)
            {
                int R = Random.Range(0, SwapiesIndexes.Count);
                _PlayersList[i].swappyPrefab.SwappyMesh.sharedMaterial.mainTexture = GameConfigration.instance.Swappies[SwapiesIndexes[R]].swapieDisplay;
                _PlayersList[i].SwapieDisplay = GameConfigration.instance.Swappies[SwapiesIndexes[R]];
                SwapiesIndexes.RemoveAt(R);
            }
            else
            {
                _PlayersList[i].swappyPrefab.SwappyMesh.sharedMaterial.mainTexture = GameConfigration.instance.Swappies[GameConfigration.instance.currentSwappyIndex].swapieDisplay;
            }
        }
        SwapiesIndexes.Clear();

        Shuffle(_PlayersList);

    }

    void Shuffle(List<SwappyPlayer> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            SwappyPlayer value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void SwapTheSwapies(DragAndDrop PrevSwapy, DragAndDrop swapyToSwap)
    {
        if (!smartAIPhase)
            ShowSwappingShape(PrevSwapy, swapyToSwap);
        Tiles temp = swapyToSwap.MyTile;

        currentSwappyTile = swapyToSwap.MyTile;

        swapyToSwap.MyTile = PrevSwapy.MyTile;
        swapyToSwap.MyTile.ThisSwappy = swapyToSwap;

        PrevSwapy.MyTile = temp;
        temp.ThisSwappy = PrevSwapy;

        PrevSwapy.transform.SetParent(PrevSwapy.MyTile.transform);
        PrevSwapy.transform.localPosition = new Vector3(0, 0, 1);

        swapyToSwap.transform.SetParent(swapyToSwap.MyTile.transform);
        swapyToSwap.transform.localPosition = new Vector3(0, 0, 1);

    }

    public void SelectNSwap(DragAndDrop slctdSwappy)
    {
        if(!IntrectAble)
        {
            return;
        }

        if (CurrentPlayer.mySwappies.Contains(slctdSwappy))
        {
            if (CurrentPlayer.mySwappies.Contains(slctdSwappy))
            {
                if(SelectedSwappy!=null)
                {
                    SelectedSwappy.transform.localPosition = new Vector3(0, 0, 1);
                }
                SelectedSwappy = slctdSwappy;

                slctdSwappy.transform.localPosition = new Vector3(0, 0, 2);

                currentSwappyTile = slctdSwappy.MyTile;

                FillAdjacentTiles(currentSwappyTile);
                FillSwapableTiles();
            }
        }
        else
        {
            if (SelectedSwappy == null)
            {
                return;
            }
            else
            {
                if (SwapableSwapies.Contains(slctdSwappy))
                {
                    GameConfigration.instance.PlayerSound(2);

                    pendingPlacementComboHighlight = false;
                    swapHighlightPartnerTile = SelectedSwappy.MyTile;
                    pendingSwapWithOpponentHighlight = true;
                    SwapTheSwapies(SelectedSwappy, slctdSwappy);

                    CheckCombos();

                    SelectedSwappy = null;
                }
                else
                {
                    GameConfigration.instance.PlayerSound(27);
                }
            }
            
        }
    }

    public void ShowSwappingShape(DragAndDrop PrevSwapy, DragAndDrop swapyToSwap)
    {
        Tiles TempTile = swapyToSwap.MyTile;
        Tiles MyTile = PrevSwapy.MyTile;

        int range = 1;
        int b = 0;
        int selectedSwappyRow = MyTile.rowNum;
        int selectedSwappyCol = MyTile.colNum;

        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                int newRow = selectedSwappyRow + i;
                int newCol = selectedSwappyCol + j;
                Tiles Tempar = new Tiles();
                if (newRow >= 0 && newRow < B_Size && newCol >= 0 && newCol < B_Size)
                {
                    Tempar = BoardTiles[newRow, newCol];
                    if (Tempar  == TempTile)
                    {
                        ShowShape(SwappingLineShape[b], MyTile);
                    }
                }
                if (Tempar != MyTile)
                    b++;
            }
        }
    }

    public void NextTurn()
    {
        if (PopUpTimer.instance != null)
        {
            PopUpTimer.instance.DestroyObj();
        }


        SetScore();
        AssignBoardPartition();

        CheckFourMoves = CheckFour();

        MovesCount++;

        if (MovesCount >= _PlayersList.Count && CurrentPlayer.FirstSwappyPlaced == null)
        {
            if (CurrentPlayer.AiSwappy)
                CurrentPlayer.FirstSwappyPlaced = CurrentPlayer.mySwappies[0].MyTile;
        }
        CurrentPlayerIndex++;

        CurrentPlayer.mySwappies.RemoveAll(a => a == null);

        if (!GameOver)
        {
            if (CurrentPlayerIndex == _PlayersList.Count)
            {
                CurrentPlayerIndex = 0;
            }
            CurrentPlayer = _PlayersList[CurrentPlayerIndex];

        }



        if (CurrentPlayer.AiSwappy)
        {
            if (CurrentPlayer.Dead)
            {
                NextTurn();
                return;
            }
            IntrectAble = false;
            if (!GameOver)
                SmartAIManager.instance.SmartAI();
        }
        else
        {
            IntrectAble = true;
        }
        if (timerView != null)
        {
            Destroy(timerView);
        }

        if (!GameOver)
            Instantiate(TimerPrefab, GamePlayCanvas.instance.Displays[CurrentPlayerIndex].TimerPos);

        CheckforColors();

    }

    public void AssignBoardPartition()
    {
        if (!CurrentPlayer.PartitionAssigned)
        {
            if (PartitionAssigned[0] == false)
            {

                foreach (var Tile in topLeftBox)
                {
                    if (Tile.alreadyInstantiated == true)
                    {
                        CurrentPlayer.PartitionAssigned = true;
                        CurrentPlayer._BoardPartition = BoardPartition.Tleft;
                        PartitionAssigned[0] = true;
                        GameConfigration.instance.PlayerSound(33);
                    }
                }

            }
            if (PartitionAssigned[1] == false)
            {


                foreach (var Tile in topRightBox)
                {
                    if (Tile.alreadyInstantiated == true)
                    {
                        CurrentPlayer.PartitionAssigned = true;
                        CurrentPlayer._BoardPartition = BoardPartition.TRight;
                        PartitionAssigned[1] = true;
                        GameConfigration.instance.PlayerSound(33);
                    }
                }
            }
            if (PartitionAssigned[2] == false)
            {
                foreach (var Tile in bottomLeftBox)
                {
                    if (Tile.alreadyInstantiated == true)
                    {
                        CurrentPlayer.PartitionAssigned = true;
                        CurrentPlayer._BoardPartition = BoardPartition.BLeft;
                        PartitionAssigned[2] = true;
                        GameConfigration.instance.PlayerSound(33);
                    }
                }

            }
            if (PartitionAssigned[3] == false)
            {
                foreach (var Tile in bottomRightBox)
                {
                    if (Tile.alreadyInstantiated == true)
                    {
                        CurrentPlayer.PartitionAssigned = true;
                        CurrentPlayer._BoardPartition = BoardPartition.BRight;
                        PartitionAssigned[3] = true;
                        GameConfigration.instance.PlayerSound(33);
                    }
                }

            }
        }
    }

    public void SetScore()
    {

        int MoveScore = 0;

        for (int i = 0; i < _PlayersList.Count; i++)
        {
            MoveScore = _PlayersList[i].mySwappies.Count - _PlayersList[i].score;

            if ((MoveScore < 0 || MoveScore > 1) && !_PlayersList[i].AiSwappy)
            {
                PlayerScorePopUp.ShowUI();
                PlayerScorePopUp.instance.ShowScore(MoveScore);

                //Give trophies

                if (CurrentComboCount > 1)
                {
                    GameConfigration.instance.PlayerSound(28);
                    
                    //show combo effect
                }
            }

            _PlayersList[i].score = _PlayersList[i].mySwappies.Count;


            if (MovesCount >= _PlayersList.Count)
            {
                if (((_PlayersList[i].score + PossiblePositions()) < 5) || _PlayersList[i].score == 0)
                {
                    if (!_PlayersList[i].Dead)
                    {
                        _PlayersList[i].Position = _PlayersList.Count - _PlayersList.Count(a => a.Dead);
                        _PlayersList[i].Dead = true;
                        TotalDead++;

                        
                        if ((TotalDead == 1) && (!_PlayersList[i].AiSwappy))
                        {
                            GameOver = true;
                            StartCoroutine(YouLoseSequence());
                            playerStartingIndex = _PlayersList.IndexOf(_PlayersList.Find(a => !a.AiSwappy));

                            CurrentPlayer = _PlayersList[playerStartingIndex];
                        }
                        else if ((TotalDead == _PlayersList.Count - 1) || (!_PlayersList[i].AiSwappy))
                        {
                            playerStartingIndex = _PlayersList.IndexOf(_PlayersList.Find(a => !a.AiSwappy));

                            CurrentPlayer = _PlayersList[playerStartingIndex];


                            GameOver = true;
                            //call gameover panel
                            if (_PlayersList[playerStartingIndex].Dead == false)
                            {
                                StartCoroutine(YouWinSequence());
                            }
                            else
                            {
                                StartCoroutine(GameOverSequence());
                            }
                        }
                    }
                }
            }
            GamePlayCanvas.instance.UpdatePlayerScore();
        }
    }
    IEnumerator GameOverSequence()
    {
        GameConfigration.instance.PlayerSound(30);
        PopupScript.ShowUI("");
        PopupScript.instance._PopImage.gameObject.SetActive(false);
        PopupScript.instance.ExtraInfoPopUp.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        PopupScript.instance.GameOver.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        PointsCalculator.ShowUI();
    }
    IEnumerator YouWinSequence()
    {
        GameConfigration.instance.PlayerSound(7);
        PopupScript.ShowUI("");
        PopupScript.instance._PopImage.gameObject.SetActive(false);
        PopupScript.instance.ExtraInfoPopUp.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        PopupScript.instance.YouWin.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        PointsCalculator.ShowUI();
    }
    IEnumerator YouLoseSequence()
    {
        GameConfigration.instance.PlayerSound(6);
        PopupScript.ShowUI("");
        PopupScript.instance._PopImage.gameObject.SetActive(false);
        PopupScript.instance.ExtraInfoPopUp.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        PopupScript.instance.YouLose.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        PointsCalculator.ShowUI();
    }
    public void CheckCombos()
    {
        if (ComboChecker)
        {
            print("mil gya");
            pendingPlacementComboHighlight = false;
            pendingSwapWithOpponentHighlight = false;
            swapHighlightPartnerTile = null;
            return;
        }
        ComboChecker = true;
        StartCoroutine(CheckShapes());
    }

    IEnumerator CheckShapes()
    {
        TilesToSpawn.Clear();
        TilesToDeSpawn.Clear();

        L1 = false; L2=false; L3 = false; L4 = false;

        int comboCountBefore = TotalCombos;

        CheckHorizontalShape(currentSwappyTile.rowNum, currentSwappyTile.colNum, 2);
        CheckVerticalShapes(currentSwappyTile.rowNum, currentSwappyTile.colNum, 2);
        CheckDiagnol1Shapes(currentSwappyTile.rowNum, currentSwappyTile.colNum, 2);
        CheckDiagnol2Shapes(currentSwappyTile.rowNum, currentSwappyTile.colNum, 2);
        CheckCrossShapes(currentSwappyTile.rowNum, currentSwappyTile.colNum, 1);
        CheckPlusShapes(currentSwappyTile.rowNum, currentSwappyTile.colNum, 1);
        CheckLShapeShapes(currentSwappyTile.rowNum, currentSwappyTile.colNum, 2);

        bool comboCompleted = TotalCombos > comboCountBefore;
        if (!smartAIPhase && placementComboYellowSprite != null)
        {
            if (pendingSwapWithOpponentHighlight && swapHighlightPartnerTile != null && currentSwappyTile != null)
            {
                ShowPlacementComboYellowOverlay(swapHighlightPartnerTile);
                ShowPlacementComboYellowOverlay(currentSwappyTile);
            }
            else if (pendingPlacementComboHighlight && comboCompleted && currentSwappyTile != null)
                ShowPlacementComboYellowOverlay(currentSwappyTile);
        }
        pendingPlacementComboHighlight = false;
        pendingSwapWithOpponentHighlight = false;
        swapHighlightPartnerTile = null;

        yield return new WaitForEndOfFrame();

        if (!smartAIPhase)
        {
            for (int i = 0; i < TilesToDeSpawn.Count; i++)
            {
                TilesToDeSpawn[i].DeSpawnSwappy();
            }

            yield return new WaitForSeconds(0.1f);
            yield return new WaitForEndOfFrame();

            if (comboSpawnGreenSprite != null && TilesToSpawn.Count > 0)
            {
                for (int g = 0; g < TilesToSpawn.Count; g++)
                    ShowComboSpawnGreenOverlay(TilesToSpawn[g]);
            }

            for (int i = 0; i < TilesToSpawn.Count; i++)
            {
                TilesToSpawn[i].SpawnSwappy();
            }

            yield return new WaitForSeconds(0.1f);
            yield return new WaitForEndOfFrame();


            yield return new WaitForSeconds(0.5f);
            Invoke(nameof(NextTurn), 0.5f);
        }
        else
        {
            SmartAIManager.instance.AiScoreCount = TilesToSpawn.Count;
        }

        ComboChecker = false;
    }


    bool IsAvailableAndMineSwappy(int row, int col)
    {
        if (row >= 0 && row < B_Size)
        {
            if (col >= 0 && col < B_Size)
            {
                if (BoardTiles[row, col].alreadyInstantiated)
                {
                    if (CurrentPlayer.mySwappies.Contains(BoardTiles[row, col].ThisSwappy))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    //Making Shapes
    void ShowShape(GameObject Shape, Tiles Pos)
    {
        Destroy(Instantiate(Shape, Pos.transform.position + new Vector3(0, 0, 3), Pos.transform.rotation), 0.8f);
    }

    void ShowPlacementComboYellowOverlay(Tiles tile)
    {
        GameObject go = new GameObject("PlacementComboYellow");
        go.transform.SetParent(tile.transform, false);
        go.transform.localPosition = new Vector3(0f, 0f, placementComboYellowLocalZ);
        go.transform.localRotation = Quaternion.identity;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = placementComboYellowSprite;
        sr.sortingOrder = 50;

        if (placementComboYellowSprite.bounds.size.x > 0.0001f)
            go.transform.localScale = Vector3.one * (placementComboYellowScale / placementComboYellowSprite.bounds.size.x);

        Destroy(go, placementComboYellowDuration);
    }

    void ShowComboSpawnGreenOverlay(Tiles tile)
    {
        if (tile == null || comboSpawnGreenSprite == null)
            return;

        GameObject go = new GameObject("ComboSpawnGreen");
        go.transform.SetParent(tile.transform, false);
        go.transform.localPosition = new Vector3(0f, 0f, comboSpawnGreenLocalZ);
        go.transform.localRotation = Quaternion.identity;

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = comboSpawnGreenSprite;
        sr.sortingOrder = comboSpawnGreenSortingOrder;

        if (comboSpawnGreenSprite.bounds.size.x > 0.0001f)
            go.transform.localScale = Vector3.one * (comboSpawnGreenScale / comboSpawnGreenSprite.bounds.size.x);

        Destroy(go, comboSpawnGreenDuration);
    }

    public void CheckHorizontalShape(int row, int col, int depth)
    {

        Tiles _tileTemp;
        if (!IsAvailableAndMineSwappy(row, col))
            return;
        int min = col - 2, max = col + 2;
        int totalSwappies = 0;
        bool hasIncludedOriginal = false;

        for (int i = min; i <= max; i++)
        {
            if (IsAvailableAndMineSwappy(row, i))
            {
                totalSwappies++;
                if (row == currentSwappyTile.rowNum && i == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
        }

        if (totalSwappies == 5 && hasIncludedOriginal)
        {
            TotalCombos++;
            if (!CurrentPlayer.AiSwappy)
            {
                GameConfigration.instance.PlayerSound(9);
            }
            if (!smartAIPhase)
            {
                Destroy(Instantiate(ComboShapes[0], BoardTiles[row, col].transform.position + new Vector3(0, 0, 3), BoardTiles[row, col].transform.rotation), 0.8f);
            }

             
            for (int i = 0; i < B_Size; i++)
            {
                _tileTemp = BoardTiles[row, i];

                SpawnComboSwappies(_tileTemp);
            }
             
        }
        else if (depth > 0)
        {
            depth--;
            CheckHorizontalShape(row, col + 1, depth);
            CheckHorizontalShape(row, col - 1, depth);
        }
         
    }

    public void CheckVerticalShapes(int row, int col, int depth)
    {
        Tiles _tileTemp;
        if (!IsAvailableAndMineSwappy(row, col))
            return;

        int min = row - 2, max = row + 2;
        int totalSwappies = 0;
        bool hasIncludedOriginal = false;

        for (int i = min; i <= max; i++)
        {
            if (IsAvailableAndMineSwappy(i, col))
            {
                totalSwappies++;
                if (i == currentSwappyTile.rowNum && col == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
        }
        if (totalSwappies == 5 && hasIncludedOriginal)
        {
            if (!smartAIPhase)
            {
                Destroy(Instantiate(ComboShapes[1], BoardTiles[row, col].transform.position + new Vector3(0, 0, 3), BoardTiles[row, col].transform.rotation), 0.8f);
            }
            TotalCombos++;
            if (!CurrentPlayer.AiSwappy)
            {
                GameConfigration.instance.PlayerSound(9);
            }
             
            for (int i = 0; i < B_Size; i++)
            {
                _tileTemp = BoardTiles[i, col];

                SpawnComboSwappies(_tileTemp);
            }
             
        }
        else if (depth > 0)
        {
            depth--;
            CheckVerticalShapes(row + 1, col, depth);
            CheckVerticalShapes(row - 1, col, depth);
        }
    }

    public void CheckDiagnol1Shapes(int row, int col, int depth)
    {
        Tiles _tileTemp;
        if (!IsAvailableAndMineSwappy(row, col))
            return;

        int min = -2, max = 2;
        int totalSwappies = 0;
        bool hasIncludedOriginal = false;

        for (int i = min; i <= max; i++)
        {
            if (IsAvailableAndMineSwappy(row + i, col + i))
            {
                totalSwappies++;
                if (row + i == currentSwappyTile.rowNum && col + i == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
        }

        if (totalSwappies == 5 && hasIncludedOriginal)
        {
            if (!smartAIPhase)
            {
                Destroy(Instantiate(ComboShapes[2], BoardTiles[row, col].transform.position + new Vector3(0, 0, 3), BoardTiles[row, col].transform.rotation), 0.8f);
            }
            TotalCombos++;
            if (!CurrentPlayer.AiSwappy)
            {
                GameConfigration.instance.PlayerSound(9);
            }
             
            for (int i = 0; i < B_Size; i++)
            {
                if (row - i >= 0 && (col - i) >= 0)
                {
                    _tileTemp = BoardTiles[row - i, col - i];

                    SpawnComboSwappies(_tileTemp);
                }
            }

            for (int i = 0; i < B_Size; i++)
            {
                if (row + i < B_Size && (col + i) < B_Size)
                {
                    _tileTemp = BoardTiles[row + i, col + i];

                    SpawnComboSwappies(_tileTemp);

                }
            }
             
        }
        else if (depth > 0)
        {
            depth--;
            CheckDiagnol1Shapes(row - 1, col - 1, depth);
            CheckDiagnol1Shapes(row + 1, col + 1, depth);
        }
    }

    public void CheckDiagnol2Shapes(int row, int col, int depth)
    {
        Tiles _tileTemp;
        if (!IsAvailableAndMineSwappy(row, col))
            return;

        int min = -2, max = 2;
        int totalSwappies = 0;
        bool hasIncludedOriginal = false;

        for (int i = min; i <= max; i++)
        {
            if (IsAvailableAndMineSwappy(row - i, col + i))
            {
                totalSwappies++;
                if (row - i == currentSwappyTile.rowNum && col + i == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
        }

        if (totalSwappies == 5 && hasIncludedOriginal)
        {
            if (!smartAIPhase)
            {
                Destroy(Instantiate(ComboShapes[3], BoardTiles[row, col].transform.position + new Vector3(0, 0, 3), BoardTiles[row, col].transform.rotation), 0.8f);
            }
            TotalCombos++;
            if (!CurrentPlayer.AiSwappy)
            {
                GameConfigration.instance.PlayerSound(9);
            }
             
            for (int i = 0; i < B_Size; i++)
            {
                if (row - i >= 0 && (col + i) < B_Size)
                {
                    _tileTemp = BoardTiles[row - i, col + i];

                    SpawnComboSwappies(_tileTemp);
                }
            }

            for (int i = 0; i < B_Size; i++)
            {
                if (row + i < B_Size && (col - i) >= 0)
                {
                    _tileTemp = BoardTiles[row + i, col - i];

                    SpawnComboSwappies(_tileTemp);
                }
            }
             
        }
        else if (depth > 0)
        {
            depth--;
            CheckDiagnol2Shapes(row - 1, col + 1, depth);
            CheckDiagnol2Shapes(row + 1, col - 1, depth);
        }
    }

    public void CheckCrossShapes(int row, int col, int depth)
    {
        Tiles _tileTemp;
        if (!IsAvailableAndMineSwappy(row, col))
            return;

        int min = -1, max = 1;
        int totalSwappies = 0;
        bool hasIncludedOriginal = false;

        for (int i = min; i <= max; i++)
        {
            if (IsAvailableAndMineSwappy(row - i, col + i))
            {
                totalSwappies++;
                if (row - i == currentSwappyTile.rowNum && col + i == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
            if (IsAvailableAndMineSwappy(row + i, col + i))
            {
                totalSwappies++;
                if (row + i == currentSwappyTile.rowNum && col + i == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
        }
        if (totalSwappies == 6 && hasIncludedOriginal)
        {
            if (!smartAIPhase)
            {
                Destroy(Instantiate(ComboShapes[4], BoardTiles[row, col].transform.position + new Vector3(0, 0, 3), BoardTiles[row, col].transform.rotation), 0.8f);
            }
            TotalCombos++;
            if (!CurrentPlayer.AiSwappy)
            {
                GameConfigration.instance.PlayerSound(8);
            }
             
            if ((row - 1) >= 0)
            {
                _tileTemp = BoardTiles[row - 1, col];

                SpawnComboSwappies(_tileTemp);
            }
            if ((row + 1) < B_Size)
            {
                _tileTemp = BoardTiles[row + 1, col];

                SpawnComboSwappies(_tileTemp);
            }
            if ((col - 1) >= 0)
            {
                _tileTemp = BoardTiles[row, col - 1];

                SpawnComboSwappies(_tileTemp);
            }
            if ((col + 1) < B_Size)
            {
                _tileTemp = BoardTiles[row, col + 1];

                SpawnComboSwappies(_tileTemp);
            }
             
        }
        else if (depth > 0)
        {
            depth--;
            CheckCrossShapes(row - 1, col - 1, depth);
            CheckCrossShapes(row + 1, col + 1, depth);
            CheckCrossShapes(row + 1, col - 1, depth);
            CheckCrossShapes(row - 1, col + 1, depth);
        }
    }

    public void CheckPlusShapes(int row, int col, int depth)
    {
        Tiles _tileTemp;
        if (!IsAvailableAndMineSwappy(row, col))
            return;

        int min = -1, max = 1;
        int totalSwappies = 0;
        bool hasIncludedOriginal = false;

        for (int i = min; i <= max; i++)
        {
            if (IsAvailableAndMineSwappy(row, col + i))
            {
                totalSwappies++;
                if (row == currentSwappyTile.rowNum && col + i == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
            if (IsAvailableAndMineSwappy(row + i, col))
            {
                totalSwappies++;
                if (row + i == currentSwappyTile.rowNum && col == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
        }
        if (totalSwappies == 6 && hasIncludedOriginal)
        {
            if (!smartAIPhase)
            {
                Destroy(Instantiate(ComboShapes[5], BoardTiles[row, col].transform.position + new Vector3(0, 0, 3), BoardTiles[row, col].transform.rotation), 0.8f);
            }
            TotalCombos++;
            if (!CurrentPlayer.AiSwappy)
            {
                GameConfigration.instance.PlayerSound(10);
            }
             
            if ((row - 1) >= 0 && (col - 1) >= 0)
            {
                _tileTemp = BoardTiles[row - 1, col - 1];

                SpawnComboSwappies(_tileTemp);
            }
            if ((row + 1) < B_Size && (col + 1) < B_Size)
            {
                _tileTemp = BoardTiles[row + 1, col + 1];

                SpawnComboSwappies(_tileTemp);
            }
            if ((col - 1) >= 0 && (row + 1) < B_Size)
            {
                _tileTemp = BoardTiles[row + 1, col - 1];

                SpawnComboSwappies(_tileTemp);
            }
            if ((col + 1) < B_Size && (row - 1) >= 0)
            {
                _tileTemp = BoardTiles[row - 1, col + 1];

                SpawnComboSwappies(_tileTemp);
            }
             
        }
        else if (depth > 0)
        {
            depth--;
            CheckPlusShapes(row, col + 1, depth);
            CheckPlusShapes(row, col - 1, depth);
            CheckPlusShapes(row + 1, col, depth);
            CheckPlusShapes(row - 1, col, depth);
        }
    }

    public void CheckLShapeShapes(int row, int col, int depth)
    {
        if (!IsAvailableAndMineSwappy(row, col))
            return;

        int min1 = -2, max1 = 0;
        int min2 = 0, max2 = 2;
        int totalSwappiesLU = 0;
        int totalSwappiesLD = 0;
        int totalSwappiesRU = 0;
        int totalSwappiesRD = 0;
        bool hasIncludedOriginal = false;
        int shape = -1;

        //Left-Up
        for (int i = min1; i <= max1; i++)
        {
            if (IsAvailableAndMineSwappy(row, col + i))
            {
                totalSwappiesLU++;
                if (row == currentSwappyTile.rowNum && col + i == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
            if (IsAvailableAndMineSwappy(row + i, col))
            {
                totalSwappiesLU++;
                if (row + i == currentSwappyTile.rowNum && col == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
        }
        if (totalSwappiesLU == 6 && hasIncludedOriginal)
        {
            if (!smartAIPhase)
            {
                Destroy(Instantiate(ComboShapes[6], BoardTiles[row, col].transform.position + new Vector3(0, 0, 3), BoardTiles[row, col].transform.rotation), 0.8f);
            }
            TotalCombos++;
            if (!CurrentPlayer.AiSwappy)
            {
                GameConfigration.instance.PlayerSound(11);
            }
            SpawnLeftUpS(row, col);
        }
        totalSwappiesLD = 0;
        hasIncludedOriginal = false;

        //Left-Down
        for (int i = min1; i <= max1; i++)
        {
            if (IsAvailableAndMineSwappy(row, col + i))
            {
                totalSwappiesLD++;
                if (row == currentSwappyTile.rowNum && col + i == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
            if (IsAvailableAndMineSwappy(row - i, col))
            {
                totalSwappiesLD++;
                if (row - i == currentSwappyTile.rowNum && col == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
        }
        if (totalSwappiesLD == 6 && hasIncludedOriginal)
        {
            if (!smartAIPhase)
            {
                Destroy(Instantiate(ComboShapes[7], BoardTiles[row, col].transform.position + new Vector3(0, 0, 3), BoardTiles[row, col].transform.rotation), 0.8f);
            }
            TotalCombos++;
            if (!CurrentPlayer.AiSwappy)
            {
                GameConfigration.instance.PlayerSound(11);
            }
            SpawnLeftDownS(row, col);
        }
        totalSwappiesRD = 0;
        hasIncludedOriginal = false;

        //Right-Down
        for (int i = min2; i <= max2; i++)
        {
            if (IsAvailableAndMineSwappy(row, col + i))
            {
                totalSwappiesRD++;
                if (row == currentSwappyTile.rowNum && col + i == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
            if (IsAvailableAndMineSwappy(row + i, col))
            {
                totalSwappiesRD++;
                if (row + i == currentSwappyTile.rowNum && col == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
        }
        if (totalSwappiesRD == 6 && hasIncludedOriginal)
        {
            if (!smartAIPhase)
            {
                Destroy(Instantiate(ComboShapes[8], BoardTiles[row, col].transform.position + new Vector3(0, 0, 3), BoardTiles[row, col].transform.rotation), 0.8f);
            }
            TotalCombos++;
            if (!CurrentPlayer.AiSwappy )
            {
                GameConfigration.instance.PlayerSound(11);
            }
            SpawnRightDownS(row, col);
        }
        totalSwappiesRU = 0;
        hasIncludedOriginal = false;

        //Right-Up
        for (int i = min2; i <= max2; i++)
        {
            if (IsAvailableAndMineSwappy(row, col + i))
            {
                totalSwappiesRU++;
                if (row == currentSwappyTile.rowNum && col + i == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
            if (IsAvailableAndMineSwappy(row - i, col))
            {
                totalSwappiesRU++;
                if (row - i == currentSwappyTile.rowNum && col == currentSwappyTile.colNum)
                    hasIncludedOriginal = true;
            }
        }
        if (totalSwappiesRU == 6 && hasIncludedOriginal)
        {
            if (!smartAIPhase)
            {
                Destroy(Instantiate(ComboShapes[9], BoardTiles[row, col].transform.position + new Vector3(0, 0, 3), BoardTiles[row, col].transform.rotation), 0.8f);
            }
            TotalCombos++;
            if (!CurrentPlayer.AiSwappy )
            {
                GameConfigration.instance.PlayerSound(11);
            }
            SpawnRightUpS(row, col);
        }
        if (shape < 0 && depth > 0)
        {
            depth--;
            CheckLShapeShapes(row, col + 1, depth );
            CheckLShapeShapes(row, col - 1, depth );
            CheckLShapeShapes(row + 1, col, depth );
            CheckLShapeShapes(row - 1, col, depth );
        }
    }

    void SpawnLeftUpS(int r, int c)
    {
        Tiles _tileTemp;
        if (L1)
        {
            return;
        }
        L1 = true;
        int row = r;
        int col = c;
         
        _tileTemp = BoardTiles[row - 1, col - 1];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row - 2, col - 1];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row - 2, col - 2];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row - 1, col - 2];

        SpawnComboSwappies(_tileTemp);

         
    }
    void SpawnLeftDownS(int r, int c)
    {
        Tiles _tileTemp;
        if (L2)
        {
            return;
        }
        L2 = true;
        int row = r;
        int col = c;
         
        _tileTemp = BoardTiles[row + 1, col - 1];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row + 2, col - 1];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row + 2, col - 2];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row + 1, col - 2];

        SpawnComboSwappies(_tileTemp);

         
    }
    void SpawnRightDownS(int r, int c)
    {
        Tiles _tileTemp;
        if (L3)
        {
            return;
        }
        L3 = true;
        int row = r;
        int col = c;
         
        _tileTemp = BoardTiles[row + 1, col + 1];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row + 2, col + 1];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row + 2, col + 2];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row + 1, col + 2];

        SpawnComboSwappies(_tileTemp);

         
    }
    void SpawnRightUpS(int r, int c)
    {
        Tiles _tileTemp;
        if (L4)
        {
            return;
        }
        L4 = true;
        int row = r;
        int col = c;
         
        _tileTemp = BoardTiles[row - 1, col + 1];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row - 2, col + 1];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row - 2, col + 2];

        SpawnComboSwappies(_tileTemp);

        _tileTemp = BoardTiles[row - 1, col + 2];

        SpawnComboSwappies(_tileTemp);

         
    }

    IEnumerator PlaceTheSwappies()
    {
        yield return null;
         
    }

    void SpawnComboSwappies(Tiles _tile)
    {
        if (!_tile.alreadyInstantiated)
        {
            SpawnMySwappy(_tile);
        }
        else if (!CurrentPlayer.mySwappies.Contains(_tile.ThisSwappy))
        {
            DestroyNSpawnMySwappy(_tile);
        }

        //StartCoroutine(PlaceTheSwappies());
    }

    void SpawnMySwappy(Tiles _tileTemp)
    {
            if (!TilesToSpawn.Contains(_tileTemp))
            {
                TilesToSpawn.Add(_tileTemp);
            }
    }

    void DestroyNSpawnMySwappy(Tiles _tileTemp)
    {
        if (!TilesToDeSpawn.Contains(_tileTemp))
        {
            TilesToDeSpawn.Add(_tileTemp);
        }
        if (!TilesToSpawn.Contains(_tileTemp))
        {
            TilesToSpawn.Add(_tileTemp);
        }
    }

    public int PossiblePositions()
    {
        int count = 0;
        for (int i = 0; i < B_Size; i++)
        {
            for (int j = 0; j < B_Size; j++)
            {
                if (!BoardTiles[i,j].alreadyInstantiated)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public void FillSwapableTiles()
    {
        SwapableSwapies.Clear();
        for (int i = 0; i < AdjacentTiles.Count; i++)
        {
            if (AdjacentTiles[i].alreadyInstantiated == true)
            {
                if (!CurrentPlayer.mySwappies.Contains(AdjacentTiles[i].ThisSwappy))
                {
                    SwapableSwapies.Add(AdjacentTiles[i].ThisSwappy);
                }
            }
        }
    }


    public void GetEmptyBoxTiles(Tiles ActiveTile)
    {
        int range = 2;
        int selectedSwappyRow = ActiveTile.rowNum;
        int selectedSwappyCol = ActiveTile.colNum;

        AdjacentTiles.Clear();

        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                int newRow = selectedSwappyRow + i;
                int newCol = selectedSwappyCol + j;

                if (newRow >= 0 && newRow < B_Size && newCol >= 0 && newCol < B_Size)
                {
                    if (!BoardTiles[newRow, newCol].alreadyInstantiated)
                    {
                        AdjacentTiles.Add(BoardTiles[newRow, newCol]);
                    }
                }
            }
        }
    }

    public void FillAdjacentTiles(Tiles ActiveTile)
    {
        int range = 1;
        int selectedSwappyRow = ActiveTile.rowNum;
        int selectedSwappyCol = ActiveTile.colNum;

        AdjacentTiles.Clear();

        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                int newRow = selectedSwappyRow + i;
                int newCol = selectedSwappyCol + j;

                if (newRow >= 0 && newRow < B_Size && newCol >= 0 && newCol < B_Size)
                {
                    AdjacentTiles.Add(BoardTiles[newRow, newCol]);
                }
            }
        }
    }

    void AddFirstShapesTiles(Tiles tempTile)
    {
        if (!tempTile.alreadyInstantiated)
        {
            AdjacentTiles.Add(tempTile);
        }
        else
        {
            if (CurrentPlayer.mySwappies.Contains(tempTile.ThisSwappy))
            {
                AdjacentTiles.Add(tempTile);
            }
        }
    }

    public bool CheckPartitionSides(Tiles TheTile)
    {
        List<Tiles> TempList = new List<Tiles>();   

        if (topLeftBox.Contains(TheTile))
            TempList = topLeftBox;
        else if (topRightBox.Contains(TheTile))
            TempList = topRightBox;
        else if (bottomLeftBox.Contains(TheTile))
            TempList = bottomLeftBox;
        else if (bottomRightBox.Contains(TheTile))
            TempList = bottomRightBox;

        foreach(Tiles Tile in AdjacentTiles)
        {
            if (!TempList.Contains(Tile))
            {
                return false;
            }
        }
        return true;
    }

    public void GetEmptyAdjacent()
    {
        EmptyAdjacentTiles.Clear();
        for (int i = 0; i < AdjacentTiles.Count; i++)
        {
            if(!AdjacentTiles[i].alreadyInstantiated)
            {
                EmptyAdjacentTiles.Add(AdjacentTiles[i]);
            }
        }
    }
    public bool CheckFourMoves = false;
    public bool MovedChecked = false;
    public bool CheckFour()
    {
        int a = 0;
        if (!CheckFourMoves)
        {
            foreach (var item in _PlayersList)
            {
                if (item.mySwappies.Count >= 4)
                {
                    a++;
                }
            }

            if (a == _PlayersList.Count)
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
            return true ;
        }
       
    }
    public void CheckforColors()
    {
        if (!CheckFourMoves)
        {
            for (int i = 0; i < PartitionAssigned.Count; i++)
            {
                if (PartitionAssigned[i] == false)
                {
                    Boxcolors[i].SetActive(false);
                }
                else
                {
                    Boxcolors[i].SetActive(true);
                }
            }

            
            if ((_PlayersList.Find(a => a.AiSwappy == false)).PartitionAssigned)
            {
                switch ((_PlayersList.Find(a => a.AiSwappy == false))._BoardPartition)
                {
                    case BoardPartition.Tleft:
                        Boxcolors[0].SetActive(false);
                        break;
                    case BoardPartition.TRight:
                        Boxcolors[1].SetActive(false);
                        break;
                    case BoardPartition.BLeft:
                        Boxcolors[2].SetActive(false);
                        break;
                    case BoardPartition.BRight:
                        Boxcolors[3].SetActive(false);
                        break;
                }
            }


            Boxcolors[4].SetActive(true);
        }
        else
        {
            if (MovedChecked)
                return;
            foreach (var item in Boxcolors)
            {
                item.SetActive(false);
                PopupScript.ShowUI("7");
                Invoke("fightoff", 1f);
            }
            MovedChecked = true;
        }
    }
    public void fightoff()
    {
       PopupScript.instance.gameObject.SetActive(false);
    }
    public bool FirstAIShapes(Tiles _tiles, bool Cross)
    {
        int selectedSwappyRow = _tiles.rowNum;
        int selectedSwappyCol = _tiles.colNum;

        AdjacentTiles.Clear();

        if (Cross)
        {
            if (selectedSwappyRow - 1 >= 0 && selectedSwappyCol - 1 >= 0)
            {
                AddFirstShapesTiles(BoardTiles[selectedSwappyRow - 1, selectedSwappyCol - 1]);
            }
            if (selectedSwappyRow - 1 >= 0 && selectedSwappyCol + 1 < B_Size)
            {
                AddFirstShapesTiles(BoardTiles[selectedSwappyRow - 1, selectedSwappyCol + 1]);
            }
            if (selectedSwappyRow + 1 < B_Size && selectedSwappyCol - 1 >= 0)
            {
                AddFirstShapesTiles(BoardTiles[selectedSwappyRow + 1, selectedSwappyCol - 1]);
            }
            if (selectedSwappyRow + 1 < B_Size && selectedSwappyCol + 1 < B_Size)
            {
                AddFirstShapesTiles(BoardTiles[selectedSwappyRow + 1, selectedSwappyCol + 1]);
            }
        }
        else
        {
            if (selectedSwappyRow >= 0 && selectedSwappyCol - 1 >= 0)
            {
                AddFirstShapesTiles(BoardTiles[selectedSwappyRow, selectedSwappyCol - 1]);
            }
            if (selectedSwappyRow >= 0 && selectedSwappyCol + 1 < B_Size)
            {
                AddFirstShapesTiles(BoardTiles[selectedSwappyRow, selectedSwappyCol + 1]);
            }
            if (selectedSwappyRow - 1 >= 0 && selectedSwappyCol >= 0)
            {
                AddFirstShapesTiles(BoardTiles[selectedSwappyRow - 1, selectedSwappyCol]);
            }
            if (selectedSwappyRow + 1 < B_Size && selectedSwappyCol >= 0)
            {
                AddFirstShapesTiles(BoardTiles[selectedSwappyRow + 1, selectedSwappyCol]);
            }
        }

        if (AdjacentTiles.Count == 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void GiveScoreAppreciation(int currentScore,int lastScore)
    {

        switch (currentScore - lastScore)
        {
            case > 29:
                if (_PlayersList.Count < TotalLeft)
                {
                    GameConfigration.instance.PlayerSound(12);
                    PopupScript.ShowUI("6");
                    if (PlayerPrefs.GetInt("CarnageGoatTrophy", 0) == 0)
                    {
                        //PopupScript.instance.ExtraPopUp("You got the Carnage goat trophy");
                        PopupScript.instance.MedalPopUp.gameObject.SetActive(true);
                        PopupScript.instance.MedalPopUp.sprite = GoldMedal;
                        PlayerPrefs.SetInt("CarnageGoatTrophy", 1);
                    }
                    TotalLeft--;
                }
                else
                {
                    GameConfigration.instance.PlayerSound(13);
                    PopupScript.ShowUI("6");
                    //PopupScript.instance.ExtraPopUp("You got the goat combo trophy");
                    PopupScript.instance.MedalPopUp.gameObject.SetActive(true);
                    PopupScript.instance.MedalPopUp.sprite = GoldMedal;
                }
                break;
            case > 27:
                GameConfigration.instance.PlayerSound(14);
                PopupScript.ShowUI("5");
                //PopupScript.instance.ExtraPopUp("You got the Combo master trophy");
                PopupScript.instance.MedalPopUp.gameObject.SetActive(true);
                PopupScript.instance.MedalPopUp.sprite = GoldMedal;
                break;
            case > 24:
                if (_PlayersList.Count < TotalLeft)
                {
                    GameConfigration.instance.PlayerSound(15);
                    PopupScript.ShowUI("5");
                    if (PlayerPrefs.GetInt("CarnageMasterTrophy", 0) == 0)
                    {
                        //PopupScript.instance.ExtraPopUp("You got the Carnage master trophy");
                        PopupScript.instance.MedalPopUp.gameObject.SetActive(true);
                        PopupScript.instance.MedalPopUp.sprite = GoldMedal;
                        PlayerPrefs.SetInt("CarnageMasterTrophy", 1);
                    }
                    TotalLeft = _PlayersList.Count;
                }
                else
                {
                    GameConfigration.instance.PlayerSound(16);
                    PopupScript.ShowUI("5");
                    //PopupScript.instance.ExtraPopUp("Gold medal");
                    PopupScript.instance.MedalPopUp.gameObject.SetActive(true);
                    PopupScript.instance.MedalPopUp.sprite = GoldMedal;
                }
                break;
            case > 21:
                GameConfigration.instance.PlayerSound(17);
                PopupScript.ShowUI("Outstanding");
                break;
            case > 19:
                if (_PlayersList.Count < TotalLeft)
                {
                    GameConfigration.instance.PlayerSound(18);
                    PopupScript.ShowUI("Brutal");
                    if (PlayerPrefs.GetInt("GoldMedal", 0) == 0)
                    {
                        //PopupScript.instance.ExtraPopUp("Gold medal");
                        PopupScript.instance.MedalPopUp.gameObject.SetActive(true);
                        PopupScript.instance.MedalPopUp.sprite = GoldMedal;
                        PlayerPrefs.SetInt("GoldMedal", 1);
                    }
                    TotalLeft = _PlayersList.Count;
                }
                else
                {
                    GameConfigration.instance.PlayerSound(19);
                    PopupScript.ShowUI("4");
                    //PopupScript.instance.ExtraPopUp("Silver medal");
                    PopupScript.instance.MedalPopUp.gameObject.SetActive(true);
                    PopupScript.instance.MedalPopUp.sprite = SilverMedal;
                }
                break;
            case > 17:
                GameConfigration.instance.PlayerSound(20);
                PopupScript.ShowUI("Amazing");
                break;
            case > 14:
                if (_PlayersList.Count < TotalLeft)
                {
                    GameConfigration.instance.PlayerSound(21);
                    PopupScript.ShowUI("Cruel");
                    if (PlayerPrefs.GetInt("SilverMedal", 0) == 0)
                    {
                        //PopupScript.instance.ExtraPopUp("Silver medal");
                        PopupScript.instance.MedalPopUp.gameObject.SetActive(true);
                        PopupScript.instance.MedalPopUp.sprite = SilverMedal;
                        PlayerPrefs.SetInt("SilverMedal", 1);
                    }
                    TotalLeft = _PlayersList.Count;
                }
                else
                {
                    GameConfigration.instance.PlayerSound(22);
                    PopupScript.ShowUI("3");
                    //PopupScript.instance.ExtraPopUp("Bronze medal");
                    PopupScript.instance.MedalPopUp.gameObject.SetActive(true);
                    PopupScript.instance.MedalPopUp.sprite = BronzeMedal;
                }
                break;
            case > 11:
                if (_PlayersList.Count < TotalLeft)
                {
                    GameConfigration.instance.PlayerSound(23);
                    PopupScript.ShowUI("Painful");
                    if (PlayerPrefs.GetInt("BronzeMedal", 0) == 0)
                    {
                        //PopupScript.instance.ExtraPopUp("Bronze medal");
                        PopupScript.instance.MedalPopUp.gameObject.SetActive(true);
                        PopupScript.instance.MedalPopUp.sprite = BronzeMedal;
                        PlayerPrefs.SetInt("BronzeMedal", 1);
                    }
                    TotalLeft = _PlayersList.Count;
                }
                else
                {
                    GameConfigration.instance.PlayerSound(24);
                    PopupScript.ShowUI("1");
                }
                break;
            case > 9:
                if (_PlayersList.Count < TotalLeft)
                {
                    GameConfigration.instance.PlayerSound(25);
                    PopupScript.ShowUI("Carnage");
                    TotalLeft = _PlayersList.Count;
                }
                else
                {
                    GameConfigration.instance.PlayerSound(26);
                    PopupScript.ShowUI("0");
                }
                break;
        }
    }
    public void ForFit()
    {
        //make it
    }
}

