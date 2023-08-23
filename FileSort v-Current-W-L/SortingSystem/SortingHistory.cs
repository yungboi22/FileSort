using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace SortingSystem;

public static class SortingHistory
{
    public static List<SortedItem> SortedItems { get; set; }
    public static List<SortedItem> LastSearchResult { get; set; }
    
    public static void LoadHistory()
    {
        if (File.Exists("sortinghistory.json"))
        {
            string obj = File.ReadAllText("sortinghistory.json");
            var options = new JsonSerializerOptions { WriteIndented = true }; 
            SortedItems = JsonSerializer.Deserialize<List<SortedItem>>(obj,options)!; 
        }
        else
        {
            SortedItems = new List<SortedItem>();    
        }
    }
    
    public static void SaveHistory(List<SortedItem> sortedItemsToSave)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string obj = JsonSerializer.Serialize(sortedItemsToSave,options);
        File.WriteAllText("sortinghistory.json",obj);
    }

    public static void Add(string aOriginPath, string aDestinyPath,string aCategory)
    {
        SortedItem sortedItem = new SortedItem(aOriginPath, aDestinyPath, aCategory);
        SortedItems.Add(sortedItem);
        SaveHistory(SortedItems);
    }

    public static List<SortedItem> SearchListBy(List<SortedItem> itemsToSearch, string searchValue)
    {
        itemsToSearch = itemsToSearch.FindAll(x => x.Name.ToLower().Contains(searchValue.Trim()));
        LastSearchResult = itemsToSearch;

        return itemsToSearch;
    }
    
    public static List<SortedItem> Sort(List<SortedItem> itemsToSort, SortBy sortBy,char reverseSymbol)
    {
        switch (sortBy)
        {
            case SortBy.Alphabet:
                itemsToSort.Sort((x,y)=> string.Compare(x.Name,y.Name));
                break;
            case SortBy.Category:
                itemsToSort.Sort((x,y)=> string.Compare(x.Category,y.Category));
                break;
            case SortBy.Date:
                itemsToSort.Sort((x,y)=> DateTime.Compare(x.DateOfSort,y.DateOfSort));
                break;
            case SortBy.Size:
                itemsToSort.Sort((x,y)=>  x.fileSize.CompareTo((y.fileSize)));
                break;
        }
        
        if (reverseSymbol == '!')
            itemsToSort.Reverse();
        
        return itemsToSort;
    }

    public static List<SortedItem> Filter(List<SortedItem> itemsToFilter, FilterBy filterBy, string filterValue)
    {
        int neededCompValue = -1;
        
        //function umbauen
        
        switch (filterBy)
        {
            case FilterBy.Category:
                return itemsToFilter.Where(x => x.Category.ToLower().Equals(filterValue)).ToList();
            case FilterBy.Date:
                return itemsToFilter.Where(x => x.DateOfSort.CompareTo(DateTime.Parse(filterValue))
                        .Equals(neededCompValue)).ToList();
            case FilterBy.Size:
                return itemsToFilter.Where(x => x.fileSize.CompareTo(Convert.ToInt32(filterValue))
                        .Equals(neededCompValue)).ToList();
            default:
                throw new Exception("Unknown filter");
        }
    }



    public static void ToTable(List<SortedItem> aSortedItems)
    {
        List<string> head = new List<string>();
        typeof(SortedItem).GetProperties().ToList().ForEach(x => head.Add(x.Name));


        ConsoleTable table = new ConsoleTable(head.ToArray(), 112);
        
        
        foreach (SortedItem sortedItem in aSortedItems)
        {
            List<string> row = new List<string>();
            
            foreach (PropertyInfo propertyInfo in typeof(SortedItem).GetProperties())
            {
                row.Add(propertyInfo.GetValue(sortedItem).ToString());
            }
            
            table.AddRow(row);
        }
       
        
        
        table.Print();
    }

}

public class SortedItem
{
    public string Name { get; set; }
    public string Category { get; set; }
    public string OriginPath { get; set; }
    public string CurrentPath { get; set; }
    public long fileSize { get; set; }
    public DateTime DateOfSort { get; set; }

    public SortedItem(string aOriginPath, string aDestinyPath,string aCategory)
    {
        Name = Path.GetFileName(aDestinyPath);
        Category = aCategory;
        OriginPath = aOriginPath;
        CurrentPath = aDestinyPath;

        if (File.GetAttributes(CurrentPath).HasFlag(FileAttributes.Directory))
            fileSize = -1;
        else
            fileSize = new FileInfo(CurrentPath).Length;
        
        DateOfSort = DateTime.Now;
    }

    public SortedItem()
    {
        
    }


}

public enum SortBy
{
    Alphabet,
    Category,
    Date,
    Size
}

public enum FilterBy
{
    Category,
    Date,
    Size
}

