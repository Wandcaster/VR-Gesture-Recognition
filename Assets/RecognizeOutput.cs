public class RecognizeOutput
{
    public IGesture recognizedGesture;
    public float probability;
    public RecognizeOutput(IGesture gesture, float probability)
    {
        recognizedGesture = gesture;
        this.probability = probability;
    }
}