using UnityEngine;
using DG.Tweening;

namespace PruebaEdissonChaves
{
    public class Settings : MonoBehaviour
    {
        //Welcome to gameplay settings
        //The variables here are call from other scripts
        //Long names in variables are used only for this test

        [Header("Environment Variables")]
        //public int grassPerMeter;
        public float pointsToBreakWall;
        [Tooltip("Distance from item to player position for Ray Activation")]
        public float distanceToActivateItemRay;
        [Tooltip("Ray lenght")]
        public float distanceToPickUpItem;

        [Header("Player Settings")]
        public float jumpForce;
        [Tooltip("Velocity of the ball movement")]
        public float velocityMovement; //Recommended 1.5
        [Tooltip("Quantity of fuel, this value is modified in RUNTIME by +fuel items")]
        public float fuel;
        public float jetpackForce;
        public float camerafollowSpeed;
        [Range(-3f, 6f)]
        public float heightCamera = 1f;
        [Range (-2f, -8f)]
        public float zoomCamera = -1f;


        [Header("Enable game?")]
        public bool inGame;

        public static Settings instance {get; private set;}
        public GameObject player;
        public PlayerInputs plyInputs;
        public PlayerMotion plyMotion;
        public PlayerController plyController;
        public FollowPlayer flwPlayer;
        public ShowFeedBack showFeedback;

        private void Awake()
        {
            if (instance != null && instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                instance = this; 
            }
            SetVars();
            DOTween.Init();
        }

        private void SetVars()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            plyInputs = player.GetComponent<PlayerInputs>();
            plyMotion = player.GetComponent<PlayerMotion>();
            plyController = player.GetComponent<PlayerController>();
            flwPlayer = GameObject.FindGameObjectWithTag("CamObject").GetComponent<FollowPlayer>();
            showFeedback = FindObjectOfType<ShowFeedBack>();
        }

    }
}