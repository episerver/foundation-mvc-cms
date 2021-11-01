using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

[ContentType]
public class GenericFile : MediaData
{
    public virtual string Description { get; set; }
}