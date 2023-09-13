# IGC Unity
This document is currently a work in progress.

Built on Unity version 2021.3.28f1. Works with URP

This plugin will capture renders spun around a target object and export out a JSON file of the relative camera transforms. It can upload the necessary files to the IGC APII to process to re-create a printable mesh.

## How to use
### Capturing
* Place *Capturer* Prefab within Scene and add the object to capture to the Target parameter within the _CharacterCapture.cs_ Script
* Adjust the Height Offset within the prefab to point to the center of Target character
* Adjust _Capture Radius_ to fit whole character within renders
* Renders/files will be outputed to *PROJECT_NAME/OUTPUT/Captures/*
* Use function *Capture()* within *CharacterCapture.cs* Script to export out all renders/files. This may freeze the game up to a couple seconds depending on the dimensions of the renders.
### Uploading
* Use *UploadCaptures()* within *CharacterCapture.cs* Script. This will upload all images and files within *PROJECTNAME/OUTPUTS/Captures/* to the API
* * The API will return a URL to the checkout page after the upload is complete
* The *Capture Checkout* Script has an empty function *UseQRTexture* to put any logic in needed to integrate a QR Code texture that leads to the checkout

### Parameters
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

## Best Practices
* At least 100 frames at 2048x2048 should be uploaded for best quality.
* The character should take up as much space possible within the renders without cutting anything off.
* The mesh should not have any floating pieces.
* Having an evenly lit character will give the best results. Any shadows on the mesh will be baked into the final model.
* Pure unlit shaders are not recommended. The meshing process needs shading to figure out the depth of points within a model.
* Recommended:
    * Lit shaders with fixed lights around character
    * Unlit shaders with strong ambient occlusion
* Materials with high roughness and little specular are recommended.
* Highly specular and highly metallic materials will create artifacts within the final model.
* Lighting is even more important on simple, stylized characters, as any shadows baked on are a lot more noticeable on simple characters rather than detailed characters.

<p align="center">
<img src="https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/e018ec43-adc9-499a-93c0-48ae76b465e7" width="512" height="256">
<img src="https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/3025d1d8-fd54-4fff-b602-1d2a4935f81f" width="512" height="256">
 <p align="center">Directional Lighting vs. Evenly Shaded</p>
</p>

### Ideal Lighting Examples
<p align="center">
 
![pigeon](https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/0de3ea1d-7b58-44ab-aae1-a9b133c3b298)

![robot](https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/011f8c02-86a3-467d-bde5-a02fbf65b6ba)

![mushroom](https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/6c4fd85a-ff61-4e3e-ad42-46d6b329a899)

</p>

[Pigeon](https://sketchfab.com/3d-models/pigeon-quirky-series-e607ed34d37d433496d5a557c8230b28)

[Robot](https://sketchfab.com/3d-models/robot-4-b0c5f2f5ac04402dad029d6516d706b9)

[Mushroom](https://sketchfab.com/3d-models/cuute-mushroom-ffc370ddc6d542d590b9f503d0892ce0)
