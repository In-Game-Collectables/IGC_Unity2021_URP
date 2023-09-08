# IGC Unity

Built on Unity version 2021.3.28f1. Works with URP

This plugin will capture renders spun around a target object and export out a JSON file of the relative camera transforms. It can upload the necessary files to the IGC APII to process to re-create a printable mesh.

 ## How to use
* Place *Capturer* Prefab within Scene and add the object to capture to the Target parameter within _Capturer/CharacterCapture_ Script
* Adjust the Height Offset within the prefab to point to the center of Target character
* Adjust _Capture Radius_ to fit whole character within renders
* Renders/files will be outputed to *PROJECT_NAME/OUTPUT/Captures/*
* The *Capture Checkout* Script has an empty function *UseQRTexture* to put any logic in needed to integrate the QR Code that leads to the checkout

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
 
## Best Practices
* Having an evenly lit character will give the best results. Any shadows on the mesh will be baked into the final model.
* Pure unlit shaders are not recommended. The meshing process needs shading to figure out the depth of points within a model.
* Recommended:
    * Lit shaders with fixed lights around character
    * Unlit shaders with strong ambient occlusion
* Highly specular materials can create artifacts within the final model
* Lighting is even more important on simple, stylized characters, as any shadows baked on are a lot more noticeable than detailed characters.

![shaded](https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/71d33916-9a49-4877-a4d0-e9e998340bb1)
Un-ideal: Would have shadows baked in and model will be printed out darker compared to 

![lit](https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/2a2e12b0-9354-4175-9926-84b5c4418576)
Ideal: Mostly evenly lit with rough materials.
