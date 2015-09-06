using UnityEngine;
using TheForest.Items;
using TheForest.Utils;

namespace InventoryMod
{
    class Inventory : MonoBehaviour
    {
        protected bool visible = false;
        protected GUIStyle labelStyle;
        public Vector2 scrollPosition = Vector2.zero;
        private float cY;


        [ModAPI.Attributes.ExecuteOnGameStart]
        static void AddMeToScene()
        {
            GameObject GO = new GameObject("__InventoryMenu__");
            GO.AddComponent<Inventory>();
        }

        private void OnGUI()
        {
            if (this.visible)
            {
                GUI.skin = ModAPI.GUI.Skin;

                Matrix4x4 bkpMatrix = GUI.matrix;

                if (labelStyle == null)
                {
                    labelStyle = new GUIStyle(GUI.skin.label);
                    labelStyle.fontSize = 12;
                }
                
                GUI.Box(new Rect(10, 10, 400, 450), "Cheat menu", GUI.skin.window);
                scrollPosition = GUI.BeginScrollView(new Rect(10, 50, 390, 350), scrollPosition, new Rect(0, 0, 350, cY));
                this.cY = 25f;
                for (int index = 0; index < ItemDatabase.Items.Length; ++index)
                {
                    GUI.Label(new Rect(20f, cY, 150f, 20f), ItemDatabase.Items[index]._name, labelStyle);
                    if(GUI.Button(new Rect(170f, cY, 150f, 20f), "Add"))
                    {
                        LocalPlayer.Inventory.AddItem(ItemDatabase.Items[index]._id);
                    }
                    this.cY += 30f;
                }
                GUI.EndScrollView();
                
                if(GUI.Button(new Rect(20f, 410f, 100f, 20f), "Close"))
                {
                    this.visible = false;
                }

                GUI.matrix = bkpMatrix;
            }
        }

        private void GenerateList()
        {
            for(int index = 0; index < ItemDatabase.Items.Length; ++index)
                ModAPI.Console.Write("itemName" + ItemDatabase.Items[index]._name + " itemID: " + ItemDatabase.Items[index]._id );
        }

        private void Update()
        {
            if (ModAPI.Input.GetButtonDown("Start"))
            {
                if (this.visible)
                {
                    TheForest.Utils.LocalPlayer.FpCharacter.UnLockView();
                }
                else
                {
                    TheForest.Utils.LocalPlayer.FpCharacter.LockView();
                }
                this.visible = !this.visible;
            }

        }
    }
}
