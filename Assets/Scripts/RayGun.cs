using UnityEngine;
using UnityEngine.InputSystem;

public class RayGun : MonoBehaviour
{
    [SerializeField] private AudioSource scanAudio;

    [SerializeField] private PointRenderer pointRenderer;

    [SerializeField] private GameObject rayPrefab;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private GameObject barrelLight;

    [SerializeField] private Scanner scanner;
    [SerializeField] private Painter painter;

    public bool Scanning
    {
        get => scanner.Scanning;
        set
        {
            scanner.Scanning = value;
            Painting = !Scanning && Painting;
        }
    }

    public bool Painting
    {
        get => painter.Painting;
        set
        {
            if (Scanning)
                return;

            painter.Painting = value;
        }
    }

    private void Awake()
    {
        painter.Setup(pointRenderer, rayDistance, layerMask, rayPrefab);
        scanner.Setup(pointRenderer, rayDistance, layerMask, rayPrefab);
    }

    private void FixedUpdate()
    {
        bool firePressed = Mouse.current.leftButton.isPressed;
        if (firePressed && !Scanning)
        {
            scanAudio.Play();
        }
        Scanning = firePressed;

        barrelLight.SetActive(Painting || Scanning);
        painter.Paint();
        scanner.Scan(Time.fixedDeltaTime);
    }


    public void AdjustPaintAngle(float scrollDelta)
    {
        painter.AdjustAngle(scrollDelta);
    }
}
