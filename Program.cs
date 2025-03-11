using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using System.Buffers.Text;
using System.Runtime.InteropServices.JavaScript;

namespace UnoWasm //todo-------------------------------- 할당한 메모리 삭제좀;
{
    public unsafe class Colorify
    {
        
        public static List<Type> filters = new List<Type>();

        public static string parseJSString(byte* inputAsUtf8)
        {
            try
            {
                int length = 0;
                while (inputAsUtf8[length] != 0) 
                {
                    length++;
                }

                ReadOnlySpan<byte> input = new ReadOnlySpan<byte>(inputAsUtf8, length);

                string inputString = System.Text.Encoding.UTF8.GetString(input);
                return inputString;
            }
            catch(Exception e){

            }
            return "imreadytosuck";
        }
        public static void Log(string a)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(a);
            fixed(byte* p = bytes){
                JSInterop.sans(p);
            }
        }
        [UnmanagedCallersOnly(EntryPoint = "SetONOFF")]
        public static void SetONOFF(int camID, int monoID, int value){
            camOptions[camID].components[monoID].enabled = (value == 1);
        }
        
        [UnmanagedCallersOnly(EntryPoint = "SetONOFFs")]
        public static void SetONOFFs(byte* camID, int* monoID, byte* value, int length){
            for (int i = 0; i < length; i++){
                camOptions[Convert.ToInt32(camID[i])].components[monoID[i]].enabled = (Convert.ToInt32(value[i]) == 1);
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "SetOFFSetting")]
        public static void SetOFFSetting(int camID, int monoID, bool me, bool offothers){
            if (offothers){ 
                foreach(var kvp in camOptions[camID].components){
                    Type t = kvp.Value.GetType();
                    if (!t.Equals(typeof(CameraFilterPack_Bloom)) && !t.Equals(typeof(PenTile))){
                        kvp.Value.enabled = false;
                    }
                }
            }
            camOptions[camID].components[monoID].enabled = me;
        }
        public static void SetOFFSettingsex(int camID, int monoID, bool me, bool offothers){
            if (offothers){ 
                foreach(var kvp in camOptions[camID].components){
                    Type t = kvp.Value.GetType();
                    if (!t.Equals(typeof(CameraFilterPack_Bloom)) && !t.Equals(typeof(PenTile))){
                        kvp.Value.enabled = false;
                    }
                }
            }
            camOptions[camID].components[monoID].enabled = me;
        }
        [UnmanagedCallersOnly(EntryPoint = "SetOFFSettings")]
        public static void SetOFFSettings(byte* camID, int* monoID, byte* value, byte* offotherss, int length){
            for (int i = 0; i < length; i++){
                SetOFFSettingsex(Convert.ToInt32(camID[i]), monoID[i], (Convert.ToInt32(value[i]) == 1), (Convert.ToInt32(offotherss[i]) == 1));
            }
        }
        [UnmanagedCallersOnly(EntryPoint = "GetCamCount")]
        public static int GetCamCount(){
            return camOptions.Count;
        }


        public static MonoBehaviour makeInstance(Type t, bool isDefault = false){
            MonoBehaviour o = (MonoBehaviour)Activator.CreateInstance(t);

            if (o != null){
                o.method = t.GetMethod("OnRenderImage", BindingFlags.NonPublic|BindingFlags.Instance);
                o.fields = t.GetFields(BindingFlags.Public|BindingFlags.Instance);
                o.tweens = new Dictionary<int, Tween>();
                o.enabled = false;
                
                if (!isDefault){
                    foreach (FieldInfo fi in o.fields){
                        Type ft = fi.FieldType;
                        object value = null;
                        if (ft == typeof(UnityEngine.Vector2)){
                            value = new UnityEngine.Vector2(0f, 0f);
                        }
                        else if (ft == typeof(UnityEngine.Vector3)){
                            value = new UnityEngine.Vector3(0f, 0f, 0f);
                        }
                        else if (ft == typeof(UnityEngine.Vector4)){
                            value = new UnityEngine.Vector4(0f, 0f, 0f, 0f);
                        }
                        else if (ft == typeof(UnityEngine.Color)){
                            value = new UnityEngine.Color(1f, 1f, 1f, 1f);
                        }
                        else if (ft == typeof(System.Single)){
                            value = 0f;
                        }
                        else if (ft == typeof(System.Int32)){
                            value = 0;
                        }


                        if (value != null){
                            fi.SetValue(o,value);
                        }
                    }
                }
                
                MethodInfo mi = t.GetMethod("Start", BindingFlags.NonPublic|BindingFlags.Instance);
                mi.Invoke(o, null);
                return o;
            }
            return null;
        }

