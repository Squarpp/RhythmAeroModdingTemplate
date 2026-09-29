RHYTHM AERO - CUSTOM CHARACTER MODDING TEMPLATE
Unity Version: 6000.3.13f1

Welcome to the Rhythm Aero SDK! This template allows you to easily create, pack, and export your own custom 3D characters into the game.

STEP 1: PREPARE YOUR CHARACTER

Import your 3D model and textures into the "Assets" folder.

Drag your model into the Scene.

IMPORTANT: Select your character and change its Layer to "UI" (located in the top-right corner of the Inspector).

Drag your character from the Scene back into the "Assets" window to convert it into a Prefab (it will turn into a blue box icon).

STEP 2: TAGGING FOR EXPORT

Select your new Prefab in the "Assets" window.

Look at the very bottom-right of the Inspector window for the "AssetBundle" section.

Click the dropdown and assign an AssetBundle name (or click "New..." and type a simple name).

STEP 3: EXPORTING THE BUNDLE

Open the main Scene provided in this template.

Enter Play Mode (press the Play button at the top of the editor).

Click the "Create AssetBundle" button visible on the game screen.

Exit Play Mode. Your exported file will now be automatically generated inside the "Assets/ExportedBundles" folder.

STEP 4: PACKAGING THE MOD

Go to the "Assets/Exported_EXAMPLE" folder. You will see a template folder named "YourCharacter_1".

Make a copy of "YourCharacter_1" and rename it to your actual character's name.

Move the file you exported in Step 3 into your new character folder.

IMPORTANT: Rename your exported file to exactly "character" (in lowercase, with no file extension).

STEP 5: METADATA & COVER ART
Inside your character's folder, you need two additional files to make it work in the game menus:

cover.png or cover.jpg: A square image (e.g., 512x512) that will be displayed in the game's character selection screen.

data.json: A text file that contains your character's metadata and positional offsets.

Example of data.json:
{
"creatorName": "Squarp",
"characterName": "Bart Simpson",
"offsetX": 0.0,
"offsetY": 0.0,
"offsetZ": 1.5,
"rotX": 0.0,
"rotY": 0.0,
"rotZ": 0.0
}

(Tip: If your character is floating, misaligned, or facing the wrong way in-game, simply adjust the offset and rotation values in this JSON file. You don't need to rebuild the AssetBundle in Unity!)

STEP 6: INSTALLING THE MOD IN RHYTHM AERO

Open your computer's Documents folder.

Navigate to: Documents / Rhythm Aero

Inside, look for the "CUSTOM_CHARACTERS" folder. If it doesn't exist, create it. Make sure it is completely in UPPERCASE.

Move your entire finalized character folder (the one containing 'character', 'cover.png', and 'data.json') inside CUSTOM_CHARACTERS.

Launch Rhythm Aero, head to the Pre-Level configuration screen, and your new character will be waiting for you!
