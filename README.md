# IGC Unity

[In Game Collectables](https://www.igc.studio/) package for URP, developed under Unity 2021, tested with version 2022, on PC and Mobile platforms. This package includes assets to capture camera transforms and rendered images of a target 3D model and upload the result to the IGC platform.

## Usage

### Step 1: Set Up

* Get your API Key from the [IGC Platform](https://platform.igc.studio/).
* Place `Capturer` Prefab within Scene, arrange so that the origin of the prefab is at the centroid of the target model.
* Configure the capturer prefab:
    * Set the `API_Key` with the value from the IGC Platform.
    * Add all the layers that contain the target model to the `Shown Layers` array.
    * Adjust `Capture Radius` so that the target model is completely captured in all the rendered images.
* Read the [Best Practices](#Best-Practices) to ensure that images are captured under ideal lighting and rendering conditions.

### Step 2: Capturing

* Use function `StartCapture()`  (`CharacterCapture.cs`) to start a capture. Chose whether or not to use the `asyncCapture` parameter:
    * Set to `true` if the character and lighting does not change between frames. This is preferred since it avoids interrupting the UI, but you need to make sure that any dynamic effects and idle animations are disabled for the duration of the capture.
    * Set to `false` where the character or lighting might change. The game may momentarily freeze for up to a few seconds while the images are rendered.
* The `onCaptureFinish` will be called when Capturing is finished
* Captured data is saved to `PROJECT_NAME/OUTPUT/Captures/`, you set the `Custom Output Path` property of the `Capturer` prefab to override this

### Step 3: Uploading

* Use `UploadCaptures()` (`CharacterCapture.cs`) to start uploading the data.
    * The `onUploadSuccess` event will be called when the Upload is finished
    * The `onUploadFailed` event will be called and give a string description on why the Upload has failed

### Step 4: Checkout

* After the upload is done, `onUploadSuccess` will be given a URL to the checkout page and a QR Code texture leading to the same page

### Combined Capture + Upload

* The event `StartCaptureAndUpload()` can be used to combine the steps for Capturing and Uploading

## Best Practices

* Capture at least 100 images with resolutions of  `1024x1024` or larger.
* Characters should occupy as much of the rendered images as possible (you may need some trial captures to test this).

### Manufacturing Limitations

In order to ensure that results are manufacturable:

* Avoid models with disconnected or floating components
* Ensure mesh back-faces are not missing
* Materials should be opaque and diffuse. Transparent or reflective materials should be replaced with similar non-reflective equivalents.

### Lighting

* Unlit shading is not recommended; the meshing process needs shading to figure out the depth of points within a model.
* Uniform lighting with minimal shadows is recommended, since any shadows present in images will impact model quality and may be baked into the final reconstruction.
    * This is especially important for simple models with large uniform surfaces
* Recommended:
    * Lit shaders with ambient lighting
    * Lit shaders with fixed lights evenly around character
    * Unlit shaders with strong ambient occlusion
* Materials with high roughness and little specular are recommended.
    * Highly specular and highly metallic materials will create artifacts within the final model.

### Shadows (left) vs Ideal Lighting (right)

![pigeon](https://github.com/In-Game-Collectables/IGC_Unity2021_URP/assets/35625367/efab12b6-946e-4895-bd84-4a9d530ff68a)

![robot](https://github.com/In-Game-Collectables/IGC_Unity2021_URP/assets/35625367/84cd22ed-6767-4901-9bfd-4b7bb670a4d6)

### Ideal Lighting Examples

![pigeon](https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/13398085-397f-43d2-8756-01e94a8c5d3d)

![robot](https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/51be5bf6-64f0-45fa-85ec-996c11f8b183)

![mushroom](https://github.com/In-Game-Collectables/IGC_UE4/assets/35625367/1604f6ef-7124-40d0-9a0d-7403ae29ded6)

## Notes
The variable `CurrentStage` on the `Capturer` can be used to determine whether the current capture state is `Capturing`, `Uploading`, `CheckingOut`, or `None` (idle).

## Support
Join the [Discord](https://discord.gg/JP2fEh4cNP) for any questions, feedback or even just a chat! (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧
