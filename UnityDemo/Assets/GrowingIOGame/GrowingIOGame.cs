using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class GrowingIOGame {
#if UNITY_ANDROID

    private static string ANDROID_CLASS = "com.growingio.android.plugin.game.GrowingIOGame";

    private static AndroidJavaObject dicToMap(Dictionary<string, object> dictionary) {
        if (dictionary == null) {
            return null;
        }

        AndroidJavaObject map = new AndroidJavaObject("java.util.HashMap");
        foreach (KeyValuePair<string, object> pair in dictionary) {
            object value;
            if (pair.Value is string) {
                value = pair.Value;
            }
            else if (pair.Value is bool) {
                value = new AndroidJavaObject("java.lang.Boolean", pair.Value);
            }
            else if (pair.Value is ValueType) {
                value = new AndroidJavaObject("java.lang.Double", pair.Value.ToString());
            }
            else {
                value = pair.Value.ToString();
            }

            map.Call<AndroidJavaClass>("put", pair.Key, value);
        }

        return map;
    }

#endif

#if UNITY_IPHONE
        /* Interface to native implementation */

    [DllImport("__Internal")]
    private static extern void gioTrack(string eventId);

    [DllImport("__Internal")]
    private static extern void gioTrackWithNumber(string eventId, double number);

    [DllImport("__Internal")]
    private static extern void gioTrackWithNumberAndVariable(string eventId, double number, string[] keys, string[] stringValues, double[] numberValues, int count);

    [DllImport("__Internal")]
    private static extern void gioTrackWithVariable(string eventId, string[] keys, string[] stringValues, double[] numberValues, int count);

    [DllImport("__Internal")]
    private static extern void gioSetEvar(string[] keys, string[] stringValues, double[] numberValues, int count);

    [DllImport("__Internal")]
    private static extern void gioSetEvarWithKeyAndString(string key, string stringValue);

    [DllImport("__Internal")]
    private static extern void gioSetEvarWithKeyAndNumber(string key, double number);

    [DllImport("__Internal")]
    private static extern void gioSetPeople(string[] keys, string[] stringValues, double[] numberValues, int count);

    [DllImport("__Internal")]
    private static extern void gioSetPeopleWithKeyAndString(string key, string stringValue);

    [DllImport("__Internal")]
    private static extern void gioSetPeopleWithKeyAndNumber(string key, double number);

    [DllImport("__Internal")]
    private static extern void gioSetVistor(string[] keys, string[] stringValues, double[] numberValues, int count);

    [DllImport("__Internal")]
    private static extern void gioSetUserId(string userId);

    [DllImport("__Internal")]
    private static extern void gioClearUserId();

    private class GIOIOSObject {
        public string[] keys;
        public string[] values;
        public double[] numbers;
        
    }

    private static GIOIOSObject DicToObject(Dictionary<string, object> variable) {
        GIOIOSObject gioObject = new GIOIOSObject();
        gioObject.keys = new String[variable.Count];
        gioObject.values = new String[variable.Count];
        gioObject.numbers = new double[variable.Count];
        int index = 0;
        if (variable != null && variable.Count > 0) {
            foreach (KeyValuePair<string, object> kvp in variable) {
                gioObject.keys[index] = kvp.Key;
                if (kvp.Value is string) {
                    gioObject.values[index] = (string)kvp.Value;
                }
                else if (kvp.Value is ValueType) {
                    gioObject.numbers[index] = System.Convert.ToDouble(kvp.Value);
                }
                else {
                    gioObject.values[index] = System.Convert.ToString(kvp.Value);
                }
                index++;
            }
        }
        return gioObject;
    }

#endif

    public static void Track(string eventId) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
                gioTrack(eventId);
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("track", eventId);
#endif
        }
    }

    public static void Track(string eventId, double number) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
            gioTrackWithNumber(eventId, number);
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("track", eventId, number);
#endif
        }
    }

    public static void Track(string eventId, Dictionary<string, object> variable) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
            if (variable != null && variable.Count > 0) {
                gioTrackWithVariable(eventId, DicToObject(variable).keys, DicToObject(variable).values, DicToObject(variable).numbers, variable.Count);
            } else {
                gioTrack(eventId);
            }
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("track", eventId, dicToMap(variable));
#endif
        }
    }

    public static void Track(string eventId, double number, Dictionary<string, object> variable) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
            if (variable != null && variable.Count > 0) {
                gioTrackWithNumberAndVariable(eventId, number, DicToObject(variable).keys, DicToObject(variable).values, DicToObject(variable).numbers, variable.Count);
            } else {
                gioTrackWithNumber(eventId, number);
            }
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("track", eventId, number, dicToMap(variable));
#endif
        }
    }

    public static void SetEvar(string key, string value) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
             gioSetEvarWithKeyAndString(key, value);
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("setEvar", key, value);
#endif
        }
    }

    public static void SetEvar(string key, double number) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
            gioSetEvarWithKeyAndNumber(key, number);
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("setEvar", key, number);
#endif
        }
    }

    public static void SetEvar(Dictionary<string, object> variable) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
            if (variable != null && variable.Count > 0) {
                gioSetEvar(DicToObject(variable).keys, DicToObject(variable).values, DicToObject(variable).numbers, variable.Count);
            }
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("setEvar", dicToMap(variable));
#endif
        }
    }

    public static void SetPeopleVariable(string key, string value) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
            gioSetPeopleWithKeyAndString(key, value);
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("setPeopleVariable", key, value);
#endif
        }
    }

    public static void SetPeopleVariable(string key, double number) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
            gioSetPeopleWithKeyAndNumber(key, number);
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("setPeopleVariable", key, number);
#endif
        }
    }

    public static void SetPeopleVariable(Dictionary<string, object> variable) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
            if (variable != null && variable.Count > 0) {
                 gioSetPeople(DicToObject(variable).keys, DicToObject(variable).values, DicToObject(variable).numbers, variable.Count);
            }
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("setPeopleVariable", dicToMap(variable));
#endif
        }
    }

    public static void SetVisitor(Dictionary<string, object> variable) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
            if (variable != null && variable.Count > 0) {
                gioSetVistor(DicToObject(variable).keys, DicToObject(variable).values, DicToObject(variable).numbers, variable.Count);
            }
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("setVisitor", dicToMap(variable));
#endif
        }
    }

    public static void SetUserId(string userId) {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
            gioSetUserId(userId);
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("setUserId", userId);
#endif
        }
    }

    public static void ClearUserId() {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) {
#if UNITY_IPHONE
            gioClearUserId();
#endif
#if UNITY_ANDROID
            new AndroidJavaClass(ANDROID_CLASS).CallStatic("clearUserId");
#endif
        }
    }
}