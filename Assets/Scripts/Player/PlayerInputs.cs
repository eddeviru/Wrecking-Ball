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
        private Vector2 movePlaver;
        [Header ("Visible Inputs")]
        public float moveX;
        public float moveY;
        public bool jumpInput;
        
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
            if (!Settings.instance.plyController.onGroundPlayer)
                return;

            if (jumpInput && !Settings.instance.plyController.isJumping)
            {
                Settings.instance.plyController.isJumping = true;
                Settings.instance.plyMotion.Handlejump();
                jumpInput = false;
            }
        }

        private void HandlejetPackInput()
        {
            if (Settings.instance.plyController.onGroundPlayer || !Settings.instance.plyController.jetPackReady || Settings.instance.plyMotion.currentFuel <= 0)
                return;

            if (jumpInput)
                Settings.instance.plyController.isflying = true;
            else
                Settings.instance.plyController.isflying = false;
        }
        #endregion
    }

}
