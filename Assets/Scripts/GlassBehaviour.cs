using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float pointY;

    //pour savoir si le verre arrive en sautant
    public bool isjumpingAtStart;
    public float JumpStr;
    Transform forcePoint;

    //au début le verre sort du sol, on limite donc temporairement la barre du sol pour qu'il puisse passer au travers
    private bool isIntangible;
    private bool isDead;

    public SpawnPoint whereItSpawned;

    private Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        forcePoint = GetComponentInChildren<Transform>();
        isIntangible = true;
        collider = GetComponent<Collider2D>();
        Collider2D colliderTable = GameObject.FindGameObjectWithTag("table").GetComponent<Collider2D>();

        rb.gravityScale = 0f;

        Physics2D.IgnoreCollision(collider,colliderTable);

        if(isjumpingAtStart)
        {
            JumpStr = 0.2f;
        }
        else
        {
            JumpStr = 0.01f;
        }

        if(whereItSpawned != null)
        {
            Invoke("Dies", Random.Range(GlassManager.minGlassLifeTime, GlassManager.maxGlassLifeTime));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            if (!isIntangible)
            {
                if (transform.position.y < pointY)
                {
                    transform.position = new Vector2(transform.position.x, pointY);
                    rb.gravityScale = 0.0f;
                }
            }
            else
            {
                if (transform.position.y > pointY)
                {
                    isIntangible = false;
                    rb.gravityScale = 1.0f;
                }
                else
                {
                    rb.velocity += new Vector2(0f, JumpStr);
                }
            }
        }
    }

    void Dies()
    {
        rb.velocity = new Vector2(0.0f,0.0f);
        rb.gravityScale = 0.1f;
        isDead = true;
        whereItSpawned.isHavingAGlassAbove = false;
        Destroy(gameObject, 3.0f);
    }
}
