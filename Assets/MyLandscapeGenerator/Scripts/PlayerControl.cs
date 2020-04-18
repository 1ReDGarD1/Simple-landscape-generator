using UnityEngine;

namespace MyLandscapeGenerator.Scripts
{
    public sealed class PlayerControl : MonoBehaviour
    {
        [SerializeField]
        private float _forwardSpeed;

        [SerializeField]
        private float _rotationSpeed;

        private void Update()
        {
            var curTransform = transform;

            //forward
            var forwardDelta = Time.deltaTime * _forwardSpeed;
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space))
            {
                curTransform.position += curTransform.forward * forwardDelta;
            }

            //rotation
            var rotationDelta = Time.deltaTime * _rotationSpeed;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0, rotationDelta, 0);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0, -rotationDelta, 0);
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.Rotate(-rotationDelta, 0, 0);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.Rotate(rotationDelta, 0, 0);
            }
        }
    }
}