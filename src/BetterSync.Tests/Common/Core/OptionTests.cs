using BetterSync.Common.Core;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace BetterSync.Tests.Common.Core;

[TestFixture]
public class OptionTests
{
    [Test]
    public void Some_ShouldContainValue()
    {
        // Arrange
        var option = Option<string>.Some("Test");

        // Act & Assert
        Assert.That(option.IsSome, Is.True);
        Assert.That(option.IsNone, Is.False);
        Assert.That("Test", Is.EqualTo(option.ValueOr(null)));
    }      
    
    [Test]
    public void None_ShouldContainNoValue()
    {
        // Arrange
        var option = Option<string>.None;

        // Act & Assert
        Assert.That(option.IsSome, Is.False);
        Assert.That(option.IsNone, Is.True);
        Assert.That(option.ValueOr(null), Is.Null);
    }
    
    [Test]
    public void Map_ShouldTransformValue_WhenSome()
    {
        // Arrange
        var option = Option<string>.Some("Test");

        // Act
        var mappedOption = option.Map(s => s.Length);
     
        // Assert
        Assert.That(mappedOption.IsSome, Is.True);
        Assert.That(mappedOption.ValueOr(0), Is.EqualTo(4));
    }
    
    [Test]
    public void Map_ShouldReturnNone_WhenNone()
    {
        // Arrange
        var option = Option<string>.None;

        // Act
        var mappedOption = option.Map(s => s.Length);

        // Assert
        Assert.That(mappedOption.IsSome, Is.False);
        Assert.That(mappedOption.ValueOr(0), Is.EqualTo(0));
    }
    
    [Test]
    public void Bind_ShouldReturnMappedOption_WhenSome()
    {
        // Arrange
        var option = Option<string>.Some("Test");

        // Act
        var boundOption = option.Bind(s => Option<int>.Some(s.Length));

        // Assert
        Assert.That(boundOption.IsSome, Is.True);
        Assert.That(boundOption.ValueOr(0), Is.EqualTo(4));
    }
    
    [Test]
    public void Bind_ShouldReturnNone_WhenNone()
    {
        // Arrange
        var option = Option<string>.None;

        // Act
        var boundOption = option.Bind(s => Option<int>.Some(s.Length));

        // Assert
        Assert.That(boundOption.IsSome, Is.False);
        Assert.That(boundOption.ValueOr(0), Is.EqualTo(0));
    }
    
    [Test]
    public void Match_ShouldReturnValue_WhenSome()
    {
        // Arrange
        var option = Option<string>.Some("Test");

        // Act
        var result = option.Match(s => s.Length, () => 0);

        // Assert
        Assert.That(result, Is.EqualTo(4));
    }
    
    [Test]
    public void Match_ShouldReturnNoneValue_WhenNone()
    {
        // Arrange
        var option = Option<string>.None;

        // Act
        var result = option.Match(s => s.Length, () => 0);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
    
    [Test]
    public void Filter_ShouldReturnSameOption_WhenPredicateIsTrue()
    {
        // Arrange
        var option = Option<string>.Some("Test");

        // Act
        var filteredOption = option.Filter(s => s.Length == 4);

        // Assert
        Assert.That(filteredOption.IsSome, Is.True);
        Assert.That(filteredOption.IsNone, Is.False);
    }
    
    [Test]
    public void Filter_ShouldReturnNone_WhenPredicateIsFalse()
    {
        // Arrange
        var option = Option<string>.Some("Test");

        // Act
        var filteredOption = option.Filter(s => s.Length > 4);

        // Assert
        Assert.That(filteredOption.IsNone, Is.True);
        Assert.That(filteredOption.IsSome, Is.False);
    }
    
    [Test]
    public void OrElse_ShouldReturnOriginalOption_WhenSome()
    {
        // Arrange
        var option = Option<string>.Some("Test");

        // Act
        var result = option.OrElse(() => Option<string>.Some("Fallback"));

        // Assert
        Assert.That(result.ValueOr(null), Is.EqualTo("Test"));
    }
    
    [Test]
    public void OrElse_ShouldReturnFallbackOption_WhenNone()
    {
        // Arrange
        var option = Option<string>.None;

        // Act
        var result = option.OrElse(() => Option<string>.Some("Fallback"));

        // Assert
        Assert.That(result.ValueOr(null), Is.EqualTo("Fallback"));
    }
    
    [Test]
    public void IfSome_ShouldExecuteAction_WhenSome()
    {
        // Arrange
        var option = Option<string>.Some("Test");
        var executed = false;

        // Act
        option.IfSome(_ => executed = true);

        // Assert
        Assert.That(executed, Is.True);
    }
    
    [Test]
    public void IfNone_ShouldExecuteAction_WhenNone()
    {
        // Arrange
        var option = Option<string>.None;
        var executed = false;

        // Act
        option.IfNone(() => executed = true);

        // Assert
        Assert.That(executed, Is.True);
    }
    
    [Test]
    public void Flatten_ShouldReturnInnerOption_WhenSome()
    {
        // Arrange
        var option = Option<Option<string>>.Some(Option<string>.Some("Test"));

        // Act
        var flattenedOption = Option<string>.Flatten(option);

        // Assert
        Assert.That(flattenedOption.ValueOr(null), Is.EqualTo("Test"));
    }
    
    [Test]
    public void Flatten_ShouldReturnNone_WhenOuterOptionIsNone()
    {
        // Arrange
        var option = Option<Option<string>>.None;

        // Act
        var flattenedOption = Option<string>.Flatten(option);

        // Assert
        Assert.That(flattenedOption.IsNone, Is.True);
    }
}
