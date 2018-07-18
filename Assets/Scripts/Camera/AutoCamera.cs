using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HaruScene
{

    public class AutoCamera : MonoBehaviour
    {

        public Transform target;
        public float damping = 0.1f;
        public float lookAheadFactor = 1;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position;
            m_OffsetZ = transform.position.z - target.position.z;
            transform.parent = null;
        }

        // Update is called once per frame
        private void Update()
        {
            TraceTarget(Time.deltaTime);
        }

        private void TraceTarget(float deltaTime)
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, deltaTime * lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            newPos.y = Mathf.Clamp(newPos.y, 9.6f, 182.4f);
            transform.position = newPos;

            m_LastTargetPosition = target.position;
        }
    }
}
