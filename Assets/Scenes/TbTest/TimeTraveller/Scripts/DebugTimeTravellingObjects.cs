using UnityEngine;

public class DebugTimeTravellingObjects : MonoBehaviour
{
    private ITimeTraveller _selectedTimeTraveller;

	void Update ()
	{
	    if (Input.GetMouseButtonDown(0))
	        SelectTimeTraveller();

	    if (_selectedTimeTraveller == null)
	        return;

        if (Input.GetKeyDown(KeyCode.A))
            _selectedTimeTraveller.SetPast();
	    if (Input.GetKeyDown(KeyCode.S))
            _selectedTimeTraveller.SetPresent();
        if (Input.GetKeyDown(KeyCode.D))
            _selectedTimeTraveller.SetFuture();
        if (Input.GetKeyDown(KeyCode.F))
            _selectedTimeTraveller.ResetToDefaultTimePeriod();
        if (Input.GetKeyDown(KeyCode.Q))
            Debug.Log(_selectedTimeTraveller.GetCurrentTimePeriod());
        if (Input.GetKeyDown(KeyCode.W))
            Debug.Log(_selectedTimeTraveller.GetDefaultTimePeriod());
	}

    private void SelectTimeTraveller()
    {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            return;
        
        _selectedTimeTraveller = hit.collider.GetComponent<ITimeTraveller>() ?? _selectedTimeTraveller;
        Debug.Log("Selected Timetraveller: " + hit.collider.name);
    }
}