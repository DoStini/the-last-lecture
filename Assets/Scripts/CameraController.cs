using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [Range(1, 20)] public float height = 15f;
 
    void FollowPlayer()
    {
        transform.position = player.transform.position + Vector3.up * height;
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        FollowPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }
}
