using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Teleport
{
    public class Xml
    {
        

        public void Create(String path)
        {
            XmlDocument doc = new XmlDocument();
            //xml declaration
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);

            //(2) string.Empty makes cleaner code
            XmlElement body = doc.CreateElement("body");
            doc.AppendChild(body);
            
            doc.Save(path + "tp.xml");
        }

        public void Update(string path, string name, float x, float y, float z)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path + "tp.xml");

            XmlNode body = doc.SelectSingleNode("/body");

            //Create position element
            XmlElement positionNode = doc.CreateElement("Location");

            //Add attributes
            XmlAttribute attribute = doc.CreateAttribute("x");
            attribute.Value =x.ToString();
            positionNode.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("y");
            attribute.Value = y.ToString();
            positionNode.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("z");
            attribute.Value = z.ToString();
            positionNode.Attributes.Append(attribute);

            attribute = doc.CreateAttribute("name");
            attribute.Value = name;
            positionNode.Attributes.Append(attribute);

            body.AppendChild(positionNode);

            doc.Save(path + "tp.xml");

        }

        public List<Location> Read(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path + "tp.xml");

            XmlNodeList aNodes = doc.SelectNodes("/body/Location");
            List<Location> locations = new List<Location>();
            foreach(XmlNode aNode in aNodes)
            {
                XmlAttribute nameAttribute = aNode.Attributes["name"];
                XmlAttribute xAttribute = aNode.Attributes["x"];
                XmlAttribute yAttribute = aNode.Attributes["y"];
                XmlAttribute zAttribute = aNode.Attributes["z"];
                

                string name = nameAttribute.Value;
                float x = float.Parse(xAttribute.Value);
                float y = float.Parse(yAttribute.Value);
                float z = float.Parse(zAttribute.Value);
                Location loc = new Location(x,y,z,name);
                locations.Add(loc);
            }

            return locations;
        }

        public void Delete(string path, Location location)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path +  "tp.xml");


            XmlNode lo = doc.SelectSingleNode("/body/Location[@name='" + location.GetName() + "']");
            lo.ParentNode.RemoveChild(lo);

            doc.Save(path + "tp.xml");

        }

    }
}
