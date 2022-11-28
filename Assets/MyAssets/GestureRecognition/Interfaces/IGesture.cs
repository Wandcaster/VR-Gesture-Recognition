using System;
namespace VRGesureRecognition
{
    [Serializable]
    public class IGesture
    {
        public IGestureData gestureData;
        public int gestureID
        {
            get { return gestureData.ID; }
            set { gestureData.ID = value; }
        }
        public string gestureName
        {
            get { return gestureData.gestureName; }
            set { gestureData.gestureName = value; }
        }
        public IGesture(IGestureData gestureData)
        {
            this.gestureData = gestureData;
        }
        public virtual void Save(string path) { }
    }
}
