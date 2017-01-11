using System;
using System.Collections.Generic;


public enum EntryMode { Lista, Grilla }

[Serializable]
public class Attributes
{
    public string label;
    public string rel;
    public string type;
    public string href;
    public string schema;
    public string term;
    public string imId;
    public string imBundleId;
    public string amount;
    public string currency;
    public string height;

    public void SetLabel(string _label)
    {
        label = _label;
    }

    public void SetRel(string _rel)
    {
        rel = _rel;
    }
    public void SetType(string _type)
    {
        type = _type;
    }
    public void SetHref(string _href)
    {
        href = _href;
    }
    public void SetSchema(string _schema)
    {
        schema = _schema;
    }
    public void SetTerm(string _term)
    {
        term = _term;
    }
    public void SetImId(string _imId)
    {
        imId = _imId;
    }
    public void SetImBundleId(string _imBundleId)
    {
        imBundleId = _imBundleId;
    }
    public void SetAmount(string _amount)
    {
        amount = _amount;
    }
    public void SetCurrency(string _currency)
    {
        currency = _currency;
    }
    public void SetHeight(string _height)
    {
        height = _height;
    }
}

[Serializable]
public class Author
{
    public Name name;
    public Uri uri;

    public Author(Name _name, Uri _uri)
    {
        name = _name;
        uri = _uri;
    }
}

[Serializable]
public class Category
{
    public Attributes attributes;

    public Category(Attributes _attributes)
    {
        attributes = _attributes;
    }
}

[Serializable]
public class Entry
{
    public ImName imName;
    public List<ImImage> imImage;
    public Summary summary;
    public ImPrice imPrice;
    public ImContentType imContentType;
    public Rights rights;
    public Title title;
    public Link link;
    public Id id;
    public ImArtist imArtist;
    public Category category;
    public ImReleaseDate imReleaseDate;

    public Entry(ImName _imName, List<ImImage> _imImage, Summary _summary, ImPrice _imPrice, ImContentType _imContentType, Rights _rights, Title _title, Link _link, Id _id, ImArtist _imArtist, Category _category, ImReleaseDate _imReleaseDate)
    {
        imName = _imName;
        imImage = _imImage;
        summary = _summary;
        imPrice = _imPrice;
        imContentType = _imContentType;
        rights = _rights;
        title = _title;
        link = _link;
        id = _id;
        imArtist = _imArtist;
        category = _category;
        imReleaseDate = _imReleaseDate;
    }
}

[Serializable]
public class Feed
{
    public Author author;
    public List<Entry> entry;
    public Updated updated;
    public Rights rights;
    public Title title;
    public Icon icon;
    public List<Link> link;
    public Id id;
}

[Serializable]
public class Icon
{
    public string label;

    public Icon(string _label)
    {
        label = _label;
    }
}

[Serializable]
public class Id
{
    public string label;
    public Attributes attributes;

    public Id(string _label, Attributes _attributes)
    {
        label = _label;
        attributes = _attributes;
    }
}

[Serializable]
public class ImArtist
{
    public string label;
    public Attributes attributes;

    public ImArtist(string _label, Attributes _attributes)
    {
        label = _label;
        attributes = _attributes;
    }
}

[Serializable]
public class ImContentType
{
    public Attributes attributes;

    public ImContentType(Attributes _attributes)
    {
        attributes = _attributes;
    }
}

[Serializable]
public class ImImage
{
    public string label;
    public Attributes attributes;

    public ImImage(string _label, Attributes _attributes)
    {
        label = _label;
        attributes = _attributes;
    }
}

[Serializable]
public class ImName
{
    public string label;

    public ImName(string _label)
    {
        label = _label;
    }
}

[Serializable]
public class ImPrice
{
    public string label;

    public Attributes attributes;

    public ImPrice(string _label, Attributes _attributes)
    {
        label = _label;
        attributes = _attributes;
    }
}

[Serializable]
public class ImReleaseDate
{
    public string label;
    public Attributes attributes;

    public ImReleaseDate(string _label, Attributes _attributes)
    {
        label = _label;
        attributes = _attributes;
    }
}

[Serializable]
public class Link
{
    public Attributes attributes;

    public Link(Attributes _attributes)
    {
        attributes = _attributes;
    }
}

[Serializable]
public class Name
{
    public string label;

    public Name(string _label)
    {
        label = _label;
    }
}

[Serializable]
public class Rights
{
    public string label;

    public Rights(string _label)
    {
        label = _label;
    }
}

[Serializable]
public class Summary
{
    public string label;

    public Summary(string _label)
    {
        label = _label;
    }
}

[Serializable]
public class Title
{
    public string label;

    public Title(string _label)
    {
        label = _label;
    }
}

[Serializable]
public class Updated
{
    public string label;

    public Updated(string _label)
    {
        label = _label;
    }
}

[Serializable]
public class Uri
{
    public string label;

    public Uri(string _label)
    {
        label = _label;
    }
}