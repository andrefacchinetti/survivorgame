BEFORE YOU START:
- you need Unity  2023.2 or higher 
- you need HD SRP pipeline 16.0 if you use higher etc custom shaders could not work but seems they should. 
That's why we provide 16.0 version which seems to work with much higher versions aswell. 
For all higher RP versions please use 16.0 HD RP support pack.

Be patient this tech is so fluid... we coudn't fallow ever beta version

Step 1 
	- !!!! IMPORTANT !!!! Find diffusion profile list section at HDRP Default Settings and drag and drop our SSS settings diffusion profiles for foliage and water into Diffusion profile list:
		  NM_SSSSettings_Skin_NM Foliage Trees
		  NM_SSSSettings_Skin_NM Foliage
		  NM_SSSSettings_Water_Forest
		  
	Without this foliage, water materials will not become affected by scattering and they will look wrong.
	Open "HDRPMediumQuality" in project settings or "HDRPHighQuality" depends what unity use i your projectas default and:
	- Check if contact shadows are turned on
	- LOD Bias to = 1 or 1.5


Step 2 Go to project settings and quality and set:
	- Set VSync to don't sync

Step 3 Find "Game Streamer Scene" and open it.

Step 4 - Add scenes from streamer into build settings

Step 5 - HIT PLAY!:)

IMPORTANT:
If you notice in console error:
No more space in Reflection Probe Atlas. To solve this issue, increase the size of the Reflection Probe Atlas in the HDRP settings.
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)
Just change reflection atlas size at hd rp settings into 4kx8k. They mess something in 2023.1 version like hell and engine have huge issues, memory leaks etc at 2022.2f0 2022.2f1. We will see how it will behave in the future. 
I feel atm it's the worst and most bugged version ever released so be patient as unity will fix everything step by step as always.

