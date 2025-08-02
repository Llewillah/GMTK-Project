using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeVerlet : MonoBehaviour
{
    [Header("Rope")]
    [SerializeField] private int numOfRopeSeg = 50;
    [SerializeField] private float ropeSegLength = 0.225f;
    [SerializeField] private Texture tex;

    [Header("Physics")]
    [SerializeField] private Vector2 gravityForce = new Vector2(0f, -2f);
    [SerializeField] private float dampingFactor = 0.98f;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float collisionRadius = 0.1f;
    [SerializeField] private float bounceFactor = 0.1f;
    [SerializeField] private float correctionClamp = 0.1f;

    [Header("Constraints")]
    [SerializeField] private int numOfConstraints = 50;
    [SerializeField] private Transform anchorObject, player;

    [Header("Optimisations")]
    [SerializeField] private int collisionSegmentInterval = 2;

    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();

    private Vector3 ropeStartPoint;

    private void Awake()
    {
       lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numOfRopeSeg;
        //lineRenderer.material.SetTexture("_MainText", tex);

        ropeStartPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        for (int i = 0; i < numOfRopeSeg; i++) 
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLength;
        }
    }

    private void Update()
    {
        DrawRope();
    }

    private void FixedUpdate()
    {
        Simulate();

        for (int i = 0; i < numOfConstraints; i++) 
        {
            ApplyConstraints();

            if (i % collisionSegmentInterval == 0)
            {
                HandleCollisions();
            }
        }
    }
    private void DrawRope() 
    {
        Vector3[] ropePositions = new Vector3[numOfRopeSeg];
        for (int i = 0; i < numOfRopeSeg; i++) 
        {
            ropePositions[i] = ropeSegments[i].currentPosition;
        }

        lineRenderer.SetPositions(ropePositions);
    }

    private void Simulate()
    {
        for (int i = 0; i < ropeSegments.Count; i++) 
        { 
            RopeSegment segment = ropeSegments[i];
            Vector2 velocity = (segment.currentPosition - segment.oldPosition) * dampingFactor;

            segment.oldPosition = segment.currentPosition;
            segment.currentPosition += velocity;
            segment.currentPosition += gravityForce * Time.fixedDeltaTime;
            ropeSegments[i] = segment;
        }
    }

    private void ApplyConstraints() 
    {
        RopeSegment firstSeg = ropeSegments[0];
        firstSeg.currentPosition = player.position;
        ropeSegments[0] = firstSeg;

        RopeSegment lastSeg = ropeSegments[numOfRopeSeg - 1];
        lastSeg.currentPosition = anchorObject.position;
        ropeSegments[numOfRopeSeg - 1] = lastSeg;

        for (int i = 0; i < numOfRopeSeg - 1; i++) 
        { 
            RopeSegment curSeg = ropeSegments[i];
            RopeSegment nextSeg = ropeSegments[i + 1];

            float dist = (curSeg.currentPosition - nextSeg.currentPosition).magnitude;
            float difference = (dist - ropeSegLength);

            Vector2 changeDir = (curSeg.currentPosition - nextSeg.currentPosition).normalized;
            Vector2 changeVector = changeDir * difference;

            if (i != 0)
            {
                curSeg.currentPosition -= (changeVector * 0.5f);
                nextSeg.currentPosition += (changeVector * 0.5f);
            }
            else if (i == numOfRopeSeg - 2) 
            {
                curSeg.currentPosition += changeVector;
            }
            else
            {
                nextSeg.currentPosition += changeVector;
            }

            ropeSegments[i] = curSeg;
            ropeSegments[i + 1] = nextSeg;
        }
    }

    private void HandleCollisions() 
    {
        for (int i = 1; i < ropeSegments.Count; i++) 
        {
            RopeSegment segment = ropeSegments[i];
            Vector2 velocity = segment.currentPosition - segment.oldPosition;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(segment.currentPosition, collisionRadius, collisionMask);

            foreach (Collider2D collider in colliders) 
            {
                Vector2 closestPoint = collider.ClosestPoint(segment.currentPosition);
                float distance = Vector2.Distance(segment.currentPosition, closestPoint);

                if (distance < collisionRadius) 
                {
                    Vector2 normal = (segment.currentPosition - closestPoint).normalized;
                    if (normal == Vector2.zero) 
                    {
                        normal = (segment.currentPosition - (Vector2)collider.transform.position).normalized;
                    }

                    float depth = collisionRadius - distance;
                    segment.currentPosition += normal * depth;

                    velocity = Vector2.Reflect(velocity, normal) * bounceFactor;
                }
            }

            segment.oldPosition = segment.currentPosition - velocity;
            ropeSegments[i] = segment;
        }
    }
    public struct RopeSegment 
    {
        public Vector2 currentPosition;
        public Vector2 oldPosition;

        public RopeSegment(Vector2 pos) 
        { 
            currentPosition = pos;
            oldPosition = pos;
        }
    }
}
