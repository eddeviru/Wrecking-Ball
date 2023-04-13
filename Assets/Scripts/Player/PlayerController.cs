using System.ComponentModel;
using UnityEngine;

namespace PruebaEdissonChaves
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Ground, jetPack and jump status")]
        public bool onGroundPlayer;
        public bool isJumping;
        public bool isflying;
        public bool jetPackReady;
        private float timerJump;

        private void Update()
        {
            if (Settings.instance.inGame)
            {
                Settings.instance.plyInputs.ReadInputs();

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
                    Settings.instance.plyMotion.SetInitialPosition();
                }
            }
        }

        private void FixedUpdate()
        {
            if (Settings.instance.inGame)
                Settings.instance.plyMotion.ReadMovementPlayer();
        }

        private void LateUpdate()
        {
            Settings.instance.flwPlayer.FollowTarget();
        }
    }
}
