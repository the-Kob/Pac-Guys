using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehavior initialBehavior;
    public List<Transform> targets;
    [HideInInspector]
    public Transform mainTarget;
    public int points = 200;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior) {
            home.Disable();
        }

        if (initialBehavior != null) {
            initialBehavior.Enable();
        }

        RandomlyChangeTarget();
    }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled) {
                FindObjectOfType<GameManager>().GhostEaten(this, collision.gameObject.GetComponent<Pacman>());
            } else {
                bool isP1 = collision.gameObject.GetComponent<Pacman>().isP1;
                FindObjectOfType<GameManager>().PacmanEaten(false, isP1);
            }
        }
    }

    private void ResetMovementSpeed()
    {
        movement.ResetSpeedMultiplyer();
    }

    public void SlowMovementSpeed(float slowMultiplier, float duration)
    {
        movement.speedMultiplier = slowMultiplier;

        Invoke("ResetMovementSpeed", duration);
    }

    public void RandomlyChangeTarget()
    {
        int index = Random.Range(0, targets.Count);

        mainTarget = targets[index];
    }
}
