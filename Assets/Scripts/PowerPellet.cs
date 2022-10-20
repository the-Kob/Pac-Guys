using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f;

    protected override void Eat(Pacman player)
    {
        FindObjectOfType<GameManager>().PowerPelletEaten(this, player);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat(other.gameObject.GetComponent<Pacman>());
        }
    }


}
