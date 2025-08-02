using UnityEngine;

public class testCamPause : MonoBehaviour
{
    public Rigidbody2D rb;

    public bool isInCameraArea;
    public float timeToResume;
    public Vector2 velocityStorage;
    public bool velocityRestored = true;
    public PauseCameraController cameraController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        cameraController = GameObject.Find("CameraAim").GetComponent<PauseCameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log((Input.GetKey(KeyCode.Mouse0), cameraController.camActive, cameraController.cooldown, isInCameraArea));
        if (!velocityRestored && timeToResume <= 0)
        {
            velocityRestored = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.linearVelocity = velocityStorage;
        }

        if (Input.GetKey(KeyCode.R))
        {
            gameObject.transform.position = new Vector2(11, 11);
            rb.linearVelocity = new Vector2(-10, 0);
        }

        Debug.Log(Input.GetKey(KeyCode.Mouse0) && cameraController.camActive && cameraController.cooldown <= 0 && isInCameraArea ? "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" : "");
        if (Input.GetKey(KeyCode.Mouse0) && cameraController.camActive && cameraController.cooldown <= 0 && isInCameraArea)
        {
            timeToResume = cameraController.snapshotPauseDuration;
            velocityStorage = rb.linearVelocity;
            //rb.linearVelocity = new Vector2(0, 0);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            velocityRestored = false;
            cameraController.camActive = !cameraController.camActive;
            cameraController.cooldown = cameraController.snapshotPauseDuration;
        }
        timeToResume -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        cameraController.pausableObjectsInRange += 1;
        isInCameraArea = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        cameraController.pausableObjectsInRange -= 1;
        isInCameraArea = false;
    }
}