        [UnmanagedCallersOnly(EntryPoint = "LoadFilters")]
        public static int LoadFilters()
        {
            Log("suc k-wasm");//한국웹어셈블리가 성공하다!!
            foreach (Type type2 in from t in typeof(CameraFilterPack_3D_Anomaly).Assembly.GetTypes()
                                   where t.Name.StartsWith("CameraFilterPack_") select t)
            {
                // Log(type2.Name);
                filters.Add(type2);
            }
            filters.Add(typeof(PenTile));
            return filters.Count;
        }

    

        // [UnmanagedCallersOnly(EntryPoint = "testF")]
        // public static float* rrr(float* array, int length)
        // {
        //     float[] a = new float[length];
        //     for (int i = 0; i< length; i ++){
        //         a[i] = array[i];
        //     }
        //     fixed(float* ptr = a){
        //         return ptr;
        //     }
        //     return array;
        // }


        public static List<Camera> camOptions = new List<Camera>();

        [UnmanagedCallersOnly(EntryPoint = "CreateCamOption")]
        public static int CreateCamOption()
        {
            Camera cam = new Camera();
            camOptions.Add(cam);
            return (camOptions.Count-1);
        }
        [UnmanagedCallersOnly(EntryPoint = "DestroyAllCamOptions")]
        public static void DestroyAllCamOptions(){
            camOptions.Clear();
        }
        public static Camera currentCam = null;
        [UnmanagedCallersOnly(EntryPoint = "AddCamComponent")]
        public static void AddCamComponent(int monoID)
        {
            if (currentCam != null){
                currentCam.components[monoID] = makeInstance(filters[monoID]);
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "AddCamComponents")]
        public static void AddCamComponents(int* monoID, int length)
        {
            if (currentCam != null){
                for (int i = 0; i< length; i++)
                {
                    currentCam.components[monoID[i]] = makeInstance(filters[monoID[i]]);
                }
            }
        }
        [UnmanagedCallersOnly(EntryPoint = "AddDefaultCamComponents")]
        public static void AddDefaultCamComponents(int* monoID, int length)
        {
            if (currentCam != null){
                for (int i = 0; i< length; i++)
                {
                    currentCam.components[monoID[i]] = makeInstance(filters[monoID[i]], true);
                }
            }
        }
        [UnmanagedCallersOnly(EntryPoint = "ChangeCurrCamOption")]
        public static void ChangeCurrCamOption(int id){
            currentCam = camOptions[id];
        }
        [UnmanagedCallersOnly(EntryPoint = "GetComponentFields")]
        public static byte* GetComponentFields(int id){
            FieldInfo[] fi = filters[id].GetFields(BindingFlags.Public |
                                    BindingFlags.Instance);
            int len = fi.Length;
            string res = "";
            
            for (int i = 1; i <= len; i++){
                
                string count = "";
                Type t = fi[i-1].FieldType;
                if (t == typeof(UnityEngine.Vector2)){
                    count = "2";
                }
                else if (t == typeof(UnityEngine.Vector3)){
                    count = "3";
                }
                else if (t == typeof(UnityEngine.Vector4)){
                    count = "4";
                }
                else if (t == typeof(UnityEngine.Color)){
                    count = "5";
                }
                else if (t == typeof(System.Single)){
                    count = "1";
                }
                else if (t == typeof(System.Int32)){
                    count = "6";
                }
                else{
                    count = "0";
                }
                    
                
                
                res+= (fi[i-1].Name+count) + (i == len ? "" : "|");
            }
            fixed(byte* ptr = System.Text.Encoding.UTF8.GetBytes(res)){
                return ptr;
            }
        }
        [UnmanagedCallersOnly(EntryPoint = "GetFloatArrayFromFloatArrayArray")]
        public static float* GetFloatArrayFromFloatArrayArray(float[]* ptr, int i){
            float[] res = ptr[i];
            fixed(float* p = res){
                return p;
            }
        }
        public static object getRV(float* value, byte valueCount){
            switch(valueCount){
                case 1:
                    return value[0]/100f;
                case 2:
                    return new UnityEngine.Vector2(value[0], value[1]);
                case 3:
                    return new UnityEngine.Vector3(value[0], value[1], value[2]);
                case 4:
                    return new UnityEngine.Vector4(value[0], value[1], value[2], value[3]);
                case 5:
                    return new UnityEngine.Color(value[0]/255f, value[1]/255f, value[2]/255f, value[3]/255f);
                case 6:
                    return Convert.ToInt32(value[0]);
            }
            return 0f;
        }

        
        public static float currentTime = 0;
        public static void AddTween(byte camID, int monoID, byte fieldIDb, float* value, byte valueCount, float time, float duration, byte easeID){
            MonoBehaviour comp = camOptions[Convert.ToInt32(camID)].components[monoID];
            if (comp == null) return;

            int fieldID = Convert.ToInt32(fieldIDb);
            object rvalue = getRV(value, valueCount);
            // if (duration == 0) {
            //     comp.fields[fieldID].SetValue(comp, rvalue);
            //     return;
            // } tween은 DoAllTween 실행시에 순서대로 실행되는데, 이렇게 해버리면 순서가 꼬임

            object fromValue;
            if (comp.tweens.ContainsKey(fieldID)){
                fromValue = comp.tweens[fieldID].toValue;
            }
            else{
                fromValue = comp.fields[fieldID].GetValue(comp);
            }


            Tween t = new Tween(comp, comp.fields[fieldID], fromValue, rvalue, valueCount, time, duration, easeID);
            comp.tweens[fieldID] = t;
        }
        [UnmanagedCallersOnly(EntryPoint = "AddTweens")]
        public static void AddTweens(byte* camID, int* monoID, byte* fieldID, float** value, byte* valueCount, float* time, float* duration, byte* easeID, int length){
            
            for (var i = 0; i < length; ++i){
                AddTween(camID[i], monoID[i], fieldID[i], value[i], valueCount[i], time[i], duration[i], easeID[i]);
            }
            
        }
        [UnmanagedCallersOnly(EntryPoint = "AddTween")]
        public static void AddTweenProxy(int camID, int monoID, int fieldID, float* value, int valueCount, float time, float duration, int easeID){
            AddTween(Convert.ToByte(camID), monoID, Convert.ToByte(fieldID), value, Convert.ToByte(valueCount), time, duration, Convert.ToByte(easeID));
        }
        [UnmanagedCallersOnly(EntryPoint = "SetCurrentTime")]
        public static void SetCurrentTime(float time){
            Time.deltaTime = (time-currentTime)/1000f;
            currentTime = time;
        }

