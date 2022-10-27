using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{
    [SerializeField]
    FruitTreeController fruitTree;
    private void OnCollisionEnter(Collision collision)
    {
        fruitTree.OnCollisionEnter(collision);
    }
}
