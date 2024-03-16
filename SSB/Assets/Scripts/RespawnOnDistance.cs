using UnityEngine;
using UnityEngine.Events;

namespace Oculus.Interaction.Samples
{
    public class RespawnOnDistance : MonoBehaviour
    {
        /// <summary>
        /// The player's transform.
        /// </summary>
        [SerializeField]
        [Tooltip("The player's transform.")]
        private Transform playerTransform;

        /// <summary>
        /// Respawn will happen when the distance to the player exceeds this value.
        /// </summary>
        [SerializeField]
        [Tooltip("Respawn will happen when the distance to the player exceeds this value.")]
        private float distanceThresholdForRespawn = 10.0f;

        /// <summary>
        /// UnityEvent triggered when a respawn occurs.
        /// </summary>
        [SerializeField]
        [Tooltip("UnityEvent triggered when a respawn occurs.")]
        private UnityEvent _whenRespawned = new UnityEvent();

         /// <summary>
        /// If the transform has an associated rigidbody, make it kinematic during this
        /// number of frames after a respawn, in order to avoid ghost collisions.
        /// </summary>
        [SerializeField]
        [Tooltip("If the transform has an associated rigidbody, make it kinematic during this number of frames after a respawn, in order to avoid ghost collisions.")]
        private int _sleepFrames = 0;

        public UnityEvent WhenRespawned => _whenRespawned;

        // cached starting transform
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Vector3 _initialScale;

        private TwoGrabFreeTransformer[] _freeTransformers;
        private Rigidbody _rigidBody;
        private int _sleepCountDown;

        protected virtual void OnEnable()
        {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            _initialScale = transform.localScale;
            _freeTransformers = GetComponents<TwoGrabFreeTransformer>();
            _rigidBody = GetComponent<Rigidbody>();
        }

        protected virtual void Update()
        {
            if (playerTransform == null) return;

            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer > distanceThresholdForRespawn)
            {
                Respawn();
                Debug.Log("Respawned");
            }
        }

        protected virtual void FixedUpdate()
        {
            if (_sleepCountDown > 0)
            {
                if (--_sleepCountDown == 0)
                {
                    _rigidBody.isKinematic = false;
                }
            }
        }

        public void Respawn()
        {
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
            transform.localScale = _initialScale;

            if (_rigidBody)
            {
                _rigidBody.velocity = Vector3.zero;
                _rigidBody.angularVelocity = Vector3.zero;

                if (!_rigidBody.isKinematic && _sleepFrames > 0)
                {
                    _sleepCountDown = _sleepFrames;
                    _rigidBody.isKinematic = true;
                }
            }

            foreach (var freeTransformer in _freeTransformers)
            {
                freeTransformer.MarkAsBaseScale();
            }

            _whenRespawned.Invoke();
        }
    }
}
