using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            Ghost ghost = collision.gameObject.GetComponent<Ghost>();
            FindObjectOfType<GameManager>().GhostHit(ghost);

            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Pacman pacman = collision.gameObject.GetComponent<Pacman>();
            FindObjectOfType<GameManager>().PacmanHit(pacman);

            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
