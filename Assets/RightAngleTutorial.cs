using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class RightAngleTutorial : MonoBehaviour
{
    public GameObject swappy;
    public GameObject Enemy;

    // public GameObject Hand1st;
    //public GameObject Hand2nd;
    // public GameObject Hand3rd;
    public GameObject Hand4th;
    public GameObject Hand5th;


    //public Transform Tile1st;

    public Transform Tile4th;
    public Transform Tile5th;

    public Transform Tile6th;
    public Transform Tile7th;
    public Transform Tile8th;
    public Transform Tile9th;


    public Transform EnemyTile1st;
    public Transform EnemyTile2nd;
    public Transform EnemyTile3rd;
    public Transform EnemyTile4th;
    public Transform EnemyTile5th;

    public Transform Tile10th;
    public Transform Tile11th;


    public Image shape;
    public Image shape2;
    public Image shape3;

    public GameObject EndText;
    public GameObject CompTurn;
    public GameObject YourTurn;

    public GameObject BTN;


    //public void FifthSwappy()
    //{
    //    GameObject player = Instantiate(swappy);
    //    player.transform.position = Tile1st.transform.position + new Vector3(0, 0, -1);
    //    player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

    //    Hand1st.SetActive(false);
    //    Hand2nd.SetActive(true);
    //}

    //public void FourthSwappy()
    //{
    //    GameObject player = Instantiate(swappy);
    //    player.transform.position = Tile2nd.transform.position + new Vector3(0, 0, -1);
    //    player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

    //    Hand2nd.SetActive(false);
    //    Hand3rd.SetActive(true);
    //}


    //public void ThirdSwappy()
    //{
    //    GameObject player = Instantiate(swappy);
    //    player.transform.position = Tile3rd.transform.position + new Vector3(0, 0, -1);
    //    player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

    //    Hand3rd.SetActive(false);
    //    Hand4th.SetActive(true);
    //}

    public void SecondSwappy()
    {
        GameObject player = Instantiate(swappy);
        player.transform.position = Tile4th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        shape.gameObject.SetActive(false);

        //Invoke("Shape3", 0.5f);

        Invoke("SecondLineComplete", 1f);

        shape3.gameObject.SetActive(true);
        Invoke("ChildButtonShape3", 0.8f);

        Hand4th.SetActive(false);


    }
    void ChildButtonShape3()
    {
        shape3.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void FirstSwappy()
    {
        GameObject player = Instantiate(swappy);
        player.transform.position = Tile5th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);



        Hand5th.SetActive(false);
        //LineComplete();
        Invoke("Shape", 1f);

        Invoke("LineComplete", 0.5f);

        //Invoke("TextEnd", 1f);

        Invoke("ComPlay", 1.8f);

    }

    public void LineComplete()
    {
        GameObject player = Instantiate(swappy);
        player.transform.position = Tile6th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile7th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile8th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile9th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);



        //player = Instantiate(swappy);
        //player.transform.position = Tile11th.transform.position + new Vector3(0, 0, -1);
        //player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);


    }


    public void ComputerTurn()
    {


        GameObject player = Instantiate(Enemy);
        player.transform.position = EnemyTile1st.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(Enemy);
        player.transform.position = EnemyTile2nd.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(Enemy);
        player.transform.position = EnemyTile3rd.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(Enemy);
        player.transform.position = EnemyTile4th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(Enemy);
        player.transform.position = EnemyTile5th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        Invoke("Shape2", 0.8f);


        Invoke("ItsYourTurn", 2f);

    }

    public void SecondLineComplete()
    {
        GameObject player = Instantiate(swappy);
        player.transform.position = Tile10th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

        player = Instantiate(swappy);
        player.transform.position = Tile11th.transform.position + new Vector3(0, 0, -1);
        player.transform.DOScale(new Vector3(1f, 1f, 1f), 0.09f);

    }
    public void OKBTN()
    {
        GameConfigration.instance.PlayerSound(0);

        TextEnd();

        Destroy(BTN);

    }

    public void Shape()
    {
        shape.gameObject.SetActive(true);
        GameConfigration.instance.PlayerSound(8);



    }
    public void Shape2()
    {
        shape2.gameObject.SetActive(true);
        GameConfigration.instance.PlayerSound(8);

    }
    public void Shape3()
    {


        shape3.gameObject.SetActive(true);

        GameConfigration.instance.PlayerSound(8);

    }

    public void TextEnd()
    {
        EndText.SetActive(true);

    }
    public void ItsYourTurn()
    {

        CompTurn.SetActive(false);

        YourTurn.SetActive(true);

        Hand4th.SetActive(true);

    }

    public void ComPlay()
    {
        CompTurn.SetActive(true);

        Invoke("ComputerTurn", 0.2f);



    }
    public void NextBTN()
    {
        GameConfigration.instance.PlayerSound(0);

        SceneManager.LoadScene("RightAngleCombo");
    }
}
