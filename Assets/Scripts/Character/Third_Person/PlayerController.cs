using KinematicCharacterController;
using UnityEngine;

namespace Assets.Scripts.Third_Person
{
    public class PlayerController : MonoBehaviour, ICharacterController, ICustomInputHandler
    {
        [Header("Components")]
        public KinematicCharacterMotor Motor;
        public Transform CameraTransform;
        public GameObject

        [Header("Settings")]
        [Tooltip("Whether to stop immediately when there's no input. If false, the character will decelerate to a stop.")]
        [InspectorName("Stop Instantly On No Input")]
        public bool stopInstantlyOnNoInput = true;
        public float cableLength = 5f;
        public Transform plugPosition;

        [Header("Stable Movement")]
        public float stableSpeed = 10f;
        public float deceleration = 1f;
        public float jumpUpSpeed = 10f;

        [Header("Gravity")]
        public Vector3 gravity = new Vector3(0, -30, 0);
        public float gravityFallMultiplier = 2f;

        [Header("Vaccuming")]
        private bool _isVaccuming = false;
        private float _vaccumPower = 10f;
        [SerializeField] private GameObject vaccumCone;
        [SerializeField] private VaccumControlling vaccumControlling;

        private Vector2 _movementInput;
        private Vector3 _movementVector;
        private bool _jumpRequested;
        private Vector2 _mousePosition;

        private void Start()
        {
            Motor.CharacterController = this;
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_isVaccuming)
            {
                Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
                Plane groundPlane = new Plane(Motor.CharacterUp, transform.position);

                if (groundPlane.Raycast(ray, out float distance))
                {
                    Vector3 mousePosWorld = ray.GetPoint(distance);
                    Vector3 direction = mousePosWorld - transform.position;
                    direction = Vector3.ProjectOnPlane(direction, Motor.CharacterUp);

                    if (direction != Vector3.zero)
                    {
                        currentRotation = Quaternion.LookRotation(direction, Motor.CharacterUp);
                    }
                }
                return;
            }

            currentRotation = _movementVector.sqrMagnitude > 0.001f 
                ? Quaternion.LookRotation(_movementVector, Motor.CharacterUp) 
                : currentRotation;
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            CalculateMovementDirection();

            if (Motor.GroundingStatus.IsStableOnGround)
            {
                HandleGroundedMovement(ref currentVelocity, deltaTime);
            }
            else
            {
                HandleAirMovement(ref currentVelocity, deltaTime);
            }
        }

        private void HandleGroundedMovement(ref Vector3 currentVelocity, float deltaTime)
        {
            if (_jumpRequested)
            {
                PerformJump(ref currentVelocity);
            }

            if (stopInstantlyOnNoInput)
            {
                currentVelocity = CalculateGroundedMovement(_movementVector, currentVelocity.y, deltaTime);
            }
            else if (_movementInput.magnitude < 0.001f)
            {
                Vector3 targetVelocity = new Vector3(0, currentVelocity.y, 0);
                currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, deceleration * deltaTime);
            }
            else
            {
                currentVelocity = CalculateGroundedMovement(_movementVector, currentVelocity.y, deltaTime);
            }
        }

        private Vector3 CalculateGroundedMovement(Vector3 direction, float currentY, float deltaTime)
        {
            float speed = _isVaccuming ? (stableSpeed / 2f) : stableSpeed;
            Vector3 horizontalMovement = direction * speed;

            if (plugPosition != null)
            {
                Vector3 predictedPosition = transform.position + (horizontalMovement * deltaTime);
                Vector3 fromPlugToPredicted = predictedPosition - plugPosition.position;
                fromPlugToPredicted.y = 0f;

                if (fromPlugToPredicted.magnitude > cableLength)
                {
                    Vector3 fromPlugToCurrent = transform.position - plugPosition.position;
                    fromPlugToCurrent.y = 0f;

                    Vector3 awayFromPlugDir = fromPlugToCurrent.normalized;
                    float dot = Vector3.Dot(horizontalMovement, awayFromPlugDir);

                    if (dot > 0f)
                    {
                        horizontalMovement -= awayFromPlugDir * dot;
                    }
                }
            }

            return new Vector3(horizontalMovement.x, currentY, horizontalMovement.z);
        }

        private void PerformJump(ref Vector3 currentVelocity)
        {
            Vector3 jumpDirection = Motor.CharacterUp;
            if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
            {
                jumpDirection = Motor.GroundingStatus.GroundNormal;
            }

            Motor.ForceUnground();
            currentVelocity += (jumpDirection * jumpUpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
            _jumpRequested = false;
        }

        private void HandleAirMovement(ref Vector3 currentVelocity, float deltaTime)
        {
            float gravityMultiplier = currentVelocity.y < 0 ? gravityFallMultiplier : 1f;
            currentVelocity += gravityMultiplier * gravity * deltaTime;

            if (plugPosition != null)
            {
                Vector3 predictedPos = transform.position + (currentVelocity * deltaTime);
                Vector3 fromPlug = predictedPos - plugPosition.position;
                fromPlug.y = 0f;

                if (fromPlug.magnitude > cableLength)
                {
                    Vector3 awayDir = transform.position - plugPosition.position;
                    awayDir.y = 0f;
                    awayDir.Normalize();

                    Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
                    float dot = Vector3.Dot(horizontalVelocity, awayDir);

                    if (dot > 0f)
                    {
                        horizontalVelocity -= awayDir * dot;
                        currentVelocity = new Vector3(horizontalVelocity.x, currentVelocity.y, horizontalVelocity.z);
                    }
                }
            }
        }

        public void HandleMovementInput(Vector2 movementInput)
        {
            _movementInput = movementInput;
        }

        public void HandleJumpInput(bool jumpInput)
        {
            _jumpRequested = jumpInput;
        }

        public void HandleLookInputMouse(Vector2 mousePos)
        {
            _mousePosition = mousePos;
        }

        private void CalculateMovementDirection()
        {
            _movementVector = new Vector3(_movementInput.x, 0f, _movementInput.y);

            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(CameraTransform.rotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(CameraTransform.rotation * Vector3.up, Motor.CharacterUp).normalized;
            }

            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);
            _movementVector = cameraPlanarRotation * _movementVector;
        }

        public void ToggleVaccum()
        {
            _isVaccuming = !_isVaccuming;

            if (_isVaccuming && vaccumCone != null)
            {
                vaccumCone.SetActive(true);
                Debug.Log("Vaccum On");
            }
            else
            {
                if (vaccumControlling != null)
                {
                    vaccumControlling.TurnOffVaccum();
                }

                if (vaccumCone != null)
                {
                    vaccumCone.SetActive(false);
                }

                Debug.Log("Vaccum Off");
            }
        }
    }
}