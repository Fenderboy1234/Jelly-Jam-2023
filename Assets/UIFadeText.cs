using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeText : MonoBehaviour
{
    // Start is called before the first frame update
    const float timer = 5.0f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timer)
        {
            Destroy(gameObject);
        }

        Color colorOfObject = GetComponent<Text>().color;
        float prop = (Time.time / timer);
        colorOfObject.a = Mathf.Lerp(1, 0, prop);
        GetComponent<Text>().color = colorOfObject;

    }
}
