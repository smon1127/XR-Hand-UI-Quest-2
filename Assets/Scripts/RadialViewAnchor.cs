// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Microsoft.MixedReality.Toolkit.Utilities.Solvers
{
    /// <summary>
    /// RadialViewPoser solver locks a tag-along type object within a view cone
    /// </summary>
    [AddComponentMenu("Scripts/MRTK/SDK/RadialView")]
    public class RadialViewAnchor : Solver
    {

        public Transform anchors;
        public Transform anchorSolvers;
        public Interactable toggleSettings;
        public bool isFollow = false;
        public bool isAppActive = false;
        public bool toggleUi = false;
        public bool anchorTouched = false;
        public bool tiltedWindow = false;
        public float moveFollowLerpTime = 0.3f;
        public Color isFollowColor = new Vector4(0.14f, 0.21f, 0.63f, 1);
        public Color defaultColor = new Vector4(0, 0, 0, 1);
        public GameObject coloredObject;
        //public Transform anchorButton = null;
        public SolverHandler solverHandler;
        private Vector3 LocalOffset = new Vector3(0, 0, 0);


        [SerializeField]
        [Tooltip("XYZ offset for this object oriented with the TrackedObject/TargetTransform's forward. Mixing local and world offsets is not recommended. Local offsets are applied before world offsets.")]
        private Vector3 localOffsetHead = new Vector3(0, -1, 1);

        [SerializeField]
        [Tooltip("XYZ offset for this object oriented with the TrackedObject/TargetTransform's forward. Mixing local and world offsets is not recommended. Local offsets are applied before world offsets.")]
        private Vector3 localOffsetCustom = new Vector3(0, -1, 1);

        /// <summary>
        /// XYZ offset for this object in relation to the TrackedObject/TargetTransform.
        /// </summary>
        /// <remarks>
        /// Mixing local and world offsets is not recommended.
        /// </remarks>
        public Vector3 LocalOffsetHead
        {
            get { return localOffsetHead; }
            set { localOffsetHead = value; }
        }

        public Vector3 LocalOffsetCustom
        {
            get { return localOffsetCustom; }
            set { localOffsetCustom = value; }
        }

        [SerializeField]
        [Tooltip("The desired orientation of this object. Default sets the object to face the TrackedObject/TargetTransform. CameraFacing sets the object to always face the user.")]
        private SolverOrientationType orientationType = SolverOrientationType.FollowTrackedObject;

        /// <summary>
        /// The desired orientation of this object.
        /// </summary>
        /// <remarks>
        /// Default sets the object to face the TrackedObject/TargetTransform. CameraFacing sets the object to always face the user.
        /// </remarks>
        public SolverOrientationType OrientationType
        {
            get { return orientationType; }
            set { orientationType = value; }
        }

        [SerializeField]
        [Tooltip("XYZ offset for this object in worldspace, best used with the YawOnly orientationType. Mixing local and world offsets is not recommended. Local offsets are applied before world offsets.")]
        private Vector3 worldOffset = Vector3.zero;

        /// <summary>
        /// XYZ offset for this object in worldspace, best used with the YawOnly orientationType.
        /// </summary>
        /// <remarks>
        /// Mixing local and world offsets is not recommended.
        /// </remarks>
        public Vector3 WorldOffset
        {
            get { return worldOffset; }
            set { worldOffset = value; }
        }

        [SerializeField]
        [FormerlySerializedAs(oldName: "useAngleSteppingForWorldOffset")]
        [Tooltip("Lock the rotation to a specified number of steps around the tracked object.")]
        private bool useAngleStepping = false;

        /// <summary>
        /// Lock the rotation to a specified number of steps around the tracked object.
        /// </summary>
        public bool UseAngleStepping
        {
            get { return useAngleStepping; }
            set { useAngleStepping = value; }
        }

        [Range(2, 24)]
        [SerializeField]
        [Tooltip("The division of steps this object can tether to. Higher the number, the more snapping steps.")]
        private int tetherAngleSteps = 6;

        /// <summary>
        /// The division of steps this object can tether to. Higher the number, the more snapping steps.
        /// </summary>
        public int TetherAngleSteps
        {
            get { return tetherAngleSteps; }
            set
            {
                tetherAngleSteps = Mathf.Clamp(value, 2, 24);
            }
        }

        [SerializeField]
        [Tooltip("Which direction to position the element relative to: HeadOriented rolls with the head, HeadFacingWorldUp view direction but ignores head roll, and HeadMoveDirection uses the direction the head last moved without roll")]
        private RadialViewReferenceDirection referenceDirection = RadialViewReferenceDirection.FacingWorldUp;

        /// <summary>
        /// Which direction to position the element relative to:
        /// HeadOriented rolls with the head,
        /// HeadFacingWorldUp view direction but ignores head roll,
        /// and HeadMoveDirection uses the direction the head last moved without roll.
        /// </summary>
        public RadialViewReferenceDirection ReferenceDirection
        {
            get => referenceDirection;
            set => referenceDirection = value;
        }

        [SerializeField]
        [Tooltip("Min distance from eye to position element around, i.e. the sphere radius")]
        private float minDistance = 1f;

        /// <summary>
        /// Min distance from eye to position element around, i.e. the sphere radius.
        /// </summary>
        public float MinDistance
        {
            get => minDistance;
            set => minDistance = value;
        }

        [SerializeField]
        [Tooltip("Max distance from eye to element")]
        private float maxDistance = 2f;

        /// <summary>
        /// Max distance from eye to element.
        /// </summary>
        public float MaxDistance
        {
            get => maxDistance;
            set => maxDistance = value;
        }

        [SerializeField]
        [Tooltip("The element will stay at least this far away from the center of view")]
        private float minViewDegrees = 0f;

        /// <summary>
        /// The element will stay at least this far away from the center of view.
        /// </summary>
        public float MinViewDegrees
        {
            get => minViewDegrees;
            set => minViewDegrees = value;
        }

        [SerializeField]
        [Tooltip("The element will stay at least this close to the center of view")]
        private float maxViewDegrees = 30f;

        /// <summary>
        /// The element will stay at least this close to the center of view.
        /// </summary>
        public float MaxViewDegrees
        {
            get => maxViewDegrees;
            set => maxViewDegrees = value;
        }

        [SerializeField]
        [Tooltip("Apply a different clamp to vertical FOV than horizontal. Vertical = Horizontal * aspectV")]
        private float aspectV = 1f;

        /// <summary>
        /// Apply a different clamp to vertical FOV than horizontal. Vertical = Horizontal * AspectV.
        /// </summary>
        public float AspectV
        {
            get => aspectV;
            set => aspectV = value;
        }

        [SerializeField]
        [Tooltip("Option to ignore angle clamping")]
        private bool ignoreAngleClamp = false;

        /// <summary>
        /// Option to ignore angle clamping.
        /// </summary>
        public bool IgnoreAngleClamp
        {
            get => ignoreAngleClamp;
            set => ignoreAngleClamp = value;
        }

        [SerializeField]
        [Tooltip("Option to ignore distance clamping")]
        private bool ignoreDistanceClamp = false;

        /// <summary>
        /// Option to ignore distance clamping.
        /// </summary>
        public bool IgnoreDistanceClamp
        {
            get => ignoreDistanceClamp;
            set => ignoreDistanceClamp = value;
        }

        [SerializeField]
        [Tooltip("Ignore vertical movement and lock the Y position of the object")]
        private bool useFixedVerticalPosition = false;

        /// <summary>
        /// Ignore vertical movement and lock the Y position of the object.
        /// </summary>
        public bool UseFixedVerticalPosition
        {
            get => useFixedVerticalPosition;
            set => useFixedVerticalPosition = value;
        }

        [SerializeField]
        [Tooltip("Offset amount of the vertical position")]
        private float fixedVerticalPosition = -0.4f;

        /// <summary>
        /// Offset amount of the vertical position.
        /// </summary>
        public float FixedVerticalPosition
        {
            get => fixedVerticalPosition;
            set => fixedVerticalPosition = value;
        }

        [SerializeField]
        [Tooltip("If true, element will orient to ReferenceDirection, otherwise it will orient to ref position.")]
        private bool orientToReferenceDirection = false;

        /// <summary>
        /// If true, element will orient to ReferenceDirection, otherwise it will orient to ref position.
        /// </summary>
        public bool OrientToReferenceDirection
        {
            get => orientToReferenceDirection;
            set => orientToReferenceDirection = value;
        }

        /// <summary>
        /// Position to the view direction, or the movement direction, or the direction of the view cone.
        /// </summary>
        private Vector3 SolverReferenceDirection => SolverHandler.TransformTarget != null ? SolverHandler.TransformTarget.forward : Vector3.forward;

        /// <summary>
        /// The up direction to use for orientation.
        /// </summary>
        /// <remarks>Cone may roll with head, or not.</remarks>
        private Vector3 UpReference
        {
            get
            {
                Vector3 upReference = Vector3.up;

                if (referenceDirection == RadialViewReferenceDirection.ObjectOriented)
                {
                    upReference = SolverHandler.TransformTarget != null ? SolverHandler.TransformTarget.up : Vector3.up;
                }

                return upReference;
            }
        }

        private Vector3 ReferencePoint => SolverHandler.TransformTarget != null ? SolverHandler.TransformTarget.position : Vector3.zero;

        protected override void Start()
        {
            base.Start();
            solverHandler = gameObject.GetComponent<SolverHandler>();
            coloredObject.GetComponent<MeshRenderer>().material.SetColor("_RimColor", defaultColor);
            anchors.gameObject.SetActive(false);
        }

       

        /// <inheritdoc />
        public override void SolverUpdate()
        {
          

            if (toggleUi)
            {              

                //anchorButton.GetComponent<Interactable>().IsToggled = true;
                gameObject.SetActive(true);
                anchorSolvers.GetComponent<RadialView>().enabled = false;
            }
            else
            {
                //anchorButton.GetComponent<Interactable>().IsToggled = false;
                gameObject.SetActive(false);
                anchorSolvers.GetComponent<RadialView>().enabled = true;
            }

            if (isFollow)
            {
                coloredObject.GetComponent<MeshRenderer>().material.SetColor("_RimColor", isFollowColor);

                LocalOffset = LocalOffsetHead;
                solverHandler.TrackedTargetType = TrackedObjectType.Head;
                gameObject.GetComponent<Solver>().MoveLerpTime = moveFollowLerpTime;
                Vector3 anchorOffset = transform.position;
                anchorOffset -= LocalOffsetCustom;
                anchorSolvers.position = anchorOffset;
                anchorSolvers.localRotation = transform.localRotation;                  

                Vector3 goalPosition = WorkingPosition;

                if (ignoreAngleClamp)
                {
                    if (ignoreDistanceClamp)
                    {
                        goalPosition = transform.position;
                    }
                    else
                    {
                        GetDesiredOrientation_DistanceOnly(ref goalPosition);
                    }
                }
                else
                {
                    GetDesiredOrientation(ref goalPosition);
                }

                // Element orientation
                Vector3 refDirUp = UpReference;
                Quaternion goalRotation;

                if (orientToReferenceDirection)
                {
                    goalRotation = Quaternion.LookRotation(SolverReferenceDirection, refDirUp);
                }
                else
                {
                    goalRotation = Quaternion.LookRotation(goalPosition - ReferencePoint, refDirUp);
                }

                // If gravity aligned then zero out the x and z axes on the rotation
                if (referenceDirection == RadialViewReferenceDirection.GravityAligned)
                {
                    goalRotation.x = goalRotation.z = 0f;
                }

                if (UseFixedVerticalPosition)
                {
                    goalPosition.y = ReferencePoint.y + FixedVerticalPosition;
                }

                GoalPosition = goalPosition;
                GoalRotation = goalRotation;

            }
            else
            {
                coloredObject.GetComponent<MeshRenderer>().material.SetColor("_RimColor", defaultColor);
                LocalOffset = LocalOffsetCustom;
                solverHandler.TrackedTargetType = TrackedObjectType.CustomOverride;
                gameObject.GetComponent<Solver>().MoveLerpTime = 0.1f;


                Vector3 desiredPos = SolverHandler.TransformTarget != null ? SolverHandler.TransformTarget.position : Vector3.zero;

                Quaternion targetRot = SolverHandler.TransformTarget != null ? SolverHandler.TransformTarget.rotation : Quaternion.Euler(0, 1, 0);
                Quaternion yawOnlyRot = Quaternion.Euler(0, targetRot.eulerAngles.y, 0);
                desiredPos += (SnapToTetherAngleSteps(targetRot) * LocalOffset);
                desiredPos += (SnapToTetherAngleSteps(yawOnlyRot) * WorldOffset);

                Quaternion desiredRot = CalculateDesiredRotation(desiredPos);

                GoalPosition = desiredPos;
                GoalRotation = desiredRot;
            }
        }

        public void IsFollowing()
        {
            isFollow = !isFollow;
            SolverUpdate();

        }

        public void ToggleTiltedWindow() {
            tiltedWindow = !tiltedWindow;

            if (tiltedWindow)
            {
                solverHandler.AdditionalRotation = new Vector3(25,0,0);
            }
            else
            {
                solverHandler.AdditionalRotation = new Vector3(0, 0, 0);
            }
        }


        public void AnchorTouched(bool isTouching)
        {
            anchorTouched = isTouching;

            if (anchorTouched && !toggleUi)
            {
                isFollow = false;
                anchorSolvers.GetComponent<RadialView>().enabled = false;
            }

            if (anchorTouched && toggleUi)
            {
                isFollow = false;
            }
        }

        public void IsAppActive(bool isActive)
        {
            //anchors.gameObject.SetActive(isActive);
            toggleUi = isActive;            
            
            SolverUpdate();
        }

        private Quaternion SnapToTetherAngleSteps(Quaternion rotationToSnap)
        {
            if (!UseAngleStepping || SolverHandler.TransformTarget == null)
            {
                return rotationToSnap;
            }

            float stepAngle = 360f / tetherAngleSteps;
            int numberOfSteps = Mathf.RoundToInt(SolverHandler.TransformTarget.transform.eulerAngles.y / stepAngle);

            float newAngle = stepAngle * numberOfSteps;

            return Quaternion.Euler(rotationToSnap.eulerAngles.x, newAngle, rotationToSnap.eulerAngles.z);
        }

        private Quaternion CalculateDesiredRotation(Vector3 desiredPos)
        {
            Quaternion desiredRot = Quaternion.identity;

            switch (orientationType)
            {
                case SolverOrientationType.YawOnly:
                    float targetYRotation = SolverHandler.TransformTarget != null ? SolverHandler.TransformTarget.eulerAngles.y : 0.0f;
                    desiredRot = Quaternion.Euler(0f, targetYRotation, 0f);
                    break;
                case SolverOrientationType.Unmodified:
                    desiredRot = transform.rotation;
                    break;
                case SolverOrientationType.CameraAligned:
                    desiredRot = CameraCache.Main.transform.rotation;
                    break;
                case SolverOrientationType.FaceTrackedObject:
                    desiredRot = SolverHandler.TransformTarget != null ? Quaternion.LookRotation(SolverHandler.TransformTarget.position - desiredPos) : Quaternion.identity;
                    break;
                case SolverOrientationType.CameraFacing:
                    desiredRot = SolverHandler.TransformTarget != null ? Quaternion.LookRotation(CameraCache.Main.transform.position - desiredPos) : Quaternion.identity;
                    break;
                case SolverOrientationType.FollowTrackedObject:
                    desiredRot = SolverHandler.TransformTarget != null ? SolverHandler.TransformTarget.rotation : Quaternion.identity;
                    break;
                default:
                    Debug.LogError($"Invalid OrientationType for Orbital Solver on {gameObject.name}");
                    break;
            }

            if (UseAngleStepping)
            {
                desiredRot = SnapToTetherAngleSteps(desiredRot);
            }

            return desiredRot;
        }

        /// <summary>
        /// Optimized version of GetDesiredOrientation.
        /// </summary>
        private void GetDesiredOrientation_DistanceOnly(ref Vector3 desiredPos)
        {
            // TODO: There should be a different solver for distance constraint.
            // Determine reference locations and directions
            Vector3 refPoint = ReferencePoint;
            Vector3 elementPoint = transform.position;
            Vector3 elementDelta = elementPoint - refPoint;
            float elementDist = elementDelta.magnitude;
            Vector3 elementDir = elementDist > 0 ? elementDelta / elementDist : Vector3.one;

            // Clamp distance too
            float clampedDistance = Mathf.Clamp(elementDist, minDistance, maxDistance);

            if (!clampedDistance.Equals(elementDist))
            {
                desiredPos = refPoint + clampedDistance * elementDir;
            }
        }

        private void GetDesiredOrientation(ref Vector3 desiredPos)
        {
            // Determine reference locations and directions
            Vector3 direction = SolverReferenceDirection;
            Vector3 upDirection = UpReference;
            Vector3 referencePoint = ReferencePoint;
            Vector3 elementPoint = transform.position;
            Vector3 elementDelta = elementPoint - referencePoint;
            float elementDist = elementDelta.magnitude;
            Vector3 elementDir = elementDist > 0 ? elementDelta / elementDist : Vector3.one;

            // Generate basis: First get axis perpendicular to reference direction pointing toward element
            Vector3 perpendicularDirection = (elementDir - direction);
            perpendicularDirection -= direction * Vector3.Dot(perpendicularDirection, direction);
            perpendicularDirection.Normalize();

            // Calculate the clamping angles, accounting for aspect (need the angle relative to view plane)
            float heightToViewAngle = Vector3.Angle(perpendicularDirection, upDirection);
            float verticalAspectScale = Mathf.Lerp(aspectV, 1f, Mathf.Abs(Mathf.Sin(heightToViewAngle * Mathf.Deg2Rad)));

            // Calculate the current angle
            float currentAngle = Vector3.Angle(elementDir, direction);
            float currentAngleClamped = Mathf.Clamp(currentAngle, minViewDegrees * verticalAspectScale, maxViewDegrees * verticalAspectScale);

            // Clamp distance too, if desired
            float clampedDistance = ignoreDistanceClamp ? elementDist : Mathf.Clamp(elementDist, minDistance, maxDistance);

            // If the angle was clamped, do some special update stuff
            if (currentAngle != currentAngleClamped)
            {
                float angRad = currentAngleClamped * Mathf.Deg2Rad;

                // Calculate new position
                desiredPos = referencePoint + clampedDistance * (direction * Mathf.Cos(angRad) + perpendicularDirection * Mathf.Sin(angRad));
            }
            else if (!clampedDistance.Equals(elementDist))
            {
                // Only need to apply distance
                desiredPos = referencePoint + clampedDistance * elementDir;
            }
        }
    }
}
