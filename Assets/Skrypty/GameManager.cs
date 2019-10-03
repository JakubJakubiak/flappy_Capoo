using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public RectTransform menuTr;
    public RectTransform respawnMenuTr;
    public AudioSource dzwiekiAS;
    public Text punktyTxt;
    public Text highscoreTxt;
    public Button grajBtn;
    public static bool gameOver = true;
    public Vector2 ptakStartPos;
    public Transform bg1, bg2;
    public Vector2/* bg1DefPos,*/ bg2DefPos;

    private static int punkty;

    public static int Punkty
    {
        get {
            return punkty;
        }

        set {
            punkty = value;
            instance.punktyTxt.text = value.ToString();
        }
    }

    public static int Highscore
    {
        get {
            return highscore;
        }

        set {
            if(value > highscore) { 
            highscore = value;
                instance.highscoreTxt.text = value.ToString();
            }
        }
    }

    private static int highscore;

    void Start() {
        bg2DefPos = bg2.transform.position;
        ptakStartPos = Ptak.ptak.transform.position;
        instance = this;
        Highscore = PlayerPrefs.GetInt("Highscore", 0 );
    }


    void FixedUpdate() { //przesuwa tło 
        if (Ptak.ptak.Zyje)
        PrzesuwajTlo();
    }

    public void GrajBtn() {
        menuTr.gameObject.SetActive(false);
        Ptak.ptak.rg.isKinematic = false;
        gameOver = false;
    }

    void PrzesuwajTlo() {
        float predkosc = Rura.predkosc / 3f;
        float cameraXLeft = Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height;
        bg1.Translate(Vector2.left * Time.deltaTime * predkosc, Space.World);
        bg2.Translate(Vector2.left * Time.deltaTime * predkosc, Space.World);
        if (bg1.position.x <= cameraXLeft)
            bg1.position = bg2DefPos;
        if (bg2.position.x <= cameraXLeft)
            bg2.position = bg2DefPos;
    }

    public void RespawnBtn(){
        Ptak.ptak.Zyje = true;
        gameOver = false;
        Ptak.ptak.GetComponent<PolygonCollider2D>().isTrigger = false;
        for (int i = Spawner.instance.zespawnowaneRury.Count-1; i >=0 ; i--) {
            GameObject.Destroy(Spawner.instance.zespawnowaneRury[i].gameObject);
        }
        Spawner.instance.zespawnowaneRury.Clear();
        Ptak.ptak.transform.position = ptakStartPos;
        Ptak.ptak.rg.velocity = Vector2.zero;
        respawnMenuTr.gameObject.SetActive(false);
        Punkty = 0;
    }
}
