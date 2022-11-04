using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{
    public Sprite off;
    public Sprite on;
    private SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = off;
    }

    public void On()
    {
        renderer.sprite = on;
    }

    public void Off()
    {
        renderer.sprite = off;
    }
}
