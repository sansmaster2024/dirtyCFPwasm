using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks.Dataflow;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using UnoWasm;
namespace UnityEngine;
	// Token: 0x0200022B RID: 555
public sealed class ExecuteInEditMode : Attribute
{
}
public sealed class HideInInspector : Attribute
{
}
public static class Mathf
{
	public static float PI{
		get{
			return (float)Math.PI;
		}
	}
	public static int FloorToInt(float f)
	{
		return Convert.ToInt32(Math.Floor(f));
	}
	public static int CeilToInt(float f)
	{
		return Convert.ToInt32(Math.Ceiling(f));
	}
	public static double Sqrt(double f)
	{
		return Math.Sqrt(f);
	}
	public static float Sqrt(float f)
	{
		return (float)Math.Sqrt((double)f);
	}
	public static float Pow(float a, float b){
		return (float)Math.Pow((double)a,(double)b);
	}
	public static float Sin(float f){
		return (float)Math.Sin((double)f);
	}
	public static float Cos(float f){
		return (float)Math.Cos((double)f);
	}

	public static float Pow(double a, double b){
		return (float)Math.Pow(a,b);
	}
	public static float Sin(double f){
		return (float)Math.Sin(f);
	}
	public static float Cos(double f){
		return (float)Math.Cos(f);
	}

	public static double Clamp(double f, double min, double max)
	{
		return Math.Clamp(f, min, max);
	}
	public static double Clamp01(double f)
	{
		return Math.Clamp(f, 0, 1);
	}
	public static float Round(float f)
	{
		return Convert.ToSingle(Math.Round((double)f));
	}
}
public static class Debug
{
	public static void LogWarning(string message) { }

}
public class Rotation : Vector3
{
    public Rotation(float x, float y, float z) : base(x, y, z)
    {
    }
}
public class Transform
{
	public Rotation rotation;
}
public class Camera
{
	public DepthTextureMode depthTextureMode = DepthTextureMode.None;
	public float farClipPlane = 0;
	public Texture targetTexture = null;
	public Transform transform = null;
	public Dictionary<int, MonoBehaviour> components;
	public int textureID = -1;

	public bool zazisPIngpings00 = false;
	public void reRender(RenderTexture src, RenderTexture dst, bool export, bool continu)
	{	
		ScreenTexture pingpong0 = UnoWasm.Colorify.pingpong0;
		ScreenTexture pingpong1 = UnoWasm.Colorify.pingpong1;

		// JSInterop.bindFramebufferInfo(pingpong0.fbiID);
		if (!continu){
			JSInterop.clearFBI(pingpong0.fbiID); // bind, clear
			JSInterop.drawTextureBasic(src.texID);
			UnoWasm.Colorify.pingpingpongpong00 = true;
		}
		

		foreach (var kvp in components)
		{
			MonoBehaviour comp = kvp.Value;
			if (comp.enabled){
				if (UnoWasm.Colorify.pingpingpongpong00){
					try{
						comp.method.Invoke(comp, new object[] {pingpong0, pingpong1}); //dst = pingpong1
					}
					catch(Exception e){
						
					}
					UnoWasm.Colorify.pingpingpongpong00 = false;
				}
				else{
					try{
						comp.method.Invoke(comp, new object[] {pingpong1, pingpong0}); //dst = pingpong0
					}
					catch(Exception e){
						
					}
					UnoWasm.Colorify.pingpingpongpong00 = true;
				}
			}
			
		}
		if (export){
			JSInterop.clearFBI(dst.fbiID); // bind, clear
			JSInterop.drawTextureBasic( (UnoWasm.Colorify.pingpingpongpong00 ? pingpong0 : pingpong1).texID );
		}
	}
	public Camera()
	{
        components = new Dictionary<int, MonoBehaviour>();
    }
	public void addComponent(int id, MonoBehaviour component)
	{
		components[id] = component;
	}
}
public class Tween
{
	public object obj;
	public FieldInfo field;
	public object fromValue;
	public object toValue;
	public byte valueCount;
	public float time;
	public float duration;
	public byte easeID;
	public bool ended;
	public Tween(object obj, FieldInfo field, object fromValue, object toValue, byte valueCount, float time, float duration, byte easeID){
		this.obj = obj;
		this.field = field;
		this.fromValue = fromValue;
		this.toValue = toValue;
		this.valueCount = valueCount;
		this.time = time;
		this.duration = duration;
		this.easeID = easeID;
		this.ended = false;
	}
	public void Update(){
		float currentTime = UnoWasm.Colorify.currentTime;
		if (duration == 0f){
			this.ended = true;
			this.field.SetValue(this.obj, this.toValue);
			return;
		}
		float pureProgress = (currentTime-this.time)/duration;
		this.ended = (pureProgress > 1f);
		if (this.ended){
			pureProgress = 1f;
		}
		pureProgress = EaseManager.ease(pureProgress, this.easeID);
		this.field.SetValue(this.obj, EaseManager.Lerp(this.fromValue, this.toValue, pureProgress, this.valueCount));
	}
	public void hurryUpBeach(){
		this.field.SetValue(this.obj, this.toValue);
	}
}
public static class Flash
{
	public static float Ease(float time, float duration, float overshootOrAmplitude, float period)
	{
		int num = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
		float num2 = duration / overshootOrAmplitude;
		time -= num2 * (float)(num - 1);
		float num3 = (float)((num % 2 != 0) ? 1 : -1);
		if (num3 < 0f)
		{
			time -= num2;
		}
		float res = time * num3 / num2;
		return Flash.WeightedEase(overshootOrAmplitude, period, num, num2, num3, res);
	}

