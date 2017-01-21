using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandardBehaviour : MonoBehaviour {

    Vector2 center;
    public bool walking;
    public float angle, length, velocity;
    Vector2 position;
    // Use this for initialization
    virtual protected  void Start () {
        position = new Vector2();
	}
	
    public void Initialize(float a, float l, float v)
    {
        angle = a;
        length = l;
        velocity = v;
        walking = true;
    }

	// Update is called once per frame
	virtual protected void Update () {
        if (walking)
        {
            length -= velocity * Time.deltaTime;
            position.x = center.x + length * Mathf.Cos(angle * Mathf.Deg2Rad);
            position.y = center.y + length * Mathf.Sin(angle * Mathf.Deg2Rad);

            transform.position = position;
        }

        if(Vector2.Distance(transform.position, center) > 1)
        {
            Debug.Log("Die");
        }
	}
}
