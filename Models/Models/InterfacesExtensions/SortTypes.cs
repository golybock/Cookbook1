namespace Models.Models.InterfacesExtensions;

public class SortTypes
{
    public static SortType Default =>
        new SortType()
        {
            Id = 1,
            Name = "По умолчанию",
            Tag = "Default", 
            IconPath = "../../Resources/MenuIcons/Sort/defaultsort.png"
        };
    
    public static SortType ByUpper =>
        new SortType()
        {
            Id = 2,
            Name = "По возрастанию",
            Tag = "ByUpper", 
            IconPath = "../../Resources/MenuIcons/Sort/lowerToUpperSort.png"
        };
    
    public static SortType ByLower =>
        new SortType()
        {
            Id = 3,
            Name = "По убыванию",
            Tag = "ByLower", 
            IconPath = "../../Resources/MenuIcons/Sort/upperToLowerSort.png"
        };
    
    public static List<SortType> SortTypesList =>
        new List<SortType> {Default, ByUpper, ByLower};

    public static List<SortType> GetSortTypes() =>
        new List<SortType> {Default, ByUpper, ByLower};
}