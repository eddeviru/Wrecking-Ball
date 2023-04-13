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

        private void Awake()
        {
            itemActive = true;
        }

        private void Update()
        {
            if (itemActive)
            {
                Circle.transform.Rotate(Vector3.forward * velR, Space.Self);

                if (Vector3.Distance(transform.position, Settings.instance.player.transform.position) < Settings.instance.distanceToActivateItemRay)
                {

                    // Ray to check where's the player
                    Ray ray = new Ray(transform.position, Settings.instance.player.transform.position - transform.position);
                    RaycastHit hit;
                    Debug.DrawRay(ray.origin, ray.direction * Settings.instance.distanceToPickUpItem, Color.red);
                    
                    if (Physics.Raycast(ray, out hit, Settings.instance.distanceToPickUpItem))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            if (jetPack)
                            {
                                Settings.instance.plyController.jetPackReady = true;
                                Settings.instance.plyMotion.HandleJetPack();
                                Settings.instance.showFeedback.SetMsg("You pick up " + gameObject.name + ". " + "Press space bar in the air to use it.");
                                Settings.instance.plyMotion.currentFuel = Settings.instance.fuel;
                                Settings.instance.plyMotion.NewFuel(Settings.instance.plyMotion.measurer.transform.localScale.z + 1f);
                            }
                            if (plusfuel)
                            {
                                Settings.instance.GetComponent<Settings>().fuel++;
                                Settings.instance.showFeedback.forceAccumulated++;
                                Settings.instance.showFeedback.SetMsg("You pick up " + gameObject.name + " " + "item");
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
