using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    #region Components
    Rigidbody2D rb2D;
    //Composant enfant pour afficher le dash
    LineRenderer dashLine;
    #endregion

    [Tooltip("Les objets possédant ce layermask seront des plateformes pour le joueur")]
    [SerializeField] LayerMask glassMask;

    #region Movements
    bool hasControl = true;
    bool isInTheAir;
    [Header("Aerial control")]
    public float airControl;
    public float fallMultiplier;

    [Header("Grounded control")]
    public float horizontalSpeed;
    #endregion

    #region Dash
    [SerializeField] int dashCapacity = 0;
    bool dashing;
    bool preparingDash;
    public float prepareDashTime = 2f;
    Vector2 dashAcceleration = Vector2.zero;
    Vector3 firstMousePosition;
    [SerializeField] bool haveControlDuringDash;
    #endregion

    void Start()
    {
        isInTheAir = true;
        rb2D = GetComponent<Rigidbody2D>();
        dashLine = GetComponentInChildren<LineRenderer>();

        PlayerUIManager.Instance.UpdateDashAmount(dashCapacity);
    }

    void FixedUpdate()
    {
        //Vérifie constamment si le joueur n'est pas dans les airs
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.75f, glassMask);

        if (hit.collider != null)
            isInTheAir = false;

        else
            isInTheAir = true;

        //Si le joueur n'est pas en train de dasher, on lui applique la physique normale
        //Debug.Log("Normal physique ? " + (!dashing && !preparingDash));
        //Debug.Log("dashing ? " + dashing);
        //Debug.Log("preparingDash ? " + preparingDash);
        if (!dashing && !preparingDash && hasControl)
        {
            if (!isInTheAir)
            {
                rb2D.velocity += new Vector2(Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime, 0f);
            }
            else
            {
                //Permet de changer de direction instantanément
                //if (rb2D.velocity.x > 0 && Input.GetAxis("Horizontal") < 0)
                //{
                //    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                //}
                //else if (rb2D.velocity.x < 0 && Input.GetAxis("Horizontal") > 0)
                //{
                //    rb2D.velocity = new Vector2(0, rb2D.velocity.y);
                //}

                //Si on va vers le haut, on ralentit la chute, mais on veut un plus faible air control
                float xAerialVelocity = 0f;
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    xAerialVelocity = Mathf.Sqrt(airControl) * Input.GetAxis("Horizontal") * Time.deltaTime;
                }
                else
                {
                    xAerialVelocity = airControl * Input.GetAxis("Horizontal") * Time.deltaTime;
                }

                rb2D.velocity += new Vector2(xAerialVelocity, (fallMultiplier - Input.GetAxis("Vertical") * fallMultiplier) * Time.deltaTime * Physics2D.gravity.y / 10);
            }
            rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, 30f);
        }
    }

    private void Update()
    {
        //Au moment où on appuie pour dash, on ralentit le joueur
        if (Input.GetButtonDown("Fire1") && !dashing && dashCapacity > 0 && hasControl)
        {
            firstMousePosition = Input.mousePosition;
            StartCoroutine(WaitBeforeDashDeceleration(0.5f));
            //DashDrawer.ClearLine(dashLine);
        }
        //Dessine une ligne tant que le bouton est appuyé
        if (Input.GetButton("Fire1") && preparingDash)
        {
            Vector3 newMousePos = Input.mousePosition;
            //Debug.Log("First mouse pos : " + firstMousePosition);
            //Debug.Log("newMousePos : " + newMousePos);
            //Debug.Log(firstMousePosition - newMousePos);
            DashDrawer.DrawLine(dashLine, transform.position - (firstMousePosition - newMousePos) / 100, transform.position, Color.black, 0.15f);
        }

        if (Input.GetButtonUp("Fire1") && preparingDash)
        {
            //if (dashAcceleration != Vector2.zero && dashAcceleration != null)
            //{
            preparingDash = false;
            StartCoroutine(WaitBeforeLineFaded(0.1f, dashLine.GetPosition(0) - dashLine.GetPosition(1)));
            //}
            StartCoroutine(DashDrawer.FadeLine(dashLine, 0.1f));
            StopCoroutine(DashDrawer.FadeLine(dashLine, 0.1f));
        }
    }

    Vector2 GetDashForce(/*Vector2 acceleration*/Vector3 distance)
    {

        //Vector3 distance = Camera.main.WorldToScreenPoint(transform.position) - Input.mousePosition;
        //Vector3 distance = dashLine.GetPosition(0) - dashLine.GetPosition(1);
        //F = m x a (et plus on est proche, moins ça va dash loin, quoique osef de l'acceleration)
        Vector2 force = rb2D.mass * distance /* * acceleration */;
        return force;
    }

    IEnumerator WaitBeforeDashDeceleration(float dashDecelerationTime)
    {
        Vector2 oldVelocity = rb2D.velocity;
        preparingDash = true;
        float timer = 0f;
        float lerpValue = 0;
        while (lerpValue <= 1f && preparingDash)
        {
            rb2D.velocity = Vector2.Lerp(oldVelocity, Vector2.zero, lerpValue);
            lerpValue += Time.deltaTime;
            yield return new WaitForSeconds(dashDecelerationTime * Time.deltaTime);
            timer += dashDecelerationTime * Time.deltaTime;
            //Debug.Log("Velocity dash :" + rb2D.velocity);
        }
        timer = 0f;
        //S'il prepare encore son dash 
        while (preparingDash && timer < prepareDashTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if (preparingDash)
            {
                rb2D.velocity = Vector2.zero;
                Debug.Log("Attente :" + timer);

            }
            timer += Time.deltaTime;
        }
        if (timer >= prepareDashTime)
        {
            Debug.Log(preparingDash);
            preparingDash = false;
            StartCoroutine(DashDrawer.FadeLine(dashLine, 0.3f));
            StopCoroutine(DashDrawer.FadeLine(dashLine, 0.3f));
        }

        dashAcceleration = CalculateAcceleration(oldVelocity, rb2D.velocity, timer);

        StopCoroutine(WaitBeforeDashDeceleration(dashDecelerationTime));
    }

    IEnumerator WaitBeforeLineFaded(float fadeLineTime, Vector2 force)
    {
        yield return new WaitForSeconds(fadeLineTime);

        StopCoroutine(WaitBeforeLineFaded(fadeLineTime, force));
        Dash(GetDashForce(force), 2f);
        Debug.Log("Dash !");
    }
    Vector2 CalculateAcceleration(Vector2 oldVelocity, Vector2 newVelocity, float timeBetween)
    {
        return (oldVelocity - newVelocity) / timeBetween;
    }

    void Dash(Vector2 force, float dashTime)
    {
        dashing = true;
        dashCapacity--;
        PlayerUIManager.Instance.UpdateDashAmount(dashCapacity);
        rb2D.AddForce(-force * 100);
        dashAcceleration = Vector2.zero;
        if (haveControlDuringDash)
        {
            dashing = false;
        }
        else
        {
            Invoke(nameof(SlowDown), dashTime);
        }
    }

    void SlowDown()
    {
        dashing = false;
        //rb2D.velocity = Vector2.zero;
    }

    public void AddDash(int amount)
    {
        dashCapacity += amount;

        PlayerUIManager.Instance.UpdateDashAmount(dashCapacity);
    }

    public void UpdateControl(bool control)
    {
        hasControl = control;
    }

    private void OnBecameInvisible()
    {
        UpdateControl(false);
        GameManager.Instance.DisplayDefeatHUD();
    }
    public void Boost(float multiplier)
    {
        rb2D.velocity = rb2D.velocity * multiplier;
    }
}
