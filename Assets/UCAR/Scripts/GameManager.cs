using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TotemData[] totemDatas;

    private HashSet<GameObject> actived = new HashSet<GameObject>();
    private void Start()
    {
        LocationManager.Event.LocationChanged += LocationChanged_EventHandler;
        foreach (TotemData totem in totemDatas)
        {
            GameObject created = Instantiate(totem.Prefab);
            created.transform.position = totem.Location;
            actived.Add(created);
        }
    }

    private void LocationChanged_EventHandler(Vector3 location)
    {
        // latitude, altitude, longitude
        foreach (GameObject gameObject in actived)
        {
            Vector3 p = gameObject.transform.position;
            gameObject.transform.position = new Vector3(p.x, location.y, p.z);
        }
    }
}
