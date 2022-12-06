using UnityEngine;

namespace PruebaEdissonChaves
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(PlayerMotion))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInputs : MonoBehaviour
    {
        private PlayerControls playerControls;
        private PlayerController playerController;
        private PlayerMotion playerMotion;
        private Vector2 movePlaver;
        [Header ("Visible Inputs")]
        public float moveX;
        public float moveY;
        public bool jumpInput;
        

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            playerMotion = GetComponent<PlayerMotion>();
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += i => movePlaver = i.ReadValue<Vector2>();

                playerControls.PlayerActions.JumpAndJetpack.performed += i => jumpInput = true;
                playerControls.PlayerActions.JumpAndJetpack.canceled += i => jumpInput = false;
            }
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        public void ReadInputs()
        {
            HandleMovementBallInputs();
            HandleJumpInput();
            HandlejetPackInput();
        }

        #region Input handles functions
        private void HandleMovementBallInputs()
        {
            moveX = movePlaver.x;
            moveY = movePlaver.y;
        }

        private void HandleJumpInput()
        {
            if (!playerController.onGroundPlayer)
                return;

            if (jumpInput && !playerController.isJumping)
            {
                playerController.isJumping = true;
                playerMotion.Handlejump();
                jumpInput = false;
            }
        }

        private void HandlejetPackInput()
        {
            if (playerController.onGroundPlayer || !playerController.jetPackReady || playerMotion.currentFuel <= 0)
                return;

            if (jumpInput)
                playerController.isflying = true;
            else
                playerController.isflying = false;
        }
        #endregion
    }

}
