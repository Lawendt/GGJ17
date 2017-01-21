using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugTime : MonoBehaviour {

    [AutoFind(typeof(Text), true)]
    public Text text;

	void Update () {
        text.text = Time.time.ToString();
	}
}
