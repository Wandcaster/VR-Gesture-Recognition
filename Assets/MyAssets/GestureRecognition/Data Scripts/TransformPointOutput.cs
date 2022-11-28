using System.Collections.Generic;
using UnityEngine;
namespace VRGesureRecognition
{
    class TransformPointOutput
    {
        public int[,] outputPoints { get; }
        public List<Vector2> pointsAfterCheck { get; }
        public TransformPointOutput(int[,] outputPoints, List<Vector2> pointsAfterCheck)
        {
            this.outputPoints = outputPoints;
            this.pointsAfterCheck = pointsAfterCheck;
        }
    }
}

