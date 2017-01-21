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
    public float timetoWaitToEnjoy = 1f;
    private float startLenght;
    Vector2 center;
    public bool walking;
    public float angle, length, velocity;
    Vector2 position;
    Animator animator;
    public EnemyType type;
    public float velScaleDown;

    public List<GameObject> prefabType;
    // Use this for initialization
    virtual protected void Start()
    {
        position = new Vector2();
    }

    public void setType(EnemyType type)
    {
        this.type = type;
        int i = (int)type;
        if (prefabType[i] != null)
        {
            GameObject g = GameObject.Instantiate<GameObject>(prefabType[i], transform, false);
            animator = g.GetComponent<Animator>();
        }
    }
    public void Initialize(float a, float l, float v)
    {
        startLenght = l;
        angle = a;
        length = l;
        velocity = v;
        walking = true;
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
                
                while (v.x > 0)
                {

                    v.x -= velScaleDown * Time.deltaTime;
                    v.y -= velScaleDown * Time.deltaTime;
                    transform.localScale = v;

                    yield return new WaitForEndOfFrame();
                }
                break;
        }

        Die();
    }

    IEnumerator _waitToEnjoy()
    {
        yield return new WaitForSeconds(Vector2.Distance(transform.position, center) / startLenght * timetoWaitToEnjoy);
        animator.SetTrigger("Enjoy");
        walking = false;
        StartCoroutine("_scaleDown");
    }

    IEnumerator _waitToStop()
    {
        yield return new WaitForSeconds(Vector2.Distance(transform.position, center) / startLenght * timetoWaitToEnjoy);
        if (!walking)
        {
            StopCoroutine("_scaleDown");
            animator.SetTrigger("StopEnjoying");
            walking = true;
        }
    }

    virtual public void Enjoy()
    {
        StartCoroutine("_waitToEnjoy");
    }

    virtual public void StopEnjoying()
    {

        StartCoroutine("_waitToStop");
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
