using System;
using System.Collections.Generic;
using dotnetcoreproject.Domain;


public class TextValue
{
    public object Id { get; set; }
    public object Name { get; set; }
    public object text { get; set; }
    public object value { get; set; }
    public object dataid { get; set; }
}

public class EnumModel : BaseEntity
{
    public string name { get; set; }
    public string value { get; set; }
    public string value2 { get; set; }
    public string text { get; set; }
    public string text1 { get; set; }
    public bool selected { get; set; }
}

public class EnumModel<T>
{
    public T RowModel { get; set; }
    public string name { get; set; }
    public string value { get; set; }
    public string text { get; set; }
    public bool selected { get; set; }
}