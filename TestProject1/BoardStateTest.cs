using NUnit.Framework;
using SiSE;

namespace TestProject1;

[TestFixture]
public class BoardStateTests
{
    public int[,] tiles;
    public int[,] tiles2;

    
    [SetUp]
    public void Setup()
    {
        // Initialize the BoardState object with test data
        tiles = new int[,]
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };
        tiles2 = new int[,]
        {
            { 1, 8, 3 },
            { 4, 5, 6 },
            { 7, 2, 0 }
        };

    }
    
    [Test]
    public void AreEqualAndSame()
    {
        var _boardState = new GameState(tiles);
        var _boardState2 = new GameState(tiles);

        Assert.AreNotEqual(_boardState, _boardState2);
    }
  
    
    
    /*
    [SetUp]
    public void Setup()
    {
        // Initialize the BoardState object with test data
        int[,] tiles =
        {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };

        _boardState = new BoardState(tiles);
    }

    
    [Test]
    public void Move_ValidDirection_ReturnsNewBoardState()
    {
        // Arrange
        var direction = Direction.Right;

        // Act
        BoardState? newBoardState = _boardState.Move(direction);

        // Assert
        Assert.IsNotNull(newBoardState);
        Assert.That(newBoardState?.Tiles[2, 2], Is.EqualTo(0)); // The empty tile moved to the right
        // Add more assertions to verify the contents of the new board state after the move
    }
    

    [Test]
    public void GetNeighbours_ValidDirections_ReturnsListWithNeighbours()
    {
        // Arrange
        var directions = new Direction[] { Direction.Up, Direction.Right };

        // Act
        var neighbours = _boardState.GetNeighbours(directions);

        // Assert
        Assert.IsNotNull(neighbours);
        Assert.That(neighbours.Count, Is.EqualTo(2)); // There should be 2 valid neighbours in this case
        // Add more assertions to verify each neighbour's contents
    }

    // Add more test methods to cover all the functionalities of the BoardState struct
    */
}