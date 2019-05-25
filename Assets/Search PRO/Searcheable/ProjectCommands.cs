#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.U2D;
using UnityEngine.Tilemaps;
using UnityEditor.Animations;

namespace SearchPRO {
	public static class ProjectCommands {
		
		[Command]
		[Category("Project")]
		[Title("Create Folder")]
		[Icon(typeof(DefaultAsset))]
		[Description("Creates a new folder in the project.")]
		public static void CreateFolder() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Folder");
		}

		[Command]
		[Category("Project")]
		[Title("Create C# Script")]
		[Icon(typeof(MonoScript))]
		[Description("Creates a new C# script in the project.")]
		public static void CreateCSharpScript() {
			EditorApplication.ExecuteMenuItem("Assets/Create/C# Script");
		}

		[Command]
		[Category("Project/Shader")]
		[Title("Create Standard Surface Shader")]
		[Icon(typeof(Shader))]
		[Description("Creates a new Standard Surface Shader in the project.")]
		public static void CreateShaderStandardSurface() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Standard Surface Shader");
		}

		[Command]
		[Category("Project/Shader")]
		[Title("Create Unlit Shader")]
		[Icon(typeof(Shader))]
		[Description("Creates a new Unlit Shader in the project.")]
		public static void CreateShaderUnlit() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Unlit Shader");
		}

		[Command]
		[Category("Project/Shader")]
		[Title("Create Image Effect Shader")]
		[Icon(typeof(Shader))]
		[Description("Creates a new Image Effect Shader in the project.")]
		public static void CreateShaderImageEffect() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Image Effect Shader");
		}

		[Command]
		[Category("Project/Shader")]
		[Title("Create Shader Variant Collection")]
		[Icon(typeof(Shader))]
		[Description("Creates a new Shader Variant Collection in the project.")]
		public static void CreateShaderVariantCollection() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Shader Variant Collection");
		}

		[Command]
		[Category("Project/Testing")]
		[Title("Create Tests Assembly Folder")]
		[Description("Creates a new Tests Assembly Folder in the project.")]
		public static void CreateTestingTestsAssemblyFolder() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Testing/Tests Assembly Folder");
		}

		[Command]
		[Category("Project/Playables")]
		[Title("Create Playable Behaviour C# Script")]
		[Description("Creates a new Playable Behaviour C# Script in the project.")]
		public static void CreatePlayablesBehaviourCSharpScript() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Playables/Playable Behaviour C# Script");
		}

		[Command]
		[Category("Project/Playables")]
		[Title("Create Playable Asset C# Script")]
		[Description("Creates a new Playable Asset C# Script in the project.")]
		public static void CreatePlayablesAssetCSharpScript() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Playables/Playable Asset C# Script");
		}

		[Command]
		[Category("Project")]
		[Title("Create Assembly Definition")]
		[Description("Creates a new Assembly Definition in the project.")]
		public static void CreateAssemblyDefinition() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Assembly Definition");
		}

		[Command]
		[Category("Project/TextMeshPro")]
		[Title("Create Font Asset")]
		[Icon(typeof(Font))]
		[Description("Creates a new Font Asset in the project.")]
		public static void CreateTMPFontAsset() {
			EditorApplication.ExecuteMenuItem("Assets/Create/TextMeshPro/Font Asset");
		}

		[Command]
		[Category("Project/TextMeshPro")]
		[Title("Create Sprite Asset")]
		[Icon(typeof(Sprite))]
		[Description("Creates a new Sprite Asset in the project.")]
		public static void CreateTMPSpriteAsset() {
			EditorApplication.ExecuteMenuItem("Assets/Create/TextMeshPro/Sprite Asset");
		}

		[Command]
		[Category("Project/TextMeshPro")]
		[Title("Create Color Gradient")]
		[Description("Creates a new Color Gradient in the project.")]
		public static void CreateTMPColorGradient() {
			EditorApplication.ExecuteMenuItem("Assets/Create/TextMeshPro/Color Gradient");
		}

		[Command]
		[Category("Project/TextMeshPro")]
		[Title("Create Style Sheet")]
		[Description("Creates a new Style Sheet in the project.")]
		public static void CreateTMPStyleSheet() {
			EditorApplication.ExecuteMenuItem("Assets/Create/TextMeshPro/Style Sheet");
		}

		[Command]
		[Category("Project")]
		[Title("Create Scene")]
		[Icon(typeof(SceneAsset))]
		[Description("Creates a new Scene in the project.")]
		public static void CreateScene() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Scene");
		}

		[Command]
		[Category("Project")]
		[Title("Create Audio Mixer")]
		[Icon(typeof(AudioMixer))]
		[Description("Creates a new Audio Mixer in the project.")]
		public static void CreateAudioMixer() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Audio Mixer");
		}

		[Command]
		[Category("Project")]
		[Title("Create Material")]
		[Icon(typeof(Material))]
		[Description("Creates a new Material in the project.")]
		public static void CreateMaterial() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Material");
		}

		[Command]
		[Category("Project")]
		[Title("Create Lens Flare")]
		[Icon(typeof(LensFlare))]
		[Description("Creates a new Material in the project.")]
		public static void CreateLensFlare() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Lens Flare");
		}

		[Command]
		[Category("Project")]
		[Title("Create Render Texture")]
		[Icon(typeof(RenderTexture))]
		[Description("Creates a new Render Texture in the project.")]
		public static void CreateRenderTexture() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Render Texture");
		}

		[Command]
		[Category("Project")]
		[Title("Create Lightmap Parameters")]
		[Icon(typeof(LightmapParameters))]
		[Description("Creates a new Lightmap Parameters in the project.")]
		public static void CreateLightmapParameters() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Lightmap Parameters");
		}

		[Command]
		[Category("Project")]
		[Title("Create Custom Render Texture")]
		[Icon(typeof(CustomRenderTexture))]
		[Description("Creates a new Custom Render Texture in the project.")]
		public static void CreateCustomRenderTexture() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Custom Render Texture");
		}

		[Command]
		[Category("Project")]
		[Title("Create Sprite Atlas")]
		[Icon(typeof(SpriteAtlas))]
		[Description("Creates a new Sprite Atlas in the project.")]
		public static void CreateSpriteAtlas() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Sprite Atlas");
		}

		[Command]
		[Category("Project/Sprites")]
		[Title("Create Square")]
		[Icon(typeof(Sprite))]
		[Description("Creates a new Sprite Square in the project.")]
		public static void CreateSpriteSquare() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Sprites/Square");
		}

		[Command]
		[Category("Project/Sprites")]
		[Title("Create Triangle")]
		[Icon(typeof(Sprite))]
		[Description("Creates a new Sprite Triangle in the project.")]
		public static void CreateSpriteTriangle() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Sprites/Triangle");
		}

		[Command]
		[Category("Project/Sprites")]
		[Title("Create Diamond")]
		[Icon(typeof(Sprite))]
		[Description("Creates a new Sprite Diamond in the project.")]
		public static void CreateSpriteDiamond() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Sprites/Diamond");
		}

		[Command]
		[Category("Project/Sprites")]
		[Title("Create Hexagon")]
		[Icon(typeof(Sprite))]
		[Description("Creates a new Sprite Hexagon in the project.")]
		public static void CreateSpriteHexagon() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Sprites/Hexagon");
		}

		[Command]
		[Category("Project/Sprites")]
		[Title("Create Circle")]
		[Icon(typeof(Sprite))]
		[Description("Creates a new Sprite Circle in the project.")]
		public static void CreateSpriteCircle() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Sprites/Circle");
		}

		[Command]
		[Category("Project/Sprites")]
		[Title("Create Polygon")]
		[Icon(typeof(Sprite))]
		[Description("Creates a new Sprite Polygon in the project.")]
		public static void CreateSpritePolygon() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Sprites/Polygon");
		}

		[Command]
		[Category("Project")]
		[Title("Create Tile")]
		[Icon(typeof(Tile))]
		[Description("Creates a new Tile in the project.")]
		public static void CreateTile() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Tile");
		}

		[Command]
		[Category("Project")]
		[Title("Create Animator Controller")]
		[Icon(typeof(AnimatorController))]
		[Description("Creates a new Animator Controller in the project.")]
		public static void CreateAnimatorController() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Animator Controller");
		}

		[Command]
		[Category("Project")]
		[Title("Create Animation")]
		[Icon(typeof(Animation))]
		[Description("Creates a new Animation in the project.")]
		public static void CreateAnimation() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Animation");
		}

		[Command]
		[Category("Project")]
		[Title("Create Animator Override Controller")]
		[Icon(typeof(AnimatorOverrideController))]
		[Description("Creates a new Animator Override Controller in the project.")]
		public static void CreateAnimatorOverrideController() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Animator Override Controller");
		}

		[Command]
		[Category("Project")]
		[Title("Create Avatar Mask")]
		[Icon(typeof(AvatarMask))]
		[Description("Creates a new Avatar Mask in the project.")]
		public static void CreateAvatarMask() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Avatar Mask");
		}

		[Command]
		[Category("Project")]
		[Title("Create Timeline")]
		[Description("Creates a new Timeline in the project.")]
		public static void CreateTimeline() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Timeline");
		}

		[Command]
		[Category("Project")]
		[Title("Create Signal")]
		[Description("Creates a new Signal in the project.")]
		public static void CreateSignal() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Signal");
		}

		[Command]
		[Category("Project")]
		[Title("Create Physic Material")]
		[Icon(typeof(PhysicMaterial))]
		[Description("Creates a new Physic Material in the project.")]
		public static void CreatePhysicMaterial() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Physic Material");
		}

		[Command]
		[Category("Project")]
		[Title("Create Physic Material 2D")]
		[Icon(typeof(PhysicsMaterial2D))]
		[Description("Creates a new Physic Material 2D in the project.")]
		public static void CreatePhysicMaterial2D() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Physic Material 2D");
		}

		[Command]
		[Category("Project")]
		[Title("Create GUI Skin")]
		[Icon(typeof(GUISkin))]
		[Description("Creates a new GUI Skin in the project.")]
		public static void CreateGUISkin() {
			EditorApplication.ExecuteMenuItem("Assets/Create/GUI Skin");
		}

		[Command]
		[Category("Project")]
		[Title("Create Custom Font")]
		[Description("Creates a new Custom Font in the project.")]
		public static void CreateCustomFont() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Custom Font");
		}

		[Command]
		[Category("Project/Legacy")]
		[Title("Create Cubemap")]
		[Icon(typeof(Cubemap))]
		[Description("Creates a new Custom Font in the project.")]
		public static void CreateLegacyCubemap() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Legacy/Cubemap");
		}

		[Command]
		[Category("Project")]
		[Title("Create UIElements Editor Window")]
		[Description("Creates a new UIElements Editor Window in the project.")]
		public static void CreateUIElementsEditorWindow() {
			EditorApplication.ExecuteMenuItem("Assets/Create/UIElements Editor Window");
		}

		[Command]
		[Category("Project")]
		[Title("Create Brush")]
		[Description("Creates a new Brush in the project.")]
		public static void CreateBrush() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Brush");
		}

		[Command]
		[Category("Project")]
		[Title("Create Terrain Layer")]
		[Icon(typeof(TerrainLayer))]
		[Description("Creates a new Terrain Layer in the project.")]
		public static void CreateTerrainLayer() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Terrain Layer");
		}
	}
}

#endif
