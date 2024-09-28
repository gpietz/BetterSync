using System.Text.Json;
using BetterSync.Common.Configuration;
using BetterSync.Common.Services;
using BetterSync.Common.Utilities;
using NUnit.Framework;

namespace BetterSync.Tests.Common.Services;

[TestFixture]
public class ConfigurationServiceTests
{
    private ConfigurationService _configurationService;
    private string? _invalidFilePath;
    private string? _validFilePath;
    
    [SetUp]
    public void Setup()
    {
        _configurationService = new ConfigurationService();
    }
    
    [Test]
    public async Task LoadConfigurationAsync_ReturnsError_WhenFilePathIsNullOrEmpty()
    {
        // Arrange
        var invalidFilePath = string.Empty;

        // Act
        var result = await _configurationService.LoadConfigurationAsync(invalidFilePath);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Kind, Is.EqualTo(SyncConfigurationError.ErrorKind.InvalidFileName));
    }
    
    [Test]
    public async Task LoadConfigurationJsonAsync_ReturnsError_WhenFileDoesNotExist()
    {
        // Arrange
        var invalidFilePath = "non_existing_file.json";

        // Act
        var result = await _configurationService.LoadConfigurationAsync(invalidFilePath);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(SyncConfigurationError.ErrorKind.FileNotFound, Is.EqualTo(SyncConfigurationError.ErrorKind.FileNotFound));
    }
    
    [Test]
    public async Task LoadConfigurationJsonAsync_ReturnsError_WhenFileCannotBeLoaded()
    {
        // Arrange
        _invalidFilePath = Path.Combine(Path.GetTempPath(), "restricted_file.json");
        await File.Create(_invalidFilePath).DisposeAsync(); // Create an empty file to simulate access issues

        // Simulate UnauthorizedAccessException
        File.SetAttributes(_invalidFilePath, FileAttributes.ReadOnly);

        // Act
        Result<SyncConfiguration, SyncConfigurationError> result;
        await using (File.Open(_invalidFilePath, FileMode.Open, FileAccess.Read, FileShare.None))
        {
            result = await _configurationService.LoadConfigurationAsync(_invalidFilePath);    
        }
        
        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Kind, Is.EqualTo(SyncConfigurationError.ErrorKind.AccessDenied));

        // Clean up
        File.SetAttributes(_invalidFilePath, FileAttributes.Normal);
        File.Delete(_invalidFilePath);
    }
    
    [Test]
    public async Task LoadConfigurationJsonAsync_ReturnsError_WhenJsonIsInvalid()
    {
        // Arrange
        _invalidFilePath = Path.Combine(Path.GetTempPath(),  "invalid_config.json");
        await File.WriteAllTextAsync(_invalidFilePath, "{ invalid json }");

        // Act
        var result = await _configurationService.LoadConfigurationAsync(_invalidFilePath);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Kind, Is.EqualTo(SyncConfigurationError.ErrorKind.DeserializeError));

        // Clean up
        File.Delete(_invalidFilePath);
    }
    
    [Test]
    public async Task LoadConfigurationJsonAsync_ReturnsOk_WhenValidJsonProvided()
    {
        // Arrange
        var validJson = JsonSerializer.Serialize(new SyncConfiguration());
        _validFilePath = Path.Combine(Path.GetTempPath(), "valid_config.json");
        await File.WriteAllTextAsync(_validFilePath, validJson);

        // Act
        var result = await _configurationService.LoadConfigurationAsync(_validFilePath);

        // Assert
        Assert.That(result.IsSuccess, Is.EqualTo(true));
        Assert.That(result.Error, Is.Null);

        // Clean up
        File.Delete(_validFilePath);
    }
    
    [TearDown]
    public void TearDown()
    {
        foreach (var filePath in (string?[]) [_invalidFilePath, _validFilePath])
        {
            if (filePath == null || !File.Exists(filePath)) continue;
            File.SetAttributes(filePath, FileAttributes.Normal);
            File.Delete(filePath);
        }
    }
}
