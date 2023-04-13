using UnityEngine;
using DG.Tweening;

namespace PruebaEdissonChaves
{
    public class PlayerMotion : MonoBehaviour
    {
        private Vector3 directionToMove;
        private Transform cameraGO;
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
            cameraGO = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            rb = GetComponent<Rigidbody>();
            InitialPosition = transform.position;

            efxJetpack = jetpack.GetComponentsInChildren<ParticleSystem>();
        }

        void Start()
        {
            Settings.instance.showFeedback.stopWatch.transform.DOScale(1.2f, 1f).SetLoops(-1, LoopType.Yoyo);
        }

        public void ReadMovementPlayer()
        {

            if (currentFuel <= 0 || !Settings.instance.plyInputs.jumpInput)
            {
                rb.useGravity = true;
                Settings.instance.plyController.isflying = false;
                efxJetPackOn(false);
            }

            if (Settings.instance.plyController.isflying && Settings.instance.plyInputs.jumpInput) 
            {
                HandleFly();
                return;
            }
            
            HandleFallingAndGround();
                        
            if (!Settings.instance.plyController.onGroundPlayer || Settings.instance.plyController.isJumping || Settings.instance.plyController.isflying)
                return;

            HandleMovement();
            HandleRotation();
        }

        #region Movement, rotation, falling, jump and jetpack fly Handles
        private void HandleMovement() 
        {
            NormalizedDirections();
            directionToMove.y = 0;

            rb.velocity = directionToMove * Settings.instance.velocityMovement;
        }

        private void HandleRotation()
        {
            if (!Settings.instance.plyController.onGroundPlayer)
                return;

            Vector3 dirTarget = Vector3.zero;
            float ballrot = Settings.instance.velocityMovement * 4;

            dirTarget = cameraGO.forward * Settings.instance.plyInputs.moveX;
            dirTarget = dirTarget + cameraGO.right * Settings.instance.plyInputs.moveY;
            dirTarget.Normalize();

            Quaternion rotTarget = transform.rotation;
            Quaternion smotarget = Quaternion.Slerp(transform.rotation, rotTarget, ballrot * Time.deltaTime);

            transform.rotation = smotarget;
        }

        public void HandleFallingAndGround()
        {
            Ray ray = new Ray(transform.position, -Vector3.up);
            RaycastHit hit;

            if (!Settings.instance.plyController.onGroundPlayer && !Settings.instance.plyController.isJumping && !Settings.instance.plyController.isflying)
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
                Settings.instance.plyController.onGroundPlayer = true;
                Settings.instance.plyController.isJumping = false;
            }
            else
            {
                Settings.instance.plyController.onGroundPlayer = false;
            }
        }

        public void HandleJetPack()
        {
            if (Settings.instance.plyController.jetPackReady)
            {
                jetpack.SetActive(true);
                jetpack.transform.DOScale(1, 0.5f);
            }
        }

        public void Handlejump()
        {
            if (Settings.instance.plyController.onGroundPlayer)
            {
                float jumpVel = Mathf.Sqrt(-2 * -150 * Settings.instance.jumpForce);
                Vector3 playerVel = directionToMove;
                playerVel.y = transform.position.y + jumpVel;
                rb.AddForce(playerVel, ForceMode.Impulse);
            }
        }
        
        public void HandleFly()
        {
            if (Settings.instance.plyController.isJumping || !Settings.instance.plyController.onGroundPlayer && currentFuel > 0)
            {
                efxJetPackOn(true);
                NormalizedDirections();
                rb.useGravity = false;
                Vector3 jetForce = new Vector3 (directionToMove.x, transform.position.y + Settings.instance.jetpackForce * 2, directionToMove.z);
                transform.DORotate(new Vector3(0,0,0), 0.5f);
                rb.velocity = directionToMove * Settings.instance.velocityMovement;
                rb.AddForce(jetForce, ForceMode.Impulse);
                currentFuel -= Time.deltaTime * 2;
                Fuelstatus(currentFuel);
            }
        }

        private void RecoverFuel()
        {
            if (!Settings.instance.plyController.onGroundPlayer)
                return;

            if (currentFuel <= Settings.instance.fuel && Settings.instance.plyController.jetPackReady)
            {
                currentFuel += Time.deltaTime * 2;
            }
            else if (currentFuel > Settings.instance.fuel && Settings.instance.plyController.jetPackReady)
                currentFuel = Settings.instance.fuel;

            Fuelstatus(currentFuel);
        }
        
        private void NormalizedDirections()
        {
            directionToMove = cameraGO.forward * Settings.instance.plyInputs.moveY;
            directionToMove = directionToMove + cameraGO.right * Settings.instance.plyInputs.moveX;
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

            if (status >= Settings.instance.fuel)
                colorWarning.DOColor(greenCol, 0.2f);

            if(status >= Settings.instance.fuel / 2 - 1 && status <= Settings.instance.fuel / 2 + 1)
                colorWarning.DOColor(yellowCol, 0.2f);

            if(status <= 1)
                colorWarning.DOColor(redCol, 0.2f);
        }
        #endregion
    }
}
