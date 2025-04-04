using System;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Controller
{
   
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public static PlayerController Instance;
        //I think the player controller is canceling my bounce mechanic
        //Adding variables to allow for bounce on trampoline
        private bool isBouncing = false;
        private float bounceDuration = 0.5f; 
        private float bounceTimer = 0f;
        private float _bounceVelocity = 0f;


       
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        //adding some variables to detect slopes for sliding
        private Vector2 slopeNormal;
        private float slopeAngle;
        private bool onSlope;
        private float maxSlopeAngle = 45f;
        public PhysicsMaterial2D NoFriction;
        public PhysicsMaterial2D RegFriction;
        private bool wasOnSlope = false;
        private float slopeStayTimer = 0f;
        private const float slopeTolerance = 0.3f;
        private float slopeLDTimer = 0f;
        private const float slopeLDTime = 0.3f;

        private void Awake()
        {
            Instance = this;
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();
        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
        }

        public void TriggerBounce(float velocity)
        {
            _bounceVelocity = velocity;
            isBouncing = true;
            bounceTimer = bounceDuration;
        }

        private void FixedUpdate()
        {
            CheckCollisions();
            HandleJump();
            HandleDirection();
            if (isBouncing)
            {
                bounceTimer -= Time.fixedDeltaTime;
                _frameVelocity.y = _bounceVelocity;

                if (bounceTimer <= 0f)
                {
                    isBouncing = false;
                }
            }
            else {
                HandleGravity();
            }
            ApplyMovement();
        }

        #region Collisions
        
        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);
            RaycastHit2D hit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            //detect slopes 
            if (groundHit)
            {
                slopeNormal = hit.normal;
                slopeAngle = Vector2.Angle(slopeNormal, Vector2.up);
                onSlope = slopeAngle > 5f && slopeAngle <= maxSlopeAngle;
            }
            else
            {
                onSlope = false;
            }

            if (onSlope)
            {
                slopeStayTimer = slopeTolerance;
                wasOnSlope = true;
            }
            else
            {
                slopeStayTimer -= Time.fixedDeltaTime;
                if (slopeStayTimer <= 0)
                {
                    wasOnSlope = false;
                }
            }

            if (_grounded && wasOnSlope)
            {
                _col.sharedMaterial = NoFriction;
            }
            else
            {
                _col.sharedMaterial = RegFriction;
            }

            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.linearVelocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_grounded || CanUseCoyote) ExecuteJump();

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f )
            {
                //if the player is on a slope 
                if (onSlope)
                {
                    slopeLDTimer = slopeLDTime;
                    //Figure out which direction the slope is 
                    Vector2 slideDirection = new Vector2(slopeNormal.y, -slopeNormal.x).normalized;
                    float slideDot = Vector2.Dot(slideDirection, Vector2.down);

                    //If sloped down apply sliding to player
                    if (slideDot > 0)
                    {
                        _rb.linearDamping = 0f;
                        _rb.AddForce(slideDirection * _stats.FallAcceleration * 5f, ForceMode2D.Force);

                    }
                }
                else if (slopeLDTimer > 0f){
                    slopeLDTimer -= Time.fixedDeltaTime;
                    _rb.linearDamping = 0f;
                }
                else
                {
                    _rb.linearDamping = 1f;
                    _frameVelocity.y = _stats.GroundingForce;
                }
                    }
            else
            {
                _rb.linearDamping = 1f;
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void ApplyMovement() => _rb.linearVelocity = _frameVelocity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif
    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}