using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

//This script handles moving the character on the X axis, both on the ground and in the air.

public class characterMovement : MonoBehaviour
{

    [Header("Components")]
    private Rigidbody2D body;
    private characterGround ground;
    [SerializeField] private Light2D aura;
    
    private characterJump jump;

    [Header("Movement Stats")]
    [SerializeField, Range(0f, 20f)][Tooltip("Maximum movement speed")] public float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed")] public float maxAcceleration = 52f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop after letting go")] public float maxDecceleration = 52f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction")] public float maxTurnSpeed = 80f;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to reach max speed when in mid-air")] public float maxAirAcceleration;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop in mid-air when no direction is used")] public float maxAirDeceleration;
    [SerializeField, Range(0f, 100f)][Tooltip("How fast to stop when changing direction when in mid-air")] public float maxAirTurnSpeed = 80f;
    [SerializeField][Tooltip("Friction to apply against movement on stick")] private float friction;
    public bool useAcceleration;

    [Header("Dash Stats")] 
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashDistance;

    [Header("Calculations")] 
    public Vector2 direction;
    private Vector2 desiredVelocity;
    public Vector2 velocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;
    
    private bool onGround;
    private bool againstWall;
    private bool pressingKey;

    private Vector2 onDashDirection;
    private float dashTimer;
    public bool isDashing { get; private set; }
    public bool hasDashed;

    [Header("Stand Stats")] 
    [SerializeField] private float standDuration = 0.5f;
    [SerializeField] private float lightOnDuration = 0.7f;
    [SerializeField] private float lightOffDuration = 0.7f;
    [SerializeField] private AnimationCurve lightCurve;
    [SerializeField] private float lightRadiusGain = 15.0f;
    [SerializeField] private float lightFalloffLoss = 0.25f;
    
    private float standTimer;
    private float lightTimer;
    private bool lightOn;

    private void Awake()
    {
        //Find the character's Rigidbody and ground detection script
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<characterGround>();
        jump = GetComponent<characterJump>();

        isDashing = false;
        lightOn = true;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        onDashDirection = direction == Vector2.zero ? Vector2.right : direction.normalized;
        Debug.Log(onDashDirection);

        isDashing = true;
        dashTimer = dashDuration;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        //This is called when you input a direction on a valid input type, such as arrow keys or analogue stick
        //The value will read -1 when pressing left, 0 when idle, and 1 when pressing right.
        direction = context.ReadValue<Vector2>();
        //Debug.Log(direction);
    }

    private void Update()
    {
        //Used to flip the character's sprite when she changes direction
        //Also tells us that we are currently pressing a direction button
        if (direction.x != 0 && !jump.currentlyWallJumping)
        {
            transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
            pressingKey = true;
        }
        else
        {
            pressingKey = false;
        }

        //Calculate's the character's desired velocity - which is the direction you are facing, multiplied by the character's maximum speed
        //Friction is not used in this game
        if (isDashing)
        {
            if (dashTimer < 0 || hasDashed)
            {
                isDashing = false;
                hasDashed = true;
                desiredVelocity = Vector2.zero;
                body.velocity = desiredVelocity;
            }

            dashTimer -= Time.deltaTime;
            desiredVelocity = onDashDirection * (dashDistance / dashDuration);
        }
        else
        {
            desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - friction, 0f);
        }

        if (body.velocity.magnitude <= 0.0f)
        {
            if (!lightOn && standTimer <= 0.0f)
            {
                standTimer = standDuration;
            } else if (standTimer > 0.0f)
            {
                standTimer -= Time.deltaTime;

                if (standTimer < 0.0f)
                {
                    lightOn = true;
                    lightTimer = lightOnDuration;
                    aura.falloffIntensity = 0.25f;
                }
            }

            if (lightOn)
            {
                if (lightTimer > 0.0f) lightTimer = Mathf.Max(0.0f, lightTimer - Time.deltaTime);

                aura.falloffIntensity = 0.5f - lightFalloffLoss * lightCurve.Evaluate(1 - lightTimer / lightOnDuration);
                aura.pointLightOuterRadius = 5.0f + lightRadiusGain * lightCurve.Evaluate(1 - lightTimer / lightOnDuration);
            }
        }
        else
        {
            if (lightOn)
            {
                lightOn = false;
                
                standTimer = standDuration;

                lightTimer = lightOffDuration;
            }

            if (!lightOn && lightTimer > 0.0f)
            {
                lightTimer = Mathf.Max(0.0f, lightTimer - Time.deltaTime);
                aura.falloffIntensity = 0.5f - lightFalloffLoss * lightCurve.Evaluate(lightTimer / lightOffDuration);
                aura.pointLightOuterRadius = 5.0f + lightRadiusGain * lightCurve.Evaluate(lightTimer / lightOffDuration);
            }
        }
    }

    private void FixedUpdate()
    {
        //Fixed update runs in sync with Unity's physics engine

        //Get Kit's current ground status from her ground script
        onGround = ground.GetOnGround();
        againstWall = ground.GetAgainstWall();

        if(onGround || againstWall){
            hasDashed = false;
        }

        //Get the Rigidbody's current velocity
        velocity = body.velocity;

        if (isDashing)
        {
            body.velocity = desiredVelocity;
            return;
        }

        //Calculate movement, depending on whether "Instant Movement" has been checked
        if (useAcceleration)
        {
            runWithAcceleration();
        }
        else
        {
            if (onGround)
            {
                runWithoutAcceleration();
            }
            else
            {
                runWithAcceleration();
            }
        }
    }

    private void runWithAcceleration()
    {
        //Set our acceleration, deceleration, and turn speed stats, based on whether we're on the ground on in the air

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        deceleration = onGround ? maxDecceleration : maxAirDeceleration;
        turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

        if (pressingKey)
        {
            //If the sign (i.e. positive or negative) of our input direction doesn't match our movement, it means we're turning around and so should use the turn speed stat.
            if (Mathf.Sign(direction.x) != Mathf.Sign(velocity.x))
            {
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
            else
            {
                //If they match, it means we're simply running along and so should use the acceleration stat
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else
        {
            //And if we're not pressing a direction at all, use the deceleration stat
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        //Move our velocity towards the desired velocity, at the rate of the number calculated above
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        //Update the Rigidbody with this new velocity
        body.velocity = velocity;

    }

    private void runWithoutAcceleration()
    {
        //If we're not using acceleration and deceleration, just send our desired velocity (direction * max speed) to the Rigidbody
        velocity.x = desiredVelocity.x;

        body.velocity = velocity;
    }
}