using UnityEngine;

namespace Unity.Cinemachine
{
    /// <summary>
    /// A simplified version of group framing to avoid layout issues.
    /// This script adjusts camera framing for a target group while maintaining performance
    /// and ensures players remain within the camera's view.
    /// </summary>
    [AddComponentMenu("Cinemachine/Procedural/Extensions/Simple Cinemachine Group Framing")]
    [ExecuteAlways]
    public class SimpleCinemachineGroupFraming : CinemachineExtension
    {
        [SerializeField] private CinemachineCamera virtualCamera;
        [SerializeField] private CinemachineTargetGroup targetGroup;

        [Tooltip("How much of the screen to fill with the bounding box of the targets.")]
        [Range(0, 2)]
        public float framingSize = 0.8f;

        [Tooltip("Damping for camera adjustments.")]
        public float damping = 2f;

        [Tooltip("Maximum view boundaries for the players.")]
        public Vector2 minBounds;
        public Vector2 maxBounds;

        private void LateUpdate()
        {
            if (targetGroup == null || targetGroup.IsEmpty)
                return;

            FrameGroup();
            ClampPlayerPositions();
        }

        private void FrameGroup()
        {
            Matrix4x4 cameraMatrix = Matrix4x4.TRS(virtualCamera.transform.position, virtualCamera.transform.rotation, Vector3.one);
            Bounds groupBounds = targetGroup.GetViewSpaceBoundingBox(cameraMatrix, true);
            Vector3 desiredPosition = CalculateDesiredPosition(groupBounds);
            AdjustCamera(desiredPosition);
        }

        private Vector3 CalculateDesiredPosition(Bounds groupBounds)
        {
            Vector3 center = groupBounds.center;
            float distance = Mathf.Max(groupBounds.extents.x, groupBounds.extents.y, groupBounds.extents.z) * framingSize;

            return center + new Vector3(0, 0, -distance);
        }

        private void AdjustCamera(Vector3 desiredPosition)
        {
            Vector3 currentPosition = virtualCamera.transform.position;
            virtualCamera.transform.position = Vector3.Lerp(currentPosition, desiredPosition, Time.deltaTime * damping);
            virtualCamera.transform.LookAt(targetGroup.transform.position);
        }

        private void ClampPlayerPositions()
        {
            foreach (Transform player in targetGroup.transform)
            {
                Vector3 clampedPosition = player.position;

                // Clamp the position within the specified bounds
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
                clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBounds.y, maxBounds.y); // Assuming Y is up and Z is forward in 3D space

                player.position = clampedPosition;
            }
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            // Placeholder for future enhancements
            if (stage == CinemachineCore.Stage.Body)
            {
                // Additional camera state adjustments can be implemented here
            }
        }
    }
}