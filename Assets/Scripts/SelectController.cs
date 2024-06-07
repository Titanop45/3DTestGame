using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Purchasing.Security;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SelectController : MonoBehaviour
{
    public GameObject cube;
    public List<GameObject> players;
    private Camera _cam;
    public LayerMask layer, layerMask;
    private GameObject _cubeSelection;
    private RaycastHit _hit;
    private void Awake()
    {
        _cam = GetComponent<Camera>(); 

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && players.Count > 0)
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit agentTarget, 1000f, layer))
                foreach (var el in players)
                 el.GetComponent<NavMeshAgent>().SetDestination(agentTarget.point);


        }

        if (Input.GetMouseButtonDown(0))
        {
            foreach (var el in players)
                if (el != null)
                el.transform.GetChild(0).gameObject.SetActive(false);


            players.Clear();

            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out _hit, 1000f, layer))
                _cubeSelection = Instantiate(cube, new Vector3(_hit.point.x, 1 , _hit.point.z), Quaternion.identity);
        }
        if (_cubeSelection) 
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hitDrag, 1000f, layer))
            {
                float xscale = (_hit.point.x - hitDrag.point.x) * -1;
                float zscale = _hit.point.z - hitDrag.point.z;
                if(xscale < 0.0f && zscale < 0.0f)
                    _cubeSelection.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
                else if (xscale < 0.0f)
                    _cubeSelection.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                else if (zscale < 0.0f)
                    _cubeSelection.transform.localRotation = Quaternion.Euler(new Vector3(180, 0, 0));
                else
                    _cubeSelection.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                _cubeSelection.transform.localScale = new Vector3(Mathf.Abs(xscale), 1, Mathf.Abs(zscale));
            }
        }


        if (Input.GetMouseButtonUp(0) && _cubeSelection) 
        {
           RaycastHit[] hits =  Physics.BoxCastAll(
                _cubeSelection.transform.position,
                _cubeSelection.transform.localScale,
                Vector3.up,
                Quaternion.identity,
                0,
                layerMask);

            foreach (RaycastHit el in hits)
            {
                if (el.collider.CompareTag("Enemy"))
                continue;

                players.Add(el.transform.gameObject);
                el.transform.GetChild(0).gameObject.SetActive(true);
            }

            Destroy(_cubeSelection);
        }              
    }
}
