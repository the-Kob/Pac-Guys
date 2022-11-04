using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{
    public Sprite off;
    public Sprite on;
    private SpriteRenderer rend;

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = off;
    }

    public void On()
    {
        rend.sprite = on;
    }

    public void Off()
    {
        Debug.Log(rend);
        Debug.Log(rend.sprite);
        rend.sprite = off;
    }
}
