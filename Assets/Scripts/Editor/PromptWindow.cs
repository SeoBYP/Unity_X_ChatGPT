using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using StoryModel;
using UnityEditor;
using UnityEngine;

namespace PromptEditor
{
    public class PromptWindow : OdinMenuEditorWindow
    {
        [MenuItem("Window/ChatGPT/Prompt Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(PromptWindow), false, "Prompt Editor");
        }
        
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
            
            tree.Add("Prompts", new PromptMenu());
            var allAssets = AssetDatabase.GetAllAssetPaths()
                .Where(x => x.StartsWith("Assets/PromptModels/"))
                .OrderBy(x => x);

            foreach (var path in allAssets)
            {
                string menuName = path.Substring("Assets/PromptModels/".Length);
                int assetExtensionIndex = menuName.LastIndexOf(".asset");
                if (assetExtensionIndex > -1)
                {
                    menuName = menuName.Substring(0, assetExtensionIndex);
                }

                tree.AddAssetAtPath(menuName, path, typeof(SerializedScriptableObject));
            }
            tree.Add("Create New Story", new CreatePromptMenuItem());
            tree.EnumerateTree().AddThumbnailIcons();
            return tree;
        }
    }

    public class PromptMenu
    {
        [BoxGroup("프롬프트")] 
        [SerializeField,ProgressBar(20,100)] 
        private int Episodes = 50;
        
        [BoxGroup("프롬프트")] 
        [SerializeField, MultiLineProperty(20)]
        private string Prompt = "We will be playing a TRPG game. As a moderator, \n" +
                                "you must explain to us the settings and situations that fit the game's universe. \n" +
                                "After explanation, options appropriate for each situation are sent to the user. \n" +
                                "There are a total of 3 choices, and new situations and stories are created based on the choices sent by the user. \n" +
                                "Each new episode offers three choices at the end.\n\n" +
                                
                                "Players may die in the story, so a \"GAME OVER\" message will be sent at the end if the player dies.\n\n" +
                                
                                "The story progresses over a total of 50 episodes. \n" +
                                "After number 50, you must finish the story with various endings, \n" +
                                "such as a happy ending, an unhappy ending, or a normal ending.";

    }
    // 'Create New Prompt' 버튼에 대한 액션을 정의하는 커스텀 메뉴 항목 클래스
    public class CreatePromptMenuItem
    {
        private const string folderPath = "Assets/PromptModels/";
        [BoxGroup("스토리 정보")]
        [SerializeField, TextArea] public string AssetName = "NewStoryPrompt";
        [BoxGroup("스토리 정보")]
        [SerializeField, TextArea] public string StoryTitle = "Story Title";
        [BoxGroup("스토리 정보")]
        [SerializeField, MultiLineProperty(20)] public string StoryDescription = "Story Description";

        [Button("Create New Prompt")]
        private void CreateNewPrompt()
        {
            // 새 StoryPromptModel 인스턴스를 생성합니다.
            StoryPromptModel newStory = ScriptableObject.CreateInstance<StoryPromptModel>();
            newStory.name = AssetName;
            // 새 StoryPromptModel 에셋에 제목과 설명을 설정합니다.
            newStory.StoryTitle = StoryTitle;
            newStory.StoryDescription = StoryDescription;

            // 생성된 StoryPromptModel 인스턴스를 에셋으로 저장합니다.
            // 파일 이름에 .asset 확장자를 추가합니다.
            string assetPath = folderPath + AssetName + ".asset";
            AssetDatabase.CreateAsset(newStory, assetPath);
            AssetDatabase.SaveAssets();

            // 생성된 에셋을 에디터에서 선택합니다.
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newStory;

            Debug.Log("New story prompt created and saved: " + assetPath);
        }
    }
}