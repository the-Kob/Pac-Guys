using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PowerPellet : Pellet
{
    public float duration { get; private set; } = 0;
    public float power { get; private set; } = 0;
    public Type type = Type.nothing;
    public int usesRemaining = 0;

    public enum Type
    {
        star, 
        refill,
        projectile,
        freeze,
        speed,
        nothing
    }

    public PowerPellet()
    {
        type = Type.nothing;
        UpdateDuration();
        UpdatePower();
        UpdateUses();
    }

    public void SelectType()
    {
        Array values = Enum.GetValues(typeof(Type));
        while(type == Type.nothing)
        {
            type = (Type)values.GetValue(Random.Range(0, values.Length));
        } 
    }

    protected override void Eat(Pacman player)
    {
        FindObjectOfType<GameManager>().PowerPelletEaten(this, player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(this.gameObject.GetComponent<SpriteRenderer>().enabled);
        if (this.gameObject.GetComponent<SpriteRenderer>().enabled)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
            {
                Eat(other.gameObject.GetComponent<Pacman>());
                SelectType();
                UpdateDuration();
                UpdatePower();
                UpdateUses();
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
        else if (this.type == Type.freeze || this.type == Type.speed)
        {
            duration = 3.0f;
        }
        else if (this.type == Type.projectile || this.type == Type.refill || this.type == Type.nothing)
        {
            duration = 0f;
        }
    }

    private void UpdatePower()
    {
        if (this.type == Type.projectile)
        {
            // Projectile speed - value TBD
            power = 2.0f;
        }
        else if (this.type == Type.freeze)
        {
            // Slow multiplier
            power = 0.25f;
        }
        else if (this.type == Type.speed)
        {
            // Speed multiplier
            power = 1.5f;
        }
        else if (this.type == Type.star || this.type == Type.refill || this.type == Type.nothing)
        {
            power = 0f;
        }
    }

    private void UpdateUses()
    {
        if (this.type == Type.projectile)
        {
            usesRemaining = 3;
        }
        else if (this.type == Type.freeze || this.type == Type.speed || this.type == Type.star || this.type == Type.refill)
        {
            usesRemaining = 1;
        } 
        else if(this.type == Type.nothing)
        {
            usesRemaining = 0;
        }
    }
}
