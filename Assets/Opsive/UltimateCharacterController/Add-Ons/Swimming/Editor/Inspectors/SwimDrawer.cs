/// ---------------------------------------------
/// Ultimate Character Controller
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateCharacterController.Editor.Inspectors.Character.Abilities
{
	using Opsive.Shared.Editor.UIElements.Controls;
	using Opsive.UltimateCharacterController.Editor.Controls.Types.AbilityDrawers;
	using Opsive.UltimateCharacterController.Editor.Utility;
	using UnityEditor;
	using UnityEditor.Animations;
	using UnityEngine;

	/// <summary>
	/// Draws a custom inspector for the Swim Ability.
	/// </summary>
	[ControlType(typeof(Opsive.UltimateCharacterController.AddOns.Swimming.Swim))]
	public class SwimDrawer : DetectObjectAbilityBaseDrawer
	{
		// ------------------------------------------- Start Generated Code -------------------------------------------
		// ------- Do NOT make any changes below. Changes will be removed when the animator is generated again. -------
		// ------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Returns true if the ability can build to the animator.
		/// </summary>
		public override bool CanBuildAnimator { get { return true; } }

		/// <summary>
		/// An editor only method which can add the abilities states/transitions to the animator.
		/// </summary>
		/// <param name="animatorControllers">The Animator Controllers to add the states to.</param>
		/// <param name="firstPersonAnimatorControllers">The first person Animator Controllers to add the states to.</param>
		public override void BuildAnimator(AnimatorController[] animatorControllers, AnimatorController[] firstPersonAnimatorControllers)
		{
			for (int i = 0; i < animatorControllers.Length; ++i) {
				if (animatorControllers[i].layers.Length <= 0) {
					Debug.LogWarning("Warning: The animator controller does not contain the same number of layers as the demo animator. All of the animations cannot be added.");
					return;
				}

				var baseStateMachine1414889828 = animatorControllers[i].layers[0].stateMachine;

				// The state machine should start fresh.
				for (int j = 0; j < animatorControllers[i].layers.Length; ++j) {
					for (int k = 0; k < baseStateMachine1414889828.stateMachines.Length; ++k) {
						if (baseStateMachine1414889828.stateMachines[k].stateMachine.name == "Swim") {
							baseStateMachine1414889828.RemoveStateMachine(baseStateMachine1414889828.stateMachines[k].stateMachine);
							break;
						}
					}
				}

				// AnimationClip references.
				var fallInWaterAnimationClip26892Path = AssetDatabase.GUIDToAssetPath("5417f5795beed3748a4a339e448541f9"); 
				var fallInWaterAnimationClip26892 = AnimatorBuilder.GetAnimationClip(fallInWaterAnimationClip26892Path, "FallInWater");
				var surfaceSwimIdleAnimationClip26906Path = AssetDatabase.GUIDToAssetPath("49a950985203bcd47b45a342dd66617e"); 
				var surfaceSwimIdleAnimationClip26906 = AnimatorBuilder.GetAnimationClip(surfaceSwimIdleAnimationClip26906Path, "SurfaceSwimIdle");
				var surfacePowerSwimBwdAnimationClip26922Path = AssetDatabase.GUIDToAssetPath("656232ee21ea0ff41abf60f04e64858c"); 
				var surfacePowerSwimBwdAnimationClip26922 = AnimatorBuilder.GetAnimationClip(surfacePowerSwimBwdAnimationClip26922Path, "SurfacePowerSwimBwd");
				var surfaceSwimBwdAnimationClip26924Path = AssetDatabase.GUIDToAssetPath("1c6fd722260a32f49b477f2ec339ab20"); 
				var surfaceSwimBwdAnimationClip26924 = AnimatorBuilder.GetAnimationClip(surfaceSwimBwdAnimationClip26924Path, "SurfaceSwimBwd");
				var surfacePowerSwimStrafeAnimationClip26926Path = AssetDatabase.GUIDToAssetPath("bb557eb0c7ebd964b9909d247bf877d3"); 
				var surfacePowerSwimStrafeAnimationClip26926 = AnimatorBuilder.GetAnimationClip(surfacePowerSwimStrafeAnimationClip26926Path, "SurfacePowerSwimStrafe");
				var surfaceSwimStrafeAnimationClip26928Path = AssetDatabase.GUIDToAssetPath("8469ba753dfb3ce4fabe0bab48e638d0"); 
				var surfaceSwimStrafeAnimationClip26928 = AnimatorBuilder.GetAnimationClip(surfaceSwimStrafeAnimationClip26928Path, "SurfaceSwimStrafe");
				var surfaceSwimFwdAnimationClip26930Path = AssetDatabase.GUIDToAssetPath("34888dbddbf44204cb7ce3d920c28fc6"); 
				var surfaceSwimFwdAnimationClip26930 = AnimatorBuilder.GetAnimationClip(surfaceSwimFwdAnimationClip26930Path, "SurfaceSwimFwd");
				var surfacePowerSwimFwdAnimationClip26932Path = AssetDatabase.GUIDToAssetPath("60ee98433976c904fa0f81f9698a3f57"); 
				var surfacePowerSwimFwdAnimationClip26932 = AnimatorBuilder.GetAnimationClip(surfacePowerSwimFwdAnimationClip26932Path, "SurfacePowerSwimFwd");
				var underwaterSwimBwdAnimationClip26950Path = AssetDatabase.GUIDToAssetPath("18ed38574f4ad754c911778975d2963b"); 
				var underwaterSwimBwdAnimationClip26950 = AnimatorBuilder.GetAnimationClip(underwaterSwimBwdAnimationClip26950Path, "UnderwaterSwimBwd");
				var underwaterKickStrafeAnimationClip26952Path = AssetDatabase.GUIDToAssetPath("44eb2f91ce4dd134ca5df47dad8e2128"); 
				var underwaterKickStrafeAnimationClip26952 = AnimatorBuilder.GetAnimationClip(underwaterKickStrafeAnimationClip26952Path, "UnderwaterKickStrafe");
				var underwaterStrokeStrafeAnimationClip26954Path = AssetDatabase.GUIDToAssetPath("1b3960f74d07e0342b8a63b4bb757ca4"); 
				var underwaterStrokeStrafeAnimationClip26954 = AnimatorBuilder.GetAnimationClip(underwaterStrokeStrafeAnimationClip26954Path, "UnderwaterStrokeStrafe");
				var underwaterIdleUpAnimationClip26962Path = AssetDatabase.GUIDToAssetPath("04bb0ea0103e9534d99025bd9c28c33e"); 
				var underwaterIdleUpAnimationClip26962 = AnimatorBuilder.GetAnimationClip(underwaterIdleUpAnimationClip26962Path, "UnderwaterIdleUp");
				var underwaterIdleFwdAnimationClip26964Path = AssetDatabase.GUIDToAssetPath("99c8c8d422f251a48b0dcdae23379ed3"); 
				var underwaterIdleFwdAnimationClip26964 = AnimatorBuilder.GetAnimationClip(underwaterIdleFwdAnimationClip26964Path, "UnderwaterIdleFwd");
				var underwaterIdleDownAnimationClip26966Path = AssetDatabase.GUIDToAssetPath("a55727babd23dbf4c8e77812456885df"); 
				var underwaterIdleDownAnimationClip26966 = AnimatorBuilder.GetAnimationClip(underwaterIdleDownAnimationClip26966Path, "UnderwaterIdleDown");
				var underwaterStrokeUpAnimationClip26968Path = AssetDatabase.GUIDToAssetPath("d8bd72cf843e70642bf0ea85025013ba"); 
				var underwaterStrokeUpAnimationClip26968 = AnimatorBuilder.GetAnimationClip(underwaterStrokeUpAnimationClip26968Path, "UnderwaterStrokeUp");
				var underwaterStrokeFwdAnimationClip26970Path = AssetDatabase.GUIDToAssetPath("eaa27658d6a72d14ca7f62229295b62d"); 
				var underwaterStrokeFwdAnimationClip26970 = AnimatorBuilder.GetAnimationClip(underwaterStrokeFwdAnimationClip26970Path, "UnderwaterStrokeFwd");
				var underwaterStrokeDownAnimationClip26972Path = AssetDatabase.GUIDToAssetPath("52fcf2bd8220b464d8498e0642c27250"); 
				var underwaterStrokeDownAnimationClip26972 = AnimatorBuilder.GetAnimationClip(underwaterStrokeDownAnimationClip26972Path, "UnderwaterStrokeDown");
				var underwaterKickUpAnimationClip26974Path = AssetDatabase.GUIDToAssetPath("426dee5db36de4944860bae40dc080a5"); 
				var underwaterKickUpAnimationClip26974 = AnimatorBuilder.GetAnimationClip(underwaterKickUpAnimationClip26974Path, "UnderwaterKickUp");
				var underwaterKickFwdAnimationClip26976Path = AssetDatabase.GUIDToAssetPath("274cdf2dc4e78834c813439bb16217ee"); 
				var underwaterKickFwdAnimationClip26976 = AnimatorBuilder.GetAnimationClip(underwaterKickFwdAnimationClip26976Path, "UnderwaterKickFwd");
				var underwaterKickDownAnimationClip26978Path = AssetDatabase.GUIDToAssetPath("0c15d136a6ed78940adfecd9eb19afb4"); 
				var underwaterKickDownAnimationClip26978 = AnimatorBuilder.GetAnimationClip(underwaterKickDownAnimationClip26978Path, "UnderwaterKickDown");
				var diveFromSurfaceAnimationClip26984Path = AssetDatabase.GUIDToAssetPath("c49a2659f0fec5d4898d185c3fc51a99"); 
				var diveFromSurfaceAnimationClip26984 = AnimatorBuilder.GetAnimationClip(diveFromSurfaceAnimationClip26984Path, "DiveFromSurface");
				var swimExitWaterAnimationClip26988Path = AssetDatabase.GUIDToAssetPath("4805feacf83e71f4d85fb6721373364b"); 
				var swimExitWaterAnimationClip26988 = AnimatorBuilder.GetAnimationClip(swimExitWaterAnimationClip26988Path, "SwimExitWater");
				var surfaceSwimToIdleAnimationClip26992Path = AssetDatabase.GUIDToAssetPath("59e074ba83e4ebf49bb6bbafee759137"); 
				var surfaceSwimToIdleAnimationClip26992 = AnimatorBuilder.GetAnimationClip(surfaceSwimToIdleAnimationClip26992Path, "SurfaceSwimToIdle");

				// State Machine.
				var swimAnimatorStateMachine23620 = baseStateMachine1414889828.AddStateMachine("Swim", new Vector3(624f, 108f, 0f));

				// States.
				var fallInWaterAnimatorState24444 = swimAnimatorStateMachine23620.AddState("Fall In Water", new Vector3(144f, 48f, 0f));
				fallInWaterAnimatorState24444.motion = fallInWaterAnimationClip26892;
				fallInWaterAnimatorState24444.cycleOffset = 0f;
				fallInWaterAnimatorState24444.cycleOffsetParameterActive = false;
				fallInWaterAnimatorState24444.iKOnFeet = false;
				fallInWaterAnimatorState24444.mirror = false;
				fallInWaterAnimatorState24444.mirrorParameterActive = false;
				fallInWaterAnimatorState24444.speed = 1.5f;
				fallInWaterAnimatorState24444.speedParameterActive = false;
				fallInWaterAnimatorState24444.writeDefaultValues = true;

				var surfaceIdleAnimatorState24446 = swimAnimatorStateMachine23620.AddState("Surface Idle", new Vector3(530f, -190f, 0f));
				surfaceIdleAnimatorState24446.motion = surfaceSwimIdleAnimationClip26906;
				surfaceIdleAnimatorState24446.cycleOffset = 0f;
				surfaceIdleAnimatorState24446.cycleOffsetParameterActive = false;
				surfaceIdleAnimatorState24446.iKOnFeet = false;
				surfaceIdleAnimatorState24446.mirror = false;
				surfaceIdleAnimatorState24446.mirrorParameterActive = false;
				surfaceIdleAnimatorState24446.speed = 1f;
				surfaceIdleAnimatorState24446.speedParameterActive = false;
				surfaceIdleAnimatorState24446.writeDefaultValues = true;

				var surfaceSwimAnimatorState24454 = swimAnimatorStateMachine23620.AddState("Surface Swim", new Vector3(230f, -190f, 0f));
				var surfaceSwimAnimatorState24454blendTreeBlendTree26920 = new BlendTree();
				AssetDatabase.AddObjectToAsset(surfaceSwimAnimatorState24454blendTreeBlendTree26920, animatorControllers[i]);
				surfaceSwimAnimatorState24454blendTreeBlendTree26920.hideFlags = HideFlags.HideInHierarchy;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920.blendParameter = "HorizontalMovement";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920.blendParameterY = "ForwardMovement";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920.blendType = BlendTreeType.FreeformCartesian2D;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920.maxThreshold = 8f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920.minThreshold = 0f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920.name = "Blend Tree";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920.useAutomaticThresholds = false;
				var surfaceSwimAnimatorState24454blendTreeBlendTree26920Child0 =  new ChildMotion();
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child0.motion = surfacePowerSwimBwdAnimationClip26922;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child0.cycleOffset = 0f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child0.directBlendParameter = "HorizontalMovement";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child0.mirror = false;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child0.position = new Vector2(0f, -2f);
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child0.threshold = 0f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child0.timeScale = 1.25f;
				var surfaceSwimAnimatorState24454blendTreeBlendTree26920Child1 =  new ChildMotion();
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child1.motion = surfaceSwimBwdAnimationClip26924;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child1.cycleOffset = 0f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child1.directBlendParameter = "HorizontalMovement";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child1.mirror = false;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child1.position = new Vector2(0f, -1f);
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child1.threshold = 1f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child1.timeScale = 1f;
				var surfaceSwimAnimatorState24454blendTreeBlendTree26920Child2 =  new ChildMotion();
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child2.motion = surfacePowerSwimStrafeAnimationClip26926;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child2.cycleOffset = 0f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child2.directBlendParameter = "HorizontalMovement";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child2.mirror = false;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child2.position = new Vector2(-2f, 0f);
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child2.threshold = 2f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child2.timeScale = 1.25f;
				var surfaceSwimAnimatorState24454blendTreeBlendTree26920Child3 =  new ChildMotion();
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child3.motion = surfaceSwimStrafeAnimationClip26928;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child3.cycleOffset = 0f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child3.directBlendParameter = "HorizontalMovement";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child3.mirror = false;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child3.position = new Vector2(-1f, 0f);
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child3.threshold = 3f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child3.timeScale = 1f;
				var surfaceSwimAnimatorState24454blendTreeBlendTree26920Child4 =  new ChildMotion();
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child4.motion = surfaceSwimIdleAnimationClip26906;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child4.cycleOffset = 0f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child4.directBlendParameter = "HorizontalMovement";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child4.mirror = false;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child4.position = new Vector2(0f, 0f);
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child4.threshold = 4f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child4.timeScale = 1f;
				var surfaceSwimAnimatorState24454blendTreeBlendTree26920Child5 =  new ChildMotion();
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child5.motion = surfaceSwimStrafeAnimationClip26928;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child5.cycleOffset = 0.5f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child5.directBlendParameter = "HorizontalMovement";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child5.mirror = true;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child5.position = new Vector2(1f, 0f);
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child5.threshold = 5f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child5.timeScale = 1f;
				var surfaceSwimAnimatorState24454blendTreeBlendTree26920Child6 =  new ChildMotion();
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child6.motion = surfacePowerSwimStrafeAnimationClip26926;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child6.cycleOffset = 0.5f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child6.directBlendParameter = "HorizontalMovement";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child6.mirror = true;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child6.position = new Vector2(2f, 0f);
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child6.threshold = 6f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child6.timeScale = 1.25f;
				var surfaceSwimAnimatorState24454blendTreeBlendTree26920Child7 =  new ChildMotion();
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child7.motion = surfaceSwimFwdAnimationClip26930;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child7.cycleOffset = 0f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child7.directBlendParameter = "HorizontalMovement";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child7.mirror = false;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child7.position = new Vector2(0f, 1f);
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child7.threshold = 7f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child7.timeScale = 1f;
				var surfaceSwimAnimatorState24454blendTreeBlendTree26920Child8 =  new ChildMotion();
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child8.motion = surfacePowerSwimFwdAnimationClip26932;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child8.cycleOffset = 0f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child8.directBlendParameter = "HorizontalMovement";
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child8.mirror = false;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child8.position = new Vector2(0f, 2f);
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child8.threshold = 8f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920Child8.timeScale = 1.25f;
				surfaceSwimAnimatorState24454blendTreeBlendTree26920.children = new ChildMotion[] {
					surfaceSwimAnimatorState24454blendTreeBlendTree26920Child0,
					surfaceSwimAnimatorState24454blendTreeBlendTree26920Child1,
					surfaceSwimAnimatorState24454blendTreeBlendTree26920Child2,
					surfaceSwimAnimatorState24454blendTreeBlendTree26920Child3,
					surfaceSwimAnimatorState24454blendTreeBlendTree26920Child4,
					surfaceSwimAnimatorState24454blendTreeBlendTree26920Child5,
					surfaceSwimAnimatorState24454blendTreeBlendTree26920Child6,
					surfaceSwimAnimatorState24454blendTreeBlendTree26920Child7,
					surfaceSwimAnimatorState24454blendTreeBlendTree26920Child8
				};
				surfaceSwimAnimatorState24454.motion = surfaceSwimAnimatorState24454blendTreeBlendTree26920;
				surfaceSwimAnimatorState24454.cycleOffset = 0f;
				surfaceSwimAnimatorState24454.cycleOffsetParameterActive = false;
				surfaceSwimAnimatorState24454.iKOnFeet = false;
				surfaceSwimAnimatorState24454.mirror = false;
				surfaceSwimAnimatorState24454.mirrorParameterActive = false;
				surfaceSwimAnimatorState24454.speed = 1f;
				surfaceSwimAnimatorState24454.speedParameterActive = false;
				surfaceSwimAnimatorState24454.writeDefaultValues = true;

				var underwaterSwimAnimatorState24448 = swimAnimatorStateMachine23620.AddState("Underwater Swim", new Vector3(390f, 240f, 0f));
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948 = new BlendTree();
				AssetDatabase.AddObjectToAsset(underwaterSwimAnimatorState24448blendTreeBlendTree26948, animatorControllers[i]);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948.hideFlags = HideFlags.HideInHierarchy;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948.blendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948.blendParameterY = "ForwardMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948.blendType = BlendTreeType.FreeformCartesian2D;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948.maxThreshold = 90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948.minThreshold = -90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948.name = "Blend Tree";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948.useAutomaticThresholds = true;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948Child0 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child0.motion = underwaterSwimBwdAnimationClip26950;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child0.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child0.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child0.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child0.position = new Vector2(0f, -2f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child0.threshold = -90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child0.timeScale = 1.5f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948Child1 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child1.motion = underwaterSwimBwdAnimationClip26950;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child1.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child1.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child1.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child1.position = new Vector2(0f, -1f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child1.threshold = -67.5f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child1.timeScale = 1f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948Child2 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child2.motion = underwaterKickStrafeAnimationClip26952;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child2.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child2.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child2.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child2.position = new Vector2(-2f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child2.threshold = -45f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child2.timeScale = 1.5f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948Child3 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child3.motion = underwaterStrokeStrafeAnimationClip26954;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child3.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child3.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child3.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child3.position = new Vector2(-1f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child3.threshold = -22.5f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child3.timeScale = 1f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948Child4 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child4.motion = underwaterStrokeStrafeAnimationClip26954;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child4.cycleOffset = 0.5f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child4.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child4.mirror = true;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child4.position = new Vector2(1f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child4.threshold = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child4.timeScale = 1f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948Child5 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child5.motion = underwaterKickStrafeAnimationClip26952;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child5.cycleOffset = 0.5f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child5.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child5.mirror = true;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child5.position = new Vector2(2f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child5.threshold = 22.5f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child5.timeScale = 1.5f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956 = new BlendTree();
				AssetDatabase.AddObjectToAsset(underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956, animatorControllers[i]);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956.hideFlags = HideFlags.HideInHierarchy;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956.blendParameter = "AbilityFloatData";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956.blendParameterY = "Blend";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956.blendType = BlendTreeType.Simple1D;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956.maxThreshold = 90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956.minThreshold = -90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956.name = "BlendTree";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956.useAutomaticThresholds = false;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child0 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child0.motion = underwaterIdleUpAnimationClip26962;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child0.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child0.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child0.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child0.position = new Vector2(0f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child0.threshold = -90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child0.timeScale = 1f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child1 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child1.motion = underwaterIdleFwdAnimationClip26964;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child1.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child1.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child1.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child1.position = new Vector2(0f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child1.threshold = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child1.timeScale = 1f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child2 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child2.motion = underwaterIdleDownAnimationClip26966;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child2.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child2.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child2.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child2.position = new Vector2(0f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child2.threshold = 90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child2.timeScale = 1f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956.children = new ChildMotion[] {
					underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child0,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child1,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956Child2
				};
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948Child6 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child6.motion = underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26956;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child6.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child6.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child6.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child6.position = new Vector2(0f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child6.threshold = 45f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child6.timeScale = 1f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958 = new BlendTree();
				AssetDatabase.AddObjectToAsset(underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958, animatorControllers[i]);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958.hideFlags = HideFlags.HideInHierarchy;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958.blendParameter = "AbilityFloatData";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958.blendParameterY = "Blend";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958.blendType = BlendTreeType.Simple1D;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958.maxThreshold = 90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958.minThreshold = -90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958.name = "BlendTree";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958.useAutomaticThresholds = false;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child0 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child0.motion = underwaterStrokeUpAnimationClip26968;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child0.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child0.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child0.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child0.position = new Vector2(0f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child0.threshold = -90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child0.timeScale = 1f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child1 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child1.motion = underwaterStrokeFwdAnimationClip26970;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child1.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child1.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child1.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child1.position = new Vector2(0f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child1.threshold = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child1.timeScale = 1f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child2 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child2.motion = underwaterStrokeDownAnimationClip26972;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child2.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child2.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child2.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child2.position = new Vector2(0f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child2.threshold = 90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child2.timeScale = 1f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958.children = new ChildMotion[] {
					underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child0,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child1,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958Child2
				};
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948Child7 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child7.motion = underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26958;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child7.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child7.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child7.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child7.position = new Vector2(0f, 1f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child7.threshold = 67.5f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child7.timeScale = 1f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960 = new BlendTree();
				AssetDatabase.AddObjectToAsset(underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960, animatorControllers[i]);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960.hideFlags = HideFlags.HideInHierarchy;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960.blendParameter = "AbilityFloatData";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960.blendParameterY = "Blend";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960.blendType = BlendTreeType.Simple1D;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960.maxThreshold = 90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960.minThreshold = -90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960.name = "BlendTree";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960.useAutomaticThresholds = false;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child0 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child0.motion = underwaterKickUpAnimationClip26974;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child0.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child0.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child0.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child0.position = new Vector2(0f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child0.threshold = -90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child0.timeScale = 1.5f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child1 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child1.motion = underwaterKickFwdAnimationClip26976;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child1.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child1.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child1.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child1.position = new Vector2(0f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child1.threshold = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child1.timeScale = 1.5f;
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child2 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child2.motion = underwaterKickDownAnimationClip26978;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child2.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child2.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child2.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child2.position = new Vector2(0f, 0f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child2.threshold = 90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child2.timeScale = 1.5f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960.children = new ChildMotion[] {
					underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child0,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child1,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960Child2
				};
				var underwaterSwimAnimatorState24448blendTreeBlendTree26948Child8 =  new ChildMotion();
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child8.motion = underwaterSwimAnimatorState24448blendTreeBlendTree26948blendTreeBlendTree26960;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child8.cycleOffset = 0f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child8.directBlendParameter = "HorizontalMovement";
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child8.mirror = false;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child8.position = new Vector2(0f, 2f);
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child8.threshold = 90f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948Child8.timeScale = 1f;
				underwaterSwimAnimatorState24448blendTreeBlendTree26948.children = new ChildMotion[] {
					underwaterSwimAnimatorState24448blendTreeBlendTree26948Child0,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948Child1,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948Child2,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948Child3,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948Child4,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948Child5,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948Child6,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948Child7,
					underwaterSwimAnimatorState24448blendTreeBlendTree26948Child8
				};
				underwaterSwimAnimatorState24448.motion = underwaterSwimAnimatorState24448blendTreeBlendTree26948;
				underwaterSwimAnimatorState24448.cycleOffset = 0f;
				underwaterSwimAnimatorState24448.cycleOffsetParameterActive = false;
				underwaterSwimAnimatorState24448.iKOnFeet = true;
				underwaterSwimAnimatorState24448.mirror = false;
				underwaterSwimAnimatorState24448.mirrorParameterActive = false;
				underwaterSwimAnimatorState24448.speed = 1f;
				underwaterSwimAnimatorState24448.speedParameterActive = false;
				underwaterSwimAnimatorState24448.writeDefaultValues = true;

				var diveFromSurfaceAnimatorState26870 = swimAnimatorStateMachine23620.AddState("Dive From Surface", new Vector3(432f, 48f, 0f));
				diveFromSurfaceAnimatorState26870.motion = diveFromSurfaceAnimationClip26984;
				diveFromSurfaceAnimatorState26870.cycleOffset = 0f;
				diveFromSurfaceAnimatorState26870.cycleOffsetParameterActive = false;
				diveFromSurfaceAnimatorState26870.iKOnFeet = false;
				diveFromSurfaceAnimatorState26870.mirror = false;
				diveFromSurfaceAnimatorState26870.mirrorParameterActive = false;
				diveFromSurfaceAnimatorState26870.speed = 1.5f;
				diveFromSurfaceAnimatorState26870.speedParameterActive = false;
				diveFromSurfaceAnimatorState26870.writeDefaultValues = true;

				var exitWaterMovingAnimatorState26872 = swimAnimatorStateMachine23620.AddState("Exit Water Moving", new Vector3(744f, 12f, 0f));
				exitWaterMovingAnimatorState26872.motion = swimExitWaterAnimationClip26988;
				exitWaterMovingAnimatorState26872.cycleOffset = 0f;
				exitWaterMovingAnimatorState26872.cycleOffsetParameterActive = false;
				exitWaterMovingAnimatorState26872.iKOnFeet = false;
				exitWaterMovingAnimatorState26872.mirror = false;
				exitWaterMovingAnimatorState26872.mirrorParameterActive = false;
				exitWaterMovingAnimatorState26872.speed = 1.5f;
				exitWaterMovingAnimatorState26872.speedParameterActive = false;
				exitWaterMovingAnimatorState26872.writeDefaultValues = true;

				var exitWaterIdleAnimatorState26874 = swimAnimatorStateMachine23620.AddState("Exit Water Idle", new Vector3(744f, -48f, 0f));
				exitWaterIdleAnimatorState26874.motion = surfaceSwimToIdleAnimationClip26992;
				exitWaterIdleAnimatorState26874.cycleOffset = 0f;
				exitWaterIdleAnimatorState26874.cycleOffsetParameterActive = false;
				exitWaterIdleAnimatorState26874.iKOnFeet = false;
				exitWaterIdleAnimatorState26874.mirror = false;
				exitWaterIdleAnimatorState26874.mirrorParameterActive = false;
				exitWaterIdleAnimatorState26874.speed = 1.5f;
				exitWaterIdleAnimatorState26874.speedParameterActive = false;
				exitWaterIdleAnimatorState26874.writeDefaultValues = true;

				// State Machine Defaults.
				swimAnimatorStateMachine23620.anyStatePosition = new Vector3(-168f, 48f, 0f);
				swimAnimatorStateMachine23620.defaultState = surfaceIdleAnimatorState24446;
				swimAnimatorStateMachine23620.entryPosition = new Vector3(-204f, -36f, 0f);
				swimAnimatorStateMachine23620.exitPosition = new Vector3(1140f, 48f, 0f);
				swimAnimatorStateMachine23620.parentStateMachinePosition = new Vector3(1128f, -48f, 0f);

				// State Transitions.
				var animatorStateTransition26876 = fallInWaterAnimatorState24444.AddTransition(surfaceIdleAnimatorState24446);
				animatorStateTransition26876.canTransitionToSelf = true;
				animatorStateTransition26876.duration = 0.5f;
				animatorStateTransition26876.exitTime = 0.95f;
				animatorStateTransition26876.hasExitTime = true;
				animatorStateTransition26876.hasFixedDuration = true;
				animatorStateTransition26876.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26876.offset = 0f;
				animatorStateTransition26876.orderedInterruption = true;
				animatorStateTransition26876.isExit = false;
				animatorStateTransition26876.mute = false;
				animatorStateTransition26876.solo = false;
				animatorStateTransition26876.AddCondition(AnimatorConditionMode.Less, 0.1f, "ForwardMovement");
				animatorStateTransition26876.AddCondition(AnimatorConditionMode.Equals, 1f, "AbilityIntData");

				var animatorStateTransition26878 = fallInWaterAnimatorState24444.AddTransition(surfaceSwimAnimatorState24454);
				animatorStateTransition26878.canTransitionToSelf = true;
				animatorStateTransition26878.duration = 0.25f;
				animatorStateTransition26878.exitTime = 0.8f;
				animatorStateTransition26878.hasExitTime = false;
				animatorStateTransition26878.hasFixedDuration = true;
				animatorStateTransition26878.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26878.offset = 0f;
				animatorStateTransition26878.orderedInterruption = true;
				animatorStateTransition26878.isExit = false;
				animatorStateTransition26878.mute = false;
				animatorStateTransition26878.solo = false;
				animatorStateTransition26878.AddCondition(AnimatorConditionMode.Equals, 1f, "AbilityIntData");
				animatorStateTransition26878.AddCondition(AnimatorConditionMode.If, 0f, "Moving");

				var animatorStateTransition26880 = fallInWaterAnimatorState24444.AddExitTransition();
				animatorStateTransition26880.canTransitionToSelf = true;
				animatorStateTransition26880.duration = 0.25f;
				animatorStateTransition26880.exitTime = 0.8888889f;
				animatorStateTransition26880.hasExitTime = false;
				animatorStateTransition26880.hasFixedDuration = true;
				animatorStateTransition26880.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26880.offset = 0f;
				animatorStateTransition26880.orderedInterruption = true;
				animatorStateTransition26880.isExit = true;
				animatorStateTransition26880.mute = false;
				animatorStateTransition26880.solo = false;
				animatorStateTransition26880.AddCondition(AnimatorConditionMode.NotEqual, 301f, "AbilityIndex");

				var animatorStateTransition26882 = fallInWaterAnimatorState24444.AddTransition(underwaterSwimAnimatorState24448);
				animatorStateTransition26882.canTransitionToSelf = true;
				animatorStateTransition26882.duration = 0.5f;
				animatorStateTransition26882.exitTime = 0.9f;
				animatorStateTransition26882.hasExitTime = true;
				animatorStateTransition26882.hasFixedDuration = true;
				animatorStateTransition26882.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26882.offset = 0f;
				animatorStateTransition26882.orderedInterruption = true;
				animatorStateTransition26882.isExit = false;
				animatorStateTransition26882.mute = false;
				animatorStateTransition26882.solo = false;
				animatorStateTransition26882.AddCondition(AnimatorConditionMode.Equals, 2f, "AbilityIntData");
				animatorStateTransition26882.AddCondition(AnimatorConditionMode.IfNot, 0f, "Moving");

				var animatorStateTransition26886 = fallInWaterAnimatorState24444.AddTransition(underwaterSwimAnimatorState24448);
				animatorStateTransition26886.canTransitionToSelf = true;
				animatorStateTransition26886.duration = 0.25f;
				animatorStateTransition26886.exitTime = 0.8888889f;
				animatorStateTransition26886.hasExitTime = false;
				animatorStateTransition26886.hasFixedDuration = true;
				animatorStateTransition26886.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26886.offset = 0f;
				animatorStateTransition26886.orderedInterruption = true;
				animatorStateTransition26886.isExit = false;
				animatorStateTransition26886.mute = false;
				animatorStateTransition26886.solo = false;
				animatorStateTransition26886.AddCondition(AnimatorConditionMode.Equals, 2f, "AbilityIntData");
				animatorStateTransition26886.AddCondition(AnimatorConditionMode.If, 0f, "Moving");

				var animatorStateTransition26894 = surfaceIdleAnimatorState24446.AddTransition(surfaceSwimAnimatorState24454);
				animatorStateTransition26894.canTransitionToSelf = true;
				animatorStateTransition26894.duration = 0.4f;
				animatorStateTransition26894.exitTime = 0.75f;
				animatorStateTransition26894.hasExitTime = false;
				animatorStateTransition26894.hasFixedDuration = true;
				animatorStateTransition26894.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26894.offset = 0f;
				animatorStateTransition26894.orderedInterruption = true;
				animatorStateTransition26894.isExit = false;
				animatorStateTransition26894.mute = false;
				animatorStateTransition26894.solo = false;
				animatorStateTransition26894.AddCondition(AnimatorConditionMode.Equals, 1f, "AbilityIntData");
				animatorStateTransition26894.AddCondition(AnimatorConditionMode.If, 0f, "Moving");
				animatorStateTransition26894.AddCondition(AnimatorConditionMode.Equals, 0f, "Slot0ItemID");

				var animatorStateTransition26896 = surfaceIdleAnimatorState24446.AddTransition(diveFromSurfaceAnimatorState26870);
				animatorStateTransition26896.canTransitionToSelf = true;
				animatorStateTransition26896.duration = 0.25f;
				animatorStateTransition26896.exitTime = 0.75f;
				animatorStateTransition26896.hasExitTime = false;
				animatorStateTransition26896.hasFixedDuration = true;
				animatorStateTransition26896.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26896.offset = 0f;
				animatorStateTransition26896.orderedInterruption = true;
				animatorStateTransition26896.isExit = false;
				animatorStateTransition26896.mute = false;
				animatorStateTransition26896.solo = false;
				animatorStateTransition26896.AddCondition(AnimatorConditionMode.Equals, 2f, "AbilityIntData");

				var animatorStateTransition26898 = surfaceIdleAnimatorState24446.AddExitTransition();
				animatorStateTransition26898.canTransitionToSelf = true;
				animatorStateTransition26898.duration = 0.25f;
				animatorStateTransition26898.exitTime = 0.8f;
				animatorStateTransition26898.hasExitTime = false;
				animatorStateTransition26898.hasFixedDuration = true;
				animatorStateTransition26898.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26898.offset = 0f;
				animatorStateTransition26898.orderedInterruption = true;
				animatorStateTransition26898.isExit = true;
				animatorStateTransition26898.mute = false;
				animatorStateTransition26898.solo = false;
				animatorStateTransition26898.AddCondition(AnimatorConditionMode.NotEqual, 301f, "AbilityIndex");

				var animatorStateTransition26900 = surfaceIdleAnimatorState24446.AddTransition(exitWaterIdleAnimatorState26874);
				animatorStateTransition26900.canTransitionToSelf = true;
				animatorStateTransition26900.duration = 0.15f;
				animatorStateTransition26900.exitTime = 0f;
				animatorStateTransition26900.hasExitTime = false;
				animatorStateTransition26900.hasFixedDuration = true;
				animatorStateTransition26900.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26900.offset = 0f;
				animatorStateTransition26900.orderedInterruption = true;
				animatorStateTransition26900.isExit = false;
				animatorStateTransition26900.mute = false;
				animatorStateTransition26900.solo = false;
				animatorStateTransition26900.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

				var animatorStateTransition26902 = surfaceIdleAnimatorState24446.AddTransition(exitWaterMovingAnimatorState26872);
				animatorStateTransition26902.canTransitionToSelf = true;
				animatorStateTransition26902.duration = 0.15f;
				animatorStateTransition26902.exitTime = 2.63155E-10f;
				animatorStateTransition26902.hasExitTime = false;
				animatorStateTransition26902.hasFixedDuration = true;
				animatorStateTransition26902.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26902.offset = 0f;
				animatorStateTransition26902.orderedInterruption = true;
				animatorStateTransition26902.isExit = false;
				animatorStateTransition26902.mute = false;
				animatorStateTransition26902.solo = false;
				animatorStateTransition26902.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

				var animatorStateTransition26908 = surfaceSwimAnimatorState24454.AddTransition(exitWaterMovingAnimatorState26872);
				animatorStateTransition26908.canTransitionToSelf = true;
				animatorStateTransition26908.duration = 0.2259974f;
				animatorStateTransition26908.exitTime = 0.010637f;
				animatorStateTransition26908.hasExitTime = false;
				animatorStateTransition26908.hasFixedDuration = true;
				animatorStateTransition26908.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26908.offset = 0f;
				animatorStateTransition26908.orderedInterruption = true;
				animatorStateTransition26908.isExit = false;
				animatorStateTransition26908.mute = false;
				animatorStateTransition26908.solo = false;
				animatorStateTransition26908.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

				var animatorStateTransition26910 = surfaceSwimAnimatorState24454.AddTransition(exitWaterIdleAnimatorState26874);
				animatorStateTransition26910.canTransitionToSelf = true;
				animatorStateTransition26910.duration = 0.15f;
				animatorStateTransition26910.exitTime = 0f;
				animatorStateTransition26910.hasExitTime = false;
				animatorStateTransition26910.hasFixedDuration = true;
				animatorStateTransition26910.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26910.offset = 0f;
				animatorStateTransition26910.orderedInterruption = true;
				animatorStateTransition26910.isExit = false;
				animatorStateTransition26910.mute = false;
				animatorStateTransition26910.solo = false;
				animatorStateTransition26910.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

				var animatorStateTransition26912 = surfaceSwimAnimatorState24454.AddTransition(diveFromSurfaceAnimatorState26870);
				animatorStateTransition26912.canTransitionToSelf = true;
				animatorStateTransition26912.duration = 0.25f;
				animatorStateTransition26912.exitTime = 0.8f;
				animatorStateTransition26912.hasExitTime = false;
				animatorStateTransition26912.hasFixedDuration = true;
				animatorStateTransition26912.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26912.offset = 0f;
				animatorStateTransition26912.orderedInterruption = true;
				animatorStateTransition26912.isExit = false;
				animatorStateTransition26912.mute = false;
				animatorStateTransition26912.solo = false;
				animatorStateTransition26912.AddCondition(AnimatorConditionMode.Equals, 2f, "AbilityIntData");

				var animatorStateTransition26914 = surfaceSwimAnimatorState24454.AddTransition(surfaceIdleAnimatorState24446);
				animatorStateTransition26914.canTransitionToSelf = true;
				animatorStateTransition26914.duration = 0.4f;
				animatorStateTransition26914.exitTime = 0.8f;
				animatorStateTransition26914.hasExitTime = false;
				animatorStateTransition26914.hasFixedDuration = true;
				animatorStateTransition26914.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26914.offset = 0f;
				animatorStateTransition26914.orderedInterruption = true;
				animatorStateTransition26914.isExit = false;
				animatorStateTransition26914.mute = false;
				animatorStateTransition26914.solo = false;
				animatorStateTransition26914.AddCondition(AnimatorConditionMode.Equals, 1f, "AbilityIntData");
				animatorStateTransition26914.AddCondition(AnimatorConditionMode.IfNot, 0f, "Moving");

				var animatorStateTransition26916 = surfaceSwimAnimatorState24454.AddExitTransition();
				animatorStateTransition26916.canTransitionToSelf = true;
				animatorStateTransition26916.duration = 0.25f;
				animatorStateTransition26916.exitTime = 0.8f;
				animatorStateTransition26916.hasExitTime = false;
				animatorStateTransition26916.hasFixedDuration = true;
				animatorStateTransition26916.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26916.offset = 0f;
				animatorStateTransition26916.orderedInterruption = true;
				animatorStateTransition26916.isExit = true;
				animatorStateTransition26916.mute = false;
				animatorStateTransition26916.solo = false;
				animatorStateTransition26916.AddCondition(AnimatorConditionMode.NotEqual, 301f, "AbilityIndex");

				var animatorStateTransition26934 = underwaterSwimAnimatorState24448.AddTransition(surfaceIdleAnimatorState24446);
				animatorStateTransition26934.canTransitionToSelf = true;
				animatorStateTransition26934.duration = 0.1f;
				animatorStateTransition26934.exitTime = 0.8f;
				animatorStateTransition26934.hasExitTime = false;
				animatorStateTransition26934.hasFixedDuration = true;
				animatorStateTransition26934.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26934.offset = 0f;
				animatorStateTransition26934.orderedInterruption = true;
				animatorStateTransition26934.isExit = false;
				animatorStateTransition26934.mute = false;
				animatorStateTransition26934.solo = false;
				animatorStateTransition26934.AddCondition(AnimatorConditionMode.Equals, 1f, "AbilityIntData");
				animatorStateTransition26934.AddCondition(AnimatorConditionMode.IfNot, 0f, "Moving");

				var animatorStateTransition26940 = underwaterSwimAnimatorState24448.AddTransition(surfaceSwimAnimatorState24454);
				animatorStateTransition26940.canTransitionToSelf = true;
				animatorStateTransition26940.duration = 0.1f;
				animatorStateTransition26940.exitTime = 0.8285714f;
				animatorStateTransition26940.hasExitTime = false;
				animatorStateTransition26940.hasFixedDuration = true;
				animatorStateTransition26940.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26940.offset = 0f;
				animatorStateTransition26940.orderedInterruption = true;
				animatorStateTransition26940.isExit = false;
				animatorStateTransition26940.mute = false;
				animatorStateTransition26940.solo = false;
				animatorStateTransition26940.AddCondition(AnimatorConditionMode.Equals, 1f, "AbilityIntData");
				animatorStateTransition26940.AddCondition(AnimatorConditionMode.If, 0f, "Moving");

				var animatorStateTransition26938 = underwaterSwimAnimatorState24448.AddTransition(exitWaterMovingAnimatorState26872);
				animatorStateTransition26938.canTransitionToSelf = true;
				animatorStateTransition26938.duration = 0.2259974f;
				animatorStateTransition26938.exitTime = 0.01825405f;
				animatorStateTransition26938.hasExitTime = false;
				animatorStateTransition26938.hasFixedDuration = true;
				animatorStateTransition26938.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26938.offset = 0f;
				animatorStateTransition26938.orderedInterruption = true;
				animatorStateTransition26938.isExit = false;
				animatorStateTransition26938.mute = false;
				animatorStateTransition26938.solo = false;
				animatorStateTransition26938.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

				var animatorStateTransition26944 = underwaterSwimAnimatorState24448.AddTransition(exitWaterIdleAnimatorState26874);
				animatorStateTransition26944.canTransitionToSelf = true;
				animatorStateTransition26944.duration = 0.15f;
				animatorStateTransition26944.exitTime = 0.8285714f;
				animatorStateTransition26944.hasExitTime = false;
				animatorStateTransition26944.hasFixedDuration = true;
				animatorStateTransition26944.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26944.offset = 0f;
				animatorStateTransition26944.orderedInterruption = true;
				animatorStateTransition26944.isExit = false;
				animatorStateTransition26944.mute = false;
				animatorStateTransition26944.solo = false;
				animatorStateTransition26944.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

				var animatorStateTransition26936 = underwaterSwimAnimatorState24448.AddExitTransition();
				animatorStateTransition26936.canTransitionToSelf = true;
				animatorStateTransition26936.duration = 0.25f;
				animatorStateTransition26936.exitTime = 0.8f;
				animatorStateTransition26936.hasExitTime = false;
				animatorStateTransition26936.hasFixedDuration = true;
				animatorStateTransition26936.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26936.offset = 0f;
				animatorStateTransition26936.orderedInterruption = true;
				animatorStateTransition26936.isExit = true;
				animatorStateTransition26936.mute = false;
				animatorStateTransition26936.solo = false;
				animatorStateTransition26936.AddCondition(AnimatorConditionMode.NotEqual, 301f, "AbilityIndex");

				var animatorStateTransition26980 = diveFromSurfaceAnimatorState26870.AddTransition(underwaterSwimAnimatorState24448);
				animatorStateTransition26980.canTransitionToSelf = true;
				animatorStateTransition26980.duration = 0.25f;
				animatorStateTransition26980.exitTime = 0.88f;
				animatorStateTransition26980.hasExitTime = true;
				animatorStateTransition26980.hasFixedDuration = true;
				animatorStateTransition26980.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26980.offset = 0f;
				animatorStateTransition26980.orderedInterruption = true;
				animatorStateTransition26980.isExit = false;
				animatorStateTransition26980.mute = false;
				animatorStateTransition26980.solo = false;

				var animatorStateTransition26986 = exitWaterMovingAnimatorState26872.AddExitTransition();
				animatorStateTransition26986.canTransitionToSelf = true;
				animatorStateTransition26986.duration = 0.15f;
				animatorStateTransition26986.exitTime = 0.85f;
				animatorStateTransition26986.hasExitTime = true;
				animatorStateTransition26986.hasFixedDuration = true;
				animatorStateTransition26986.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26986.offset = 0f;
				animatorStateTransition26986.orderedInterruption = true;
				animatorStateTransition26986.isExit = true;
				animatorStateTransition26986.mute = false;
				animatorStateTransition26986.solo = false;
				animatorStateTransition26986.AddCondition(AnimatorConditionMode.NotEqual, 301f, "AbilityIndex");

				var animatorStateTransition26990 = exitWaterIdleAnimatorState26874.AddExitTransition();
				animatorStateTransition26990.canTransitionToSelf = true;
				animatorStateTransition26990.duration = 0.1f;
				animatorStateTransition26990.exitTime = 0.65f;
				animatorStateTransition26990.hasExitTime = false;
				animatorStateTransition26990.hasFixedDuration = true;
				animatorStateTransition26990.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition26990.offset = 0f;
				animatorStateTransition26990.orderedInterruption = true;
				animatorStateTransition26990.isExit = true;
				animatorStateTransition26990.mute = false;
				animatorStateTransition26990.solo = false;
				animatorStateTransition26990.AddCondition(AnimatorConditionMode.NotEqual, 301f, "AbilityIndex");

				// State Machine Transitions.
				var animatorStateTransition24154 = baseStateMachine1414889828.AddAnyStateTransition(fallInWaterAnimatorState24444);
				animatorStateTransition24154.canTransitionToSelf = false;
				animatorStateTransition24154.duration = 0.25f;
				animatorStateTransition24154.exitTime = 0.75f;
				animatorStateTransition24154.hasExitTime = false;
				animatorStateTransition24154.hasFixedDuration = true;
				animatorStateTransition24154.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition24154.offset = 0f;
				animatorStateTransition24154.orderedInterruption = true;
				animatorStateTransition24154.isExit = false;
				animatorStateTransition24154.mute = false;
				animatorStateTransition24154.solo = false;
				animatorStateTransition24154.AddCondition(AnimatorConditionMode.If, 0f, "AbilityChange");
				animatorStateTransition24154.AddCondition(AnimatorConditionMode.Equals, 301f, "AbilityIndex");
				animatorStateTransition24154.AddCondition(AnimatorConditionMode.Equals, 0f, "AbilityIntData");

				var animatorStateTransition24156 = baseStateMachine1414889828.AddAnyStateTransition(surfaceIdleAnimatorState24446);
				animatorStateTransition24156.canTransitionToSelf = false;
				animatorStateTransition24156.duration = 0.25f;
				animatorStateTransition24156.exitTime = 0.75f;
				animatorStateTransition24156.hasExitTime = false;
				animatorStateTransition24156.hasFixedDuration = true;
				animatorStateTransition24156.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition24156.offset = 0f;
				animatorStateTransition24156.orderedInterruption = true;
				animatorStateTransition24156.isExit = false;
				animatorStateTransition24156.mute = false;
				animatorStateTransition24156.solo = false;
				animatorStateTransition24156.AddCondition(AnimatorConditionMode.If, 0f, "AbilityChange");
				animatorStateTransition24156.AddCondition(AnimatorConditionMode.Equals, 301f, "AbilityIndex");
				animatorStateTransition24156.AddCondition(AnimatorConditionMode.Equals, 1f, "AbilityIntData");
				animatorStateTransition24156.AddCondition(AnimatorConditionMode.IfNot, 0f, "Moving");

				var animatorStateTransition24158 = baseStateMachine1414889828.AddAnyStateTransition(underwaterSwimAnimatorState24448);
				animatorStateTransition24158.canTransitionToSelf = false;
				animatorStateTransition24158.duration = 0.25f;
				animatorStateTransition24158.exitTime = 0.75f;
				animatorStateTransition24158.hasExitTime = false;
				animatorStateTransition24158.hasFixedDuration = true;
				animatorStateTransition24158.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition24158.offset = 0f;
				animatorStateTransition24158.orderedInterruption = true;
				animatorStateTransition24158.isExit = false;
				animatorStateTransition24158.mute = false;
				animatorStateTransition24158.solo = false;
				animatorStateTransition24158.AddCondition(AnimatorConditionMode.If, 0f, "AbilityChange");
				animatorStateTransition24158.AddCondition(AnimatorConditionMode.Equals, 301f, "AbilityIndex");
				animatorStateTransition24158.AddCondition(AnimatorConditionMode.Equals, 2f, "AbilityIntData");

				var animatorStateTransition24164 = baseStateMachine1414889828.AddAnyStateTransition(surfaceSwimAnimatorState24454);
				animatorStateTransition24164.canTransitionToSelf = false;
				animatorStateTransition24164.duration = 0.25f;
				animatorStateTransition24164.exitTime = 0.75f;
				animatorStateTransition24164.hasExitTime = false;
				animatorStateTransition24164.hasFixedDuration = true;
				animatorStateTransition24164.interruptionSource = TransitionInterruptionSource.None;
				animatorStateTransition24164.offset = 0f;
				animatorStateTransition24164.orderedInterruption = true;
				animatorStateTransition24164.isExit = false;
				animatorStateTransition24164.mute = false;
				animatorStateTransition24164.solo = false;
				animatorStateTransition24164.AddCondition(AnimatorConditionMode.If, 0f, "AbilityChange");
				animatorStateTransition24164.AddCondition(AnimatorConditionMode.Equals, 301f, "AbilityIndex");
				animatorStateTransition24164.AddCondition(AnimatorConditionMode.Equals, 1f, "AbilityIntData");
				animatorStateTransition24164.AddCondition(AnimatorConditionMode.If, 0f, "Moving");
			}
		}
	}
}
