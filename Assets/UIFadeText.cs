using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFadeText : MonoBehaviour
{
    float timer;
    string text = "thing"
    // Start is called before the first frame update
    void Start()
    {
        timer = 5;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= timer.deltaTime;
        if (timer > 0)
        {

        }
    }
}
