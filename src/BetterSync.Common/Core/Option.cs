namespace BetterSync.Common.Core;

/// <summary>
/// A generic class representing an optional value. It can either contain a value (Some) or no value (None).
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class Option<T> 
{
    private readonly T? _value;
    private readonly bool _hasValue;

    /// <summary>
    /// Private constructor to initialize the option with a value.
    /// </summary>
    /// <param name="value">The value to initialize the option with.</param>
    private Option(T? value)
    {
        _value = value;
        _hasValue = value is not null;
    }
    
    /// <summary>
    /// Private constructor to initialize the option without a value.
    /// </summary>
    private Option()
    {
        _value = default;
        _hasValue = false;
    }

    /// <summary>
    /// Creates an Option that contains a value.
    /// </summary>
    /// <param name="value">The value to be wrapped in the option.</param>
    /// <returns>An Option containing the specified value.</returns>
    public static Option<T> Some(T value) => new(value);

    /// <summary>
    /// Represents an Option that contains no value.
    /// </summary>
    public static Option<T> None { get; } = new();
    
    /// <summary>
    /// Returns the value if it exists, otherwise returns the provided default value.
    /// </summary>
    /// <param name="defaultValue">The default value to return if no value is present.</param>
    /// <returns>The contained value or the default value.</returns>
    public T? ValueOr(T? defaultValue) => _hasValue ? _value : defaultValue;

    /// <summary>
    /// Transforms the contained value using the specified mapping function, if a value exists.
    /// </summary>
    /// <typeparam name="TOut">The output type of the mapping function.</typeparam>
    /// <param name="map">The function to transform the contained value.</param>
    /// <returns>An Option containing the transformed value, or None if no value is present.</returns>
    public Option<TOut> Map<TOut>(Func<T, TOut> map) =>
        IsSome ? Option<TOut>.Some(map(_value)) : Option<TOut>.None;

    /// <summary>
    /// Chains another Option-returning function, if a value exists.
    /// </summary>
    /// <typeparam name="TOut">The output type of the chained function.</typeparam>
    /// <param name="bind">The function that returns an Option based on the contained value.</param>
    /// <returns>The Option returned by the bind function, or None if no value is present.</returns>
    public Option<TOut> Bind<TOut>(Func<T, Option<TOut>> bind) =>
        IsSome ? bind(_value) : Option<TOut>.None;

    /// <summary>
    /// Matches the Option, returning the result of the 'some' function if a value exists, or the 'none' function if not.
    /// </summary>
    /// <typeparam name="TOut">The type of the result.</typeparam>
    /// <param name="some">Function to execute if a value is present.</param>
    /// <param name="none">Function to execute if no value is present.</param>
    /// <returns>The result of the 'some' or 'none' function.</returns>
    public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none) =>
        IsSome ? some(_value) : none();

    /// <summary>
    /// Filters the Option based on a predicate. Returns the Option if the predicate is true; otherwise, returns None.
    /// </summary>
    /// <param name="predicate">The predicate to apply to the contained value.</param>
    /// <returns>The original Option if the predicate is true, otherwise None.</returns>
    public Option<T> Filter(Func<T, bool> predicate) =>
        IsSome && predicate(_value) ? Some(_value) : None;

    /// <summary>
    /// Returns this Option if it contains a value, otherwise returns the provided fallback Option.
    /// </summary>
    /// <param name="fallback">The fallback Option to return if this Option is None.</param>
    /// <returns>This Option if it contains a value, otherwise the fallback Option.</returns>
    public Option<T> OrElse(Func<Option<T>> fallback) =>
        IsSome ? this : fallback();
    
    /// <summary>
    /// Indicates whether this Option contains a value.
    /// </summary>
    public bool IsSome => _hasValue && _value is not null;
    
    /// <summary>
    /// Indicates whether this Option contains no value.
    /// </summary>
    public bool IsNone => !_hasValue;

    /// <summary>
    /// Executes the provided action if the Option contains a value.
    /// </summary>
    /// <param name="action">The action to execute on the contained value.</param>
    /// <returns>This Option.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the action is null.</exception>
    public Option<T> IfSome(Action<T> action)
    {
        if (action == null) 
            throw new ArgumentNullException(nameof(action));
        
        if (IsSome)
        {
            action.Invoke(_value);    
        }

        return this;
    }

    /// <summary>
    /// Executes the provided action if the Option contains no value.
    /// </summary>
    /// <param name="action">The action to execute if the Option is None.</param>
    /// <returns>This Option.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the action is null.</exception>
    public Option<T> IfNone(Action action)
    {
        if (action == null) 
            throw new ArgumentNullException(nameof(action));

        if (IsNone)
        {
            action.Invoke();
        }

        return this;
    }

    /// <summary>
    /// Flattens a nested Option<Option<T>> into a single Option<T>.
    /// </summary>
    /// <param name="option">The nested Option to flatten.</param>
    /// <returns>The flattened Option.</returns>
    public static Option<T> Flatten(Option<Option<T>> option) =>
        option.Match<Option<T>>(inner => inner, () => None);
}    
