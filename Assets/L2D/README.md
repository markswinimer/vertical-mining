Thank you for downloading L2D!

Compatibility:
	Unity 2019.3.2f1 or newer
	URP 7.3.1 or newer

Installation: 
	1. Import package into unity.
	2. Click the menu button "L2D->Run L2D Setup"
	3. Find the file L2D->Rendering->UniversalRP-L2D.asset and make sure the renderer in position 0 
		is set to the ForwardRendererL2D.asset file
	4. Open a new scene with an orthographic camera and click the menu button "L2D->Quick Import to Scene"
	5. Just start adding light to the scene! GameObject->L2D->Point Light
	6. If you are unsure what a varible does, just hover over it for a description

Modifying layers:
	L2D uses specific layers to know which GameObjects are to be treated as Lights,
	objects that block light, or objects that can't be seen without light. By default
	L2D will use layers 29, 30, 31. You can change this by opening the L2D project
	settings or by clicking the menu button "L2D->Open Settings"

Uninstallation:
	1. Delete the top level L2D folder
	2. Go to "Edit->Project Settings->Quality" and make sure Rendering is not set to null
	3. Go to "Edit->Project Settings->Graphics" and make sure Scriptable Render
	   Pipleine Settings is not set to null
	4. You may need to resolve further errors with your URPAsset but L2D is removed