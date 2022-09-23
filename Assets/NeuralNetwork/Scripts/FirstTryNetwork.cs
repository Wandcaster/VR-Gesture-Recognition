//using System.Collections;
//using System.Collections.Generic;
//using Unity.Barracuda;
//using UnityEngine;

//public class FirstTryNetwork : MonoBehaviour
//{
//    [SerializeField]
//    private NNModel NNmodel;
//    [SerializeField]
//    Texture2D Image;
//    // Start is called before the first frame update
//    void Start()
//    {
//        var model = ModelLoader.Load(NNmodel);
//        var worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);

//        var input = TransformInput(Image.GetRawTextureData());

//        //var inputs = new Dictionary<string, Tensor> {
//        //    { "XD", input }
//        //};
//        //var output = worker.Execute(inputs);
//        //Debug.Log(output);

//    }
//    Tensor TransformInput(byte[] pixels)
//    {
//        float[] transformedPixels = new float[pixels.Length];

//        for (int i = 0; i < pixels.Length; i++)
//        {
//            transformedPixels[i] = (pixels[i] - 127f) / 128f;
//        }
//        return new Tensor(1, Image.width, Image.height, 3, transformedPixels);
//    }
//}
    