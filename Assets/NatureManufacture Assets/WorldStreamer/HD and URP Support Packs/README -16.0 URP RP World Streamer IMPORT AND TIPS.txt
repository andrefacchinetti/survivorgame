
BEFORE YOU START:
- you need Unity 2023.2+
- you need URP SRP pipline 16.0
Be patient URP RP tech is still fluid and fresh...

Step 1 - You can improve FPS amount by 30% if you change rendering path from forward to deferred at rendering setting. 
Find File "UniversalRenderer" and change Rendering path from forward to deferred. Forward render is ok too but it's slower for big open scenes.

Step 2 - Setup Shadows and other render setups. Find File "UniversalRP-HighQuality" 
    - Change shadow distance to 1300 or higer
	- Turn on "Opaque Texture" this will fix water translucency and distortion
	- Turn on "Depth Texture" this will fix water visibility at playmode
	- Optionaly use 1k or 2k shadow resolution. We used 2k.
	- Turn on HDR if its turned off

Step 3 Go to project settings: 
    - Player and set:  Color Space to Linear
    - Quality settings: Go to quality settings and: 
	     * use ultra level 
	     * turn turn off vsync
		 * lod bias should be around 1.5-2 and 1 for low end devices.
                        

Step 4 Find "Game Streamer Scene" and open it.

Step 5 - Add scenes from world streamer into build settings

Step 6 - HIT PLAY!:)


