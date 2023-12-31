using UnityEngine;

public enum CameraMove {
    Left,
    Right,
    Up,
    Down
}
public class CameraManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public float cameraMoveSpeed = 5.0f;
    public bool followPlayer = true;

    private float zoom;
    public float zoommultiplier = 11f;
    public float minZoom = 20f;
    public float maxZoom = 100f;
    private float velocity = 0f;
    private float smoothTime = 0.40f;

    public bool followWeapon = false;

    public Vector3 weaponPosition;

    void Start()
    {
        zoom = GetComponent<Camera>().fieldOfView;
    }

    public void setW(Vector3 w) {
        weaponPosition = w;
    }

    void FixedUpdate()
    {
        if (followPlayer)
        {
            followObject(playerManager.currentPlayerPosition());
        }

        if (followWeapon) {
            followObject(weaponPosition);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoommultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        Camera thisCamera = GetComponent<Camera>();
        thisCamera.fieldOfView =  Mathf.SmoothDamp(thisCamera.fieldOfView, zoom, ref velocity, smoothTime);
    }



    private void followObject(Vector3 objectPosition)
    {   
        Vector3 desiredPosition = objectPosition + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;     
    }

    public void move(CameraMove movement) {
            switch(movement)
            {
                case CameraMove.Left:
                    transform.Translate(-cameraMoveSpeed* Time.deltaTime,0,0, Space.World);
                    break;
                case CameraMove.Right:
                    transform.Translate(cameraMoveSpeed* Time.deltaTime,0,0, Space.World);
                    break;
                case CameraMove.Up:
                    transform.Translate(0,cameraMoveSpeed* Time.deltaTime,0, Space.World);
                    break;
                case CameraMove.Down:
                    transform.Translate(0,-cameraMoveSpeed* Time.deltaTime,0, Space.World);
                    break;

            }
    }

}
