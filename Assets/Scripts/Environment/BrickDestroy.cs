using UnityEngine;
using DG.Tweening;

namespace PruebaEdissonChaves
{
    public class BrickDestroy : MonoBehaviour
    {
        private float limitD;
        private float timeS;
        private bool checkScale;

        private void Start()
        {
            limitD = Random.Range(-8f, -15f);
            timeS = Random.Range(0.5f, 0.9f);
        }
        private void Update()
        {
            if (gameObject.transform.position.y <= limitD)
            {
                if(!checkScale)
                {
                    Invoke("Destroy", 0.5f);
                    checkScale = true;
                }
            }
        }

        private void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}
