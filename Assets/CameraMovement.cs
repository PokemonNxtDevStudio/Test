using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float cooldown = 5f;
    float counter = 0;
    float distance = 5f;
    float height = 2f;
    float xSpeed = 60f;
    float ySpeed = 30f;
    float yMinLimit = 2f;
    float yMaxLimit = 80f;
    float x = 0.0f;
    float y = 0.0f;
    float heightDampening = 2f;
    float rotationDampening = 3f;
    Quaternion lastRotation;
    RaycastHit hit;
    Vector3 targetPos;


    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        targetPos = target.position;
        x = angles.x;
        y = angles.y;

        // Make the rigid body not change rotation
        if (rigidbody)
        {
            rigidbody.freezeRotation = true;
        }

    }

    void LateUpdate()
    {
        float wantedRotation = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;
        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotation, ySpeed * Time.deltaTime * rotationDampening);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, xSpeed * Time.deltaTime * rotationDampening);

        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0, 0, -distance) + targetPos;
            targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

            transform.rotation = rotation;
            transform.position = position;
            transform.LookAt(target);
    }

    void ClipFix()
    {
        if (Physics.Linecast(target.position, transform.position, out hit))
        {

            Debug.Log("Hit: " + hit.transform.name);
            if (hit.transform.name != "Main Camera" || hit.transform.name != "Player")
            {
                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
        }
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
