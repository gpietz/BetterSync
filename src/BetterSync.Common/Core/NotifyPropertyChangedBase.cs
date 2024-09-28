using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BetterSync.Common.Core;

/// <summary>
/// An abstract base class that provides a default implementation of the INotifyPropertyChanged interface.
/// It includes helper methods to simplify property change notification and value updates for properties
/// in derived classes.
/// </summary>
public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// This method triggers the PropertyChanged event, notifying listeners that a property value has changed.
    /// It catches and ignores any exceptions that may occur during event invocation to ensure the application
    /// continues running without disruption.
    /// </summary>
    /// <param name="propertyName">
    ///     The name of the property that changed. The default value is provided by the
    ///     [CallerMemberName] attribute, which automatically supplies the calling property's name.
    /// </param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        try
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        catch (Exception e)
        {
            //Ignore.
        }
    }
    
    /// <summary>
    /// This helper method sets the value of a field and triggers the OnPropertyChanged method if the value has changed.
    /// </summary>
    /// <param name="field"> A reference to the backing field of the property.</param>
    /// <param name="value"> The new value to set for the property.</param>
    /// <param name="propertyName">
    ///     (optional): The name of the property, automatically provided by [CallerMemberName].
    /// </param>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <returns>Returns true if the value was changed; otherwise, returns false. </returns>
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) 
            return false;
        
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
