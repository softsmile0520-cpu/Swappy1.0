using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VerticalLineTutorial : MonoBehaviour
{
    public GameObject Tip1;
    public GameObject Tip2;
    public GameObject Tip3;

    public Button Next;

    public GameObject Hand;
    public GameObject HandContainer;

    public GameObject ScoreDiff1;


    public GameObject swappy;

    //shape1Tiles — placement order: SwappyPlaced, then Tile1…Tile6
    public GameObject SwappyPlaced;
    public GameObject Tile1;
    public GameObject Tile2;
    public GameObject Tile3;
    public GameObject Tile4;
    public GameObject Tile5;
    public GameObject Tile6;

    public GameObject ShapeMade;

    [Header("A2 — 5th token (line trigger, 0.1 s)")]
    [Tooltip("Board cell for the 5th placement that triggers the line. If empty, uses Tile5.")]
    public GameObject fifthTokenAnchor;
    [Tooltip("Hold yellow lit + t100 overlay for this long.")]
    public float a2FifthTokenDuration = 0.1f;
    [Tooltip("5th swappy scale vs others (slightly larger).")]
    public float fifthTokenUniformScale = 1.12f;
    [Tooltip("Drag the Sprite from Assets/work_pngs/For Tutorial1/yellow.jpeg")]
    public Sprite yellowLitSprite;
    [Tooltip("Local scale of the yellow halo relative to the token.")]
    public float yellowGlowScale = 1.38f;
    [Tooltip("Z offset for glow (in front of token mesh).")]
    public float yellowGlowLocalZ = 0.03f;
    [Tooltip("World TMP scale for “t100 chosen” (slightly bigger than token).")]
    public float t100OverlayUniformScale = 1.22f;
    [Tooltip("Local Y offset for the label above the token.")]
    public float t100OverlayLocalY = 0.42f;
    [Tooltip("Local Z for text (in front of glow).")]
    public float t100OverlayLocalZ = -0.1f;
    [Tooltip("TMP font size (world text); tune if too large/small.")]
    public float t100FontSize = 3.2f;
    [Tooltip("Shown on the 5th token.")]
    public string t100ChosenLabel = "t100 chosen";

    //Extras
    int n = 0;


    public TextMeshProUGUI textToModify;
    public float duration = 3f;
    public float targetSize = 140f;

    /// <summary>Alias for older scenes that still wire the Next button to NextBTN.</summary>
    public void NextBTN() => NextButton();

    public void NextButton()
    {
        if (n == 0)
        {
            Tip1.SetActive(false);
            Tip2.SetActive(true);
            HandContainer.SetActive(true);
            n = 1;
            Next.gameObject.SetActive(false);
        }
        else if (n == 1)
        {
            TryPlaySound(0);
            SceneManager.LoadScene("Vertical 2");
        }
    }
    IEnumerator IncreaseTextSizeOverTime()
    {
        float startTime = Time.time;
        while (Time.time - startTime <= duration)
        {
            float t = (Time.time - startTime) / duration;
            textToModify.fontSize = (int)Mathf.Lerp(0f, targetSize, t);
            yield return null;
        }
        textToModify.fontSize = (int)targetSize;

        // Toggle off the text object
        textToModify.gameObject.SetActive(false);
    }

    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        StartCoroutine(CoPlayTutorialBackgroundWhenReady());
    }

    IEnumerator CoPlayTutorialBackgroundWhenReady()
    {
        int guard = 0;
        while (GameConfigration.instance == null && guard++ < 300)
            yield return null;
        if (GameConfigration.instance != null)
            GameConfigration.instance.BGSoundPlayer(9);
    }
    public IEnumerator HandShadking()
    {
        yield return new WaitForSeconds(1f);
        Hand.SetActive(!Hand.activeSelf);
    }
    public void MakingSahpe()
    {
        StartCoroutine(makeShape());
    }

    static void TryPlaySound(int soundIndex)
    {
        if (GameConfigration.instance != null)
            GameConfigration.instance.PlayerSound(soundIndex);
    }

    void AddYellowLitGlow(GameObject piece)
    {
        Sprite sprite = yellowLitSprite;
        if (sprite == null)
            return;

        var glow = new GameObject("YellowLit");
        glow.transform.SetParent(piece.transform, false);
        glow.transform.localPosition = new Vector3(0f, 0f, yellowGlowLocalZ);
        glow.transform.localRotation = Quaternion.identity;
        glow.transform.localScale = Vector3.one * yellowGlowScale;

        var sr = glow.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = Color.white;
        sr.sortingOrder = 32000;
    }

    void AddT100ChosenOverlay(GameObject piece)
    {
        var go = new GameObject("T100Chosen");
        go.transform.SetParent(piece.transform, false);
        go.transform.localPosition = new Vector3(0f, t100OverlayLocalY, t100OverlayLocalZ);
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one * t100OverlayUniformScale;

        var tmp = go.AddComponent<TextMeshPro>();
        tmp.text = t100ChosenLabel;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmp.fontSize = t100FontSize;
        tmp.color = new Color(0.15f, 0.15f, 0.15f, 1f);
        if (TMP_Settings.defaultFontAsset != null)
            tmp.font = TMP_Settings.defaultFontAsset;
    }

    public IEnumerator makeShape()
    {
        HandContainer.SetActive(false);

        GameObject fifthCell = fifthTokenAnchor != null ? fifthTokenAnchor : Tile5;
        GameObject[] lineAnchors = { SwappyPlaced, Tile1, Tile2, Tile3, Tile4, Tile5, Tile6 };

        ShapeMade.SetActive(true);
        TryPlaySound(2);

        for (int i = 0; i < lineAnchors.Length; i++)
        {
            GameObject cell = lineAnchors[i];
            if (cell == null)
                continue;

            bool isA2FifthTrigger = fifthCell != null && cell == fifthCell;

            GameObject piece = Instantiate(swappy);
            piece.transform.position = cell.transform.position + new Vector3(0, 0, -1);

            if (isA2FifthTrigger)
            {
                piece.transform.DOScale(Vector3.one * fifthTokenUniformScale, 0.09f);
                AddYellowLitGlow(piece);
                AddT100ChosenOverlay(piece);
                TryPlaySound(2);
                yield return new WaitForSeconds(a2FifthTokenDuration);
            }
            else
            {
                piece.transform.DOScale(Vector3.one, 0.09f);
                TryPlaySound(2);
            }
        }

        PlayerScorePopUp.ShowUI();
        PlayerScorePopUp.instance.ShowScore(+7);
        //StartCoroutine(IncreaseTextSizeOverTime());
        ScoreDiff1.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        Tip2.SetActive(false);
        Tip3.SetActive(true);
        Next.gameObject.SetActive(true);
    }
    public void BackTOMenu()
    {
        TryPlaySound(0);
        SceneManager.LoadScene("MenuScene");
    }
}
