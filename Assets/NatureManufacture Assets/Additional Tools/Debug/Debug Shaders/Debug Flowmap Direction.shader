Shader "NatureManufacture Shaders/Debug/Flowmap Direction"
{
    Properties
    {
        [NoScaleOffset]_Direction("Direction", 2D) = "white" {}
        [NoScaleOffset]_Direction_Stop("Direction_Stop", 2D) = "white" {}
        _test("test", Float) = 0
        [NonModifiableTextureData][NoScaleOffset]_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D("Texture2D", 2D) = "white" {}
        [NonModifiableTextureData][NoScaleOffset]_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D("Texture2D", 2D) = "white" {}
        [HideInInspector]_EmissionColor("Color", Color) = (1, 1, 1, 1)
        [HideInInspector]_RenderQueueType("Float", Float) = 1
        [HideInInspector][ToggleUI]_AddPrecomputedVelocity("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_DepthOffsetEnable("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_ConservativeDepthOffsetEnable("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_TransparentWritingMotionVec("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_AlphaCutoffEnable("Boolean", Float) = 1
        [HideInInspector]_TransparentSortPriority("_TransparentSortPriority", Float) = 0
        [HideInInspector][ToggleUI]_UseShadowThreshold("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_DoubleSidedEnable("Boolean", Float) = 0
        [HideInInspector][Enum(Flip, 0, Mirror, 1, None, 2)]_DoubleSidedNormalMode("Float", Float) = 2
        [HideInInspector]_DoubleSidedConstants("Vector4", Vector) = (1, 1, -1, 0)
        [HideInInspector][Enum(Auto, 0, On, 1, Off, 2)]_DoubleSidedGIMode("Float", Float) = 0
        [HideInInspector][ToggleUI]_TransparentDepthPrepassEnable("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_TransparentDepthPostpassEnable("Boolean", Float) = 0
        [HideInInspector]_SurfaceType("Float", Float) = 0
        [HideInInspector]_BlendMode("Float", Float) = 0
        [HideInInspector]_SrcBlend("Float", Float) = 1
        [HideInInspector]_DstBlend("Float", Float) = 0
        [HideInInspector]_AlphaSrcBlend("Float", Float) = 1
        [HideInInspector]_AlphaDstBlend("Float", Float) = 0
        [HideInInspector][ToggleUI]_ZWrite("Boolean", Float) = 1
        [HideInInspector][ToggleUI]_TransparentZWrite("Boolean", Float) = 0
        [HideInInspector]_CullMode("Float", Float) = 2
        [HideInInspector][ToggleUI]_EnableFogOnTransparent("Boolean", Float) = 1
        [HideInInspector]_CullModeForward("Float", Float) = 2
        [HideInInspector][Enum(Front, 1, Back, 2)]_TransparentCullMode("Float", Float) = 2
        [HideInInspector][Enum(UnityEditor.Rendering.HighDefinition.OpaqueCullMode)]_OpaqueCullMode("Float", Float) = 2
        [HideInInspector]_ZTestDepthEqualForOpaque("Float", Int) = 3
        [HideInInspector][Enum(UnityEngine.Rendering.CompareFunction)]_ZTestTransparent("Float", Float) = 4
        [HideInInspector][ToggleUI]_TransparentBackfaceEnable("Boolean", Float) = 0
        [HideInInspector][ToggleUI]_EnableBlendModePreserveSpecularLighting("Boolean", Float) = 0
        [HideInInspector]_StencilRef("Float", Int) = 0
        [HideInInspector]_StencilWriteMask("Float", Int) = 6
        [HideInInspector]_StencilRefDepth("Float", Int) = 1
        [HideInInspector]_StencilWriteMaskDepth("Float", Int) = 9
        [HideInInspector]_StencilRefMV("Float", Int) = 33
        [HideInInspector]_StencilWriteMaskMV("Float", Int) = 43
        [HideInInspector]_StencilRefDistortionVec("Float", Int) = 4
        [HideInInspector]_StencilWriteMaskDistortionVec("Float", Int) = 4
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="HDRenderPipeline"
            "RenderType"="HDUnlitShader"
            "Queue"="AlphaTest+0"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="HDUnlitSubTarget"
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
            // Render State
            Cull [_CullMode]
        ZWrite On
        ColorMask 0
        ZClip [_ZClip]
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma instancing_options renderinglayer
        #pragma target 4.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT
        
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_SHADOWS
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            // GraphFunctions: <None>
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            
        VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        
        VertexDescription GetVertexDescription(AttributesMesh input, float3 timeParameters
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
        #ifdef HAVE_VFX_MODIFICATION
            GraphProperties properties;
            ZERO_INITIALIZE(GraphProperties, properties);
        
            // Fetch the vertex graph properties for the particle instance.
            GetElementVertexProperties(element, properties);
        
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
        #else
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        #endif
            return vertexDescription;
        
        }
        
        AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
        #ifdef USE_CUSTOMINTERP_SUBSTRUCT
            #ifdef TESSELLATION_ON
            , inout VaryingsMeshToDS varyings
            #else
            , inout VaryingsMeshToPS varyings
            #endif
        #endif
        #ifdef HAVE_VFX_MODIFICATION
                , AttributesElement element
        #endif
            )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, timeParameters
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
        
            // copy graph output to the results
            input.positionOS = vertexDescription.Position;
            input.normalOS = vertexDescription.Normal;
            input.tangentOS.xyz = vertexDescription.Tangent;
        
            
        
            return input;
        }
        
        #if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
        // Return precomputed Velocity in object space
        float3 GetCustomVelocity(AttributesMesh input
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, _TimeParameters.xyz
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
            return vertexDescription.CustomVelocity;
        }
        #endif
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
        
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
        #if defined(HAVE_VFX_MODIFICATION) && defined(UNITY_INSTANCING_ENABLED)
            unity_InstanceID = input.instanceID;
        #endif
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "META"
            Tags
            {
                "LightMode" = "META"
            }
        
            // Render State
            Cull Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma instancing_options renderinglayer
        #pragma target 4.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature _ EDITOR_VISUALIZATION
        #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_POSITIONPREDISPLACEMENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
            #define VARYINGS_NEED_TEXCOORD2
            #define VARYINGS_NEED_TEXCOORD3
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD1
            #define FRAG_INPUTS_USE_TEXCOORD2
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_LIGHT_TRANSPORT
        #define RAYTRACING_SHADER_GRAPH_DEFAULT
        #define SCENEPICKINGPASS 1
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
             float4 uv3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float3 positionRWS;
             float3 positionPredisplacementRWS;
             float4 texCoord0;
             float4 texCoord1;
             float4 texCoord2;
             float4 texCoord3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord1 : INTERP1;
             float4 texCoord2 : INTERP2;
             float4 texCoord3 : INTERP3;
             float3 positionRWS : INTERP4;
             float3 positionPredisplacementRWS : INTERP5;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord1.xyzw = input.texCoord1;
            output.texCoord2.xyzw = input.texCoord2;
            output.texCoord3.xyzw = input.texCoord3;
            output.positionRWS.xyz = input.positionRWS;
            output.positionPredisplacementRWS.xyz = input.positionPredisplacementRWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord1 = input.texCoord1.xyzw;
            output.texCoord2 = input.texCoord2.xyzw;
            output.texCoord3 = input.texCoord3.xyzw;
            output.positionRWS = input.positionRWS.xyz;
            output.positionPredisplacementRWS = input.positionPredisplacementRWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            
        VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
        
            return output;
        }
        
        VertexDescription GetVertexDescription(AttributesMesh input, float3 timeParameters
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
        #ifdef HAVE_VFX_MODIFICATION
            GraphProperties properties;
            ZERO_INITIALIZE(GraphProperties, properties);
        
            // Fetch the vertex graph properties for the particle instance.
            GetElementVertexProperties(element, properties);
        
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
        #else
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        #endif
            return vertexDescription;
        
        }
        
        AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
        #ifdef USE_CUSTOMINTERP_SUBSTRUCT
            #ifdef TESSELLATION_ON
            , inout VaryingsMeshToDS varyings
            #else
            , inout VaryingsMeshToPS varyings
            #endif
        #endif
        #ifdef HAVE_VFX_MODIFICATION
                , AttributesElement element
        #endif
            )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, timeParameters
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
        
            // copy graph output to the results
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorVertMeshCustomInterpolation' */
        
            return input;
        }
        
        #if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
        // Return precomputed Velocity in object space
        float3 GetCustomVelocity(AttributesMesh input
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, _TimeParameters.xyz
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
            return vertexDescription.CustomVelocity;
        }
        #endif
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.positionRWS =                input.positionRWS;
            output.positionPredisplacementRWS = input.positionPredisplacementRWS;
            output.texCoord0 =                  input.texCoord0;
            output.texCoord1 =                  input.texCoord1;
            output.texCoord2 =                  input.texCoord2;
            output.texCoord3 =                  input.texCoord3;
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
        
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorVaryingsToFragInputs' */
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
        #if defined(HAVE_VFX_MODIFICATION) && defined(UNITY_INSTANCING_ENABLED)
            unity_InstanceID = input.instanceID;
        #endif
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassLightTransport.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
            // Render State
            Cull [_CullMode]
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma editor_sync_compilation
        #pragma instancing_options renderinglayer
        #pragma target 4.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT
        
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD3
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_DEPTH_ONLY
        #define SCENEPICKINGPASS 1
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float4 texCoord3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 tangentWS : INTERP0;
             float4 texCoord0 : INTERP1;
             float4 texCoord3 : INTERP2;
             float3 normalWS : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.tangentWS.xyzw = input.tangentWS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord3.xyzw = input.texCoord3;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.tangentWS = input.tangentWS.xyzw;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord3 = input.texCoord3.xyzw;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            
        VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        
        VertexDescription GetVertexDescription(AttributesMesh input, float3 timeParameters
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
        #ifdef HAVE_VFX_MODIFICATION
            GraphProperties properties;
            ZERO_INITIALIZE(GraphProperties, properties);
        
            // Fetch the vertex graph properties for the particle instance.
            GetElementVertexProperties(element, properties);
        
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
        #else
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        #endif
            return vertexDescription;
        
        }
        
        AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
        #ifdef USE_CUSTOMINTERP_SUBSTRUCT
            #ifdef TESSELLATION_ON
            , inout VaryingsMeshToDS varyings
            #else
            , inout VaryingsMeshToPS varyings
            #endif
        #endif
        #ifdef HAVE_VFX_MODIFICATION
                , AttributesElement element
        #endif
            )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, timeParameters
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
        
            // copy graph output to the results
            input.positionOS = vertexDescription.Position;
            input.normalOS = vertexDescription.Normal;
            input.tangentOS.xyz = vertexDescription.Tangent;
        
            
        
            return input;
        }
        
        #if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
        // Return precomputed Velocity in object space
        float3 GetCustomVelocity(AttributesMesh input
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, _TimeParameters.xyz
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
            return vertexDescription.CustomVelocity;
        }
        #endif
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.tangentToWorld =             BuildTangentToWorld(input.tangentWS, input.normalWS);
            output.texCoord0 =                  input.texCoord0;
            output.texCoord3 =                  input.texCoord3;
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
        
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
        #if defined(HAVE_VFX_MODIFICATION) && defined(UNITY_INSTANCING_ENABLED)
            unity_InstanceID = input.instanceID;
        #endif
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
            // Render State
            Cull Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma editor_sync_compilation
        #pragma instancing_options renderinglayer
        #pragma target 4.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT
        
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD3
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_DEPTH_ONLY
        #define RAYTRACING_SHADER_GRAPH_DEFAULT
        #define SCENESELECTIONPASS 1
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/PickingSpaceTransforms.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 texCoord3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord3 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord3.xyzw = input.texCoord3;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord3 = input.texCoord3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            
        VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        
        VertexDescription GetVertexDescription(AttributesMesh input, float3 timeParameters
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
        #ifdef HAVE_VFX_MODIFICATION
            GraphProperties properties;
            ZERO_INITIALIZE(GraphProperties, properties);
        
            // Fetch the vertex graph properties for the particle instance.
            GetElementVertexProperties(element, properties);
        
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
        #else
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        #endif
            return vertexDescription;
        
        }
        
        AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
        #ifdef USE_CUSTOMINTERP_SUBSTRUCT
            #ifdef TESSELLATION_ON
            , inout VaryingsMeshToDS varyings
            #else
            , inout VaryingsMeshToPS varyings
            #endif
        #endif
        #ifdef HAVE_VFX_MODIFICATION
                , AttributesElement element
        #endif
            )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, timeParameters
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
        
            // copy graph output to the results
            input.positionOS = vertexDescription.Position;
            input.normalOS = vertexDescription.Normal;
            input.tangentOS.xyz = vertexDescription.Tangent;
        
            
        
            return input;
        }
        
        #if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
        // Return precomputed Velocity in object space
        float3 GetCustomVelocity(AttributesMesh input
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, _TimeParameters.xyz
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
            return vertexDescription.CustomVelocity;
        }
        #endif
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.texCoord0 =                  input.texCoord0;
            output.texCoord3 =                  input.texCoord3;
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
        
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
        #if defined(HAVE_VFX_MODIFICATION) && defined(UNITY_INSTANCING_ENABLED)
            unity_InstanceID = input.instanceID;
        #endif
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "MotionVectors"
            Tags
            {
                "LightMode" = "MotionVectors"
            }
        
            // Render State
            Cull [_CullMode]
        ZWrite On
        Stencil
        {
        WriteMask [_StencilWriteMaskMV]
        Ref [_StencilRefMV]
        CompFront Always
        PassFront Replace
        CompBack Always
        PassBack Replace
        }
        AlphaToMask [_AlphaCutoffEnable]
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma instancing_options renderinglayer
        #pragma target 4.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma multi_compile_fragment _ WRITE_MSAA_DEPTH
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT
        
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD3
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_MOTION_VECTORS
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float3 positionRWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float4 texCoord3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 tangentWS : INTERP0;
             float4 texCoord0 : INTERP1;
             float4 texCoord3 : INTERP2;
             float3 positionRWS : INTERP3;
             float3 normalWS : INTERP4;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.tangentWS.xyzw = input.tangentWS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord3.xyzw = input.texCoord3;
            output.positionRWS.xyz = input.positionRWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.tangentWS = input.tangentWS.xyzw;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord3 = input.texCoord3.xyzw;
            output.positionRWS = input.positionRWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            
        VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        
        VertexDescription GetVertexDescription(AttributesMesh input, float3 timeParameters
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
        #ifdef HAVE_VFX_MODIFICATION
            GraphProperties properties;
            ZERO_INITIALIZE(GraphProperties, properties);
        
            // Fetch the vertex graph properties for the particle instance.
            GetElementVertexProperties(element, properties);
        
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
        #else
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        #endif
            return vertexDescription;
        
        }
        
        AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
        #ifdef USE_CUSTOMINTERP_SUBSTRUCT
            #ifdef TESSELLATION_ON
            , inout VaryingsMeshToDS varyings
            #else
            , inout VaryingsMeshToPS varyings
            #endif
        #endif
        #ifdef HAVE_VFX_MODIFICATION
                , AttributesElement element
        #endif
            )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, timeParameters
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
        
            // copy graph output to the results
            input.positionOS = vertexDescription.Position;
            input.normalOS = vertexDescription.Normal;
            input.tangentOS.xyz = vertexDescription.Tangent;
        
            
        
            return input;
        }
        
        #if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
        // Return precomputed Velocity in object space
        float3 GetCustomVelocity(AttributesMesh input
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, _TimeParameters.xyz
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
            return vertexDescription.CustomVelocity;
        }
        #endif
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.positionRWS =                input.positionRWS;
            output.tangentToWorld =             BuildTangentToWorld(input.tangentWS, input.normalWS);
            output.texCoord0 =                  input.texCoord0;
            output.texCoord3 =                  input.texCoord3;
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
        
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
        #if defined(HAVE_VFX_MODIFICATION) && defined(UNITY_INSTANCING_ENABLED)
            unity_InstanceID = input.instanceID;
        #endif
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassMotionVectors.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "DepthForwardOnly"
            Tags
            {
                "LightMode" = "DepthForwardOnly"
            }
        
            // Render State
            Cull [_CullMode]
        ZWrite On
        Stencil
        {
        WriteMask [_StencilWriteMaskDepth]
        Ref [_StencilRefDepth]
        CompFront Always
        PassFront Replace
        CompBack Always
        PassBack Replace
        }
        AlphaToMask [_AlphaCutoffEnable]
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma instancing_options renderinglayer
        #pragma target 4.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma multi_compile_fragment _ WRITE_MSAA_DEPTH
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT
        
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD3
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_DEPTH_ONLY
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float4 texCoord3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 tangentWS : INTERP0;
             float4 texCoord0 : INTERP1;
             float4 texCoord3 : INTERP2;
             float3 normalWS : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.tangentWS.xyzw = input.tangentWS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord3.xyzw = input.texCoord3;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.tangentWS = input.tangentWS.xyzw;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord3 = input.texCoord3.xyzw;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            
        VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        
        VertexDescription GetVertexDescription(AttributesMesh input, float3 timeParameters
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
        #ifdef HAVE_VFX_MODIFICATION
            GraphProperties properties;
            ZERO_INITIALIZE(GraphProperties, properties);
        
            // Fetch the vertex graph properties for the particle instance.
            GetElementVertexProperties(element, properties);
        
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
        #else
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        #endif
            return vertexDescription;
        
        }
        
        AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
        #ifdef USE_CUSTOMINTERP_SUBSTRUCT
            #ifdef TESSELLATION_ON
            , inout VaryingsMeshToDS varyings
            #else
            , inout VaryingsMeshToPS varyings
            #endif
        #endif
        #ifdef HAVE_VFX_MODIFICATION
                , AttributesElement element
        #endif
            )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, timeParameters
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
        
            // copy graph output to the results
            input.positionOS = vertexDescription.Position;
            input.normalOS = vertexDescription.Normal;
            input.tangentOS.xyz = vertexDescription.Tangent;
        
            
        
            return input;
        }
        
        #if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
        // Return precomputed Velocity in object space
        float3 GetCustomVelocity(AttributesMesh input
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, _TimeParameters.xyz
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
            return vertexDescription.CustomVelocity;
        }
        #endif
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.tangentToWorld =             BuildTangentToWorld(input.tangentWS, input.normalWS);
            output.texCoord0 =                  input.texCoord0;
            output.texCoord3 =                  input.texCoord3;
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
        
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
        #if defined(HAVE_VFX_MODIFICATION) && defined(UNITY_INSTANCING_ENABLED)
            unity_InstanceID = input.instanceID;
        #endif
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassDepthOnly.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "ForwardOnly"
            Tags
            {
                "LightMode" = "ForwardOnly"
            }
        
            // Render State
            Cull [_CullModeForward]
        Blend [_SrcBlend] [_DstBlend], [_AlphaSrcBlend] [_AlphaDstBlend]
        Blend 1 SrcAlpha OneMinusSrcAlpha
        ZTest [_ZTestDepthEqualForOpaque]
        ZWrite [_ZWrite]
        ColorMask [_ColorMaskTransparentVelOne] 1
        ColorMask [_ColorMaskTransparentVelTwo] 2
        Stencil
        {
        WriteMask [_StencilWriteMask]
        Ref [_StencilRef]
        CompFront Always
        PassFront Replace
        CompBack Always
        PassBack Replace
        }
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma instancing_options renderinglayer
        #pragma target 4.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
        #pragma multi_compile _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT
        
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD3
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_FORWARD_UNLIT
        #define RAYTRACING_SHADER_GRAPH_DEFAULT
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float3 positionRWS;
             float4 texCoord0;
             float4 texCoord3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord3 : INTERP1;
             float3 positionRWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord3.xyzw = input.texCoord3;
            output.positionRWS.xyz = input.positionRWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord3 = input.texCoord3.xyzw;
            output.positionRWS = input.positionRWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
            float4 VTPackedFeedback;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            {
                surface.VTPackedFeedback = float4(1.0f,1.0f,1.0f,1.0f);
            }
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            
        VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        
        VertexDescription GetVertexDescription(AttributesMesh input, float3 timeParameters
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
        #ifdef HAVE_VFX_MODIFICATION
            GraphProperties properties;
            ZERO_INITIALIZE(GraphProperties, properties);
        
            // Fetch the vertex graph properties for the particle instance.
            GetElementVertexProperties(element, properties);
        
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
        #else
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        #endif
            return vertexDescription;
        
        }
        
        AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
        #ifdef USE_CUSTOMINTERP_SUBSTRUCT
            #ifdef TESSELLATION_ON
            , inout VaryingsMeshToDS varyings
            #else
            , inout VaryingsMeshToPS varyings
            #endif
        #endif
        #ifdef HAVE_VFX_MODIFICATION
                , AttributesElement element
        #endif
            )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, timeParameters
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
        
            // copy graph output to the results
            input.positionOS = vertexDescription.Position;
            input.normalOS = vertexDescription.Normal;
            input.tangentOS.xyz = vertexDescription.Tangent;
        
            
        
            return input;
        }
        
        #if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
        // Return precomputed Velocity in object space
        float3 GetCustomVelocity(AttributesMesh input
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, _TimeParameters.xyz
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
            return vertexDescription.CustomVelocity;
        }
        #endif
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.positionRWS =                input.positionRWS;
            output.texCoord0 =                  input.texCoord0;
            output.texCoord3 =                  input.texCoord3;
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
        
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
        #if defined(HAVE_VFX_MODIFICATION) && defined(UNITY_INSTANCING_ENABLED)
            unity_InstanceID = input.instanceID;
        #endif
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                builtinData.vtPackedFeedback = surfaceDescription.VTPackedFeedback;
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassForwardUnlit.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "FullScreenDebug"
            Tags
            {
                "LightMode" = "FullScreenDebug"
            }
        
            // Render State
            Cull [_CullMode]
        ZTest LEqual
        ZWrite Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma instancing_options renderinglayer
        #pragma target 4.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch
        #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            struct CustomInterpolators
        {
        };
        #define USE_CUSTOMINTERP_SUBSTRUCT
        
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD3
        
            #define HAVE_MESH_MODIFICATION
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_FULL_SCREEN_DEBUG
        #define RAYTRACING_SHADER_GRAPH_DEFAULT
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Debug/DebugDisplay.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct AttributesMesh
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv3 : TEXCOORD3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct VaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 texCoord3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        struct PackedVaryingsMeshToPS
        {
            SV_POSITION_QUALIFIERS float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 texCoord3 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
        };
        
            PackedVaryingsMeshToPS PackVaryingsMeshToPS (VaryingsMeshToPS input)
        {
            PackedVaryingsMeshToPS output;
            ZERO_INITIALIZE(PackedVaryingsMeshToPS, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.texCoord3.xyzw = input.texCoord3;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        VaryingsMeshToPS UnpackVaryingsMeshToPS (PackedVaryingsMeshToPS input)
        {
            VaryingsMeshToPS output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.texCoord3 = input.texCoord3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            
        VertexDescriptionInputs AttributesMeshToVertexDescriptionInputs(AttributesMesh input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        
        VertexDescription GetVertexDescription(AttributesMesh input, float3 timeParameters
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            // build graph inputs
            VertexDescriptionInputs vertexDescriptionInputs = AttributesMeshToVertexDescriptionInputs(input);
            // Override time parameters with used one (This is required to correctly handle motion vectors for vertex animation based on time)
        
            // evaluate vertex graph
        #ifdef HAVE_VFX_MODIFICATION
            GraphProperties properties;
            ZERO_INITIALIZE(GraphProperties, properties);
        
            // Fetch the vertex graph properties for the particle instance.
            GetElementVertexProperties(element, properties);
        
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs, properties);
        #else
            VertexDescription vertexDescription = VertexDescriptionFunction(vertexDescriptionInputs);
        #endif
            return vertexDescription;
        
        }
        
        AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters
        #ifdef USE_CUSTOMINTERP_SUBSTRUCT
            #ifdef TESSELLATION_ON
            , inout VaryingsMeshToDS varyings
            #else
            , inout VaryingsMeshToPS varyings
            #endif
        #endif
        #ifdef HAVE_VFX_MODIFICATION
                , AttributesElement element
        #endif
            )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, timeParameters
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
        
            // copy graph output to the results
            input.positionOS = vertexDescription.Position;
            input.normalOS = vertexDescription.Normal;
            input.tangentOS.xyz = vertexDescription.Tangent;
        
            
        
            return input;
        }
        
        #if defined(_ADD_CUSTOM_VELOCITY) // For shader graph custom velocity
        // Return precomputed Velocity in object space
        float3 GetCustomVelocity(AttributesMesh input
        #ifdef HAVE_VFX_MODIFICATION
            , AttributesElement element
        #endif
        )
        {
            VertexDescription vertexDescription = GetVertexDescription(input, _TimeParameters.xyz
        #ifdef HAVE_VFX_MODIFICATION
                , element
        #endif
            );
            return vertexDescription.CustomVelocity;
        }
        #endif
        
        FragInputs BuildFragInputs(VaryingsMeshToPS input)
        {
            FragInputs output;
            ZERO_INITIALIZE(FragInputs, output);
        
            // Init to some default value to make the computer quiet (else it output 'divide by zero' warning even if value is not used).
            // TODO: this is a really poor workaround, but the variable is used in a bunch of places
            // to compute normals which are then passed on elsewhere to compute other values...
            output.tangentToWorld = k_identity3x3;
            output.positionSS = input.positionCS;       // input.positionCS is SV_Position
        
            output.texCoord0 =                  input.texCoord0;
            output.texCoord3 =                  input.texCoord3;
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
        
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            // splice point to copy custom interpolator fields from varyings to frag inputs
            
        
            return output;
        }
        
        // existing HDRP code uses the combined function to go directly from packed to frag inputs
        FragInputs UnpackVaryingsMeshToFragInputs(PackedVaryingsMeshToPS input)
        {
            UNITY_SETUP_INSTANCE_ID(input);
        #if defined(HAVE_VFX_MODIFICATION) && defined(UNITY_INSTANCING_ENABLED)
            unity_InstanceID = input.instanceID;
        #endif
            VaryingsMeshToPS unpacked = UnpackVaryingsMeshToPS(input);
            return BuildFragInputs(unpacked);
        }
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassFullScreenDebug.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="HDRenderPipeline"
            "RenderType"="HDUnlitShader"
            "Queue"="AlphaTest+0"
            "DisableBatching"="False"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="HDUnlitSubTarget"
        }
        Pass
        {
            Name "IndirectDXR"
            Tags
            {
                "LightMode" = "IndirectDXR"
            }
        
            // Render State
            // RenderState: <None>
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 5.0
        #pragma raytracing surface_shader
        #pragma only_renderers d3d11 xboxseries ps5
        
            // Keywords
            #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
        #pragma multi_compile _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD3
        
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_RAYTRACING_INDIRECT
        #define SHADOW_LOW
        #define RAYTRACING_SHADER_GRAPH_RAYTRACED
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/UnlitRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        
            //Interpolator Packs: <None>
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            // GraphVertex: <None>
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassRaytracingIndirect.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "VisibilityDXR"
            Tags
            {
                "LightMode" = "VisibilityDXR"
            }
        
            // Render State
            // RenderState: <None>
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 5.0
        #pragma raytracing surface_shader
        #pragma only_renderers d3d11 xboxseries ps5
        
            // Keywords
            #pragma multi_compile _ TRANSPARENT_COLOR_SHADOW
        #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD3
        
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_RAYTRACING_VISIBILITY
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/UnlitRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        
            //Interpolator Packs: <None>
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            // GraphVertex: <None>
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassRaytracingVisibility.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "ForwardDXR"
            Tags
            {
                "LightMode" = "ForwardDXR"
            }
        
            // Render State
            // RenderState: <None>
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 5.0
        #pragma raytracing surface_shader
        #pragma only_renderers d3d11 xboxseries ps5
        
            // Keywords
            #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
        #pragma multi_compile _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD3
        
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_RAYTRACING_FORWARD
        #define SHADOW_LOW
        #define RAYTRACING_SHADER_GRAPH_RAYTRACED
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/UnlitRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        
            //Interpolator Packs: <None>
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            // GraphVertex: <None>
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassRaytracingForward.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "GBufferDXR"
            Tags
            {
                "LightMode" = "GBufferDXR"
            }
        
            // Render State
            // RenderState: <None>
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 5.0
        #pragma raytracing surface_shader
        #pragma only_renderers d3d11 xboxseries ps5
        
            // Keywords
            #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
        #pragma multi_compile _ DEBUG_DISPLAY
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD3
        
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_RAYTRACING_GBUFFER
        #define SHADOW_LOW
        #define RAYTRACING_SHADER_GRAPH_RAYTRACED
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/Deferred/RaytracingIntersectonGBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/NormalBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/StandardLit/StandardLit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/UnlitRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        
            //Interpolator Packs: <None>
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            // GraphVertex: <None>
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassRaytracingGBuffer.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
        Pass
        {
            Name "DebugDXR"
            Tags
            {
                "LightMode" = "DebugDXR"
            }
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 5.0
        #pragma raytracing surface_shader
        #pragma only_renderers d3d11 xboxseries ps5
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
        	// GraphIncludes: <None>
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassRayTracingDebug.hlsl"
        
            ENDHLSL
        }
        Pass
        {
            Name "PathTracingDXR"
            Tags
            {
                "LightMode" = "PathTracingDXR"
            }
        
            // Render State
            // RenderState: <None>
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 5.0
        #pragma raytracing surface_shader
        #pragma only_renderers d3d11 xboxseries ps5
        
            // Keywords
            #pragma shader_feature_local _ _ALPHATEST_ON
        #pragma shader_feature _ _SURFACE_TYPE_TRANSPARENT
        #pragma shader_feature_local _ _ADD_PRECOMPUTED_VELOCITY
        #pragma shader_feature_local _ _TRANSPARENT_WRITES_MOTION_VEC
        #pragma shader_feature_local_fragment _ _ENABLE_FOG_ON_TRANSPARENT
            // GraphKeywords: <None>
        
            // For custom interpolators to inject a substruct definition before FragInputs definition,
            // allowing for FragInputs to capture CI's intended for ShaderGraph's SDI.
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */
        
        
            // TODO: Merge FragInputsVFX substruct with CustomInterpolators.
        	#ifdef HAVE_VFX_MODIFICATION
        	struct FragInputsVFX
            {
                /* WARNING: $splice Could not find named fragment 'FragInputsVFX' */
            };
            #endif
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GeometricTools.hlsl" // Required by Tessellation.hlsl
        	#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Tessellation.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl" // Required to be include before we include properties as it define DECLARE_STACK_CB
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphHeader.hlsl" // Need to be here for Gradient struct definition
        
            // --------------------------------------------------
            // Defines
        
            // Attribute
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD3
        
        
            //Strip down the FragInputs.hlsl (on graphics), so we can only optimize the interpolators we use.
            //if by accident something requests contents of FragInputs.hlsl, it will be caught as a compiler error
            //Frag inputs stripping is only enabled when FRAG_INPUTS_ENABLE_STRIPPING is set
            #if !defined(SHADER_STAGE_RAY_TRACING) && SHADERPASS != SHADERPASS_RAYTRACING_GBUFFER && SHADERPASS != SHADERPASS_FULL_SCREEN_DEBUG
            #define FRAG_INPUTS_ENABLE_STRIPPING
            #endif
            #define FRAG_INPUTS_USE_TEXCOORD0
            #define FRAG_INPUTS_USE_TEXCOORD3
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/FragInputs.hlsl"
        
        
        
            #define SHADERPASS SHADERPASS_PATH_TRACING
        
        
            // Following two define are a workaround introduce in 10.1.x for RaytracingQualityNode
            // The ShaderGraph don't support correctly migration of this node as it serialize all the node data
            // in the json file making it impossible to uprgrade. Until we get a fix, we do a workaround here
            // to still allow us to rename the field and keyword of this node without breaking existing code.
            #ifdef RAYTRACING_SHADER_GRAPH_DEFAULT
            #define RAYTRACING_SHADER_GRAPH_HIGH
            #endif
        
            #ifdef RAYTRACING_SHADER_GRAPH_RAYTRACED
            #define RAYTRACING_SHADER_GRAPH_LOW
            #endif
            // end
        
            #ifndef SHADER_UNLIT
            // We need isFrontFace when using double sided - it is not required for unlit as in case of unlit double sided only drive the cullmode
            // VARYINGS_NEED_CULLFACE can be define by VaryingsMeshToPS.FaceSign input if a IsFrontFace Node is included in the shader graph.
            #if defined(_DOUBLESIDED_ON) && !defined(VARYINGS_NEED_CULLFACE)
                #define VARYINGS_NEED_CULLFACE
            #endif
            #endif
        
            // Specific Material Define
        // Setup a define to say we are an unlit shader
        #define SHADER_UNLIT
        
        // Following Macro are only used by Unlit material
        #if defined(_ENABLE_SHADOW_MATTE)
            #if SHADERPASS == SHADERPASS_FORWARD_UNLIT
                #pragma multi_compile_fragment USE_FPTL_LIGHTLIST USE_CLUSTERED_LIGHTLIST
            #elif SHADERPASS == SHADERPASS_PATH_TRACING
                #define LIGHTLOOP_DISABLE_TILE_AND_CLUSTER
            #endif
        
        // We don't want to have the lightloop defined for the ray tracing passes, but we do for the rasterisation and path tracing shader passes.
        #if !defined(SHADER_STAGE_RAY_TRACING) || SHADERPASS == SHADERPASS_PATH_TRACING
            #define HAS_LIGHTLOOP
        #endif
        #endif
            // Caution: we can use the define SHADER_UNLIT onlit after the above Material include as it is the Unlit template who define it
        
            // To handle SSR on transparent correctly with a possibility to enable/disable it per framesettings
            // we should have a code like this:
            // if !defined(_DISABLE_SSR_TRANSPARENT)
            // pragma multi_compile _ WRITE_NORMAL_BUFFER
            // endif
            // i.e we enable the multicompile only if we can receive SSR or not, and then C# code drive
            // it based on if SSR transparent in frame settings and not (and stripper can strip it).
            // this is currently not possible with our current preprocessor as _DISABLE_SSR_TRANSPARENT is a keyword not a define
            // so instead we used this and chose to pay the extra cost of normal write even if SSR transaprent is disabled.
            // Ideally the shader graph generator should handle it but condition below can't be handle correctly for now.
            #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
            #if !defined(_DISABLE_SSR_TRANSPARENT) && !defined(SHADER_UNLIT)
                #define WRITE_NORMAL_BUFFER
            #endif
            #endif
        
            // See Lit.shader
            #if SHADERPASS == SHADERPASS_MOTION_VECTORS && defined(WRITE_DECAL_BUFFER_AND_RENDERING_LAYER)
                #define WRITE_DECAL_BUFFER
            #endif
        
            #ifndef DEBUG_DISPLAY
                // In case of opaque we don't want to perform the alpha test, it is done in depth prepass and we use depth equal for ztest (setup from UI)
                // Don't do it with debug display mode as it is possible there is no depth prepass in this case
                #if !defined(_SURFACE_TYPE_TRANSPARENT)
                    #if SHADERPASS == SHADERPASS_FORWARD
                    #define SHADERPASS_FORWARD_BYPASS_ALPHA_TEST
                    #elif SHADERPASS == SHADERPASS_GBUFFER
                    #define SHADERPASS_GBUFFER_BYPASS_ALPHA_TEST
                    #endif
                #endif
            #endif
        
            // Define _DEFERRED_CAPABLE_MATERIAL for shader capable to run in deferred pass
            #if defined(SHADER_LIT) && !defined(_SURFACE_TYPE_TRANSPARENT)
                #define _DEFERRED_CAPABLE_MATERIAL
            #endif
        
            // Translate transparent motion vector define
            #if defined(_TRANSPARENT_WRITES_MOTION_VEC) && defined(_SURFACE_TYPE_TRANSPARENT)
                #define _WRITE_TRANSPARENT_MOTION_VECTOR
            #endif
        
            // -- Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D_TexelSize;
        float4 _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D_TexelSize;
        float4 _Direction_TexelSize;
        float4 _Direction_Stop_TexelSize;
        float _test;
        float4 _EmissionColor;
        float _UseShadowThreshold;
        float4 _DoubleSidedConstants;
        float _BlendMode;
        float _EnableBlendModePreserveSpecularLighting;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D);
        TEXTURE2D(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        SAMPLER(sampler_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D);
        TEXTURE2D(_Direction);
        SAMPLER(sampler_Direction);
        TEXTURE2D(_Direction_Stop);
        SAMPLER(sampler_Direction_Stop);
        
            // -- Property used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // -- Properties used by SceneSelectionPass
            #ifdef SCENESELECTIONPASS
            int _ObjectId;
            int _PassValue;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingMacros.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracing.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Material.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/ShaderVariablesRaytracingLightLoop.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RaytracingIntersection.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/Unlit/Unlit.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/MaterialUtilities.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/Raytracing/Shaders/RayTracingCommon.hlsl"
        #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderGraphFunctions.hlsl"
        	// GraphIncludes: <None>
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 uv3;
             float3 TimeParameters;
        };
        
            //Interpolator Packs: <None>
        
            // --------------------------------------------------
            // Graph
        
        
            // Graph Functions
            
        void Unity_Distance_float2(float2 A, float2 B, out float Out)
        {
            Out = distance(A, B);
        }
        
        void Unity_Comparison_Equal_float(float A, float B, out float Out)
        {
            Out = A == B ? 1 : 0;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A / B;
        }
        
        void Unity_Fraction_float2(float2 In, out float2 Out)
        {
            Out = frac(In);
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A * B;
        }
        
        void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Divide_float(float A, float B, out float Out)
        {
            Out = A / B;
        }
        
        void Unity_Arctangent_float(float In, out float Out)
        {
            Out = atan(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float2(float2 In, out float2 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Length_float2(float2 In, out float Out)
        {
            Out = length(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Round_float2(float2 In, out float2 Out)
        {
            Out = round(In);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
            // Graph Vertex
            // GraphVertex: <None>
        
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float3 Emission;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4 = IN.uv3;
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2 = float2(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float);
            float _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float;
            Unity_Distance_float2(_Vector2_4aeecd1cb271dd8e96c2fb12309b9dd0_Out_0_Vector2, float2(0, 0), _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float);
            float _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean;
            Unity_Comparison_Equal_float(_Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, 0, _Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean);
            UnityTexture2D _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction_Stop);
            float4 _UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4 = IN.uv0;
            float2 _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_bee9581f75a740779ac9d0f8936b21ae_Out_0_Vector4.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2);
            float2 _Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_5480a1a3275048d989a8ada84ccf24fd_Out_0_Vector2, _Vector2_6b027c9fc0b24ae9b09ed0e39ef8afa1_Out_0_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2);
            float2 _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_e857693cf0014d2988124241518c2341_Out_3_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2);
            float2 _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_f2173bca15d64a7a95c9254f6cb8ab91_Out_2_Vector2, _Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2);
            float2 _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_50322e8363244a3fa08daa41f33e6175_Out_1_Vector2, _Divide_84a2b2746d134f888c260538db791225_Out_2_Vector2, _Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2);
            float2 _Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_57755b34c5a74f3985dd7a4eae03b858_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2 = float2(_TexelSize_57755b34c5a74f3985dd7a4eae03b858_Width_0_Float, _TexelSize_57755b34c5a74f3985dd7a4eae03b858_Height_2_Float);
            float2 _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e38c93e7ec034c5086be5b78b3beb9cf_Out_0_Vector2, _Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2);
            float2 _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_c290d61d37574049a0501237b3936c19_Out_0_Vector2, _Vector2_d1676c776254451ea5a4b594ba7d586f_Out_0_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2);
            float2 _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_2c718c8316074585a741e0df7588c4d8_Out_2_Vector2, _Multiply_aac5d221cfa14a2aaff785af6a1bfc0c_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2);
            float2 _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_c128c58c3000428cbcd857da98dcd09d_Out_2_Vector2, _Subtract_8b2f287e1d2e40b0857ff91d125ebd2e_Out_2_Vector2, _Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2);
            float2 _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_579d83b582594a4587d6a0d63b27edd2_Out_2_Vector2, _Vector2_25402d0d6dde45e4bf47c7286550d927_Out_0_Vector2, _Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2);
            float _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float;
            Unity_Multiply_float_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_G_2_Float, -1, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float);
            float _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean;
            Unity_Comparison_Greater_float(_Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, 0, _Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean);
            float _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float;
            Unity_Divide_float(_Split_11d6e1d03b6aec85a5cc295d4f2f2c3a_R_1_Float, _Multiply_3bdf02c598430c878e3738cac7f0dae8_Out_2_Float, _Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float);
            float _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float;
            Unity_Arctangent_float(_Divide_2b884a92fd783d82a9012917c2b83115_Out_2_Float, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float);
            float _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float;
            Unity_Add_float(_Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, 3.141592, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float);
            float _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float;
            Unity_Branch_float(_Comparison_e67d9847aa86858490dd0c37e68e7505_Out_2_Boolean, _Arctangent_5140ab7fbdf95a8f9329bfd0cd09226c_Out_1_Float, _Add_cf6b7aee4a487f8ca16b9171b64a8f0d_Out_2_Float, _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float);
            float2 _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_c76c3d751aa24c28a8e550926070aa18_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2);
              float4 _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.tex, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.samplerstate, _Property_a4fe676301c247e09a7b62ac797e5ede_Out_0_Texture2D.GetTransformedUV(_Rotate_f74a9f58cb534420b51559e644786c42_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_R_5_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_G_6_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_B_7_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float = _SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_RGBA_0_Vector4.a;
            Gradient _Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient = NewGradient(0, 3, 2, float4(0, 0.0569272, 1, 0),float4(1, 0.9791913, 0, 0.4882429),float4(1, 0, 0, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0));
            float4 _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, 0, _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4);
            float4 _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_44188a7043714beba9807ba93e4699de_A_8_Float.xxxx), _SampleGradient_b10701941e9b43888f964faefb9621b8_Out_2_Vector4, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4);
            UnityTexture2D _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_Direction);
            float4 _UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4 = IN.uv0;
            float _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 1, _Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float);
            float _Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[0];
            float _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[1];
            float _Split_dee6095398844cccbbdad26553f6b3d8_B_3_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[2];
            float _Split_dee6095398844cccbbdad26553f6b3d8_A_4_Float = _UV_1ca38e193be034888b8a707af9d47210_Out_0_Vector4[3];
            float2 _Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2 = float2(_Split_dee6095398844cccbbdad26553f6b3d8_R_1_Float, _Split_dee6095398844cccbbdad26553f6b3d8_G_2_Float);
            float2 _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2;
            Unity_Normalize_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2);
            float _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float;
            Unity_Length_float2(_Vector2_1c1c51262be54e80b5387bcb099af6eb_Out_0_Vector2, _Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float);
            float _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float;
            Unity_Remap_float(_Length_9ef4d3c022ed427fa562561ec96b4bd6_Out_1_Float, float2 (0, 1), float2 (0.5, 1), _Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float);
            float2 _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Normalize_5dc0eb9fe39e432fade910da9a3883fe_Out_1_Vector2, (_Remap_46e9b9b3bfb440778c03d53a88d559f8_Out_3_Float.xx), _Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2);
            float _Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float = 5;
            float2 _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_372da6ef6a334cc58e3e9e3b3d4f7ea1_Out_2_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2);
            float2 _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2;
            Unity_Round_float2(_Multiply_238859756bbd4020ba90209469bfba9d_Out_2_Vector2, _Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2);
            float2 _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2;
            Unity_Divide_float2(_Round_85f252a8937146c5b632ae04fc63341f_Out_1_Vector2, (_Float_19782764654b478f9aa7b41b32ff1c41_Out_0_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2);
            float2 _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2;
            Unity_Multiply_float2_float2((_Multiply_8d1bd7f920dc4f3ab567d1fa0e06a93e_Out_2_Float.xx), _Divide_cc6138d846924917b965327374284baa_Out_2_Vector2, _Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2);
            float2 _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_7a4ceafa564442bba419b107d8d55373_Out_2_Vector2, float2(0.1, 0.1), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2);
            float2 _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2;
            Unity_TilingAndOffset_float((_UV_7c8a457b34527181924b2b4a71350de9_Out_0_Vector4.xy), float2 (2, 2), _Multiply_d68e7dabf6e649eabbe06e09ce902ecc_Out_2_Vector2, _TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2);
            float2 _Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2 = float2(1, 1);
            float2 _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2 = float2(30, 30);
            float2 _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2;
            Unity_Divide_float2(_Vector2_9a3e446bd0dfc3868e3db6f9bf101ead_Out_0_Vector2, _Vector2_8355e4dbd796b188ba1e500dcabf0c0f_Out_0_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2);
            float2 _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2;
            Unity_Divide_float2(_TilingAndOffset_67d58b7a8092828aaece7f3acfe6e8a9_Out_3_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2);
            float2 _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2;
            Unity_Fraction_float2(_Divide_5d0f49ab4bcab689894f976217114a79_Out_2_Vector2, _Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2);
            float2 _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Fraction_e4f0f05fb76fd3809f5ec855b54a4595_Out_1_Vector2, _Divide_f0d0aaa28637b781bcd99c1c912b5eae_Out_2_Vector2, _Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2);
            float2 _Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2 = float2(256, 256);
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.z;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.w;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelWidth_3_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.x;
            float _TexelSize_18bb9f416e386f87bee91aa46804bc56_TexelHeight_4_Float = UnityBuildTexture2DStructNoScale(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Texture_1_Texture2D).texelSize.y;
            float2 _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2 = float2(_TexelSize_18bb9f416e386f87bee91aa46804bc56_Width_0_Float, _TexelSize_18bb9f416e386f87bee91aa46804bc56_Height_2_Float);
            float2 _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e6d2595367ed0380b42c7a74855838a5_Out_0_Vector2, _Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2);
            float2 _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2 = float2(10, 10);
            float2 _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Vector2_e8728bfbf194e5818f010927b4ba8d79_Out_0_Vector2, _Vector2_97c81bcbfd26f08385a101ac3da412f3_Out_0_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2);
            float2 _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2;
            Unity_Subtract_float2(_Multiply_561a2890964f4081a8cf5d16bccb889f_Out_2_Vector2, _Multiply_bccda05b3809138d8da15484dd6a7b4e_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2);
            float2 _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cbcb0de832b1d08288916e46aa8d4df2_Out_2_Vector2, _Subtract_710d4672e66f7780ad90ca6e19a733bd_Out_2_Vector2, _Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2);
            float2 _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2 = float2(0.1, 0.1);
            float2 _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2;
            Unity_Multiply_float2_float2(_Multiply_cd542324eceadd8daaca18ab78d3fbf6_Out_2_Vector2, _Vector2_df86361e9c36e7828e3097cc1fa8a75b_Out_0_Vector2, _Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2);
            float2 _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2;
            Unity_Rotate_Radians_float(_Multiply_8e351d40d36634829f2bb0905b9b9342_Out_2_Vector2, float2 (0.5, 0.5), _Branch_9de100b31215ba89bb19e5b89012a25a_Out_3_Float, _Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2);
              float4 _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4 = SAMPLE_TEXTURE2D_LOD(_Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.tex, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.samplerstate, _Property_70a6ddd048a60f81b46e2042f9ae833b_Out_0_Texture2D.GetTransformedUV(_Rotate_ff1406ef744b188ca79c6d3bf0b8bbd7_Out_3_Vector2), 0);
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_R_5_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.r;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_G_6_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.g;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_B_7_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.b;
            float _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float = _SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_RGBA_0_Vector4.a;
            float4 _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4;
            Unity_SampleGradientV1_float(_Gradient_2989320cfaf94d99b0d33a688a94428f_Out_0_Gradient, _Distance_47e812ff6269a181b6e0faa017faee3b_Out_2_Float, _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4);
            float4 _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4;
            Unity_Multiply_float4_float4((_SampleTexture2DLOD_7fdbdd0c5b377086acfc5974a225b0dd_A_8_Float.xxxx), _SampleGradient_6cc0457448dd43209d582725ad86198a_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4);
            float4 _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4;
            Unity_Branch_float4(_Comparison_e4c91e80b3934aee879db576ff293636_Out_2_Boolean, _Multiply_86f5ae6eb4de4a47adbfc66d783a430f_Out_2_Vector4, _Multiply_0563147e8bc9868b8c2a4f2c442c349f_Out_2_Vector4, _Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4);
            surface.BaseColor = (_Branch_978d36ad04c24f45b84a06c6e15c86b5_Out_3_Vector4.xyz);
            surface.Emission = float3(0, 0, 0);
            surface.Alpha = 1;
            surface.AlphaClipThreshold = 0.5;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
            #ifdef HAVE_VFX_MODIFICATION
            #define VFX_SRP_ATTRIBUTES AttributesMesh
            #define VaryingsMeshType VaryingsMeshToPS
            #define VFX_SRP_VARYINGS VaryingsMeshType
            #define VFX_SRP_SURFACE_INPUTS FragInputs
            #endif
            SurfaceDescriptionInputs FragInputsToSurfaceDescriptionInputs(FragInputs input, float3 viewWS)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            #if defined(SHADER_STAGE_RAY_TRACING)
            #else
            #endif
        
        #if UNITY_UV_STARTS_AT_TOP
        #else
        #endif
        
        
            output.uv0 =                                        input.texCoord0;
            output.uv3 =                                        input.texCoord3;
            output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        
            // splice point to copy frag inputs custom interpolator pack into the SDI
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data (Specific Material)
        
        void BuildSurfaceData(FragInputs fragInputs, inout SurfaceDescription surfaceDescription, float3 V, PositionInputs posInput, out SurfaceData surfaceData)
        {
            // setup defaults -- these are used if the graph doesn't output a value
            ZERO_INITIALIZE(SurfaceData, surfaceData);
        
            // copy across graph values, if defined
            surfaceData.color = surfaceDescription.BaseColor;
        
            #ifdef WRITE_NORMAL_BUFFER
            // When we need to export the normal (in the depth prepass, we write the geometry one)
            surfaceData.normalWS = fragInputs.tangentToWorld[2];
            #endif
        
            #if defined(DEBUG_DISPLAY)
            if (_DebugMipMapMode != DEBUGMIPMAPMODE_NONE)
            {
                // TODO
            }
            #endif
        
            #ifdef _ENABLE_SHADOW_MATTE
        
                #if (SHADERPASS == SHADERPASS_FORWARD_UNLIT) || (SHADERPASS == SHADERPASS_RAYTRACING_GBUFFER) || (SHADERPASS == SHADERPASS_RAYTRACING_INDIRECT) || (SHADERPASS == SHADERPASS_RAYTRACING_FORWARD)
        
                    HDShadowContext shadowContext = InitShadowContext();
        
                    // Evaluate the shadow, the normal is guaranteed if shadow matte is enabled on this shader.
                    float3 shadow3;
                    ShadowLoopMin(shadowContext, posInput, normalize(fragInputs.tangentToWorld[2]), asuint(_ShadowMatteFilter), GetMeshRenderingLayerMask(), shadow3);
        
                    // Compute the average value in the fourth channel
                    float4 shadow = float4(shadow3, dot(shadow3, float3(1.0/3.0, 1.0/3.0, 1.0/3.0)));
        
                    float4 shadowColor = (1.0 - shadow) * surfaceDescription.ShadowTint.rgba;
                    float  localAlpha  = saturate(shadowColor.a + surfaceDescription.Alpha);
        
                    // Keep the nested lerp
                    // With no Color (bsdfData.color.rgb, bsdfData.color.a == 0.0f), just use ShadowColor*Color to avoid a ring of "white" around the shadow
                    // And mix color to consider the Color & ShadowColor alpha (from texture or/and color picker)
                    #ifdef _SURFACE_TYPE_TRANSPARENT
                        surfaceData.color = lerp(shadowColor.rgb * surfaceData.color, lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb), surfaceDescription.Alpha);
                    #else
                        surfaceData.color = lerp(lerp(shadowColor.rgb, surfaceData.color, 1.0 - surfaceDescription.ShadowTint.a), surfaceData.color, shadow.rgb);
                    #endif
                    localAlpha = ApplyBlendMode(surfaceData.color, localAlpha).a;
        
                    surfaceDescription.Alpha = localAlpha;
        
                #elif SHADERPASS == SHADERPASS_PATH_TRACING
        
                    surfaceData.normalWS = fragInputs.tangentToWorld[2];
                    surfaceData.shadowTint = surfaceDescription.ShadowTint.rgba;
        
                #endif
        
            #endif // _ENABLE_SHADOW_MATTE
        }
        
            // --------------------------------------------------
            // Get Surface And BuiltinData
        
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 V, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData RAY_TRACING_OPTIONAL_PARAMETERS)
            {
                // Don't dither if displaced tessellation (we're fading out the displacement instead to match the next LOD)
                #if !defined(SHADER_STAGE_RAY_TRACING) && !defined(_TESSELLATION_DISPLACEMENT)
                #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                LODDitheringTransition(ComputeFadeMaskSeed(V, posInput.positionSS), unity_LODFade.x);
                #endif
                #endif
        
                #ifndef SHADER_UNLIT
                #ifdef _DOUBLESIDED_ON
                    float3 doubleSidedConstants = _DoubleSidedConstants.xyz;
                #else
                    float3 doubleSidedConstants = float3(1.0, 1.0, 1.0);
                #endif
        
                ApplyDoubleSidedFlipOrMirror(fragInputs, doubleSidedConstants); // Apply double sided flip on the vertex normal
                #endif // SHADER_UNLIT
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = FragInputsToSurfaceDescriptionInputs(fragInputs, V);
        
                #if defined(HAVE_VFX_MODIFICATION)
                GraphProperties properties;
                ZERO_INITIALIZE(GraphProperties, properties);
        
                GetElementPixelProperties(fragInputs, properties);
        
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs, properties);
                #else
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
                #endif
        
                // Perform alpha test very early to save performance (a killed pixel will not sample textures)
                // TODO: split graph evaluation to grab just alpha dependencies first? tricky..
                #ifdef _ALPHATEST_ON
                    float alphaCutoff = surfaceDescription.AlphaClipThreshold;
                    #if SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_PREPASS
                    // The TransparentDepthPrepass is also used with SSR transparent.
                    // If an artists enable transaprent SSR but not the TransparentDepthPrepass itself, then we use AlphaClipThreshold
                    // otherwise if TransparentDepthPrepass is enabled we use AlphaClipThresholdDepthPrepass
                    #elif SHADERPASS == SHADERPASS_TRANSPARENT_DEPTH_POSTPASS
                    // DepthPostpass always use its own alpha threshold
                    alphaCutoff = surfaceDescription.AlphaClipThresholdDepthPostpass;
                    #elif (SHADERPASS == SHADERPASS_SHADOWS) || (SHADERPASS == SHADERPASS_RAYTRACING_VISIBILITY)
                    // If use shadow threshold isn't enable we don't allow any test
                    #endif
        
                    GENERIC_ALPHA_TEST(surfaceDescription.Alpha, alphaCutoff);
                #endif
        
                #if !defined(SHADER_STAGE_RAY_TRACING) && _DEPTHOFFSET_ON
                ApplyDepthOffsetPositionInput(V, surfaceDescription.DepthOffset, GetViewForwardDir(), GetWorldToHClipMatrix(), posInput);
                #endif
        
                #ifndef SHADER_UNLIT
                float3 bentNormalWS;
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData, bentNormalWS);
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD1
                    float4 lightmapTexCoord1 = fragInputs.texCoord1;
                #else
                    float4 lightmapTexCoord1 = float4(0,0,0,0);
                #endif
        
                #ifdef FRAG_INPUTS_USE_TEXCOORD2
                    float4 lightmapTexCoord2 = fragInputs.texCoord2;
                #else
                    float4 lightmapTexCoord2 = float4(0,0,0,0);
                #endif
        
                float alpha = 1.0;
                alpha = surfaceDescription.Alpha;
        
                // Builtin Data
                // For back lighting we use the oposite vertex normal
                InitBuiltinData(posInput, alpha, bentNormalWS, -fragInputs.tangentToWorld[2], lightmapTexCoord1, lightmapTexCoord2, builtinData);
        
                #else
                BuildSurfaceData(fragInputs, surfaceDescription, V, posInput, surfaceData);
        
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = surfaceDescription.Alpha;
        
                #if defined(DEBUG_DISPLAY)
                    // Light Layers are currently not used for the Unlit shader (because it is not lit)
                    // But Unlit objects do cast shadows according to their rendering layer mask, which is what we want to
                    // display in the light layers visualization mode, therefore we need the renderingLayers
                    builtinData.renderingLayers = GetMeshRenderingLayerMask();
                #endif
        
                #endif // SHADER_UNLIT
        
                #ifdef _ALPHATEST_ON
                    // Used for sharpening by alpha to mask - Alpha to covertage is only used with depth only and forward pass (no shadow pass, no transparent pass)
                    builtinData.alphaClipTreshold = alphaCutoff;
                #endif
        
                // override sampleBakedGI - not used by Unlit
        		// When overriding GI, we need to force the isLightmap flag to make sure we don't add APV (sampled in the lightloop) on top of the overridden value (set at GBuffer stage)
        
                builtinData.emissiveColor = surfaceDescription.Emission;
        
                // Note this will not fully work on transparent surfaces (can check with _SURFACE_TYPE_TRANSPARENT define)
                // We will always overwrite vt feeback with the nearest. So behind transparent surfaces vt will not be resolved
                // This is a limitation of the current MRT approach.
                #ifdef UNITY_VIRTUAL_TEXTURING
                #endif
        
                #ifdef LINE_RENDERING_OFFSCREEN_SHADING
                #endif
        
                #if _DEPTHOFFSET_ON
                builtinData.depthOffset = surfaceDescription.DepthOffset;
                #endif
        
                // TODO: We should generate distortion / distortionBlur for non distortion pass
                #if (SHADERPASS == SHADERPASS_DISTORTION)
                builtinData.distortion = surfaceDescription.Distortion;
                builtinData.distortionBlur = surfaceDescription.DistortionBlur;
                #endif
        
                #ifndef SHADER_UNLIT
                // PostInitBuiltinData call ApplyDebugToBuiltinData
                PostInitBuiltinData(V, posInput, surfaceData, builtinData);
                #else
                ApplyDebugToBuiltinData(builtinData);
                #endif
        
                RAY_TRACING_OPTIONAL_ALPHA_TEST_PASS
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassPathTracing.hlsl"
        
            // --------------------------------------------------
            // Visual Effect Vertex Invocations
        
        	#ifdef HAVE_VFX_MODIFICATION
                #if !defined(SHADER_STAGE_RAY_TRACING)
        	    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/VisualEffectVertex.hlsl"
                #else
                #endif
        	#endif
        
            ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    CustomEditorForRenderPipeline "Rendering.HighDefinition.HDUnlitGUI" "UnityEngine.Rendering.HighDefinition.HDRenderPipelineAsset"
    FallBack "Hidden/Shader Graph/FallbackError"
}