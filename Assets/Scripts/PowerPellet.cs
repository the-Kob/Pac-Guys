using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerPellet : Pellet
{
    public float duration = 0f;
    public Type type;

    public enum Type
    {
        star, 
        refill,
        projectile,
        freeze
    }

    public void selectType()
    {
        Array values = Enum.GetValues(typeof(Type));
        type = (Type)values.GetValue(Random.Range(0, values.Length));
    }

    protected override void Eat(Pacman player)
    {
        FindObjectOfType<GameManager>().PowerPelletEaten(this, player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.gameObject.GetComponent<SpriteRenderer>().enabled)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
            {
                Eat(other.gameObject.GetComponent<Pacman>());
                selectType();
                UpdateDuration();
            }
        }
        else
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ghost")) {
                Refill();
            }
        }
    }

    private void UpdateDuration()
    {
        if (this.type == Type.star)
        {
            duration = 8.0f;
        } 
        else if (this.type == Type.freeze)
        {
            duration = 2.5f;
        }
        else if (this.type == Type.projectile || this.type == Type.refill)
        {
            duration = 0f;
        }
    }
}