	public static float EaseIn(float time, float duration, float overshootOrAmplitude, float period)
	{
		int num = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
		float num2 = duration / overshootOrAmplitude;
		time -= num2 * (float)(num - 1);
		float num3 = (float)((num % 2 != 0) ? 1 : -1);
		if (num3 < 0f)
		{
			time -= num2;
		}
		time *= num3;
		float res = (time /= num2) * time;
		return Flash.WeightedEase(overshootOrAmplitude, period, num, num2, num3, res);
	}

	public static float EaseOut(float time, float duration, float overshootOrAmplitude, float period)
	{
		int num = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
		float num2 = duration / overshootOrAmplitude;
		time -= num2 * (float)(num - 1);
		float num3 = (float)((num % 2 != 0) ? 1 : -1);
		if (num3 < 0f)
		{
			time -= num2;
		}
		time *= num3;
		float res = -(time /= num2) * (time - 2f);
		return Flash.WeightedEase(overshootOrAmplitude, period, num, num2, num3, res);
	}

	public static float EaseInOut(float time, float duration, float overshootOrAmplitude, float period)
	{
		int num = Mathf.CeilToInt(time / duration * overshootOrAmplitude);
		float num2 = duration / overshootOrAmplitude;
		time -= num2 * (float)(num - 1);
		float num3 = (float)((num % 2 != 0) ? 1 : -1);
		if (num3 < 0f)
		{
			time -= num2;
		}
		time *= num3;
		float res = ((time /= num2 * 0.5f) < 1f) ? (0.5f * time * time) : (-0.5f * ((time -= 1f) * (time - 2f) - 1f));
		return Flash.WeightedEase(overshootOrAmplitude, period, num, num2, num3, res);
	}

	private static float WeightedEase(float overshootOrAmplitude, float period, int stepIndex, float stepDuration, float dir, float res)
	{
		float num = 0f;
		float num2 = 0f;
		if (dir > 0f && (int)overshootOrAmplitude % 2 == 0)
		{
			stepIndex++;
		}
		else if (dir < 0f && (int)overshootOrAmplitude % 2 != 0)
		{
			stepIndex++;
		}
		if (period > 0f)
		{
			float num3 = (float)Math.Truncate((double)overshootOrAmplitude);
			num2 = overshootOrAmplitude - num3;
			if (num3 % 2f > 0f)
			{
				num2 = 1f - num2;
			}
			num2 = num2 * (float)stepIndex / overshootOrAmplitude;
			num = res * (overshootOrAmplitude - (float)stepIndex) / overshootOrAmplitude;
		}
		else if (period < 0f)
		{
			period = -period;
			num = res * (float)stepIndex / overshootOrAmplitude;
		}
		float num4 = num - res;
		res += num4 * period + num2;
		if (res > 1f)
		{
			res = 1f;
		}
		return res;
	}
}

