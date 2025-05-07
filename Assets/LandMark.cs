using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Landmark : MonoBehaviour
{
    public string landmarkName;
    public GameObject sphereIndicatorObj;
    public GameObject spriteIconObj;
    public Transform EndLinepoint;
    public LineRenderer lineRenderer;
    public int distance;
    public bool IsLandmarkRevealed;
    public bool isLandmarkRevealed
    {
        get { return IsLandmarkRevealed; }
        set
        {
            if (value == true)
            {
                StartCoroutine(MapHUDUpdate());
            }

            IsLandmarkRevealed = value;
        }
    }
    public TMP_Text landmarkNameText;
    public TMP_Text landmarkNameUI_1;
    public TMP_Text landmarkNameUI_2;
    public TMP_Text distanceText;
    public int revealedDistanceThreshold;
    public LandmarkManager landmarkManager;
    private float _timer;
    public GameObject playerPos;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.SetPosition(0, sphereIndicatorObj.transform.position);
        lineRenderer.SetPosition(1, EndLinepoint.position);
    }

    void Update()
    {
        distanceText.text = distance.ToString() + "m";

        if (isLandmarkRevealed)
        {
            landmarkNameText.text = landmarkName;
        }
        else
        {
            landmarkNameText.text = "???";
        }

        if (!isLandmarkRevealed && (distance <= revealedDistanceThreshold))
        {
            isLandmarkRevealed = true;
        }
    }

    IEnumerator MapHUDUpdate()
    {
        float _timer = 0;
        float t = 0;
        landmarkNameUI_1.text = "";
        landmarkNameUI_2.text = "";

        while (t < 1)
        {
            _timer += Time.deltaTime;
            t = Mathf.Clamp01(_timer / landmarkManager.duration);

            float evaluated = landmarkManager.easeInCurve.Evaluate(t);
            landmarkManager.playerCanvas.transform.localScale = Vector3.LerpUnclamped(
                new Vector3(0, 0, 0), new Vector3(1, 1, 1), evaluated
            );

            yield return null; // IMPORTANT: yield every frame
        }

        _timer = 0;
        t = 0;

        foreach (char c in landmarkName)
        {
            landmarkNameUI_1.text += c;
            landmarkNameUI_2.text += c;

            yield return new WaitForSeconds(0.075f);
        }

        yield return new WaitForSeconds(5f);

        for (int i = landmarkName.Length; i > 0; i--)
        {
            landmarkNameUI_1.text = landmarkName.Substring(0, i - 1);
            landmarkNameUI_2.text = landmarkName.Substring(0, i - 1);

            yield return new WaitForSeconds(0.0375f);
        }

        landmarkNameUI_1.text = "";
        landmarkNameUI_2.text = "";

        _timer = 0;
        t = 0;

        while (t < 1)
        {
            _timer += Time.deltaTime;
            t = Mathf.Clamp01(_timer / landmarkManager.duration);

            float evaluated = landmarkManager.easeInCurve.Evaluate(t);

            landmarkManager.playerCanvas.transform.localScale = Vector3.LerpUnclamped(
                new Vector3(1, 1, 1), new Vector3(0, 0, 0), evaluated
            );

            yield return null; // IMPORTANT: yield every frame
        }
    }
}
