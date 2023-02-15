# Gesture Recognition

## About
Gesture recognition for Unity.
The system allows you to create and save specific gestures and assign any actions to them.
## Requirements
[OpenCV plus Unity](https://assetstore.unity.com/packages/tools/integration/opencv-plus-unity-85928)<br />
[SteamVR plugin](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647)
## Installation
1. Install OpenCV.<br/>
2. Create Assembly Definition in OpenCV+Unity folder.
3. Allow unsafe code in created assembly.
4. Install SteamVR plugin.<br/>
5. Download package from [release](https://github.com/Wandcaster/Praca-Dyplomowa/releases). <br/>
6. In Assets/MyAssets/GestureRecognition/VRGestureRecognition add OpenCV assembly definition reference.
7. Convert materials from Assets/SteamVR/Models/Materials/ to URP 
## Creating gesture database
1. Open scene "GestureManager".
2. Enter playmode.
3. Select Gesture type using left hand ray(Recommended vector).
4. Select Mode to Create Gesture.
5. Select gesture database or create new.
6. Press button and draw gesture using right hand.
7. Enter gesture name.
8. Save gesture by press button.
9. Repeat 6-8 step to add more gesture.
10. Change mode to Test and test gesture.
## Configurate gesture GestureManager component
1. Add GestureManager prefab to scene.
2. Set key that start drawing gesture.
3. Add Trail Renderer to traced point(For example hand).
4. Set reference of tracked point in gestureManager.
5. Select database in gestureManager.
6. Add Listener to Event.
7. Check isEnabled bool is set to true.

