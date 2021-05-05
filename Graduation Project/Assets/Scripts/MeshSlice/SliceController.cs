using UnityEngine;
using System.Collections;

public class SliceController : MonoBehaviour {
	Vector3 sliceStartPos, sliceEndPos;
	bool isSlicing = false;
	GameObject startUI, endUI, lineUI;
	RectTransform lineRectTransform;
	float lineWidth;
	
	// Use this for initialization
	void Start () {
		// UV Mapping Camera
		MeshSlicer.uvCamera = Instantiate( (GameObject)Resources.Load("Prefabs/MeshSlice/UV_Camera") );
		// Init GUI
		GameObject canvas = GameObject.Find("Canvas");
		GameObject pointPrefab = (GameObject)Resources.Load("Prefabs/MeshSlice/Slice_Point");
		GameObject linePrefab = (GameObject)Resources.Load("Prefabs/MeshSlice/Slice_Line");
		startUI = Canvas.Instantiate(pointPrefab);
		endUI = Canvas.Instantiate(pointPrefab);
		lineUI = Canvas.Instantiate(linePrefab);
		startUI.transform.SetParent(canvas.transform);
		endUI.transform.SetParent(canvas.transform);
		lineUI.transform.SetParent(canvas.transform);
		startUI.SetActive(false);
		endUI.SetActive(false);
		lineUI.SetActive(false);
		lineRectTransform = lineUI.GetComponent<RectTransform>();
		lineWidth = lineRectTransform.rect.width;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePos;
		// 마우스가 눌렸을 때 그 좌표를 시작 지점으로 한다.
		if (Input.GetMouseButtonDown (0)) {
			isSlicing = true;
			mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
			sliceStartPos = Camera.main.ScreenToWorldPoint(mousePos);
			startUI.SetActive(true);
			startUI.transform.position = Input.mousePosition;
		} else if(Input.GetMouseButtonUp (0) && isSlicing) {
			isSlicing = false;
			mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f); // "z" value defines distance from camera
			sliceEndPos = Camera.main.ScreenToWorldPoint(mousePos);
			if(sliceStartPos != sliceEndPos) {
				mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
				Vector3 point3 = Camera.main.ScreenToWorldPoint(mousePos);
				MeshSlicer.CustomPlane plane = new MeshSlicer.CustomPlane(sliceStartPos, sliceEndPos, point3);
				Slice(plane);
			}
		}
		if(isSlicing) {
			endUI.SetActive(true);
			lineUI.SetActive(true);
			endUI.transform.position = Input.mousePosition;
			Vector2 linePos = (endUI.transform.position + startUI.transform.position) / 2f;
			float lineHeight = (endUI.transform.position - startUI.transform.position).magnitude;
			Vector3 lineDir = (endUI.transform.position - startUI.transform.position).normalized;
			lineUI.transform.position = linePos;
			lineRectTransform.sizeDelta = new Vector2(lineWidth, lineHeight);
			lineUI.transform.rotation = Quaternion.FromToRotation(Vector3.up, lineDir);
		} else {
			startUI.SetActive(false);
			endUI.SetActive(false);
			lineUI.SetActive(false);
		}
	}
	
	// Detect & slice "sliceable" GameObjects whose bounding box intersects slicing plane
	private void Slice(MeshSlicer.CustomPlane plane) {
		SliceableObject[] sliceableTargets = (SliceableObject[])FindObjectsOfType( typeof(SliceableObject) );
		bool isSliced = false;
		foreach(SliceableObject sliceableTarget in sliceableTargets) {
			GameObject target = sliceableTarget.gameObject;
			if(plane.HitTest(target)) {
				if(target.GetComponent<SliceableObject>().isConvex) {
					MeshSlicer.SliceMesh(target, plane, true);
					//audioSource.PlayOneShot(sliceableTarget.sliceSound, 1.0f);
					isSliced = true;
				} else {
					MeshSlicer.SliceMesh(target, plane, false);
					//audioSource.PlayOneShot(sliceableTarget.sliceSound, 1.0f);
					isSliced = true;
				}
			}
		}
		
	}
}
