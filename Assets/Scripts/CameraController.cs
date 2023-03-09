using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [Range(1, 20)] public float height = 15f;
    public float maxFov;
    public float fov = 60f;
    private Camera _camera;
    
    void FollowPlayer()
    {
        transform.position = player.transform.position + Vector3.up * height;
        transform.rotation = Quaternion.Euler(90, 0, 0);
        _camera.fieldOfView = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        FollowPlayer();
    }

    void CheckHeight()
    {
        if (!Physics.Raycast(player.transform.position, Vector3.up, out var hit, height, LayerMask.GetMask("Ground"))) return;

        transform.position = hit.point;
        float currHeight = hit.point.y - player.transform.position.y;
        float factor = currHeight / height;

        _camera.fieldOfView = Mathf.Lerp(maxFov, fov, factor);
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        CheckHeight();
    }
}
