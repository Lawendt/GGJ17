﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStandardBehaviour : MonoBehaviour
{

    public enum TypeOfDeath
    {
        fadeAlpha,
        Scale
    }
    #region SetUp
    public TypeOfDeath typeOfDeath;
    public EnemyType type;
    public float velScaleDown, timeToThrow = 2.0f;
    public List<GameObject> prefabType;
    #endregion
    #region Walk Parameters
    Vector2 center;
    public float angle, length, velocity;
    Vector2 position;
    // When Hating
    public float maxVelocity = 2.0f, acceleration = 1.0f;
    #endregion
    #region State Control Bools
    public bool walking;
    public bool hating;
    public bool receivedEnjoy;
    #endregion
    #region Local references
    private float startLenght;
    Animator animator;
    float startVelocity;
    
    #endregion
    // Use this for initialization

    virtual protected void Start()
    {
        position = new Vector2();
        center = new Vector2();

    }

    public void Initialize(float a, float l, float v, EnemyType type)
    {
        startLenght = l;
        startVelocity = v;
        angle = a;
        length = l;
        velocity = v;


        walking = true;
        hating = false;
        receivedEnjoy = false;

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
        #region Walking
        if (walking)
        {
            length -= velocity * Time.deltaTime;
            position.x = center.x + length * Mathf.Cos(angle * Mathf.Deg2Rad);
            position.y = center.y + length * Mathf.Sin(angle * Mathf.Deg2Rad);

            transform.position = position;
        }
        #endregion

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

                while (v.x > 0.15)
                {

                    v.x -= velScaleDown * Time.deltaTime;
                    v.y -= velScaleDown * Time.deltaTime;
                    transform.localScale = v;
                    //Debug.Log(v);
                    yield return new WaitForEndOfFrame();
                }
                break;
        }

        Die();
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

    #region Enjoying
    IEnumerator _waitToEnjoy(float timeToWait)
    {
        receivedEnjoy = true;
        Debug.Log("Start Enjoy " + name + "\nTold to wait " + timeToWait);

        if (timeToWait != 0)
            yield return new WaitForSeconds(Vector2.Distance(transform.position, center) / startLenght * timeToWait);
        animator.SetTrigger("Enjoy");
        walking = false;
        StartCoroutine("_scaleDown");
    }

    IEnumerator _waitToStopEnjoying(float timeToWait)
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

        StartCoroutine(_waitToStopEnjoying(timeToWait));
    }
    #endregion


    #region Hate
    Coroutine hateCoroutine;
    IEnumerator _Hating(float timeToWait)
    {
        hating = true;
        Debug.Log("Start Enjoy " + name + "\nTold to wait " + timeToWait);

        if (timeToWait != 0)
            yield return new WaitForSeconds(Vector2.Distance(transform.position, center) / startLenght * timeToWait);

        float timerHate = 0;
        while (hating)
        {
            timerHate += Time.deltaTime;
            velocity += acceleration * Time.deltaTime;
            if(velocity > maxVelocity)
            {
                velocity = maxVelocity;
            }
            if(timerHate > timeToThrow)
            {
                timerHate = 0;
                ThrowObject();
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void Hate(float timeToWait)
    {
        hateCoroutine = StartCoroutine(_Hating(timeToWait));
    }

    public void StopHating(float timeToWait)
    {
        hating = false;
        StopCoroutine(hateCoroutine);
    }

    public void ThrowObject()
    {
        Debug.Log("Throw Object");
    }

    #endregion


    public void Confuse(float timeToWait)
    {

    }
}
