using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teleport
{
    public class Location
    {
        private float x;
        private float y;
        private float z;
        private string name;

        public Location(float x, float y, float z, string name)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }

        public float GetY()
        {
            return y;
        }

        public float GetZ()
        {
            return z;
        }
        public float GetX()
        {
            return x;
        }
    }
}
