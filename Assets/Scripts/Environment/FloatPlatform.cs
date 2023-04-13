using UnityEngine;
using DG.Tweening;

namespace PruebaEdissonChaves
{
    public class FloatPlatform : MonoBehaviour
    {

        private void Start()
        {
            transform.DOMoveX(-3, 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        }
    }
}
