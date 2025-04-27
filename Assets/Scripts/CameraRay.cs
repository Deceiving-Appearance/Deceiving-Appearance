using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRay : MonoBehaviour
{
    [SerializeField] private AudioSource scanAudio;

    [SerializeField] private PointRenderer pointRenderer;

    [SerializeField] private GameObject rayPrefab;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private GameObject barrelLight;

    [SerializeField] private Scanner scanner;
    [SerializeField] private Painter painter;
    [SerializeField] private bool autoScan;

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
        if (autoScan && !Scanning)
        {
            scanAudio.Play();
        }
        Scanning = autoScan;

        barrelLight.SetActive(Painting || Scanning);
        painter.Paint();
        scanner.Scan(Time.fixedDeltaTime);
    }


    public void AdjustPaintAngle(float scrollDelta)
    {
        painter.AdjustAngle(scrollDelta);
    }
}
