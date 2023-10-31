# IGC Unity
This document is currently a work in progress.

[In Game Collectables](https://www.igc.studio/) Plugin built on Unity version 2021.3.28f1. Works with URP

This plugin will capture renders spun around a character and export out a JSON file of the relative camera transforms. It will upload the necessary files to the IGC API to process to re-create a printable mesh.

## How to use
### Step 1: Set Up
* Get your API Key from the [IGC Platform](https://platform.igc.studio/collectables)
* Place *Capturer* Prefab within Scene, positioned at the center of the character to be captured
* Input your API Key in the *Capturer*'s API_Key parameter
* Adjust _Capture Radius_ to fit whole character within renders
* Add all Layers the character is shown on to *Shown Layers*. Hide any other objects in the same layers during capture, or else they can also appear in the final model.
* Read the [Best Practices](https://github.com/In-Game-Collectables/IGC_Unity2021#best-practices) section for the ideal set up
### Step 2: Capturing
* Use function *StartCapture()* within *CharacterCapture.cs* Script to export out all renders/files.
    * The function has an optional parameter *asyncCapture* that determines if there should be a delay between captures
        * If *asyncCapture* is false, it may freeze the game up to a couple seconds depending on the dimensions of the renders
        * *asyncCapture* should NOT be used if the character or lighting can animate/change between frames
* The event *onCaptureFinish* will be called when Capturing is finished
* Renders/files will be outputed to *PROJECT_NAME/OUTPUT/Captures/* by default
* The event *StartCaptureAndUpload()* can be used to combine the steps for Capturing and Uploading
### Step 3: Uploading
* Use *UploadCaptures()* within *CharacterCapture.cs* Script. This will upload all images and files within the Captures folder to the API
    * The event *onUploadSuccess* will be called when the Upload is finished
    * The event *onUploadFailed* will be called and give a string description on why the Upload has failed
### Step 4: Checkout
* After the upload is done, *onUploadSuccess* will be given a URL to the checkout page and a QR Code texture leading to the same page

<br />

#### Parameters
* Character Capture > API_Key
    *  API Key for the IGC Platform
* Character Capture > Capture Radius
    * Size of radius of the camera
* Character Capture > FOV
    * Field of View for both cameras
* Character Capture > Frames
    * Number of images to be rendered out
* Character Capture > Dimension
    * Dimension of both sides of image
* Character Capture > Shown Layers
    * The layers that the character meshes should live on. Used for masking out character from the background. If empty, will default to rendering everything
    
<br />

## Notes
The variable *CurrentStage* on the *Capturer* can be used to see if it is currently Capturing, Uploading, CheckingOut, or not doing anything.

<br />

## Best Practices
### Settings
* At least 100 frames at 1024x1024 should be uploaded for best quality.
### Mesh
* The character mesh should take up as much space possible within the renders without cutting anything off.
* The mesh should not have any floating pieces.
* Meshes should not have their backfaces missing.
### Lighting & Materials
* Transparent materials may create artifacts within the final model so it is best not to use them if possible.
* Having an evenly lit character will give the best results. Any shadows or lighting on the mesh will be baked into the final model.
* Pure unlit shaders are not recommended. The meshing process needs shading to figure out the depth of points within a model.
* Recommended:
    * Lit shaders with ambient lighting
    * Lit shaders with fixed lights evenly around character
    * Unlit shaders with strong ambient occlusion
* Materials with high roughness and little specular are recommended.
    * Highly specular and highly metallic materials will create artifacts within the final model.
* Lighting is even more important on simple, stylized characters, as any shadows baked on are a lot more noticeable.

<p align="center">
<img src="https://github.com/In-Game-Collectables/IGC_Unity2021/assets/35625367/efab12b6-946e-4895-bd84-4a9d530ff68a" width="512" height="256">
<img src="https://github.com/In-Game-Collectables/IGC_Unity2021/assets/35625367/84cd22ed-6767-4901-9bfd-4b7bb670a4d6" width="512" height="256">
 <p align="center">Not Ideal vs. Ideal Lighting</p>
</p>

### Ideal Lighting Examples
<p align="center">

![pigeon](https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/13398085-397f-43d2-8756-01e94a8c5d3d)

![robot](https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/51be5bf6-64f0-45fa-85ec-996c11f8b183)

![mushroom](https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/1604f6ef-7124-40d0-9a0d-7403ae29ded6)

</p>

[Pigeon](https://sketchfab.com/3d-models/pigeon-quirky-series-e607ed34d37d433496d5a557c8230b28)

[Robot](https://sketchfab.com/3d-models/robot-4-b0c5f2f5ac04402dad029d6516d706b9)

[Mushroom](https://sketchfab.com/3d-models/cuute-mushroom-ffc370ddc6d542d590b9f503d0892ce0)

<br />

## Support
Join the [Discord](https://discord.gg/JP2fEh4cNP) for any questions, feedback or even just a chat!
