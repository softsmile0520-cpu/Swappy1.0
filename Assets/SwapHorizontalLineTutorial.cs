using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using DG.Tweening;


public class SwapHorizontalLineTutorial : MonoBehaviour
{
    [SerializeField]
    GameObject Hand1,Hand2,ClickOk, PassiblePoss, TextBox1, TextBox2, TextBox3;

    public GameObject player1;
    public GameObject enemy1;

    public GameObject enemy2;
    //public GameObject enemy3;
   // public GameObject enemy4;


    public GameObject playerPrefab;
    public GameObject enemyPrefab;


    public GameObject Tile1st;
    public GameObject Tile2nd;

    public GameObject Tile3rd;
    public GameObject Tile4th;
    public GameObject Tile5th;
    public GameObject Tile6th;
    public GameObject Tile7th;
    public GameObject Tile8th;

    public Image shape;


    public GameObject EndText;




    public void FirstClick()
    {
        GameConfigration.instance.PlayerSound(2);

        Hand1.SetActive(false);
        TextBox1.SetActive(false);
        PassiblePoss.SetActive(true);
        TextBox2.SetActive(true);
    }

    public void SecondClick()
    {
        GameConfigration.instance.PlayerSound(0);

        TextBox2.SetActive(false);
        Hand2.SetActive(true);
    }

    public void Swap()
    {
        GameConfigration.instance.PlayerSound(2);
        StartCoroutine(SwapAnimation());

        Invoke("Shape", 1f);


        Invoke("ShowTextBox3", 1.5f);

    }

    public void OKBTN()
    {
        GameConfigration.instance.PlayerSound(0);


        Invoke("TextEnd", 1f);
        Destroy(TextBox3);
    }

    IEnumerator SwapAnimation()
    {
        yield return new WaitForSeconds(1f);

        Destroy(player1);
        Destroy(enemy1);

        Hand2.SetActive(false);
        PassiblePoss.SetActive(false);


        GameObject player = Instantiate(playerPrefab);
        player.transform.position = Tile1st.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        yield return new WaitForSeconds(0.09f);

        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.position = Tile2nd.transform.position + new Vector3(0, 0, -1);
        enemy.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        yield return new WaitForSeconds(0.09f);



        Destroy(enemy2);
       // Destroy(enemy3);
       // Destroy(enemy4);


        GameObject player2 = Instantiate(playerPrefab);
        player2.transform.position = Tile3rd.transform.position + new Vector3(0, 0, -1);
        player2.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        yield return new WaitForSeconds(0.09f);

        GameObject player7 = Instantiate(playerPrefab);
        player7.transform.position = Tile8th.transform.position + new Vector3(0, 0, -1);
        player7.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        yield return new WaitForSeconds(0.09f);


        GameObject player3 = Instantiate(playerPrefab);
        player3.transform.position = Tile4th.transform.position + new Vector3(0, 0, -1);
        player3.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        yield return new WaitForSeconds(0.09f);

        GameObject player6 = Instantiate(playerPrefab);
        player6.transform.position = Tile7th.transform.position + new Vector3(0, 0, -1);
        player6.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);


        GameObject player4 = Instantiate(playerPrefab);
        player4.transform.position = Tile5th.transform.position + new Vector3(0, 0, -1);
        player4.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        yield return new WaitForSeconds(0.09f);

        GameObject player5 = Instantiate(playerPrefab);
        player5.transform.position = Tile6th.transform.position + new Vector3(0, 0, -1);
        player5.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        yield return new WaitForSeconds(0.09f);





       

    }
    public void Shape()
    {
        GameConfigration.instance.PlayerSound(8);

        shape.gameObject.SetActive(true);

    }

    public void ShowTextBox3()
    {
        TextBox3.SetActive(true);

    }


    public void TextEnd()
    {
        EndText.SetActive(true);
    }


    public void NextBTN()

    {
        GameConfigration.instance.PlayerSound(0);

        SceneManager.LoadScene("SwapDiagonal");
    }
}
////Once your shape is finished, any opponent in its path will be switched with the player.