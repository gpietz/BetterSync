using BetterSync.Service.Core;
using NUnit.Framework;

namespace BetterSync.Tests.Service.Core;

[TestFixture]
public class ServiceOptionsParserTests
{
    private ServiceOptionsParser _parser;
    
    [SetUp]
    public void Setup()
    {
        _parser = new ServiceOptionsParser();
    }
    
    [Test]
    public void Parse_ShouldReturnError_WhenInvalidArgumentProvided()
    {
        // Arrange
        var args = new[] { "--invalidOption" };

        // Act
        var result = _parser.Parse(args);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error?.Kind, Is.EqualTo(ServiceOptionsParseError.ErrorKind.InvalidArgument));
        Assert.That(result.Error?.Message, Is.Not.Empty);
    }
    
    [Test]
    public void Parse_ShouldReturnOptions_WhenFileOptionProvided()
    {
        // Arrange
        var args = new[] { "-f", "config.json" };

        // Act
        var result = _parser.Parse(args);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value?.ConfigFileName, Is.EqualTo("config.json"));
    }
    
    [Test]
    public void Parse_ShouldReturnError_WhenFileOptionIsMissingArgument()
    {
        // Arrange
        var args = new[] { "-f" };

        // Act
        var result = _parser.Parse(args);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Null);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error?.Kind, Is.EqualTo(ServiceOptionsParseError.ErrorKind.MissingArgument));
        Assert.That(result.Error?.Message, Is.Not.Empty);
    }
    
    [Test]
    public void Parse_ShouldReturnError_WhenFileOptionHasQuotationError_StartsWithQuoteButDoesNotEndWithQuote()
    {
        // Arrange
        var args = new[] { "-f", "\"config.json" };

        // Act
        var result = _parser.Parse(args);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Null);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error?.Kind, Is.EqualTo(ServiceOptionsParseError.ErrorKind.ArgumentQuotationError));
        Assert.That(result.Error?.Message, Is.Not.Empty);
    }
    
    [Test]
    public void Parse_ShouldReturnError_WhenFileOptionHasQuotationError_EndsWithQuoteButDoesNotStartWithQuote()
    {
        // Arrange
        var args = new[] { "-f", "config.json\"" };

        // Act
        var result = _parser.Parse(args);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Null);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error?.Kind, Is.EqualTo(ServiceOptionsParseError.ErrorKind.ArgumentQuotationError));
        Assert.That(result.Error?.Message, Is.Not.Empty);
    }
    
    [Test]
    public void Parse_ShouldReturnOptions_WithVerboseFlagSet()
    {
        // Arrange
        var args = new[] { "-v" };

        // Act
        var result = _parser.Parse(args);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value?.Verbose, Is.True);
    }
    
    [Test]
    public void Parse_ShouldHandleMultipleOptions_FileAndVerbose()
    {
        // Arrange
        var args = new[] { "-f", "config.json", "-v" };

        // Act
        var result = _parser.Parse(args);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value?.ConfigFileName, Is.EqualTo("config.json"));
        Assert.That(result.Value?.Verbose, Is.True);
    }
}
