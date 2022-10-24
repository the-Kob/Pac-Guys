using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Backpack : MonoBehaviour
{
    public bool isEmpty;

    public PowerPellet powerup;

    private void Start()
    {
        isEmpty = true;
        powerup = new PowerPellet();
    }

    public void PickupPowerup(PowerPellet powerup)
    {
        isEmpty = false;
        this.powerup = powerup;
    }

    public void UsePowerup(Pacman player)
    {
        // Use powerup in backpack
        Debug.Log(player + " has used a " + this.powerup.type + " powerup!");

        if (powerup.type == PowerPellet.Type.star)
        {
            FindObjectOfType<GameManager>().ActivateStarPowerup(powerup, player);
        }
        else if (powerup.type == PowerPellet.Type.freeze)
        {
            // Freeze opponent and ghosts
            FindObjectOfType<GameManager>().ActivateFreezePowerup(powerup, player);
        }
        else if (powerup.type == PowerPellet.Type.speed)
        {
            // Freeze opponent and ghosts
            FindObjectOfType<GameManager>().ActivateSpeedPowerup(powerup, player);
        }
        else if (powerup.type == PowerPellet.Type.refill)
        {
            // Refill the board with pellets
            FindObjectOfType<GameManager>().RefillBoard();
        }
        else if (powerup.type == PowerPellet.Type.projectile)
        {
            // Shoot projectile untils it hits a wall/enemy/other player
            FindObjectOfType<GameManager>().ActivateProjectilePowerup(powerup, player);
        }

        powerup.usesRemaining--;

        if(powerup.usesRemaining <= 0)
        {
            isEmpty = true;
            powerup.type = PowerPellet.Type.nothing;

            foreach(BackpackRenderer rend in FindObjectsOfType<BackpackRenderer>())
            {
                rend.ResetSprite();
            }
        }
    }

    public bool CanUsePowerup()
    {
        return !isEmpty && powerup.usesRemaining > 0;
    }
}
