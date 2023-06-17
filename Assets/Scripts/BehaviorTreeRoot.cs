using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class BehaviorTreeRoot : MonoBehaviour
{
    [SerializeField, SerializeReference, SubclassSelector] IBehavior RootNode;
    [SerializeField] GameObject Player;

    public bool Enable { get; set; }

    Environment _env = new Environment();

    void Start()
    {
        _env.mySelf = this.gameObject;
        _env.target = Player;
    }

    void Update()
    {
        if (!Enable) return;

        RootNode.Action(_env);
    }
}