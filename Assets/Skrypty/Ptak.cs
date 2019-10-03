using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ptak : MonoBehaviour
{
    public static Ptak ptak;
    public Rigidbody2D rg;
    public float skalaAnimacjiGoraDol = 1f;
    public float predkoscAnimacjiGoraDol = 1f;
    public float silaFlapu = 5f;
    private bool zyje = true;
    public Animator animator;
    public AudioClip flapAC; 
    public AudioClip dieAC; 
    public AudioClip fallAC; 
    public AudioClip punktAC; 

    public bool Zyje
    {
        get
        {
            return zyje;
        }

        set
        {
            zyje = value;
            animator.SetBool("Zyje", value);
        }
    }

    private void Awake() {
        ptak = this;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        rg.gravityScale = 2f; // siła grawitaci na ptaka 
        Zyje = true;
    }


    void FixedUpdate()
    {
        if (GameManager.gameOver)
        {
            float sin = Mathf.Sin(Time.time * predkoscAnimacjiGoraDol) * Time.deltaTime * skalaAnimacjiGoraDol;
            transform.Translate(new Vector3(0, sin), Space.World);
            transform.eulerAngles = new Vector3(0, 0, sin * skalaAnimacjiGoraDol * 180f);
        } else{
            transform.eulerAngles = new Vector3(0, 0, rg.velocity.y * 5f);
            print(rg.velocity.y);
            
         

        }
    }

    private void Update() {
        if (!GameManager.gameOver) {
            if (Input.GetButtonDown("Fire1")) {
                Flap();
            }
            if (Input.touchCount > 0) {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began) {
                    Flap();
                }

            }
        }
    }

    void Flap(){
        rg.velocity = Vector2.zero;
        rg.AddForce(new Vector2(0, silaFlapu), ForceMode2D.Impulse);
        animator.SetTrigger("Flap");
        GameManager.instance.dzwiekiAS.PlayOneShot(flapAC);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Zyje = false;
        GameManager.gameOver = true; // karzda kolizja = smierc
        GetComponent<PolygonCollider2D>().isTrigger = true; // wylacza kolizie po smerci 
        rg.AddForce(new Vector2(1f, 1f) * 2f, ForceMode2D.Impulse); // podskok po smierci
        GameManager.Highscore = GameManager.Punkty;
        PlayerPrefs.SetInt("Highscore", GameManager.Highscore);
        GameManager.instance.dzwiekiAS.PlayOneShot(dieAC);
        StartCoroutine(NaSmierci());
    }

    IEnumerator NaSmierci() {
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.dzwiekiAS.PlayOneShot(fallAC);
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.respawnMenuTr.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (Zyje == false)
            return;
        collision.gameObject.SetActive(false);
        GameManager.Punkty++;
        GameManager.instance.dzwiekiAS.PlayOneShot(punktAC);
    }
}

