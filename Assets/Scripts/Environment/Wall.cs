using UnityEngine;
using DG.Tweening;

namespace PruebaEdissonChaves
{
    public class Wall : MonoBehaviour
    {
        private GameObject player;
        private Settings settings;
        private PlayerMotion playerMotion;
        private ShowFeedBack showFeedback;
        private Vector3 FinalPos;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            settings = FindObjectOfType<Settings>();
            playerMotion = player.GetComponent<PlayerMotion>();
            showFeedback = FindObjectOfType<ShowFeedBack>();
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 8)
            {
                if (showFeedback.forceAccumulated >= settings.pointsToBreakWall)
                {
                    // Ray to check where's the player
                    Ray ray = new Ray(transform.position, player.transform.position - transform.position);
                    RaycastHit hit;
                    Debug.DrawRay(ray.origin, ray.direction * 3, Color.red);

                    if (Physics.Raycast(ray, out hit, 3))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            FinalPos = player.transform.position;
                            Transform[] bricks = GetComponentsInChildren<Transform>();

                            foreach (Transform brick in bricks)
                            {
                                brick.gameObject.AddComponent<Rigidbody>();
                                brick.gameObject.GetComponent<Rigidbody>().AddExplosionForce(2, player.transform.position, 1f);
                                brick.gameObject.AddComponent<BrickDestroy>();
                            }
                            showFeedback.SetMsg("Wow, you break the wall! your time was: " );
                        }
                    }
                }
                else {
                    //GameOver
                    showFeedback.SetMsg("GAME OVER, try again");
                }
                LastWords();
            }
        }

        private void LastWords()
        {
            player.transform.DOScale(0, 1f);
            player.transform.position = FinalPos;
            playerMotion.efxJetPackOn(false);
            playerMotion.rb.useGravity = false;
            playerMotion.rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
