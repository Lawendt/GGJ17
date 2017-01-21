using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoRoll : MonoBehaviour
{
    /// <summary>
    /// Speed is unit per second
    /// </summary>
    public float horizontalSpeed, verticalSpeed;
    [AutoFind(typeof(ScrollRect), true)]
    public ScrollRect scrollRect;
    public bool active = true;
    public bool autoDeactive;

    // Use this for initialization
    void Start()
    {

    }

    private bool temp = false;
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            scrollRect.horizontalNormalizedPosition -= horizontalSpeed * Time.deltaTime;
            scrollRect.verticalNormalizedPosition -= verticalSpeed * Time.deltaTime;

            if (autoDeactive)
            {
                temp = false;
                if (horizontalSpeed > 0)
                {
                    if (scrollRect.horizontalNormalizedPosition <= 0)
                        temp = true;

                }
                else if (horizontalSpeed < 0)
                {
                    if (scrollRect.horizontalNormalizedPosition >= 1)
                        temp = true;
                }
                else
                {
                    temp = true;
                }

                if (verticalSpeed > 0)
                {
                    if (scrollRect.verticalNormalizedPosition < 0)
                        temp = true;
                    else
                        temp = false;
                }
                else if (verticalSpeed < 0)
                {
                    if (scrollRect.verticalNormalizedPosition > 1)
                        temp = true;
                    else
                        temp = false;
                }
                else
                {
                    temp = true;
                }
                if (temp)
                {
                    active = false;
                }
            }

        }

    }
}
