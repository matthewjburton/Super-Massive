using UnityEngine;

[CreateAssetMenu(fileName = "Camera Settings", menuName = "ScriptableObjects/Camera Settings")]
public class CameraSettings : ScriptableObject
{
    [Tooltip("Multiplier for camera zoom based on largest mass")]
    public float sizeMultiplier;

    [Tooltip("Minimum orthographic size to avoid excessive zooming")]
    public float minSize;

    [Tooltip("Speed of zoom transition")]
    public float zoomSpeed;

    [Tooltip("Speed of background color transition")]
    public float backgroundSpeed;
}