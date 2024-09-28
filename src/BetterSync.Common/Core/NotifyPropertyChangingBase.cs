using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BetterSync.Common.Core;

public abstract class NotifyPropertyChangingBase : NotifyPropertyChangedBase, INotifyPropertyChanging
{
    public event PropertyChangingEventHandler? PropertyChanging;

    protected virtual bool OnPropertyChanging([CallerMemberName] string? propertyName = null)
    {
        var eventArgs = new CancellablePropertyChangingEventArgs(propertyName);
        try
        {
            PropertyChanging?.Invoke(this, eventArgs);
        }
        catch
        {
            //Ignore.
        }
        return !eventArgs.IsCancelRequested;        
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
    protected new bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value) || !OnPropertyChanging(propertyName)) 
            return false;
        
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

/// <summary>
/// This class extends System.ComponentModel.PropertyChangingEventArgs and adds support for canceling property
/// changes. It provides a mechanism to mark a property change as canceled before it occurs.
/// </summary>
/// <param name="propertyName">The name of the property that is about to change.</param>
public class CancellablePropertyChangingEventArgs (string? propertyName) : PropertyChangingEventArgs(propertyName)
{
    /// <summary>
    /// Indicates whether the property change has been canceled.
    /// If set to true, the property change will be aborted.
    /// </summary>
    public bool IsCancelRequested { get; private set; }

    /// <summary>
    /// This method requests that the property change be canceled. When invoked, it sets the IsCancelRequested
    /// property to true, signaling that the change should not proceed.
    /// </summary>
    public void Cancel() => IsCancelRequested = true;
}
