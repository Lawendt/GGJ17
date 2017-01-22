using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDancing : MonoBehaviour
{
	void Start ()
    {
        GetComponent<Animator>().SetTrigger("Enjoy");
	}
}
