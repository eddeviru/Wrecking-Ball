using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace PruebaEdissonChaves
{
    public class ShowFeedBack : MonoBehaviour
    {
        private Settings settings;
        private PlayerMotion playerMotion;
        [Header("Texts UI")]
        public TextMeshProUGUI messageFeedback;
        public TextMeshProUGUI pointsToBreakWall;
        public TextMeshProUGUI currentFuel;
        public TextMeshProUGUI stopWatch;
        public float forceAccumulated;
        private bool begincount;
        private float timermatch;

        private void Awake()
        {
            settings = FindObjectOfType<Settings>();
            playerMotion = FindObjectOfType<PlayerMotion>();
            StartCoroutine(msgWelcome());
        }

        public void SetMsg(string msg)
        {
            messageFeedback.transform.localScale = new Vector3(0, 0, 0);
            messageFeedback.text = msg;
            Sequence msgEFX = DOTween.Sequence();
            msgEFX.Append(messageFeedback.transform.DOScale(1, 0.5f));
            msgEFX.Append(messageFeedback.transform.DOScale(0, 0.5f).SetDelay(2f).SetEase(Ease.InBounce));
        }

        private void Update()
        {
            if (settings.inGame)
            {
                // texts UI Feedback
                pointsToBreakWall.text = forceAccumulated + "/" + settings.pointsToBreakWall;
                currentFuel.text = playerMotion.currentFuel.ToString("F0") + "/" + settings.fuel;

                if (begincount)
                {
                    timermatch += Time.deltaTime;
                    string result = $"{(int)timermatch / 60}:{timermatch % 60:00.000}";
                    stopWatch.text = result;
                }
            }
        }

        #region Courutine for begin game
        private IEnumerator msgWelcome()
        {
            SetMsg("Get to canon with most of +fuel items and destroy the wall.");

            yield return new WaitForSeconds(3);

            SetMsg("Go!");
            begincount = true;
            settings.inGame = true;
        }
        #endregion
    }
}
