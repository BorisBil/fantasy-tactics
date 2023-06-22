using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public GameObject cursor;

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            GameObject overlayTile = hit.Value.collider.gameObject;
            cursor.transform.position = overlayTile.transform.position;
            cursor.transform.position = new Vector3(cursor.transform.position.x, cursor.transform.position.y, cursor.transform.position.z + 1);
            Debug.Log(cursor.transform.position == overlayTile.transform.position);
            cursor.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder + 1;

            if (Input.GetMouseButtonDown(0))
            {
                overlayTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }
    }
    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
    
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);
        // Debug.Log(hits.Length);
        if (hits.Length > 0) 
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }
}
