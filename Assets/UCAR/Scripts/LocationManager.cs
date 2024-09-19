using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LocationManager : MonoBehaviour
{
    public static class Event
    {
        public static event System.Action<Vector3> LocationChanged;
        public static void OnLocationChangedTrigger(Vector3 data)
        {
            LocationChanged?.Invoke(data);
        }
    }

    [System.Serializable]
    public class UIManager
    {
        #region Property
        [SerializeField]
        TMP_Text latitudeText;

        [SerializeField]
        TMP_Text longitudeText;

        [SerializeField]
        TMP_Text altitudeText;
        #endregion

        public void SetLatitude(string value)
        {
            latitudeText.text = "Latitude : " + value;
        }

        public void SetLongitude(string value)
        {
            longitudeText.text = "Longitude : " + value;
        }

        public void SetAltitude(string value)
        {
            altitudeText.text = "Altitude : " + value;
        }

    }
    [SerializeField]
    UIManager _UIManager;
    private void Start()
    {
        StartCoroutine(LocationStart());
    }
    IEnumerator LocationStart()
    {
        if (!Input.location.isEnabledByUser)
            Debug.Log("권한 Error");
        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("Time Out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device Location");
            yield break;
        }
        else
        {
            LocationInfo lastData = Input.location.lastData;
            Debug.Log("Location : " + lastData.latitude + " " + lastData.longitude + " " + lastData.altitude + " " + lastData.horizontalAccuracy + " " + lastData.timestamp);
            StartCoroutine(GPSCoroutine());
        }
    }
    IEnumerator GPSCoroutine()
    {
        while (Input.location.status == LocationServiceStatus.Running)
        {
            LocationInfo lastData = Input.location.lastData;
            _UIManager.SetAltitude(lastData.altitude.ToString());
            _UIManager.SetLongitude(lastData.longitude.ToString());
            _UIManager.SetLatitude(lastData.latitude.ToString());
            Vector3 vec = new Vector3(lastData.latitude, lastData.altitude, lastData.longitude);
            transform.position = vec;
            Event.OnLocationChangedTrigger(vec);
            yield return new WaitForSeconds(0.1f);
        }
        
        yield break;
    }

    private void OnApplicationQuit()
    {
        Input.location.Stop();
    }
}
