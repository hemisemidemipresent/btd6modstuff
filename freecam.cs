
using Assets.Scripts.Unity.UI_New.InGame;
using MelonLoader;
using NKHook6.Api.Events;
using UnityEngine;

namespace Test
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            EventRegistry.subscriber.listen(typeof(Main));

        }
        static float camDist = 30f;
        static Vector3 camPos = new Vector3(-150, 0, -150);
        static float camAngle = 0;
        static float delta = 0;
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Input.GetMouseButtonDown(0))
            {
                var v3 = Input.mousePosition;
                float camDist = 30f;
                v3.z = camDist;
                v3 = InGame.instance.sceneCamera.ScreenToWorldPoint(v3);



            }
        }

        [EventAttribute("KeyPressEvent")]
        public static void onEvent(KeyEvent e)
        {

            var key = e.key.ToString();
            NKHook6.Logger.Log(key);

            InGame.instance.sceneCamera.transform.position = new UnityEngine.Vector3(0, 0, 0);
            InGame.instance.sceneCamera.transform.rotation = UnityEngine.Quaternion.Euler(60, 0, 0);



            if (key == "UpArrow")
            {
                camPos += new Vector3(0, 0, 2);
            }
            else if (key == "DownArrow")
            {
                camPos += new Vector3(0, 0, -2);
            }
            else if (key == "LeftArrow")
            {
                camPos += new Vector3(-2, 0, 0);
            }
            else if (key == "RightArrow")
            {
                camPos += new Vector3(2, 0, 0);
            }
            else if (key == "PageUp")
            {
                camPos += new Vector3(0, 1, 0);
            }
            else if (key == "PageDown")
            {
                camPos += new Vector3(0, -1, 0);
            }else if(key == "CapsLock")
            {
                camPos = new Vector3(0, 0, 0);
            }
            InGame.instance.sceneCamera.transform.position = camPos;
            camAngle += delta * 0.55f;
            camAngle %= (3.1415f * 2);
            InGame.instance.sceneCamera.transform.LookAt(new UnityEngine.Vector3(0, 0, 0));
        }


    }



}
