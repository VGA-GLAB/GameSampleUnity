using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.CinemachineTargetGroup;

public class GameManager : MonoBehaviour
{
    [SerializeField] float _fieldSize = 25.0f;
    [SerializeField] CinemachineTargetGroup _targetGroup;
    [SerializeField] GameObject _field;
    [SerializeField] GameObject _prefab;
    [SerializeField] GameObject _cube;

    List<GameObject> _charList = new List<GameObject>();

    static GameManager _gMan = null;
    static public float FieldSize => _gMan._fieldSize;

    private void Awake()
    {
        _gMan = this;
    }

    //UnityChan100体作る
    private void Start()
    {
        _field.transform.localScale = new Vector3(_fieldSize*2, 1, _fieldSize*2);

        //4点がうつるカメラを作る
        List<Target> cubeList = new List<Target>();
        for (int i = 0; i < 4; ++i)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            switch (i)
            {
                case 0: cube.transform.position = new Vector3(-_fieldSize, 0, -_fieldSize); break;
                case 1: cube.transform.position = new Vector3(_fieldSize, 0, -_fieldSize); break;
                case 2: cube.transform.position = new Vector3(-_fieldSize, 0, _fieldSize); break;
                case 3: cube.transform.position = new Vector3(_fieldSize, 0, _fieldSize); break;
            }
            cubeList.Add(new Target() { target = cube.transform, weight = 1.0f, radius = 3.0f });
        }
        _targetGroup.m_Targets = cubeList.ToArray();

        for (int i = 0; i < 100; ++i)
        {
            var go = GameObject.Instantiate(_prefab, this.transform.position + new Vector3(UnityEngine.Random.Range(-_fieldSize, _fieldSize), 0.5f, UnityEngine.Random.Range(-_fieldSize, _fieldSize)), this.transform.rotation);
            _charList.Add(go);

            var bt = go.GetComponent<BehaviorTreeRoot>();
            bt.Enable = true;
        }
    }

    void Update()
    {
        //rayとばして当たった位置にQRマーカー置く
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool is_hit = Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (is_hit)
            {
                var markerObj = GameObject.Instantiate(_cube);
                markerObj.transform.position = hit.point;
            }
        }

        //rayとばして当たった位置に爆発
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool is_hit = Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (is_hit)
            {
                foreach(var go in _charList)
                {
                    var bt = go.GetComponent<BehaviorTreeRoot>();
                    //bt.Enable = false;

                    var rb = go.GetComponent<Rigidbody>();
                    rb.AddExplosionForce(1500, hit.point, 10);
                    //rb.constraints = RigidbodyConstraints.None;
                }
            }
        }
    }
}
