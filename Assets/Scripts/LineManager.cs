using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class LineManager : MonoBehaviour
{
    #region Parameters

    [SerializeField]
    private Player player;

    [SerializeField]
    private PhysicsMaterial2D physicsMaterial2D;

    [SerializeField]
    private float lineSeparationDistance = .2f;

    [SerializeField]
    private float effectorSpeed = 2f;

    [SerializeField]
    private float lineWidth = .1f;

    [SerializeField]
    private Color lineColor = Color.black;

    [SerializeField]
    private int lineCapVertices = 5;

    #endregion

    #region Private
    private List<GameObject> lines;
    private List<Vector2> currentLine;
    private LineRenderer currentLineRenderer;
    private EdgeCollider2D currentLineEdgeCollider;

    private GameObject currentLineObject;

    private bool drawing = false;
    private bool erasing = false;

    private Camera mainCamera;

    private Pan panning;

    private Zoom zoom;
    private InputManager inputManager;
    #endregion

    private void Awake()
    {
        inputManager = InputManager.Instance;
        mainCamera = Camera.main;
        panning = GetComponent<Pan>();
        zoom = GetComponent<Zoom>();
    }

    private void OnEnable()
    {
        InputManager.OnStartDraw += OnStartDraw;
        InputManager.OnEndDraw += OnEndDraw;
        InputManager.OnStartErase += OnStartErase;
        InputManager.OnEndErase += OnEndErase;
    }

    private void OnDisable()
    {
        InputManager.OnStartDraw -= OnStartDraw;
        InputManager.OnEndDraw -= OnEndDraw;
        InputManager.OnStartErase -= OnStartErase;
        InputManager.OnEndErase -= OnEndErase;
    }

    #region Drawing

    private void OnStartDraw()
    {
        if (!erasing)
        {
            StartCoroutine("Drawing");
        }
    }

    private void OnEndDraw()
    {
        drawing = false;
    }

    IEnumerator Drawing()
    {
        drawing = true;
        StartLine();
        while (drawing)
        {
            AddPoint(GetCurrentWorldPoint());
            yield return null;
        }
        EndLine();
    }

    private void StartLine()
    {
        currentLine = new List<Vector2>();
        currentLineObject = new GameObject();
        currentLineObject.name = "Line";
        currentLineObject.transform.parent = transform;
        currentLineRenderer = currentLineObject.AddComponent<LineRenderer>();
        currentLineEdgeCollider = currentLineObject.AddComponent<EdgeCollider2D>();

        SurfaceEffector2D currentEffector = currentLineObject.AddComponent<SurfaceEffector2D>();

        currentLineRenderer.positionCount = 0;
        currentLineRenderer.startWidth = lineWidth;
        currentLineRenderer.endWidth = lineWidth;
        currentLineRenderer.numCapVertices = lineCapVertices;
        currentLineRenderer.endColor = lineColor;
        currentLineRenderer.startColor = lineColor;
        currentLineEdgeCollider.edgeRadius = .1f;

        currentLineEdgeCollider.sharedMaterial = physicsMaterial2D;

        currentLineEdgeCollider.usedByEffector = true;

        currentEffector.speed = effectorSpeed;

        currentLineEdgeCollider.sharedMaterial = physicsMaterial2D;

        // Set the layer and tag for the line object
        currentLineObject.layer = LayerMask.NameToLayer("Lines");
        currentLineObject.tag = "Line"; // Ensure

        // currentLineObject.layer = LayerMask.NameToLayer("Lines");
    }

    private void EndLine()
    {
        if (currentLine.Count == 1)
        {
            DestroyLine(currentLineObject);
        }
        else
        {
            currentLineEdgeCollider.SetPoints(currentLine);
        }
    }

    private void AddPoint(Vector2 point)
    {
        if (PlacePoint(point))
        {
            currentLine.Add(point);
            currentLineRenderer.positionCount++;
            currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, point);
        }
    }

    private bool PlacePoint(Vector2 point)
    {
        if (currentLine.Count == 0)
            return true;
        if (Vector2.Distance(point, currentLine[currentLine.Count - 1]) < lineSeparationDistance)
            return false;
        return true;
    }
    #endregion


    private void OnStartErase()
    {
        if (!drawing)
        {
            StartCoroutine("Erasing");
        }
    }

    private void OnEndErase()
    {
        erasing = false;
    }

    IEnumerator Erasing()
    {
        erasing = true;
        while (erasing)
        {
            Vector2 screenMousePosition = GetCurrentScreenPoint();
            GameObject g = Utils.Raycast(mainCamera, screenMousePosition, 1 << 3);
            if (g != null)
                DestroyLine(g);
            yield return null;
        }
    }

    private void DestroyLine(GameObject g)
    {
        Destroy(g);
    }

    private Vector2 GetCurrentScreenPoint()
    {
        return inputManager.GetMousePosisition();
    }

    private Vector2 GetCurrentWorldPoint()
    {
        return mainCamera.ScreenToWorldPoint(inputManager.GetMousePosisition());
    }

    private float GetZoomValue()
    {
        return inputManager.GetZoom();
    }

    private void Update()
    {
        if (!player.playing)
        {
            panning.PanScreen(GetCurrentScreenPoint());

            zoom.ZoomScreen(GetZoomValue());
        }
    }
}
