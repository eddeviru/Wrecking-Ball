using UnityEngine;
using DG.Tweening;

namespace PruebaEdissonChaves
{
    public class Wall : MonoBehaviour
    {
        private Vector3 FinalPos;
        private bool checkBrick;

        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, Settings.instance.player.transform.position) < 8)
            {
                if (Settings.instance.showFeedback.forceAccumulated >= Settings.instance.pointsToBreakWall)
                {
                    // Ray to check where's the player
                    Ray ray = new Ray(transform.position, Settings.instance.player.transform.position - transform.position);
                    RaycastHit hit;
                    Debug.DrawRay(ray.origin, ray.direction * 3, Color.red);

                    if (Physics.Raycast(ray, out hit, 4))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            FinalPos = Settings.instance.player.transform.position;
                            Transform[] bricks = GetComponentsInChildren<Transform>();
                            if(!checkBrick)
                            {
                                foreach (Transform brick in bricks)
                                {
                                    brick.gameObject.transform.SetParent(null);
                                    brick.gameObject.AddComponent<Rigidbody>();
                                    brick.gameObject.GetComponent<Rigidbody>().AddExplosionForce(8, Settings.instance.player.transform.position, 100f);
                                    brick.gameObject.AddComponent<BrickDestroy>();
                                }
                                Settings.instance.showFeedback.SetMsg("Wow, you break the wall! your time was: " + Settings.instance.showFeedback.stopWatch.text);
                                checkBrick = true;
                            }
                        }
                    }
                }
                else {
                    //GameOver
                    Settings.instance.showFeedback.SetMsg("GAME OVER, try again");
                }
                LastWords();
            }
        }

        private void LastWords()
        {
            Settings.instance.player.transform.DOScale(0, 1f);
            Settings.instance.player.transform.position = FinalPos;
            Settings.instance.plyMotion.efxJetPackOn(false);
            Settings.instance.plyMotion.rb.useGravity = false;
            Settings.instance.plyMotion.rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
