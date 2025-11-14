using Microsoft.AspNetCore.Mvc;
using ShoppingList.Application.Services;
using ShoppingList.Domain.Models;
using Xunit;

namespace ShoppingList.Tests;


/// <summary>
/// Unit tests for ShoppingListService.
///
/// IMPORTANT: Write your tests here using Test Driven Development (TDD)
///
/// TDD Workflow:
/// 1. Write a test for a specific behavior (RED - test fails)
/// 2. Implement minimal code to make the test pass (GREEN - test passes)
/// 3. Refactor the code if needed (REFACTOR - improve without changing behavior)
/// 4. Repeat for the next behavior
///
/// Test Examples:
/// - See ShoppingItemTests.cs for examples of well-structured unit tests
/// - Follow the Arrange-Act-Assert pattern
/// - Use descriptive test names: Method_Scenario_ExpectedBehavior
///
/// What to Test:
/// - Happy path scenarios (normal, expected usage)
/// - Input validation (null/empty IDs, invalid parameters)
/// - Edge cases (empty array, array expansion, last item, etc.)
/// - Array management (shifting after delete, compacting, reordering)
/// - Search functionality (case-insensitive, matching in name/notes)
///
/// Recommended Test Categories:
///
/// GetAll() tests:
/// - GetAll_WhenEmpty_ShouldReturnEmptyList
/// - GetAll_WithItems_ShouldReturnAllItems
/// - GetAll_ShouldNotReturnMoreThanActualItemCount
///
/// GetById() tests:
/// - GetById_WithValidId_ShouldReturnItem
/// - GetById_WithInvalidId_ShouldReturnNull
/// - GetById_WithNullId_ShouldReturnNull
/// - GetById_WithEmptyId_ShouldReturnNull
///
/// Add() tests:
/// - Add_WithValidInput_ShouldReturnItem
/// - Add_ShouldGenerateUniqueId
/// - Add_ShouldIncrementItemCount
/// - Add_WhenArrayFull_ShouldExpandArray
/// - Add_AfterArrayExpansion_ShouldContinueWorking
/// - Add_ShouldSetIsPurchasedToFalse
///
/// Update() tests:
/// - Update_WithValidId_ShouldUpdateAndReturnItem
/// - Update_WithInvalidId_ShouldReturnNull
/// - Update_ShouldNotChangeId
/// - Update_ShouldNotChangeIsPurchased
///
/// Delete() tests:
/// - Delete_WithValidId_ShouldReturnTrue
/// - Delete_WithInvalidId_ShouldReturnFalse
/// - Delete_ShouldRemoveItemFromList
/// - Delete_ShouldShiftRemainingItems
/// - Delete_ShouldDecrementItemCount
/// - Delete_LastItem_ShouldWork
/// - Delete_FirstItem_ShouldWork
/// - Delete_MiddleItem_ShouldWork
///
/// Search() tests:
/// - Search_WithEmptyQuery_ShouldReturnAllItems
/// - Search_WithNullQuery_ShouldReturnAllItems
/// - Search_MatchingName_ShouldReturnItem
/// - Search_MatchingNotes_ShouldReturnItem
/// - Search_ShouldBeCaseInsensitive
/// - Search_WithNoMatches_ShouldReturnEmpty
/// - Search_ShouldFindPartialMatches
///
/// ClearPurchased() tests:
/// - ClearPurchased_WithNoPurchasedItems_ShouldReturnZero
/// - ClearPurchased_ShouldRemoveOnlyPurchasedItems
/// - ClearPurchased_ShouldReturnCorrectCount
/// - ClearPurchased_ShouldShiftRemainingItems
///
/// TogglePurchased() tests:
/// - TogglePurchased_WithValidId_ShouldReturnTrue
/// - TogglePurchased_WithInvalidId_ShouldReturnFalse
/// - TogglePurchased_ShouldToggleFromFalseToTrue
/// - TogglePurchased_ShouldToggleFromTrueToFalse
///
/// Reorder() tests:
/// - Reorder_WithValidOrder_ShouldReturnTrue
/// - Reorder_WithInvalidId_ShouldReturnFalse
/// - Reorder_WithMissingIds_ShouldReturnFalse
/// - Reorder_WithDuplicateIds_ShouldReturnFalse
/// - Reorder_ShouldChangeItemOrder
/// - Reorder_WithEmptyList_ShouldReturnFalse
/// </summary>
public class ShoppingListServiceTests
{
    // TODO: Write your tests here following the TDD workflow

