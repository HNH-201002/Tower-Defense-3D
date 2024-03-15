﻿using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class Archer : Ranged, IFire
{
    [SerializeField] private GameObject[] characterPrefab;
    [SerializeField] private float turnSpeed = 5f; // Tốc độ xoay mới cho cả nhân vật và mũi tên
    private Vector3 lastPosition;
    private Vector3 lastLastPosition;
    public bool ContinueFiring { get { return false; } }

    public void Fire(Transform projectile, Transform target)
    {
        lastPosition = projectile.position;
        lastLastPosition = lastPosition;
        if(characterPrefab.Length > 1)
        {
            RotateTowardsTarget(characterPrefab[0].transform, target.position);
            RotateTowardsTarget(characterPrefab[1].transform, target.position);
        }
        else
        {
            RotateTowardsTarget(characterPrefab[0].transform, target.position);
        }
    }

    public void UpdateFiring(Transform projectile, Transform target)
    {
        projectile.position = Vector3.MoveTowards(projectile.position, target.position, _projectileSpeed * Time.deltaTime);
        // Calculate the direction based on the last two positions
        Vector3 arrowDirection = lastPosition - lastLastPosition;

        // Check if the arrow has moved to prevent setting the forward vector to zero
        if (arrowDirection != Vector3.zero)
        {
            projectile.up = -arrowDirection.normalized;
        }

        // Update the last positions for the next frame
        lastLastPosition = lastPosition;
        lastPosition = projectile.position;
        if (characterPrefab.Length > 1)
        {
            RotateTowardsTarget(characterPrefab[0].transform, target.position);
            RotateTowardsTarget(characterPrefab[1].transform, target.position);
        }
        else
        {
            RotateTowardsTarget(characterPrefab[0].transform, target.position);
        }
    }
    private void RotateTowardsTarget(Transform character, Vector3 targetPosition)
    {
        Vector3 directionToTarget = -(targetPosition - character.position).normalized;
        directionToTarget.y = 0; // Remove Y component to keep rotation on XZ plane
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        character.rotation = Quaternion.Slerp(character.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}