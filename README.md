# IGC Unity Plug-In

Built on Unity version 2021.3.28f1. Works with URP

This plugin will capture renders spun around a target object and export out a JSON file of the relative camera transforms. It can upload the necessary files to the IGC API to process.

 ## How to use
* Place Capturer Prefab within Scene and add the object to capture to the Target parameter within _Capturer/CharacterCapture_ Script.
* Adjust the Height Offset within the prefab to point to the center of Target character
* Adjust _Capture Radius_ to fit whole character within renders
* Renders/files will be outputed to *PROJECT_NAME/OUTPUT/Captures/*

**Parameters:**
* Character Capture > Target
    * The focus of the Capturer. Will move cameras to spin around the origin of the Target
* Character Capture > Capture Radius
    * Size of radius that spins around the Target object
* Character Capture > FOV
    * Field of View for both cameras
* Character Capture > Frames
    * Number of images to be rendered out
* Character Capture > Dimension
    * Dimension of both sides of image
* Character Capture > Height Offset
    * Offsets the y-axis of the focal point of the capture. (TODO: find focal point based on mesh)
* Character Capture > Show Layers
    * The layers that the character meshes should live on. Used for masking out character from the background. If empty, will default to rendering everything

* Capturer Lighting > Use Scene Lights
    * If false, will set up a basic point light set up around the target
* Capturer Lighting > Lights To Turn Off
    * If Use Scene Lights is false, will turn off any lights within the array during capture

## Notes
* The transforms.json file will get reformatted with the API to work with instant-ngp.