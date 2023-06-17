using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BehaviorTree
{
    [Serializable]
    public class RandomMove : IBehavior
    {
        [SerializeField] Vector3 MoveRange;

        Vector3 moveTarget;
        Vector3 moveVector;

        public Result Action(Environment env)
        {
            if (env.Visit(this))
            {
                float x = UnityEngine.Random.Range(-MoveRange.x, MoveRange.x);
                float z = UnityEngine.Random.Range(-MoveRange.z, MoveRange.z);

                moveVector = new Vector3(x, 0, z);
                moveTarget = env.mySelf.transform.position + moveVector;
                if (moveTarget.x < -GameManager.FieldSize) moveTarget.x = -GameManager.FieldSize;
                if (moveTarget.x > GameManager.FieldSize) moveTarget.x = GameManager.FieldSize;
                if (moveTarget.z < -GameManager.FieldSize) moveTarget.z = -GameManager.FieldSize;
                if (moveTarget.z > GameManager.FieldSize) moveTarget.z = GameManager.FieldSize;
                moveVector = moveTarget - env.mySelf.transform.position;
                moveVector.Normalize();

                env.mySelf.transform.LookAt(moveTarget);
                env.mySelf.GetComponent<Animator>().SetFloat("Speed", 1.0f);
            }

            env.mySelf.transform.position += moveVector * Time.deltaTime * 3;
            if ((moveTarget - env.mySelf.transform.position).magnitude > 3.0f) return Result.Running;

            env.Leave(this);
            return Result.Success;
        }
    }
}