public class EaseManager{
	public static float Lerp(float fromValue, float toValue, float progress){
		return ((toValue-fromValue)*progress) + fromValue;//-*+
	}
	public static object Lerp(object fromValue, object toValue, float progress, byte valueCount){
		switch (valueCount){
			case 1:
				float f = Convert.ToSingle(fromValue);
				float t = Convert.ToSingle(toValue);
				return ((t-f)*progress) + f;
			case 2:
				Vector2 f2 = (Vector2)fromValue;
				Vector2 t2 = (Vector2)toValue;
				return ((t2-f2)*progress) + f2;
			case 3:
				Vector3 f3 = (Vector3)fromValue;
				Vector3 t3 = (Vector3)toValue;
				return ((t3-f3)*progress) + f3;
			case 4:
				Vector4 f4 = (Vector4)fromValue;
				Vector4 t4 = (Vector4)toValue;
				return ((t4-f4)*progress) + f4;
			case 5:
				Color f5 = (Color)fromValue;
				Color t5 = (Color)toValue;
				return ((t5-f5)*progress) + f5;
			case 6:
				float f6 = Convert.ToSingle(fromValue);
				float t6 = Convert.ToSingle(toValue);
				return (int) Math.Round((double)(((t6-f6)*progress) + f6));
		}
		return 0f;
	}
	private static float easeOutBounce(float x){
		float easedValue = x;
		if (easedValue < 1/2.75){
			easedValue = 7.5625f*easedValue*easedValue;
		}
		else if (easedValue < 2/2.75){
			easedValue += (-1.5f)/2.75f;
			easedValue = 7.5625f*easedValue*easedValue + 0.75f;
		}
		else if (easedValue < 2.5/2.75){
			easedValue += (-2.25f)/2.75f;
			easedValue = 7.5625f*easedValue*easedValue + 0.9375f;
		}
		else{
			easedValue += (-2.625f)/2.75f;
			easedValue = 7.5625f*easedValue*easedValue + 0.984375f;
		}
		return easedValue;
	}
	
	public static float ease(float value, byte easeID){
		float tmp;
		float tmp2;
		switch(easeID){
			case 0:
				return value;
			case 1:
				return 1-Mathf.Cos(value*Mathf.PI/2);
			case 2:
				return Mathf.Sin(value*Mathf.PI/2);
			case 3:
				return (1-Mathf.Cos(value*Mathf.PI))/2;
			case 4:
				return Mathf.Pow(value, 2); 
			case 5:
				return value*(2-value);
			case 6:
				return (value < 0.5) ? 2*value*value : 1-(tmp = (-2*value)+2)*tmp/2;
			case 7:
				return Mathf.Pow(value, 3);  
			case 8:
				return (tmp = value-1)*tmp*tmp+1;
			case 9:
				return (value < 0.5) ? 4*Mathf.Pow(value, 3) : (tmp = 2*value-2)*tmp*tmp/2 + 1; 
			case 10:
				return Mathf.Pow(value, 4); 
			case 11:
				return 1-(tmp = value-1)*tmp*tmp*tmp;
			case 12:
				return (float)((value < 0.5) ? 8*(tmp = value*value)*tmp : (tmp = 2*value-2)*tmp*tmp*tmp*(-0.5) + 1);
			case 13:
				return Mathf.Pow(value, 5); 
			case 14:
				return Mathf.Pow(value-1, 5) + 1;  
			case 15:
				return (float)((value < 0.5) ? 16 * Mathf.Pow(value, 5) : Mathf.Pow(2*value-2, 5) * 0.5 + 1);  
			case 16:
				return value == 0 ? 0 : Mathf.Pow(2, 10*value-10);
			case 17:
				return (value == 1) ? 1 : 1-Mathf.Pow(2, -10*value);
			case 18:
				return (value == 0) ? 0 : ((value == 1) ? 1 : (
					(value < 0.5) ? Mathf.Pow(2, 20*value-10)/2 : 1-Mathf.Pow(2, -20*value+10)/2
				));
			case 19:
				return 1-Mathf.Sqrt(1-Mathf.Pow(value, 2));  
			case 20:
				return Mathf.Sqrt(1-Mathf.Pow(value-1, 2));  
			case 21:
				return (float)((value < 0.5) ? (1-Mathf.Sqrt(1-4*value*value))*0.5 : (Mathf.Sqrt(1-Mathf.Pow(-2*value+2, 2))+1)*0.5);
			case 22:
				return (float) (2.70158*Mathf.Pow(value, 3) - 1.70158*Mathf.Pow(value, 2));  
			case 23:
				tmp = value-1;
				tmp2 = Mathf.Pow(tmp, 2); 
				tmp =  Mathf.Pow(tmp, 3); 
				return (float)(1+2.70158*tmp+1.70158*tmp2);
			case 24:
				return (float)((value < 0.5) ? (tmp = 2*value)*tmp*(tmp*3.59491-2.59491)/2 : ((tmp = 2*value-2)*tmp*(3.59491*tmp+2.59491)+2)/2);
			case 25:
				return (float)((value == 0) ? 0 : ((value == 1) ? 1 : (
					(-1.70158*Mathf.Pow(2, 10*(value-1)))*Mathf.Sin((value-1.03)*20.94395)
				)));
			case 26:
				return (float)((value == 0) ? 0 : ((value == 1) ? 1 : (
					(1.70158*Mathf.Pow(2, -10*value))*Mathf.Sin((value-0.03)*20.94395) + 1
				)));
			case 27:
				tmp = value*2 - 1;
				if (value == 0) return 0f;
				if (value == 1) return 1f;
				return (float)((value < 0.5) ? 
					Mathf.Pow(2, 10*tmp)*Mathf.Sin((tmp-0.044991)*13.962634) * (-0.85079) :
					Mathf.Pow(2, -10*tmp)*Mathf.Sin((tmp-0.044991)*13.962634)*0.85079 + 1);
			case 28:
				return 1-easeOutBounce(1-value);
			case 29:
				return easeOutBounce(value);
			case 30:
				return (value < 0.5) ? (1-easeOutBounce(1-2*value))/2 : (1+easeOutBounce(2*value-1))/2;
			case 31:
				return Flash.Ease(value, 1, 1, 0);
			case 32:
				return Flash.EaseIn(value, 1, 1, 0);
			case 33:
				return Flash.EaseOut(value, 1, 1, 0);
			case 34:
				return Flash.EaseInOut(value, 1, 1, 0);
		}
		return value;
	}

}
public class MonoBehaviour
{
	
