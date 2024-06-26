﻿using Play.Common;

namespace Play.Catalog.Services.Entities;


public class Item : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

}

