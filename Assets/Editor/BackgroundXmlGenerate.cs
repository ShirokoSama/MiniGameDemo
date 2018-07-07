using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;



public class BackgroundXmlGenerate : MonoBehaviour {

    [MenuItem("Other/Background Xml/Generate Xml")]
    public static void GenerateXml()
    {
        string outputPath = Application.dataPath + @"/Resources";

        int backgroundWidth = 8640;
        int backgroundHeight = 19200;
        int splitWidth = 512;
        int splitHeight = 512;

        XmlDocument xml = new XmlDocument();
        xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
        xml.AppendChild(xml.CreateElement("BackgroundPieces"));
        XmlNode root = xml.SelectSingleNode("BackgroundPieces");
        XmlElement resNode = xml.CreateElement("Collection");
        root.AppendChild(resNode);

        for (int i = 0; i * splitWidth < backgroundWidth; i++)
        {
            for (int j = 0; j * splitHeight < backgroundHeight; j++)
            {
                int width = Mathf.Min(splitWidth, backgroundWidth - i * splitWidth);
                int height = Mathf.Min(splitHeight, backgroundHeight - j * splitHeight);
                AddSplitNodeToXml(xml, i, j, width, height, "BackgroundSplit-" + i + "-" + j, i * splitWidth + width / 2 - backgroundWidth / 2, -j * splitHeight - height / 2 + 9 * backgroundHeight / 10);
            }
        }

        xml.Save(outputPath + @"/BackgroundSplit.xml");
        System.Diagnostics.Process.Start(outputPath, outputPath);
    }

    public static void AddSplitNodeToXml(XmlDocument xml, int x, int y, int width, int height, string resName, float initialX, float initialY)
    {
        XmlNode root = xml.SelectSingleNode("BackgroundPieces/Collection");
        XmlElement element = xml.CreateElement("BackgroundPiece");
        element.SetAttribute("x", x.ToString());
        element.SetAttribute("y", y.ToString());
        element.SetAttribute("width", width.ToString());
        element.SetAttribute("height", height.ToString());
        element.SetAttribute("resName", resName);
        element.SetAttribute("initialX", initialX.ToString());
        element.SetAttribute("initialY", initialY.ToString());
        root.AppendChild(element);
    }

}
