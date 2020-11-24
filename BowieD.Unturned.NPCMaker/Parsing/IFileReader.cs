using System;
using System.Collections.Generic;

namespace BowieD.Unturned.NPCMaker.Parsing
{
    public interface IFileReader
    {
        IEnumerable<string> getKeys();
        bool containsKey(string key);
        void readKey(string key);
        int readArrayLength(string key);
        int readArrayLength();
        void readArrayIndex(string key, int index);
        void readArrayIndex(int index);
        string readValue(string key);
        string readValue(int index);
        string readValue(string key, int index);
        string readValue();
        object readValue(Type type, string key);
        object readValue(Type type, int index);
        object readValue(Type type, string key, int index);
        object readValue(Type type);
        T readValue<T>(string key);
        T readValue<T>(int index);
        T readValue<T>(string key, int index);
        T readValue<T>();
        IFileReader readObject(string key);
        IFileReader readObject(int index);
        IFileReader readObject(string key, int index);
        IFileReader readObject();
    }
}
