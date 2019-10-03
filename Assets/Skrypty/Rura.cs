using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rura : MonoBehaviour {

    public static float predkosc = 5;

    void Start () {
		
	}
	

	void FixedUpdate () {
        if (GameManager.gameOver)
            return; // po smerci rury znikajom 
        transform.Translate(Vector2.left * Time.deltaTime * predkosc, Space.World);

        if (transform.position.x < -10) {
            Spawner.instance.zespawnowaneRury.Remove(this);
            Destroy(gameObject);
        }
	}
}
