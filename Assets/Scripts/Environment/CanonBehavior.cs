using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace PruebaEdissonChaves
{
    public class CanonBehavior : MonoBehaviour
    {
        [Header("Canon Settings")]
        [Tooltip("Put a gameobject to aim")]
        [SerializeField] private GameObject targetCanon;
        private GameObject cameraGO;
        [SerializeField] private GameObject referencePoint;
        private Settings settings;

        private void Start()
        {
            cameraGO = FindObjectOfType<FollowPlayer>().gameObject;
            settings = FindObjectOfType<Settings>();
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, Settings.instance.player.transform.position) < 5)
            {

                // Ray to check where's the player
                Ray ray = new Ray(transform.position, Settings.instance.player.transform.position - transform.position);
                RaycastHit hit;
                Debug.DrawRay(ray.origin, ray.direction * 3, Color.red);

                if (Physics.Raycast(ray, out hit, 2))
                {
                    if (hit.transform.tag == "Player")
                    {
                        //Aim and shoot
                        Settings.instance.player.transform.position = transform.position;
                        Settings.instance.player.transform.DOScale(0, 0.2f);
                        StartCoroutine(Shoot());
                    }
                }
            }
        }

        private IEnumerator Shoot()
        {
            targetCanon.SetActive(true);
            transform.LookAt(targetCanon.transform);
            settings.inGame = false;
            
            cameraGO.transform.DOMove(referencePoint.transform.position, 0.8f);
            yield return new WaitForSeconds(1f);
            Settings.instance.player.transform.DOScale(1, 0.2f);
            Settings.instance.player.transform.DOMove(targetCanon.transform.position, 0.5f);
            this.enabled = false;
        }

    }
}

