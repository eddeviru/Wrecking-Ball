using UnityEngine;
using DG.Tweening;

namespace PruebaEdissonChaves
{
    public class PlayerMotion : MonoBehaviour
    {
        private Vector3 directionToMove;
        private Transform cameraGO;
        private PlayerInputs playerInputs;
        private PlayerController playerController;
        private Settings settings;
        private ShowFeedBack showFeedback;
        public Rigidbody rb;
        private float airTimer;
        [Header("Check platforms tags")]
        public LayerMask groundLayer;
        public float rayLenght = 0.5f;
        [Header("Jetpack stuff")]
        public GameObject jetpack;
        public float currentFuel;
        public GameObject measurer;
        private ParticleSystem[] efxJetpack;
        private Vector3 InitialPosition;

        private void Awake()
        {
            playerInputs = GetComponent<PlayerInputs>();
            playerController = GetComponent<PlayerController>();
            cameraGO = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            rb = GetComponent<Rigidbody>();
            settings = FindObjectOfType<Settings>();
            showFeedback = FindObjectOfType<ShowFeedBack>();
            InitialPosition = transform.position;

            efxJetpack = jetpack.GetComponentsInChildren<ParticleSystem>();
            showFeedback.stopWatch.transform.DOScale(1.2f, 1f).SetLoops(-1, LoopType.Yoyo);
        }

        public void ReadMovementPlayer()
        {

            if (currentFuel <= 0 || !playerInputs.jumpInput)
            {
                rb.useGravity = true;
                playerController.isflying = false;
                efxJetPackOn(false);
            }

            if (playerController.isflying && playerInputs.jumpInput) 
            {
                HandleFly();
                return;
            }
            
            HandleFallingAndGround();
                        
            if (!playerController.onGroundPlayer || playerController.isJumping || playerController.isflying)
                return;

            HandleMovement();
            HandleRotation();
        }

        #region Movement, rotation, falling, jump and jetpack fly Handles
        private void HandleMovement() 
        {
            NormalizedDirections();
            directionToMove.y = 0;

            rb.velocity = directionToMove * settings.velocityMovement;
        }

        private void HandleRotation()
        {
            if (!playerController.onGroundPlayer)
                return;

            Vector3 dirTarget = Vector3.zero;
            float ballrot = settings.velocityMovement * 4;

            dirTarget = cameraGO.forward * playerInputs.moveX;
            dirTarget = dirTarget + cameraGO.right * playerInputs.moveY;
            dirTarget.Normalize();

            Quaternion rotTarget = transform.rotation;
            Quaternion smotarget = Quaternion.Slerp(transform.rotation, rotTarget, ballrot * Time.deltaTime);

            transform.rotation = smotarget;
        }

        public void HandleFallingAndGround()
        {
            Ray ray = new Ray(transform.position, -Vector3.up);
            RaycastHit hit;

            if (!playerController.onGroundPlayer && !playerController.isJumping && !playerController.isflying)
            {
                directionToMove.y = transform.position.y;
                float gravityGame = Physics.gravity.y / 10;
                airTimer += Time.deltaTime;
                rb.AddForce(-Vector3.up * airTimer * gravityGame);
            }
            if (Physics.Raycast(ray.origin, ray.direction, out hit, rayLenght, groundLayer))
            {
                rb.useGravity = true;
                RecoverFuel();
                airTimer = 0;
                playerController.onGroundPlayer = true;
                playerController.isJumping = false;
            }
            else
            {
                playerController.onGroundPlayer = false;
            }
        }

        public void HandleJetPack()
        {
            if (playerController.jetPackReady)
            {
                jetpack.SetActive(true);
                jetpack.transform.DOScale(1, 0.5f);
            }
        }

        public void Handlejump()
        {
            if (playerController.onGroundPlayer)
            {
                float jumpVel = Mathf.Sqrt(-2 * -150 * settings.jumpForce);
                Vector3 playerVel = directionToMove;
                playerVel.y = transform.position.y + jumpVel;
                rb.AddForce(playerVel, ForceMode.Impulse);
            }
        }
        
        public void HandleFly()
        {
            if (playerController.isJumping || !playerController.onGroundPlayer && currentFuel > 0)
            {
                efxJetPackOn(true);
                NormalizedDirections();
                rb.useGravity = false;
                Vector3 jetForce = new Vector3 (directionToMove.x, transform.position.y + settings.jetpackForce * 2, directionToMove.z);
                transform.DORotate(new Vector3(0,0,0), 0.5f);
                rb.velocity = directionToMove * settings.velocityMovement;
                rb.AddForce(jetForce, ForceMode.Impulse);
                currentFuel -= Time.deltaTime * 2;
                Fuelstatus(currentFuel);
            }
        }

        private void RecoverFuel()
        {
            if (!playerController.onGroundPlayer)
                return;

            if (currentFuel <= settings.fuel && playerController.jetPackReady)
            {
                currentFuel += Time.deltaTime * 2;
            }
            else if (currentFuel > settings.fuel && playerController.jetPackReady)
                currentFuel = settings.fuel;

            Fuelstatus(currentFuel);
        }
        
        private void NormalizedDirections()
        {
            directionToMove = cameraGO.forward * playerInputs.moveY;
            directionToMove = directionToMove + cameraGO.right * playerInputs.moveX;
            directionToMove.Normalize();
        }

        public void SetInitialPosition()
        {
            transform.DOScale(0, 0.5f);
            Invoke("SetBeginPosition", 0.5f);
        }

        private void SetBeginPosition()
        {
            transform.DOScale(1, 0.5f);
            transform.position = InitialPosition;
        }
        #endregion

        #region Aniamtion Efx and fuel feedback
        public void efxJetPackOn(bool state)
        {
            if (state)
            {
                foreach (ParticleSystem jet in efxJetpack)
                    jet.Play();
            }
            else
            {
                foreach (ParticleSystem jet in efxJetpack)
                    jet.Stop();
            }

        }

        public void NewFuel(float extraFuel)
        {
            measurer.transform.DOScaleZ(extraFuel, 0.5f);
        }
        //fuel stats
        private void Fuelstatus(float status)
        {
            Color greenCol = Color.green;
            Color redCol = Color.red;
            Color yellowCol = Color.yellow;

            Material colorWarning = measurer.GetComponentInChildren<MeshRenderer>().material;

            measurer.transform.DOScaleZ(status, 0.1f);

            if (status >= settings.fuel)
                colorWarning.DOColor(greenCol, 0.2f);

            if(status >= settings.fuel / 2 - 1 && status <= settings.fuel / 2 + 1)
                colorWarning.DOColor(yellowCol, 0.2f);

            if(status <= 1)
                colorWarning.DOColor(redCol, 0.2f);
        }
        #endregion
    }
}
