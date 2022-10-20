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
        isEmpty = false;
        powerup = null;
    }

    public void PickupPowerup(PowerPellet powerup)
    {
        isEmpty = false;
        this.powerup = powerup;
        // Fill backpack with Powerup
    }

    public void UsePowerup(Pacman player)
    {
        // Use powerup in backpack
        isEmpty = true;
        Debug.Log(player + " has used a " + this.powerup.type + " powerup!");

        if (this.powerup.type == PowerPellet.Type.star)
        {
            FindObjectOfType<GameManager>().ActivateStarPowerup(powerup, player);
            
            this.powerup = null;
        }
        else if (powerup.type == PowerPellet.Type.freeze)
        {
            // Freeze opponent and ghosts
            FindObjectOfType<GameManager>().ActivateFreezePowerup(powerup, player);

            this.powerup = null;
        }
        else if (powerup.type == PowerPellet.Type.refill)
        {
            // Refill the board with pellets
            FindObjectOfType<GameManager>().RefillBoard();
            
            this.powerup = null;
        }
        else if (powerup.type == PowerPellet.Type.projectile)
        {
            // Shoot projectile untils it hits a wall

            // TODO
        }
    }

    public bool CanUsePowerup()
    {
        return !isEmpty;
    }
}
