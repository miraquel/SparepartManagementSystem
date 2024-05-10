// See https://aka.ms/new-console-template for more information

using SparepartManagementSystem.Shared.Helper;

var myList = new List<string> { "*&^8*1*", "82*", "83*", "831*", "832*", "72*", "7441" };
//var myList = new List<string> { "8000", "8100", "8200", "8300", "8310", "8320", "7200", "7441" };

var parms = new List<string> { "8", "82", "821", "822", "83", "831", "832", "84", "72", "74", "7", "71", "744" };

foreach (var parm in parms)
{
    // delete all special characters except '*' in the end of the string
    // var filteredList = myList
    //     .Select(item => item[^1].Equals('*') ? item.RemoveSpecialCharacters() + "*" : item.RemoveSpecialCharacters())
    //     //.FilterShortestCommonPrefixes()
    //     .Where(item => item.StartsWith(parm))
    //     .Select(item => myList.Contains(item + "*") ? item + "*" : item)
    //     .ToList();
    //
    // if (!filteredList.Any() && !string.IsNullOrEmpty(parm))
    // {
    //     var leftmostCommonCharacters = myList
    //         .Select(item => item.TrimEnd('*').CalculateLeftmostCommonCharacters(parm))
    //         .FirstOrDefault(item => myList.Contains(item + "*"));
    //
    //     if (!string.IsNullOrEmpty(leftmostCommonCharacters))
    //     {
    //         filteredList.Add(parm + "*");
    //     }
    // }

    var filteredList = myList.FilterStringsByPrefix(parm).Select(x => $"{x}*").ToList();
    
    Console.WriteLine($"parm = {parm}, expected output: {string.Join(", ", filteredList)}");

    List<string> secondList = [];
    secondList = secondList.Count != 0 ? secondList.FilterStringsByPrefix(parm).ToList() : ["*"];
    
    Console.WriteLine($"parm = {parm}, expected output: {string.Join(", ", secondList)}");
}