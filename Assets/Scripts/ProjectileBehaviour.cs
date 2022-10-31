using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float timeUntilDestroy;

    [HideInInspector]
    public Pacman owner { get; set; }

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= timeUntilDestroy)
        {
            Destroy(this.gameObject);        
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ghost"))
        {
            Ghost ghost = collision.gameObject.GetComponent<Ghost>();
            FindObjectOfType<GameManager>().GhostEaten(ghost, owner);

            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Pacman pacman = collision.gameObject.GetComponent<Pacman>();
            
            if(pacman != owner)
            {
                bool isP1 = pacman.isP1;
                FindObjectOfType<GameManager>().PacmanEaten(false, isP1, true);

                Destroy(gameObject);
            }
        }
    }
}
