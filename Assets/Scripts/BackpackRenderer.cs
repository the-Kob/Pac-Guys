using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackRenderer : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;
    SpriteRenderer rend;
    [SerializeField]
    GameObject player;
    PowerPellet powerup;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.size = new Vector2(3, 3);
        powerup = player.GetComponent<Pacman>().backpack.powerup;
        ResetSprite();
    }

    private void Update()
    {
        powerup = player.GetComponent<Pacman>().backpack.powerup;

        if(powerup.type == PowerPellet.Type.star)
        {
            Debug.Log("sprite star");
            rend.sprite = sprites[0];
        } 
        else if(powerup.type == PowerPellet.Type.freeze)
        {
            Debug.Log("sprite freeze");
            rend.sprite = sprites[1];
        }
        else if (powerup.type == PowerPellet.Type.projectile)
        {
            Debug.Log("sprite proj");
            rend.sprite = sprites[2];
        } 
        else if(powerup.type == PowerPellet.Type.refill)
        {
            Debug.Log("sprite refill");
            rend.sprite = sprites[3];
        }
        else if(powerup.type == PowerPellet.Type.speed)
        {
            Debug.Log("sprite speed");
            rend.sprite = sprites[4];
        }
    }

    public void ResetSprite()
    {
        rend.sprite = sprites[5];
    }
}