	public MethodInfo method = null;
	public FieldInfo[] fields = null;
	public Dictionary<int, Tween> tweens = null;

	public bool enabled = false;
	public T GetComponent<T>(){
		return default(T);
	}
	public void DestroyImmediate(object obj)
	{

	}
	public void setEnabled(bool v){
		this.enabled = v;
	}
	public static implicit operator bool(MonoBehaviour exists)
	{
		return true;
	}
	public MonoBehaviour(){
		
	}
}

public sealed class AddComponentMenu : Attribute
{
	public AddComponentMenu(string menuName)
	{
	}

	public AddComponentMenu(string menuName, int order)
	{

	}
}

public unsafe class Shader : MonoBehaviour
{
	
	public int id;
	public Shader(int id){
		this.id = id;
	}
	public static Shader Find(string name)
	{
		byte[] res = System.Text.Encoding.UTF8.GetBytes(name);
		fixed(byte* ptr = res){
			return new Shader(JSInterop.findShader(ptr));
		}
	}
}
public class Material : MonoBehaviour
{
	public HideFlags hideFlags = HideFlags.None;

	public Dictionary<string, object> uniforms;
	public int shaderID;
	public Material(Shader shader)
	{
		shaderID = shader.id;
		uniforms = new Dictionary<string, object>();
	}
	public void SetInt(string name, int value) 
	{ 
		uniforms[name] = Convert.ToSingle(value);
	}
	public void SetFloat(string name, float value) 
	{
        uniforms[name] = value;
    }
    public void SetFloat(string name, double value)
	{
        uniforms[name] = Convert.ToSingle(value);
    }
    public void SetVector(string name, Vector2 value)
	{
        uniforms[name] = value;
    }
    public void SetVector(string name, Vector3 value)
    {
        uniforms[name] = value;
    }
    public void SetVector(string name, Vector4 value)
    {
        uniforms[name] = value;
    }
    public void SetColor(string name, Color value)
	{
        uniforms[name] = value;
    }
    public void SetTexture(string name, Texture value)
	{
        uniforms[name] = value;
    }
	
}
public static class QualitySettings
{
	public static ColorSpace activeColorSpace = ColorSpace.Linear;
}

