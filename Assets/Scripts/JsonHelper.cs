using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }

    public static List<T> FromJson<T>(string json)
    {
        return JsonUtility.FromJson<Wrapper<T>>("{\"Items\":" + json + "}").Items;
    }
}