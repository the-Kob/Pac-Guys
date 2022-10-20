using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pellet : MonoBehaviour
{
    public int points = 10;

    protected virtual void Eat(Pacman player)
    {
        FindObjectOfType<GameManager>().PelletEaten(this, player);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman")) {
            Eat(other.gameObject.GetComponent<Pacman>());
        }
    }
}
