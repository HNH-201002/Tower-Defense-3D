using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform pivotPoint;
    public float rotationSpeed = 20.0f;
    public float zoomSpeed = 400.0f;
    public float minDistance = 10.0f;
    public float maxDistance = 50.0f;

    private Vector3 pivotOffset;
    private float currentZoomLevel;


    public float minVerticalAngle = -30f; 
    public float maxVerticalAngle = 60f; 
    private float currentVerticalAngle = 0f; 
    private void Start()
    {
        if (pivotPoint == null)
        {
            Debug.LogWarning("CameraController: Pivot point not assigned, using camera's current position.");
            pivotPoint = new GameObject("PivotPoint").transform;
            pivotPoint.position = transform.position;
        }

        pivotOffset = transform.position - pivotPoint.position;
        currentZoomLevel = pivotOffset.magnitude;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            // Calculate the new vertical angle, clamped between the min and max limits
            currentVerticalAngle -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);

            transform.RotateAround(pivotPoint.position, Vector3.up, horizontalRotation);
            transform.RotateAround(pivotPoint.position, transform.right, currentVerticalAngle - transform.eulerAngles.x);

            pivotOffset = transform.position - pivotPoint.position;
            currentZoomLevel = pivotOffset.magnitude;
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        if (Mathf.Abs(scroll) > 0.01f) 
        {
            currentZoomLevel -= scroll;
            currentZoomLevel = Mathf.Clamp(currentZoomLevel, minDistance, maxDistance);
            pivotOffset = pivotOffset.normalized * currentZoomLevel;
            transform.position = Vector3.Lerp(transform.position, pivotPoint.position + pivotOffset, Time.deltaTime * zoomSpeed); 
        }
        transform.LookAt(pivotPoint.position);

    }
}