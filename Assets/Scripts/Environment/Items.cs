using UnityEngine;
using DG.Tweening;

namespace PruebaEdissonChaves
{
    public class Items : MonoBehaviour
    {
        [SerializeField] private GameObject Circle;
        [SerializeField] private float velR;
        [SerializeField] private bool plusfuel;
        [SerializeField] private bool jetPack;
        [SerializeField] private bool itemActive;

        private GameObject player;
        private PlayerController playerController;
        private PlayerMotion playerMotion;
        private ShowFeedBack showFeedback;
        private Settings settings;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
            playerMotion = player.GetComponent<PlayerMotion>();
            showFeedback = FindObjectOfType<ShowFeedBack>();
            settings = FindObjectOfType<Settings>();
            itemActive = true;
        }

        private void Update()
        {
            if (itemActive)
            {
                Circle.transform.Rotate(Vector3.forward * velR, Space.Self);

                if (Vector3.Distance(transform.position, player.transform.position) < settings.distanceToActivateItemRay)
                {

                    // Ray to check where's the player
                    Ray ray = new Ray(transform.position, player.transform.position - transform.position);
                    RaycastHit hit;
                    Debug.DrawRay(ray.origin, ray.direction * settings.distanceToPickUpItem, Color.red);
                    
                    if (Physics.Raycast(ray, out hit, settings.distanceToPickUpItem))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            if (jetPack)
                            {
                                playerController.jetPackReady = true;
                                playerMotion.HandleJetPack();
                                showFeedback.SetMsg("Congrats you pick up " + gameObject.name + ". " + "Press space bar in the air to use it.");
                                playerMotion.currentFuel = settings.fuel;
                                playerMotion.NewFuel(playerMotion.measurer.transform.localScale.z + 1f);
                            }
                            if (plusfuel)
                            {
                                settings.GetComponent<Settings>().fuel++;
                                showFeedback.forceAccumulated++;
                                showFeedback.SetMsg("Congrats you pick up " + gameObject.name + "item");
                            }
                            Vector3 sizeF = new Vector3(0, 0, 0);
                            transform.DOScale(0, 0.5f);
                            itemActive = false;
                        }
                    }
                }
            }
            if (!itemActive)
            {
                Invoke("DestroyItem", 0.5f);
            }
        }

        private void DestroyItem()
        {
            gameObject.SetActive(false);
        }
    }
}
