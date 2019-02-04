/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
[XmlRoot(ElementName = "Item")]
public class Item
{
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    [XmlElement(ElementName = "Image")]
    public string Image { get; set; }
    [XmlElement(ElementName = "Damage")]
    public string Damage { get; set; }
    [XmlElement(ElementName = "Range")]
    public string Range { get; set; }
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; }
    [XmlElement(ElementName = "Value")]
    public string Value { get; set; }
    [XmlElement(ElementName = "Cost")]
    public int Cost { get; set; }
}

[XmlRoot(ElementName = "Items")]
public class Items
{
    [XmlElement(ElementName = "Item")]
    public List<Item> Item { get; set; }

    private void test()
    {

    }
}

