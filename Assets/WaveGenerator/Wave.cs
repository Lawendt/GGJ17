using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

	public WaveGenerator myPool;
	public float lifeSpan = 1f;
	public float targetSize = 1f;
	public float life = 0f;
    public Vector3 baseScale;

	// Use this for initialization
	void OnStart() {
        baseScale = new Vector3(0.5f,0.15f,0.5f);
    }

    // Update is called once per frame
    void Update () 
	{
		life += Time.deltaTime;
		if (life >= lifeSpan) {
			myPool.addToPool (this.gameObject);
		}
		Vector3 sca = this.transform.localScale;
		sca += new Vector3 (Time.deltaTime * targetSize/lifeSpan, Time.deltaTime * targetSize/lifeSpan*0.15f, Time.deltaTime * targetSize/lifeSpan);
		this.transform.localScale = sca;
		Color col = this.GetComponent<SpriteRenderer> ().color;
		col.a -= Time.deltaTime / lifeSpan;
		this.GetComponent<SpriteRenderer> ().color= col;
	}
}
