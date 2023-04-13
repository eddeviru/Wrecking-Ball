using UnityEngine;

namespace PruebaEdissonChaves
{
    public class FollowPlayer : MonoBehaviour
    {
        //Player position to follow
        public Transform targetPos;
        private Vector3 cameraVel = Vector3.zero;
        private Transform cameraPivot;
        private Transform cameraGO;

        private void Awake()
        {
            cameraPivot = GameObject.FindGameObjectWithTag("CamPivot").GetComponent<Transform>();
            cameraGO = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            targetPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }


        public void FollowTarget()
        {
            cameraPivot.localPosition = new Vector3 (0, Settings.instance.heightCamera, 0);
            cameraGO.localPosition = new Vector3(0, 0, Settings.instance.zoomCamera);
            cameraGO.LookAt(targetPos);

            Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetPos.position, ref cameraVel, Settings.instance.camerafollowSpeed);
            if(Settings.instance.inGame)
                transform.position = targetPosition;
        }
    }
}
