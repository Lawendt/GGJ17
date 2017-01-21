using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTip : MonoBehaviour
{
    public KeyCode key;
    bool down = false;
    float timer = 0f;
    void Update()
    {
        if (down)
        {
            timer += Time.deltaTime;
            if (timer < 1f)
            {
                transform.localScale = new Vector3(1f + timer*2f, 1f + timer * 2f, 1f + timer * 2f);
                Color c = GetComponent<Image>().color;
                GetComponent<Image>().color = new Color(c.r, c.g, c.b, c.a - timer);
            }
            else
            {
                transform.localScale = new Vector3(1.5f - timer, 1.5f - timer, 1.5f - timer);
                Color c = GetComponent<Image>().color;
                GetComponent<Image>().color = new Color(c.r, c.g, c.b, c.a - timer / 2f);
            }

        }
        if (Input.GetKeyDown(key))
        {
            Destroy(gameObject, 2f);
            down = true;
        }
    }
}
