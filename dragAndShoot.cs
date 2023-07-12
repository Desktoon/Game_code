using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;

public class dragAndShoot : MonoBehaviour, IDataPersistence
{
    //Empty container for Slime Controller
    SlimeController sc;
    SplitScript SplitScript;
    private SwitchButton SwitchButton;
    public Rigidbody2D rb;
    private Animator m_Anim;
    Camera cam;
    



    // Changing this affects line length.
    private float linePoints = 0.4f;
    private float timeBetweenPoints = 0.1f;



    public float power = 10f;

    // Used to determine the direction player should be facing
    private bool FacingRight = true;

    public bool aiming;
    private bool readyToJump;

    public Vector2 minPower;
    public Vector2 maxPower;


    Vector2 force;
    Vector3[] points = new Vector3[2];







    void Awake()
    {
        // Assigns the Slime Controller script
        sc = GameObject.Find("Player").GetComponent<SlimeController>();
        SplitScript = GameObject.Find("Player").GetComponent<SplitScript>();
        m_Anim = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
        SwitchButton = GameObject.Find("Switch").GetComponent<SwitchButton>();
    }
    void Start()
    {
        cam = Camera.main;
        minPower = maxPower - maxPower - maxPower;
    }

    void Update()
    {
        // Uses the camerafollowcheck since it just shows what slime should be active (Blue active)
        if (sc.cameraFollowCheck == 1 && sc.isSplit)
        {
            GameObject.Find("SmallSlimeYellow").GetComponent<dragAndShoot>().enabled = false;
            GameObject.Find("SmallSlimeYellow").GetComponent<CircleCollider2D>().enabled = false;
            GameObject.Find("SmallSlimeBlue").GetComponent<dragAndShoot>().enabled = true;
            GameObject.Find("SmallSlimeBlue").GetComponent<CircleCollider2D>().enabled = true;
        }
        // Uses the camerafollowcheck since it just shows what slime should be active (Yellow active)
        if (sc.cameraFollowCheck == 2 && sc.isSplit)
        {
            GameObject.Find("SmallSlimeYellow").GetComponent<dragAndShoot>().enabled = true;
            GameObject.Find("SmallSlimeYellow").GetComponent<CircleCollider2D>().enabled = true;
            GameObject.Find("SmallSlimeBlue").GetComponent<dragAndShoot>().enabled = false;
            GameObject.Find("SmallSlimeBlue").GetComponent<CircleCollider2D>().enabled = false;
        }

        // If slime is moving disable UI buttons
        if (rb.velocity.magnitude < 0.01)
        {
            stationary = true;
            SplitScript.active = true;
            SwitchButton.active = true;
        }
        else
        {
            stationary = false;
            SplitScript.active = false;
            SwitchButton.active = false;
        }
    }


    private void OnMouseUp()
    {
        if (!gameObject.GetComponent<dragAndShoot>().enabled)
        {
            return;
        }

        if (stationary)
        {
            aiming = false;
            if (readyToJump)
            {
                DragBall();
            }
        }
    }
    private void OnMouseDrag()
    {
        if (!gameObject.GetComponent<dragAndShoot>().enabled)
        {
            return;
        }

        if (stationary)
        {
            aiming = true;

            // Using mousePosition and player's transform (on orthographic camera view)
            Vector3 delta = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            points[0] = new Vector3(transform.position.x, transform.position.y, -15);
            points[1] = new Vector3(transform.position.x - Mathf.Clamp(delta.x, -5, 5), transform.position.y - Mathf.Clamp(delta.y, -5, 0), -15);

            force = new Vector2(-(points[0].x - points[1].x), -(points[0].y - points[1].y));

            // If the player is aiming up, starts rendering line
            if (delta.y > 0 || force.magnitude < 1)
            {
                readyToJump = false;
                m_Anim.SetBool("Drag", false);
                lr.enabled = false;
            }
            else
            {
                readyToJump = true;
                m_Anim.SetBool("Drag", true);
                DrawProjection();
            }

        }
    }

    private void DragBall()
    {
        lr.enabled = false;

        rb.AddForce(force * power, ForceMode2D.Impulse);
        m_Anim.SetBool("Drag", false);
        m_Anim.SetBool("Midair", true);
        SoundManager.Instance.PlaySound(_clip);
        CreateJumpEffect();
        jumpCount++;
    }

}

