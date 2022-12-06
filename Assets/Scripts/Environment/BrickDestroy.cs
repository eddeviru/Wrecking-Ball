using UnityEngine;
using DG.Tweening;

namespace PruebaEdissonChaves
{
    public class BrickDestroy : MonoBehaviour
    {
        private float limitD;
        private float timeS;

        private void Start()
        {
            limitD = Random.Range(-9, -12);
            timeS = Random.Range(0.5f, 0.9f);
        }
        private void Update()
        {
            if (gameObject.transform.position.y <= limitD)
            {
                transform.DOScale(0, timeS);
                Invoke("Destroy", timeS);
            }
        }

        private void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}
