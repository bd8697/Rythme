using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cubequad.Tentacles2D;
public class tentacleContainer : MonoBehaviour
{

    [SerializeField] List<Tentacle> tentacles = new List<Tentacle>();
    [SerializeField] List<Tentacle> topLeft = new List<Tentacle>();
    [SerializeField] List<Tentacle> topRight = new List<Tentacle>();
    [SerializeField] List<Tentacle> projectiles = new List<Tentacle>();
    //private List<List<float>> tentacleCoordsInDread = new List<List<float>>();

    public List<Tentacle> Tentacles
    {
        get { return tentacles; }
        set
        {
            for (int i = 0; i < tentacles.Count; i++)
                tentacles[i] = value[i];
        }
    }
    public List<Tentacle> TopLeft
    {
        get { return topLeft; }
        set
        {
            for (int i = 0; i < topLeft.Count; i++)
                topLeft[i] = value[i];
        }
    }

    public List<Tentacle> TopRight
    {
        get { return topRight; }
        set
        {
            for (int i = 0; i < topRight.Count; i++)
                topRight[i] = value[i];
        }
    }

    public List<Tentacle> Projectiles
    {
        get { return projectiles; }
        set
        {
            for (int i = 0; i < projectiles.Count; i++)
                projectiles[i] = value[i];
        }
    }

    //public List<List<float>> TentacelCoordsInDread
    //{
    //    get { return tentacleCoordsInDread; }
    //    set
    //    {
    //        for (int i = 0; i < tentacleCoordsInDread.Count; i++)
    //            for (int j = 0; j < tentacleCoordsInDread.Count; j++)
    //                tentacleCoordsInDread[i][j] = value[i][j];
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
