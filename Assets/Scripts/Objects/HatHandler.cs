using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatHandler : MonoBehaviour {

    private IOHandler IO;

    [Header("Hats")]
    public GameObject[] Hatlist;

	void Start ()
    {
        IO = new IOHandler();
        loadHat();
	}

    public void loadHat()
    {
        if(gameObject.transform.childCount > 0)
        {
            GameObject child = gameObject.transform.GetChild(0).gameObject;
            if (child != null)
                Destroy(child);
        }
        Debug.Log("Hatt_ " + (IO.getSelectedHat() - 1));
        if (IO.getSelectedHat() != 0)
        {
            GameObject hat = Instantiate(Hatlist[IO.getSelectedHat()-1]);
            hat.transform.SetParent(gameObject.transform);
            hat.transform.localPosition = Vector3.zero;
            hat.name = hat.name;
        }
    }
}
