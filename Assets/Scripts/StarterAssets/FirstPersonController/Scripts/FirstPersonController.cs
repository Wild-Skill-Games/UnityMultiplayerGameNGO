using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class FirstPersonController : MonoBehaviour
    {
        #region Variables

        [Header("Player")]
        public float moveSpeed = 4.0f;
        public float sprintSpeed = 6.0f;
        public float rotationSpeed = 1.0f;
        public float speedChangeRate = 10.0f;

        [Space(10)]
        public float jumpHeight = 1.2f;
        public float gravity = -15.0f;

        [Space(10)]
        public float jumpTimeout = 0.1f;
        public float fallTimeout = 0.15f;

        [Header("Cinemachine")]
        public GameObject cinemachineCameraTarget;
        public float topClamp = 90.0f;
        public float bottomClamp = -90.0f;

        private float _cinemachineTargetPitch;

        private float _speed;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private readonly float _terminalVelocity = 53.0f;

        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private CharacterController _controller;

        #endregion


        // under construction, dont touch

        public Vector2 Move { get; set; }

        public Vector2 Look { get; set; }

        public bool Jump { get; set; }

        public bool Sprint { get; set; }


        private void Start()
        {
            _controller = GetComponent<CharacterController>();

            _jumpTimeoutDelta = jumpTimeout;
            _fallTimeoutDelta = fallTimeout;
        }

        private void Update()
        {
            JumpAndGravity();
            Movement();
            CameraStuff();
        }

        private void CameraStuff()
        {
            _cinemachineTargetPitch += Look.y * rotationSpeed;
            _rotationVelocity = Look.x * rotationSpeed;

            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

            cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

            transform.Rotate(Vector3.up * _rotationVelocity);
        }

        private void Movement()
        {
            float targetSpeed = Sprint ? sprintSpeed : moveSpeed;

            if (Move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * Move.magnitude, Time.deltaTime * speedChangeRate);

                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            Vector3 inputDirection = new Vector3(Move.x, 0.0f, Move.y).normalized;

            if (Move != Vector2.zero)
            {
                inputDirection = transform.right * Move.x + transform.forward * Move.y;
            }

            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        private void JumpAndGravity()
        {
            if (_controller.isGrounded)
            {
                _fallTimeoutDelta = fallTimeout;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (Jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = jumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                Jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f)
            {
                lfAngle += 360f;
            }

            if (lfAngle > 360f)
            {
                lfAngle -= 360f;
            }

            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}