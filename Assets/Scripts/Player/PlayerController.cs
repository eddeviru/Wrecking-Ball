using UnityEngine;
using DG.Tweening;

namespace PruebaEdissonChaves
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputs playerInputs;
        private PlayerMotion playerMotion;
        private FollowPlayer followPlayer;
        private Settings settings;
        [Header("Ground, jetPack and jump status")]
        public bool onGroundPlayer;
        public bool isJumping;
        public bool isflying;
        public bool jetPackReady;
        private float timerJump;

        private void Awake()
        {
            //Set-Up Scripts
            settings = FindObjectOfType<Settings>();
            playerInputs = GetComponent<PlayerInputs>();
            playerMotion = GetComponent<PlayerMotion>();
            followPlayer = FindObjectOfType<FollowPlayer>();
            DOTween.Init();
        }

        private void Update()
        {
            if (settings.inGame)
            {
                playerInputs.ReadInputs();

                if (isJumping)
                {
                    timerJump += Time.deltaTime;
                    if (timerJump > 2f)
                    {
                        timerJump = 0;
                        isJumping = false;
                    }
                }
                if (transform.position.y <= -9f)
                {
                    playerMotion.SetInitialPosition();
                }
            }
        }

        private void FixedUpdate()
        {
            if (settings.inGame)
                playerMotion.ReadMovementPlayer();
        }

        private void LateUpdate()
        {
            followPlayer.FollowTarget();
        }
    }
}
