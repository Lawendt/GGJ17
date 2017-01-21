using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandardBehaviour : MonoBehaviour
{

    public enum TypeOfDeath
    {
        fadeAlpha,
        Scale
    }
    public TypeOfDeath typeOfDeath;
    private float startLenght;
    Vector2 center;
    public bool walking;
    public float angle, length, velocity;
    Vector2 position;
    Animator animator;
    public EnemyType type;
    public float velScaleDown;
    public bool receivedEnjoy;
    public List<GameObject> prefabType;
    // Use this for initialization
    virtual protected void Start()
    {
        position = new Vector2();
        center = new Vector2();
    }

    public void Initialize(float a, float l, float v, EnemyType type)
    {
        startLenght = l;
        angle = a;
        length = l;
        velocity = v;
        walking = true;

        this.type = type;
        int i = (int)type;
        if (prefabType[i] != null)
        {
            GameObject g = GameObject.Instantiate<GameObject>(prefabType[i], transform, false);
            animator = g.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (walking)
        {
            length -= velocity * Time.deltaTime;
            position.x = center.x + length * Mathf.Cos(angle * Mathf.Deg2Rad);
            position.y = center.y + length * Mathf.Sin(angle * Mathf.Deg2Rad);

            transform.position = position;
        }

        if (Vector2.Distance(transform.position, center) < 0.5)
        {
            //GetComponent<SpriteRenderer>().color = Color.red;
            Die();
        }
    }

    IEnumerator _scaleDown()
    {
        switch (typeOfDeath)
        {
            case TypeOfDeath.fadeAlpha:
                Color c = Color.white;
                SpriteRenderer[] render = GetComponentsInChildren<SpriteRenderer>();
                if (render.Length == 0)
                    yield return null;
                c.a = render[0].color.a;
                while (c.a > 0)
                {
                    c.a -= velScaleDown * Time.deltaTime;
                    for (int i = 0; i < render.Length; i++)
                        render[i].color = c;
                    yield return new WaitForEndOfFrame();
                }
                break;
            case TypeOfDeath.Scale:
                Vector3 v = transform.localScale;
                
                while (v.x > 0.1)
                {

                    v.x -= velScaleDown * Time.deltaTime;
                    v.y -= velScaleDown * Time.deltaTime;
                    transform.localScale = v;
                    Debug.Log(v);
                    yield return new WaitForEndOfFrame();
                }
                break;
        }

        Die();
    }

    IEnumerator _waitToEnjoy(float timeToWait)
    {
        receivedEnjoy = true;
        Debug.Log("Start Enjoy " + name);
        yield return new WaitForSeconds(Vector2.Distance(transform.position, center) / startLenght * timeToWait);
        animator.SetTrigger("Enjoy");
        walking = false;
        StartCoroutine("_scaleDown");
    }

    IEnumerator _waitToStop(float timeToWait)
    {
        receivedEnjoy = false;
        Debug.Log("Stop Enjoy " + name);

        yield return new WaitForSeconds(Vector2.Distance(transform.position, center) / startLenght * timeToWait);
        if (!walking)
        {
            StopCoroutine("_scaleDown");
            animator.SetTrigger("StopEnjoying");
            walking = true;
        }
    }

    virtual public void Enjoy(float timeToWait)
    {
        StartCoroutine(_waitToEnjoy(timeToWait));
    }

    virtual public void StopEnjoying(float timeToWait)
    {

        StartCoroutine(_waitToStop(timeToWait));
    }

    void Die()
    {
        EnemyManager.Instance.removeEnemy(this);
        Destroy(gameObject);
    }

    public float distanceFromObjective()
    {
        return Vector2.Distance(transform.position, center);
    }
}
