using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef
{
    private GameObject _chef;
    private DetectorScript _rightDetector;
    private DetectorScript _leftDetector;
    private DetectorScript _bottomDetector;
    private DetectorScript _aboveDetector;
    private ChefMovementScript _chefMovementScript;
    private SpriteRenderer _srend;
    private Transform _transform;

    public Chef(GameObject chef)
    {
        _chef = chef;

        _rightDetector = chef.gameObject.transform.GetChild(0).GetComponent<DetectorScript>();
        _leftDetector = chef.gameObject.transform.GetChild(1).GetComponent<DetectorScript>();
        _bottomDetector = chef.gameObject.transform.GetChild(2).GetComponent<DetectorScript>();
        _aboveDetector = chef.gameObject.transform.GetChild(3).GetComponent<DetectorScript>();
        _chefMovementScript = chef.GetComponent<ChefMovementScript>();

        _srend = chef.GetComponent<SpriteRenderer>();
        _transform = chef.transform;
    }


    //public Chef()
    //{
    //    _chef = null;

    //    _rightDetector = null;
    //    _leftDetector = null;
    //    _bottomDetector = null;
    //    _aboveDetector = null;

    //    _chefMovementScript = null;
    //    _srend = null;
    //    _transform = null;
    //}

    public GameObject GetChef()
    {
        return _chef;
    }
    public DetectorScript GetRightDetector()
    {
        return _rightDetector;
    }

    public DetectorScript GetLeftDetector()
    {
        return _leftDetector;
    }

    public DetectorScript GetBottomDetector()
    {
        return _bottomDetector;
    }

    public DetectorScript GetAboveDetector()
    {
        return _aboveDetector;
    }

    public ChefMovementScript GetChefMovementScript()
    {
        return _chefMovementScript;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return _srend;
    }

    public Transform GetTransform()
    {
        return _transform;
    }
}