public static class SystemInfo
{
	public static bool supportsImageEffects = true;
	public static bool supports3DTextures = true;

}
public static class Application
{
	public static bool isPlaying = true;
}
public class Time
{
	public static float deltaTime = 0;
	public static int realtimeSinceStartup = 0;
	public static int timeScale = 1;
}
public class Vector
{

}
public class Vector4 : Vector
{
	public float x;
	public float y;
	public float z;
	public float w;
	public Vector4(float x, float y, float z = 0.0f, float w = 0.0f)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.w = w;
	}
	public Vector4(float x)
	{
		this.x = x;
		this.y = x;
		this.z = x;
		this.w = x;
	}

	public static Vector4 operator +(Vector4 a, Vector4 b){
		return new Vector4(a.x+b.x, a.y+b.y, a.z+b.z, a.w+b.w);
	}
	public static Vector4 operator -(Vector4 a, Vector4 b){
		return new Vector4(a.x-b.x, a.y-b.y, a.z-b.z, a.w-b.w);
	}
	public static Vector4 operator *(Vector4 a, Vector4 b){
		return new Vector4(a.x*b.x, a.y*b.y, a.z*b.z, a.w*b.w);
	}
	public static Vector4 operator +(Vector4 a, float b){
		return new Vector4(a.x+b, a.y+b, a.z+b, a.w+b);
	}
	public static Vector4 operator -(Vector4 a, float b){
		return new Vector4(a.x-b, a.y-b, a.z-b, a.w-b);
	}
	public static Vector4 operator *(Vector4 a, float b){
		return new Vector4(a.x*b, a.y*b, a.z*b, a.w*b);
	}
}
public class Vector3 : Vector
{
	public float x;
	public float y;
	public float z;
	public Vector3(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public Vector3(float x)
	{
		this.x = x;
		this.y = x;
		this.z = x;
	}
	public static Vector3 operator +(Vector3 a, Vector3 b){
		return new Vector3(a.x+b.x, a.y+b.y, a.z+b.z);
	}
	public static Vector3 operator -(Vector3 a, Vector3 b){
		return new Vector3(a.x-b.x, a.y-b.y, a.z-b.z);
	}
	public static Vector3 operator *(Vector3 a, Vector3 b){
		return new Vector3(a.x*b.x, a.y*b.y, a.z*b.z);
	}
	public static Vector3 operator +(Vector3 a, float b){
		return new Vector3(a.x+b, a.y+b, a.z+b);
	}
	public static Vector3 operator -(Vector3 a, float b){
		return new Vector3(a.x-b, a.y-b, a.z-b);
	}
	public static Vector3 operator *(Vector3 a, float b){
		return new Vector3(a.x*b, a.y*b, a.z*b);
	}
}
public class Vector2 : Vector
{
	public float x;
	public float y;
	public Vector2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}
	public Vector2(float x)
	{
		this.x = x;
		this.y = x;
	}
	public static Vector2 operator +(Vector2 a, Vector2 b){
		return new Vector2(a.x+b.x, a.y+b.y);
	}
	public static Vector2 operator -(Vector2 a, Vector2 b){
		return new Vector2(a.x-b.x, a.y-b.y);
	}
	public static Vector2 operator *(Vector2 a, Vector2 b){
		return new Vector2(a.x*b.x, a.y*b.y);
	}
	public static Vector2 operator +(Vector2 a, float b){
		return new Vector2(a.x+b, a.y+b);
	}
	public static Vector2 operator -(Vector2 a, float b){
		return new Vector2(a.x-b, a.y-b);
	}
	public static Vector2 operator *(Vector2 a, float b){
		return new Vector2(a.x*b, a.y*b);
	}
}
public class Color : Vector4
{
    public Color(float x, float y, float z, float w = 1.0f) : base(x, y, z, w)
    {
    }
	public static Color red = new Color(1, 0, 0, 1);
	public static Color operator +(Color a, Color b){
		return new Color(a.x+b.x, a.y+b.y, a.z+b.z, a.w+b.w);
	}
	public static Color operator -(Color a, Color b){
		return new Color(a.x-b.x, a.y-b.y, a.z-b.z, a.w-b.w);
	}
	public static Color operator *(Color a, Color b){
		return new Color(a.x*b.x, a.y*b.y, a.z*b.z, a.w*b.w);
	}
	public static Color operator +(Color a, float b){
		return new Color(a.x+b, a.y+b, a.z+b, a.w+b);
	}
	public static Color operator -(Color a, float b){
		return new Color(a.x-b, a.y-b, a.z-b, a.w-b);
	}
	public static Color operator *(Color a, float b){
		return new Color(a.x*b, a.y*b, a.z*b, a.w*b);
	}
}
// Token: 0x02000210 RID: 528
public sealed class RangeAttribute : Attribute
{
	// Token: 0x060017A1 RID: 6049 RVA: 0x000274EF File Offset: 0x000256EF
	public RangeAttribute(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	// Token: 0x0400086D RID: 2157
	public readonly float min;

	// Token: 0x0400086E RID: 2158
	public readonly float max;
}
public enum HideFlags
{
	None = 0,
	HideInHierarchy = 1,
	HideInInspector = 2,
	DontSaveInEditor = 4,
	NotEditable = 8,
	DontSaveInBuild = 16,
	DontUnloadUnusedAsset = 32,
	DontSave = 52,
	HideAndDontSave = 61
}

public enum DepthTextureMode
{
	None = 0,
	Depth = 1,
	DepthNormals = 2,
	MotionVectors = 4
}
public class ScreenTexture : RenderTexture
{
	public override int width{
		get{return UnoWasm.Colorify.width;}
	}
	public override int height{
		get{return UnoWasm.Colorify.height;}
	}
	public ScreenTexture(int fbiID, int texID){
		this.fbiID = fbiID;
		this.texID = texID;
	}
}
public unsafe class RenderTexture : Texture
{
	public int _width = -1;
	public int _height = -1;
	private static List<RenderTexture> deleted = new List<RenderTexture>();
	public virtual int width{
		get{return _width;}
	}
	public virtual int height{
		get{return _height;}
	}
	public static RenderTexture active;
	public RenderTexture (int width, int height, int what = 0)
	{
		this._width = width;
		this._height = height;
		int* ids = JSInterop.createFBI(width, height);
		this.texID = ids[0];
		this.fbiID = ids[1];
	}
	public RenderTexture ()
	{
	}
	public static RenderTexture GetTemporary(int w, int h, int what/*int depthBuffer*/)
	{
		return new RenderTexture(w, h, what);
	}
	public static void ReleaseTemporary(RenderTexture rt)
	{
		JSInterop.deleteFBI(rt.fbiID);
		RenderTexture.deleted.Add(rt);
    }
	public static void markDeleted()
    {
        foreach (RenderTexture rt in RenderTexture.deleted)
        {
            rt.isDeleted = true;
        }
        RenderTexture.deleted.Clear();
    }
}
public class Rect : Vector4
{
    public Rect(float x, float y, float z, float w) : base(x, y, z, w)
    {
    }
}
public class Texture : MonoBehaviour
{
	public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
	public FilterMode filterMode = FilterMode.Bilinear;
	public bool isDeleted = false;
    public Texture(){
		
	}

