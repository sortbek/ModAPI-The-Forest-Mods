using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using UnityEngine;
using TheForest.Utils;
using ModAPI.Attributes;

namespace Teleport
{
    class TeleportMod : MonoBehaviour
    {
        
        protected bool visible;
        protected GUIStyle labelStyle;
        public string TPname = "";
        public string Message = "";
        private static Xml xml = new Xml();
        private static string path;
        public Rect TPWindow = new Rect(420, 0, 300, 300);
        private bool clicked;

        private TeleportMod()
        {
            path = SaveSlotUtils.GetLocalPath();
        }

        [ModAPI.Attributes.ExecuteOnGameStart]
        private static void AddToScene()
        {
            new GameObject("__Teleporter__").AddComponent<TeleportMod>();
            if(!File.Exists(path + "tp.xml"))
            {
                xml.Create(path);
            }
        }

        private void OnGUI()
        {            
            if (!this.visible)
                return;
            UnityEngine.GUI.skin = ModAPI.GUI.Skin;
            Matrix4x4 matrix = UnityEngine.GUI.matrix;
            if (this.labelStyle == null)
            {
                this.labelStyle = new GUIStyle(UnityEngine.GUI.skin.label);
                this.labelStyle.fontSize = 12;
            }

            UnityEngine.GUI.Box(new Rect(10f, 10f, 400f, 280f), "Teleport menu", UnityEngine.GUI.skin.window);
            float cY = 50f;

            UnityEngine.GUI.Label(new Rect(20f, cY, 150f, 20f), "Location Name", this.labelStyle);
            TPname = UnityEngine.GUI.TextField(new Rect(170f,cY,200f,20f), TPname, 25) ;
            cY += 30f;
            float x = TheForest.Utils.LocalPlayer.GameObject.transform.position.x;
            float y = TheForest.Utils.LocalPlayer.GameObject.transform.position.y;
            float z = TheForest.Utils.LocalPlayer.GameObject.transform.position.z;
            if (UnityEngine.GUI.Button(new Rect(20f, cY, 100f, 20f), "SAVE"))
            {
                xml.Update(path, TPname,x,y,z);
                Message = "Location '" + TPname + "' added!";
                TPname = "";
            }
            cY += 30;
            if (UnityEngine.GUI.Button(new Rect(280f, cY, 100f, 20f), "Locations"))
            {
                clicked = true;
            }
            if (clicked)
            {
                TPWindow = UnityEngine.GUI.Window(0, TPWindow, FillTPWindow, "TP Locations");
            }
            cY += 30;
            UnityEngine.GUI.Label(new Rect(20f, cY, 150f, 20f), "x-" + x, this.labelStyle);
            cY += 20;
            UnityEngine.GUI.Label(new Rect(20f, cY, 150f, 20f), "y-" + y, this.labelStyle);
            cY += 20;
            UnityEngine.GUI.Label(new Rect(20f, cY, 150f, 20f), "z-" + z, this.labelStyle);
            cY += 30f;
            UnityEngine.GUI.Label(new Rect(20f, cY, 150f, 20f), Message, this.labelStyle);
            UnityEngine.GUI.matrix = matrix;
        }

        void FillTPWindow(int windowID)
        {
            float cY = 30f;
            //Fetch Players
            
            foreach (BoltEntity boltEntity in BoltNetwork.entities)
            {
                if (boltEntity.StateIs<IPlayerState>())
                {
                    string CPlayerName = boltEntity.GetState<IPlayerState>().name;
                    float CPlayerX = boltEntity.GetState<IPlayerState>().Transform.Position.x;
                    float CPlayerY = boltEntity.GetState<IPlayerState>().Transform.Position.y;
                    float CPlayerZ = boltEntity.GetState<IPlayerState>().Transform.Position.z;

                    if (UnityEngine.GUI.Button(new Rect(120f, cY, 150f, 20f), CPlayerName))
                    {
                       LocalPlayer.GameObject.transform.localPosition = new Vector3(CPlayerX, CPlayerY, CPlayerZ);
                        
                    }
                    cY += 30f;
                }
            }
            //Fetch locations
            List<Location> locations = new List<Location>();
            locations = xml.Read(path);
            
            foreach (Location location in locations)
            {
                
                UnityEngine.GUI.Label(new Rect(20f, cY, 150f, 20f), location.GetName() , this.labelStyle);
                if(UnityEngine.GUI.Button(new Rect( 120f, cY, 80f, 20f), "Teleport"))
                {
                    LocalPlayer.GameObject.transform.localPosition = new Vector3(location.GetX(), location.GetY(), location.GetZ());
                }
                if(UnityEngine.GUI.Button(new Rect(205, cY, 80f, 20f ), "Remove"))
                {
                    xml.Delete(path, location);
                }
                cY += 30;
            }

            if (UnityEngine.GUI.Button(new Rect(20f, cY, 100f, 20f), "Close"))
            {
                clicked = false;
            }

        }

        private void Update()
        {            
            if (ModAPI.Input.GetButtonDown("TeleportMe"))
            {
                if (this.visible)
                {
                    Message = "";
                    LocalPlayer.FpCharacter.UnLockView();
                }else
                {
                    LocalPlayer.FpCharacter.LockView();
                }
                this.visible = !this.visible;
                if (clicked)
                {
                    clicked = false;
                }
            }
        }
    }
}
