using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace PruebaEdissonChaves
{
    public class ShowFeedBack : MonoBehaviour
    {
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
            if (Settings.instance.inGame)
            {
                // texts UI Feedback
                pointsToBreakWall.text = forceAccumulated + "/" + Settings.instance.pointsToBreakWall;
                currentFuel.text = Settings.instance.plyMotion.currentFuel.ToString("F0") + "/" + Settings.instance.fuel;

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
            Settings.instance.inGame = true;
        }
        #endregion
    }
}