	public int texID = 0;
	public int fbiID = 0;

	public Color[] GetPixels()
	{
		return null;
	}
	public void ReadPixels(Rect r, int x, int y)
	{

	}
	public void SetPixels(Color[] pixels)
	{

	}
	public void Apply()
	{

	}
}
public class Texture2D : Texture
{
	public string name;
	public int width;
	public int height;
	public Texture2D(int width, int height)
    {
        this.width = width;
        this.height = height;
    }
	public Texture2D()
    {
    }
}
public class Texture3D : Texture
{
	public int x;
	public int y;
	public int z;

	public Texture3D(int x, int y, int z, TextureFormat f, bool what)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
	

}
public static unsafe class Resources
{
	public static Texture2D Load(string path)
	{
		byte[] bytes = System.Text.Encoding.UTF8.GetBytes(path);
		int id = 0;
		fixed(byte* ptr = bytes){
			id = JSInterop.findImage(ptr);
		}
		Texture2D t = new Texture2D();
		t.texID = id;
		UnoWasm.Colorify.Log(id.ToString() + " tex loaded!");
		return t;
	}
}
public unsafe class Graphics
{
	public static void Blit(RenderTexture src, RenderTexture dst, Material mat = null, int pass = 0)
	{
		JSInterop.clearFBI(dst.fbiID); //bind, clear
		int id = -1;
        if (mat != null)
		{
			id = mat.shaderID;
			int len = mat.uniforms.Count;

			string uniformKeys = "";
			byte[] uniformValueCounts = new byte[len];
			float[][] uniformValues = new float[len][];


			int i = 0;
			foreach(var kvp in mat.uniforms){
				object value = kvp.Value;
				if (value == null) {
					UnoWasm.Colorify.Log(kvp.Key);
					continue;
				}
				
				float[] valueArray = null;
				byte valueCount = 1;
				if (value is Vector){

					if (value is Vector2){
						Vector2 vec2 = (value as Vector2);
						valueCount = 2;
						valueArray = new float[2]{vec2.x, vec2.y};
					}
					else if (value is Vector3){
						Vector3 vec3 = (value as Vector3);
						valueCount = 3;
						valueArray = new float[3]{vec3.x, vec3.y, vec3.z};

					}
					else /*if (value is Vector4)*/{
						Vector4 vec4 = (value as Vector4);
						valueCount = 4;
						valueArray = new float[4]{vec4.x, vec4.y, vec4.z, vec4.w};
					}

				}
				else{
					if (value is Texture){
						if ((value as Texture).isDeleted) continue;
                        valueCount = 0;
						valueArray = new float[1]{(value as Texture).texID};
					}
					else {
						// valueCount = 1;
						valueArray = new float[1]{Convert.ToSingle(value)};
					}
					
				}
				if (valueArray == null) continue;


				uniformValueCounts[i] = valueCount;
				uniformValues[i] = valueArray;


				uniformKeys += (kvp.Key+"|");
				
				++i;
			}
			len = i;
			
			byte[] keybytes = System.Text.Encoding.UTF8.GetBytes(uniformKeys);
			fixed(byte* UKey = keybytes){
                fixed(float[]* UValue = uniformValues){
					fixed(byte* UValueCount = uniformValueCounts){
						JSInterop.drawTexture(src.texID, id, UKey, UValue, UValueCount, len, pass);
					}
				}
            }
			return;
        }

		JSInterop.drawTextureBasic(src.texID);
	}
}
public class Screen
{
	public static int width{
		get{return UnoWasm.Colorify.width;}
	}
	public static int height{
		get{return UnoWasm.Colorify.height;}
	}
}
public enum TextureFormat
{
	// Token: 0x0400055B RID: 1371
	Alpha8 = 1,
	// Token: 0x0400055C RID: 1372
	ARGB4444,
	// Token: 0x0400055D RID: 1373
	RGB24,
	// Token: 0x0400055E RID: 1374
	RGBA32,
	// Token: 0x0400055F RID: 1375
	ARGB32,
	// Token: 0x04000560 RID: 1376
	RGB565 = 7,
	// Token: 0x04000561 RID: 1377
	R16 = 9,
	// Token: 0x04000562 RID: 1378
	DXT1,
	// Token: 0x04000563 RID: 1379
	DXT5 = 12,
	// Token: 0x04000564 RID: 1380
	RGBA4444,
	// Token: 0x04000565 RID: 1381
	BGRA32,
	// Token: 0x04000566 RID: 1382
	RHalf,
	// Token: 0x04000567 RID: 1383
	RGHalf,
	// Token: 0x04000568 RID: 1384
	RGBAHalf,
	// Token: 0x04000569 RID: 1385
	RFloat,
	// Token: 0x0400056A RID: 1386
	RGFloat,
	// Token: 0x0400056B RID: 1387
	RGBAFloat,
	// Token: 0x0400056C RID: 1388
	YUY2,
	// Token: 0x0400056D RID: 1389
	RGB9e5Float,
	// Token: 0x0400056E RID: 1390
	BC4 = 26,
	// Token: 0x0400056F RID: 1391
	BC5,
	// Token: 0x04000570 RID: 1392
	BC6H = 24,
	// Token: 0x04000571 RID: 1393
	BC7,
	// Token: 0x04000572 RID: 1394
	DXT1Crunched = 28,
	// Token: 0x04000573 RID: 1395
	DXT5Crunched,
	// Token: 0x04000574 RID: 1396
	PVRTC_RGB2,
	// Token: 0x04000575 RID: 1397
	PVRTC_RGBA2,
	// Token: 0x04000576 RID: 1398
	PVRTC_RGB4,
	// Token: 0x04000577 RID: 1399
	PVRTC_RGBA4,
	// Token: 0x04000578 RID: 1400
	ETC_RGB4,
	// Token: 0x04000579 RID: 1401
	EAC_R = 41,
	// Token: 0x0400057A RID: 1402
	EAC_R_SIGNED,
	// Token: 0x0400057B RID: 1403
	EAC_RG,
	// Token: 0x0400057C RID: 1404
	EAC_RG_SIGNED,
	// Token: 0x0400057D RID: 1405
	ETC2_RGB,
	// Token: 0x0400057E RID: 1406
	ETC2_RGBA1,
	// Token: 0x0400057F RID: 1407
	ETC2_RGBA8,
	// Token: 0x04000580 RID: 1408
	ASTC_4x4,
	// Token: 0x04000581 RID: 1409
	ASTC_5x5,
	// Token: 0x04000582 RID: 1410
	ASTC_6x6,
	// Token: 0x04000583 RID: 1411
	ASTC_8x8,
	// Token: 0x04000584 RID: 1412
	ASTC_10x10,
	// Token: 0x04000585 RID: 1413
	ASTC_12x12,
	// Token: 0x04000586 RID: 1414
	[Obsolete("Nintendo 3DS is no longer supported.")]
	ETC_RGB4_3DS = 60,
	// Token: 0x04000587 RID: 1415
	[Obsolete("Nintendo 3DS is no longer supported.")]
	ETC_RGBA8_3DS,
	// Token: 0x04000588 RID: 1416
	RG16,
	// Token: 0x04000589 RID: 1417
	R8,
	// Token: 0x0400058A RID: 1418
	ETC_RGB4Crunched,
	// Token: 0x0400058B RID: 1419
	ETC2_RGBA8Crunched,
	// Token: 0x0400058C RID: 1420
	ASTC_HDR_4x4,
	// Token: 0x0400058D RID: 1421
	ASTC_HDR_5x5,
	// Token: 0x0400058E RID: 1422
	ASTC_HDR_6x6,
	// Token: 0x0400058F RID: 1423
	ASTC_HDR_8x8,
	// Token: 0x04000590 RID: 1424
	ASTC_HDR_10x10,
	// Token: 0x04000591 RID: 1425
	ASTC_HDR_12x12,
	// Token: 0x04000592 RID: 1426
	RG32,
	// Token: 0x04000593 RID: 1427
	RGB48,
	// Token: 0x04000594 RID: 1428
	RGBA64,
	// Token: 0x04000595 RID: 1429
	[Obsolete("Enum member TextureFormat.ASTC_RGB_4x4 has been deprecated. Use ASTC_4x4 instead (UnityUpgradable) -> ASTC_4x4")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	ASTC_RGB_4x4 = 48,
	// Token: 0x04000596 RID: 1430
	[Obsolete("Enum member TextureFormat.ASTC_RGB_5x5 has been deprecated. Use ASTC_5x5 instead (UnityUpgradable) -> ASTC_5x5")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	ASTC_RGB_5x5,
	// Token: 0x04000597 RID: 1431
	[Obsolete("Enum member TextureFormat.ASTC_RGB_6x6 has been deprecated. Use ASTC_6x6 instead (UnityUpgradable) -> ASTC_6x6")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	ASTC_RGB_6x6,
	// Token: 0x04000598 RID: 1432
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Enum member TextureFormat.ASTC_RGB_8x8 has been deprecated. Use ASTC_8x8 instead (UnityUpgradable) -> ASTC_8x8")]
	ASTC_RGB_8x8,
	// Token: 0x04000599 RID: 1433
	[Obsolete("Enum member TextureFormat.ASTC_RGB_10x10 has been deprecated. Use ASTC_10x10 instead (UnityUpgradable) -> ASTC_10x10")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	ASTC_RGB_10x10,
	// Token: 0x0400059A RID: 1434
	[Obsolete("Enum member TextureFormat.ASTC_RGB_12x12 has been deprecated. Use ASTC_12x12 instead (UnityUpgradable) -> ASTC_12x12")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	ASTC_RGB_12x12,
	// Token: 0x0400059B RID: 1435
	[Obsolete("Enum member TextureFormat.ASTC_RGBA_4x4 has been deprecated. Use ASTC_4x4 instead (UnityUpgradable) -> ASTC_4x4")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	ASTC_RGBA_4x4,
	// Token: 0x0400059C RID: 1436
	[Obsolete("Enum member TextureFormat.ASTC_RGBA_5x5 has been deprecated. Use ASTC_5x5 instead (UnityUpgradable) -> ASTC_5x5")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	ASTC_RGBA_5x5,
	// Token: 0x0400059D RID: 1437
	[Obsolete("Enum member TextureFormat.ASTC_RGBA_6x6 has been deprecated. Use ASTC_6x6 instead (UnityUpgradable) -> ASTC_6x6")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	ASTC_RGBA_6x6,
	// Token: 0x0400059E RID: 1438
	[Obsolete("Enum member TextureFormat.ASTC_RGBA_8x8 has been deprecated. Use ASTC_8x8 instead (UnityUpgradable) -> ASTC_8x8")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	ASTC_RGBA_8x8,
	// Token: 0x0400059F RID: 1439
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Enum member TextureFormat.ASTC_RGBA_10x10 has been deprecated. Use ASTC_10x10 instead (UnityUpgradable) -> ASTC_10x10")]
	ASTC_RGBA_10x10,
	// Token: 0x040005A0 RID: 1440
	[Obsolete("Enum member TextureFormat.ASTC_RGBA_12x12 has been deprecated. Use ASTC_12x12 instead (UnityUpgradable) -> ASTC_12x12")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	ASTC_RGBA_12x12
}
public enum TextureWrapMode
{
	// Token: 0x04000552 RID: 1362
	Repeat,
	// Token: 0x04000553 RID: 1363
	Clamp,
	// Token: 0x04000554 RID: 1364
	Mirror,
	// Token: 0x04000555 RID: 1365
	MirrorOnce
}
public enum ColorSpace
{
	// Token: 0x0400052B RID: 1323
	Uninitialized = -1,
	// Token: 0x0400052C RID: 1324
	Gamma,
	// Token: 0x0400052D RID: 1325
	Linear
}
public enum FilterMode
{
	// Token: 0x0400054E RID: 1358
	Point,
	// Token: 0x0400054F RID: 1359
	Bilinear,
	// Token: 0x04000550 RID: 1360
	Trilinear
}
public class YieldInstruction
{
}
public sealed class WaitForSeconds : YieldInstruction
{
	// Token: 0x06001A0A RID: 6666 RVA: 0x0002BF8C File Offset: 0x0002A18C
	public WaitForSeconds(float seconds)
	{
		this.m_Seconds = seconds;
	}

	// Token: 0x04000905 RID: 2309
	internal float m_Seconds;
}
