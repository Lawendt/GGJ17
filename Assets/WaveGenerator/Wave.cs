using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

	public WaveGenerator myPool;
	public float lifeSpan = 1f;
	public float targetSize = 1f;
	public float life = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		life += Time.deltaTime;
		if (life >= lifeSpan) {
			myPool.addToPool (this.gameObject);
		}
		Vector3 sca = this.transform.localScale;
		sca += new Vector3 (Time.deltaTime * targetSize/lifeSpan, Time.deltaTime * targetSize/lifeSpan, Time.deltaTime * targetSize/lifeSpan);
		this.transform.localScale = sca;
		Color col = this.GetComponent<SpriteRenderer> ().color;
		col.a -= Time.deltaTime / lifeSpan;
		this.GetComponent<SpriteRenderer> ().color= col;
	}
}
