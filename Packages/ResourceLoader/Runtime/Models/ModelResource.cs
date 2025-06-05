using System;

namespace Models
{
    public class ModelResource
    {
        internal string Key;
        internal readonly object Resource;
        internal readonly Action Destructor;

        public ModelResource(string key, object resource, Action destructor = null)
        {
            Key = key;
            Resource = resource;
            Destructor = destructor;
        }
    }
}