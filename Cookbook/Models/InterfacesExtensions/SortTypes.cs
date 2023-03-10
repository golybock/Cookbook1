using System.Collections.Generic;

namespace Cookbook.Models.InterfacesExtensions;

public class SortTypes
{
    public static SortType Default =>
        new()
        {
            Id = 1,
            Name = "По умолчанию",
            Tag = "Default",
            IconPath = "../../Resources/MenuIcons/Sort/defaultsort.png"
        };

    public static SortType ByUpper =>
        new()
        {
            Id = 2,
            Name = "По возрастанию",
            Tag = "ByUpper",
            IconPath = "../../Resources/MenuIcons/Sort/lowerToUpperSort.png"
        };

    public static SortType ByLower =>
        new()
        {
            Id = 3,
            Name = "По убыванию",
            Tag = "ByLower",
            IconPath = "../../Resources/MenuIcons/Sort/upperToLowerSort.png"
        };

    public static List<SortType> SortTypesList => new() {Default, ByUpper, ByLower};

    public static List<SortType> GetSortTypes()
    {
        return new() {Default, ByUpper, ByLower};
    }
}