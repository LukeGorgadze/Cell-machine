using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectOnGrid : MonoBehaviour
{
    public Transform girdCellPrefab;
    public ObjFollowMouse currentDraggableObject;
    private GameObject selectedObject;

    public Vector3 smoothMousePosition;
    [SerializeField] private int height;
    [SerializeField] int width;

    private Vector3 mousePosition;
    private Node[,] nodes;
    public Plane plane;

    void Start()
    {
        CreateGrid();
        plane = new Plane(Vector3.up, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePositionOnGrid();
        SelectObject();
    }

    void GetMousePositionOnGrid()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out var enter))
        {
            mousePosition = ray.GetPoint(enter);
            smoothMousePosition = mousePosition;
            mousePosition.y = 0;
            mousePosition = Vector3Int.RoundToInt(mousePosition);
            foreach (var node in nodes)
            {

                if (node.cellPosition == mousePosition && node.isPlaceable)
                {
                    if (Input.GetMouseButtonUp(0) && currentDraggableObject != null)
                    {
                        node.isPlaceable = false;
                        currentDraggableObject.nodeITook = node;
                        currentDraggableObject.isChosen = false;
                        currentDraggableObject.transform.position = node.cellPosition + new Vector3(0, 0.5f, 0);
                        currentDraggableObject.UpdateTargetPosition();
                    }
                }

            }
        }
    }

    private void CreateGrid()
    {
        nodes = new Node[width, height];
        var name = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3Int worldPosition = new Vector3Int(i, 0, j);
                Transform obj = Instantiate(girdCellPrefab, worldPosition, Quaternion.identity);
                obj.name = "Cell " + name;
                nodes[i, j] = new Node(true, worldPosition, obj);
                name++;
            }
        }
    }

    void SelectObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedObject == null)
            {
                RaycastHit hit = CastRay();

                if (hit.collider != null)
                {
                    currentDraggableObject = hit.collider.GetComponent<ObjFollowMouse>();

                    if (currentDraggableObject == null)
                    {
                        return;
                    }
                    currentDraggableObject.isChosen = true;
                    if (currentDraggableObject.nodeITook != null)
                        currentDraggableObject.nodeITook.isPlaceable = true;
                }
            }
        }
    }

    Vector3 worldMousePosFar;
    Vector3 worldMousePosNear;

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }
}


