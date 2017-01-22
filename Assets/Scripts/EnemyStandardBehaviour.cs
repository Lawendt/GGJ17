using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class EnemyStandardBehaviour : MonoBehaviour
{
    public float map(float value,
                               float start1, float stop1,
                               float start2, float stop2)
    {
        return
          start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }


    public enum TypeOfDeath
    {
        fadeAlpha,
        Scale
    }
    #region SetUp

    public AnimationCurve deathCurve;
    public TypeOfDeath typeOfDeath;
    public EnemyType type;
    public float velScaleDown, timeToThrow = 2.0f;
    public GameObject highlight;
    public List<GameObject> prefabType;
    public SpriteRenderer[] heads;
    public ParticleSystem star, interrogation;
    public float multiplierSpeed = 1.0f;
    public float multiplierMaximum = 3.0f;
    #endregion
    #region Walk Parameters
    public Vector2 center;
    public float angle, length, velocity;
    Vector2 position;
    // When Hating
    public float maxVelocity = 2.0f, acceleration = 1.0f;
    #endregion
    #region State Control Bools
    public bool walking;
    public bool hating;
    public bool receivedEnjoy;
    public bool shaking;
    #endregion
    #region Local references
    private float startLenght;
    private float persistanceMultiplier;
    Animator animator;
    float startVelocity;
    Player player;
    float _lifeEnemy = 0;
    float lifeEnemy
    {
        get
        {
            return _lifeEnemy;
        }
        set
        {
            player.addScore((value - _lifeEnemy));
            _lifeEnemy = value;
        }
    }


    #endregion
    // Use this for initialization

    public void SetHighlight(bool b)
    {
        highlight.SetActive(true);
    }
    virtual protected void Start()
    {
        player = Player.Instance;
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
            heads = g.GetComponent<HeadHolder>().head;
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


        //for (int i = 0; i < heads.Length; i++)
        //{
        //    heads[i].color = new Color(1, map(velocity, startVelocity, maxVelocity, 1, 0), map(velocity, startVelocity, maxVelocity, 1, 0));
        //}

        //if (Vector2.Distance(transform.position, center) < 0.5)
        //{
        //    //GetComponent<SpriteRenderer>().color = Color.red;
        //    Die();
        //}
    }

    public void Die()
    {
        if (shaking)
        {
            Debug.Log("Die Shake");
            player.removeShake();
        }
        EnemyManager.Instance.removeEnemy(this);
        Destroy(gameObject);
    }
    public float distanceFromObjective()
    {
        return Vector2.Distance(transform.position, center);
    }

    #region Enjoying

    IEnumerator _scaleDown()
    {
        persistanceMultiplier = 1.0f;

        switch (typeOfDeath)
        {
            case TypeOfDeath.fadeAlpha:
                Color c = Color.white;
                SpriteRenderer[] render = GetComponentsInChildren<SpriteRenderer>();
                if (render.Length == 0)
                    yield return null;
                c.a = render[0].color.a;
                float maxA = c.a;
                float minA = 0;
                while (c.a > minA)
                {
                    c.a = lifeEnemy * maxA + minA;
                    for (int i = 0; i < render.Length; i++)
                        render[i].color = c;
                    lifeEnemy += velScaleDown * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                break;
            case TypeOfDeath.Scale:
                Vector3 v = transform.localScale;
                Vector2 start = v;
                while (v.x > 0.15)
                {
                    //Increase multiplier
                    persistanceMultiplier = Mathf.Clamp(persistanceMultiplier + multiplierSpeed * Time.deltaTime, 1.0f, multiplierMaximum);

                    if (!star.isPlaying)
                        star.Play();
                    v.x = (deathCurve.Evaluate(lifeEnemy)) * start.x;
                    v.y = (deathCurve.Evaluate(lifeEnemy)) * start.y;
                    transform.localScale = v;
                    lifeEnemy += velScaleDown * Time.deltaTime * persistanceMultiplier;
                    yield return new WaitForEndOfFrame();
                }
                break;
        }

        Die();
    }
    IEnumerator _waitToEnjoy(float timeToWait)
    {
        receivedEnjoy = true;
        //Debug.Log("Start Enjoy " + name + "\nTold to wait " + timeToWait);
        if (timeToWait != 0)
            yield return new WaitForSeconds(Vector2.Distance(transform.position, center) / startLenght * timeToWait);

        //if (shaking)
        //{
        //    Debug.Log("Stop Enjoy remove Shake");
        //    player.removeShake();
        //}

        animator.SetTrigger("Enjoy");
        walking = false;
        StartCoroutine("_scaleDown");
    }

    IEnumerator _waitToStopEnjoying(float timeToWait)
    {
        receivedEnjoy = false;
        //Debug.Log("Stop Enjoy " + name);
        yield return new WaitForSeconds(Vector2.Distance(transform.position, center) / startLenght * timeToWait);
        star.Stop();
        if (!walking)
        {
            StopCoroutine("_scaleDown");
            animator.SetTrigger("StopEnjoying");
            walking = true;
        }
        if (shaking && lifeEnemy > 0)
        {
            //Debug.Log("Stop Enjoy add Shake");
            //player.AddShake();
            walking = false;
        }
    }

    virtual public void Enjoy(float timeToWait)
    {
        StopAllCoroutines();
        StartCoroutine(_waitToEnjoy(timeToWait));
    }

    virtual public void StopEnjoying(float timeToWait)
    {
        StartCoroutine(_waitToStopEnjoying(timeToWait));
    }
    #endregion

    #region Hate
    Coroutine hateCoroutine = null;
    Coroutine StopHateCoroutine = null;

    IEnumerator _Hating(float timeToWait)
    {
        hating = true;

        if (timeToWait != 0)
            yield return new WaitForSeconds(Vector2.Distance(transform.position, center) / startLenght * timeToWait);
        for (int i = 0; i < heads.Length; i++)
        {
            heads[i].color = Color.red;
        }

        float timerHate = 0;
        while (hating)
        {
            timerHate += Time.deltaTime;
            velocity += acceleration * Time.deltaTime;

            if (velocity > maxVelocity)
            {
                velocity = maxVelocity;
            }
            if (timerHate > timeToThrow)
            {
                timerHate = 0;
                ThrowObject();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator _StopHating(float timeToWait)
    {
        if (timeToWait != 0)
            yield return new WaitForSeconds(Vector2.Distance(transform.position, center) / startLenght * timeToWait);

        while (velocity > startVelocity)
        {
            velocity -= acceleration * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void Hate(float timeToWait)
    {
        //if (StopHateCoroutine != null)
        //    StopCoroutine(StopHateCoroutine);
        hateCoroutine = StartCoroutine(_Hating(timeToWait));
    }

    public void StopHating(float timeToWait)
    {
        if (hating)
        {
            for (int i = 0; i < heads.Length; i++)
            {
                heads[i].color = Color.white;
            }
            hating = false;
            velocity = startVelocity;

            if (hateCoroutine != null)
                StopCoroutine(hateCoroutine);
            // StopHateCoroutine = StartCoroutine(_StopHating(timeToWait));
        }
    }

    public void ThrowObject()
    {
        Debug.Log("Throw Object");
    }

    #endregion

    #region Confuse
    public void Confuse()
    {
        if (!interrogation.isPlaying)
            interrogation.Play();
    }

    public void EndConfuse()
    {
        interrogation.Stop();
    }
    #endregion

    #region Shake

    public void Shake()
    {
        walking = false;
        shaking = true;
    }
    #endregion


}
