using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject plot;
    public int width;
    public int length;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < length; ++j)
            {
                Instantiate(plot, new Vector3(i * 5, 0.75f, j * 5), Quaternion.identity);
            }
        }
    }
}