    // Example test structure:
    // [Fact]
    // public void Add_WithValidInput_ShouldReturnItem()
    // {
    //     // Arrange
    //     var service = new ShoppingListService();
    //
    //     // Act
    //     var item = service.Add("Milk", 2, "Lactose-free");
    //
    //     // Assert
    //     Assert.NotNull(item);
    //     Assert.Equal("Milk", item!.Name);
    //     Assert.Equal(2, item.Quantity);
    //Hejsan
    // }

    [Theory]
    [InlineData("Banana", 2, "Yellow")]
    [InlineData("Apple", 3, null)]
    [InlineData("Orange", null, "Orange")]
    public void AddShouldReturnShoppingItem(string name, int quantity, string? notes)
    {
        // Arrange
        var service = new ShoppingListService();
        // Act
        var item = service.Add(name, quantity, notes);
        // Assert
        Assert.IsType<ShoppingItem>(item);
        Assert.Equal(item.Name, name);
        Assert.Equal(item.Quantity, quantity);
    }

    [Theory]
    [InlineData("Banana", 2, "Yellow", true)]
    [InlineData(null, 1, "Orange", false)]
    public void AddShouldAddTenItems(string name, int quantity, string? notes, bool expected)
    {
        //Arrange
        var service =  new ShoppingListService();
        var testArray = new ShoppingItem[10];
        
        //Act
        var item = service.Add(name, quantity, notes);
        for (int i = 0; i < 10; i++)
        {
            testArray[i] = item;
        }
        
        //Assert
        Assert.Equal(expected, testArray.Length == 10);
    }

    [Fact]
    public void GetAllShouldReturnAllItems()
    {
        //Arrange
        var service = new ShoppingListService();
        var expected = 5;
        for (int i = 0; i < expected; i++)
        {
            service.Add("Banana", 2, "Yellow");
        }
        //Act
        var actual = service.GetAll();
        int x = 0;
        foreach (var item in actual)
        {
            if (item != null)
            {
                x++;
            }
        }
        
        //Assert
        Assert.Equal(expected, x);
    }
    
    [Theory]
    [InlineData("Banana", 2, "Yellow")]
    [InlineData("Orange", 1, "Orange")]
    public void GetByIdShouldReturnShoppingItem(string name, int quantity, string? notes)
    {
        // Arrange
        var service = new ShoppingListService();
        var item = service.Add("Banana", 2, "Yellow");
        // Act
        var actual = service.GetById(item.Id);
        // Assert
        Assert.Equal(item.Id, actual.Id);
    }
    
    [Fact]
    public void DeleteShouldDeleteItem()
    {
        //Arrange
        var service = new ShoppingListService();
        var testArray = new ShoppingItem[5];
        var expected = true;
        
        //Act
        for (int i = 0; i < 5; i++)
        {
            testArray[i] = service.Add("Banana", 2, "Yellow");
        }
        
        var actual = service.Delete(testArray[2].Id);
        
        //Assert
        Assert.Equal(expected, actual);
    }
    [Fact]
    public void SearchShouldReturnSpecifiedItems()
    {
        //Arrange
        var service = new ShoppingListService();
        var query = "Orange";
        service.Add("Banana", 2, "Yellow");
        service.Add("Apple", 3, "Green");
        service.Add("Orange", 4, "Orange");
        service.Add("Orange", 7, null);
        service.Add("Pear", 7, "DarkGreen");
        service.Add("Pear", 8, "DarkOrange");
        var expected = 3;
        //Act
        var actual = service.Search(query);
        //Assert
        Assert.Equal(expected, actual.Count);
    }


}

