using System.Collections.Generic;
using UnityEngine;

public class KillBlocks : MonoBehaviour
{
    [SerializeField] private bool isStatic = false;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private GameObject playerCharacter = null;
    [SerializeField] private GameObject farthestBoundary = null;

    private bool passedCharacter = false;
    private int blockWorth = 1;     // how much the block is worth for the player to pass by

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isStatic)
        {
            transform.Translate(Vector2.right * speed * Time.fixedDeltaTime);

            if (transform.position.x >= playerCharacter.transform.position.x - 0.7 && transform.position.x <= playerCharacter.transform.position.x + 0.7 && passedCharacter == false)
            {
                playerCharacter.GetComponent<PlayerMovement>().AddScore(blockWorth);
                passedCharacter = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == farthestBoundary)
        {
            Destroy(this.gameObject);
        }
    }

    public void CompareBoundaries(GameObject[] boundaries)
    {
        foreach (GameObject boundary in boundaries)
        {
            if (farthestBoundary == null || Mathf.Abs(Vector3.Distance(gameObject.transform.position, boundary.transform.position)) > Mathf.Abs(Vector3.Distance(gameObject.transform.position, farthestBoundary.transform.position)))
            {
                farthestBoundary = boundary;
            }
        }
    }

    public void SetStatic(bool newStatic)
    {
        isStatic = newStatic;
    }

    public void SetPlayer(GameObject player)
    {
        playerCharacter = player;
    }
}