        [UnmanagedCallersOnly(EntryPoint = "DoAllTween")]
        public static void DoAllTween(){
            for (int camID = 0; camID < camOptions.Count; camID++){
                Camera cam = camOptions[camID];
                foreach(var kvp in cam.components){
                    Dictionary<int, Tween> tweens = kvp.Value.tweens;
                    foreach (var ivt in tweens){
                        if (!kvp.Value.enabled && currentTime > 0){ //초기 세팅 시 tween이 적용되지 않는 문제 -> 초기 세팅 시 currentTime = -1
                            tweens.Remove(ivt.Key);
                        }
                        else{
                            ivt.Value.Update();
                            if (ivt.Value.ended){
                                tweens.Remove(ivt.Key);
                            }
                        }
                    }
                }
            }
        }
        public static int width = -1;
        public static int height = -1;
        public static ScreenTexture pingpong0 = null;
        public static ScreenTexture pingpong1 = null;

        public static bool pingpingpongpong00 = true;


        public static bool sizinping = false;
        [UnmanagedCallersOnly(EntryPoint = "zajispongpongs")]
        public static void zajispongpongs(int fbi, int tex, int fbi2, int tex2){
            
            ScreenTexture rt = new ScreenTexture(fbi, tex);
            ScreenTexture rt2 = new ScreenTexture(fbi2, tex2);
            providePingpingsPongzis(rt, rt2);
        }
        public static void providePingpingsPongzis(ScreenTexture rt, ScreenTexture rt2){
            pingpong0 = rt;
            pingpong1 = rt2;
            sizinping = true;
        }
        [UnmanagedCallersOnly(EntryPoint = "SetScreenSize")]
        public static void SetScreenSize(int _width, int _height){
            width = _width;
            height = _height;
        }
        
        [UnmanagedCallersOnly(EntryPoint = "ReRender")]
        public static void ReRender(int fbi, int tex, int fbi2, int tex2, int export, int continu){
            if (!sizinping) return;

            if (currentCam == null) return;

            ScreenTexture src = new ScreenTexture(fbi, tex);
            ScreenTexture dst = new ScreenTexture(fbi2, tex2);
            
            // Material material = new Material(Shader.Find("CameraFilterPack/BlurTiltShift_Hole"));
            // material.SetFloat("_TimeX", 0);
            // material.SetFloat("_Amount", 4);
            // material.SetFloat("_Value1", 0.75);
            // material.SetFloat("_Value2", 0.75);
            // material.SetFloat("_Value3", 0.5);
            // material.SetFloat("_Value4", 0.5);
            // RenderTexture buffer = RenderTexture.GetTemporary(width/3, height/3, 0);
            // RenderTexture buffer2 = RenderTexture.GetTemporary(width/3, height/3, 0);
            // // material.SetTexture("_MainTex2", buffer2);

            // buffer.filterMode=FilterMode.Trilinear;
            // Graphics.Blit(src, buffer, material, 2);
            // RenderTexture.ReleaseTemporary(buffer);
            // RenderTexture.ReleaseTemporary(buffer2);

            // Graphics.Blit(buffer, dst);
        
            currentCam.reRender(src, dst, export == 1, continu == 1); 

            RenderTexture.markDeleted();
        }

        [UnmanagedCallersOnly(EntryPoint = "SetPingpingPongpong")]
        public static void SetPingpingPongpong(int v){
           pingpingpongpong00 = (v == 1); 
        }
        









        
    }
}
