using System.Diagnostics;
using System.Reactive.Linq;
using ReactiveUI;

namespace StepBar.Demo;

/// <summary>
/// Represents the progress of a specific step, including its execution time and state.
/// Implements <see cref="ReactiveObject"/> for property change notifications and <see cref="IDisposable"/> for resource cleanup.
/// </summary>
public sealed class StepProgress : ReactiveObject, IDisposable
{
    private readonly StepDefinition _stepDefinition;
    private readonly Stopwatch _stopwatch = new();
    private readonly IDisposable _elapsedTimeSubscription;

    /// <summary>
    /// Gets or sets the name of the step.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the elapsed execution time of the step in milliseconds.
    /// Notifies listeners when the value changes.
    /// </summary>
    public long ElapsedTime
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StepProgress"/> class based on a step definition.
    /// Starts a timer subscription to update <see cref="ElapsedTime"/>.
    /// </summary>
    /// <param name="stepDefinition">The definition of the step to track.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stepDefinition"/> is <c>null</c>.</exception>
    public StepProgress(StepDefinition stepDefinition)
    {
        ArgumentNullException.ThrowIfNull(stepDefinition);

        _stepDefinition = stepDefinition;
        Name = stepDefinition.Name;

        _elapsedTimeSubscription = Observable
            .Interval(TimeSpan.FromMilliseconds(100))
            .Select(_ => _stopwatch.ElapsedMilliseconds)
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(x => ElapsedTime = x);
    }

    /// <summary>
    /// Executes the workload associated with the step asynchronously.
    /// Tracks the execution time using a stopwatch.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RunAsync()
    {
        _stopwatch.Start();
        await _stepDefinition.Workload();
        _stopwatch.Stop();
    }

    /// <summary>
    /// Resets the elapsed time and the internal stopwatch to zero.
    /// </summary>
    public void Reset()
    {
        ElapsedTime = 0;
        _stopwatch.Reset();
    }

    /// <summary>
    /// Disposes the resources used by the <see cref="StepProgress"/> instance.
    /// Stops the stopwatch and disposes the time update subscription.
    /// </summary>
    public void Dispose()
    {
        _stopwatch.Stop();
        _elapsedTimeSubscription.Dispose();
    }
}
