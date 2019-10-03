using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public static Spawner instance;
	public Vector2 spawnPointMin, spawnPointMax;
    public float przerwaPomiedzyRurami;
    public float spawnTimer, spawnTimerDef;

    public Rura ruraPrefab;
    public List<Rura> zespawnowaneRury = new List<Rura>();

	void Start () {
        instance = this;
	}
	
	
	void Update () {
        if (GameManager.gameOver)
            return; // rury nie pojawiam sie przd klinecie start 
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0) {
            SpawnujRure();
            spawnTimer = spawnTimerDef;
        }
	}

    void SpawnujRure() {
       Rura rura1 = Instantiate(ruraPrefab.gameObject, new Vector3(spawnPointMin.x, Random.Range(spawnPointMin.y, spawnPointMax.y)), Quaternion.identity).GetComponent<Rura>();
       Rura rura2 = Instantiate(ruraPrefab.gameObject, new Vector3(rura1.transform.position.x, rura1.transform.position.y + przerwaPomiedzyRurami), Quaternion.identity).GetComponent<Rura>();
       rura2.transform.eulerAngles += new Vector3(0, 0, 180f);
        rura2.transform.GetChild(0).gameObject.SetActive(false);// wyłacza bramke punktowam na ruze 
       zespawnowaneRury.Add(rura1);
       zespawnowaneRury.Add(rura2);
    }
}
