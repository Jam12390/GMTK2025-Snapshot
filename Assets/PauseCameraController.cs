using UnityEngine;

public class PauseCameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;

    public int pauseCamWidth;
    public int pauseCamHeight;
    public int snapshotPauseDuration = 3; //pleasepleasepleasepleaseplease dont forget to make this value obtained from savedata.json
    public int snapshotMissDuration;
    public int pausableObjectsInRange = 0;

    public GameObject cameraAim;
    public Vector3 mousePos;

    public bool camActive = false;
    public float camToggleDelay = 0.5f;
    public float delay = 0;

    public float pastCooldown = 0;
    public float cooldown = 0;
    private SpriteRenderer sr;
    public Color colour;
    private bool movementPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cameraAim = GameObject.Find("CameraAim");
        cam = Camera.main;
        sr = gameObject.GetComponent<SpriteRenderer>();
        colour = sr.color;
        cameraAim = GameObject.Find("CameraAim");
        colour.a = 0;
        sr.color = new Color(0, 0, 0, 0);
        snapshotMissDuration = snapshotPauseDuration / 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (!movementPaused)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            cameraAim.transform.position = mousePos;
        }

        if (Input.GetKey(KeyCode.Mouse1) && delay <= 0 && cooldown <= 0)
        {
            camActive = !camActive;
            sr.color = camActive ? new Color(0, 0, 0, 255) : new Color(0, 0, 0, 0); //anew Color(102, 102, 102, 0);
            delay = camToggleDelay;
        }

        if (Input.GetKey(KeyCode.Mouse0) && cooldown <= 0 && camActive)
        {
            Debug.Log("Photo taken");
            movementPaused = true;
            cooldown = pausableObjectsInRange == 0 ? snapshotMissDuration : cooldown;
        }

        delay -= Time.deltaTime;
        pastCooldown = cooldown;
        cooldown -= Time.deltaTime;
        movementPaused = cooldown <= 0 ? false : true;

        if (cooldown > 0)
        {
            colour.a = cooldown / snapshotPauseDuration;
            sr.color = colour;
        }
        //else if(camActive)
        //{
        //    colour.a = 254;
        //    sr.color = colour;
        //}
        if (pastCooldown > 0 && cooldown < 0)
        {
            camActive = false;
            sr.color = camActive ? new Color(0, 0, 0, 255) : new Color(0, 0, 0, 0);
        }
    }
}
