using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace StepBar.Demo;

/// <summary>
/// The view model for the main window, responsible for managing the list of steps and their execution.
/// </summary>
public sealed class MainWindowViewModel : ReactiveObject, IDisposable
{
    private readonly Stopwatch _stopwatch = new();
    private readonly IDisposable _elapsedTimeSubscription;

    /// <summary>
    /// Gets the collection of step progress items.
    /// </summary>
    public ObservableCollection<StepProgress> StepProgresses { get; }

    /// <summary>
    /// Gets the command to execute all steps sequentially.
    /// </summary>
    public ReactiveCommand<Unit, Unit> RunCommand { get; }

    /// <summary>
    /// Gets a value indicating whether the steps are currently running.
    /// </summary>
    public bool IsRunning
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    /// <summary>
    /// Gets or sets the current index of the step being processed.
    /// </summary>
    public int CurrentStepIndex
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    /// <summary>
    /// Gets the total elapsed time in milliseconds for all steps.
    /// </summary>
    public long TotalElapsedTime
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    /// <param name="stepDefinitions">A collection of step definitions to initialize the progress items.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="stepDefinitions"/> is <c>null</c>.</exception>
    public MainWindowViewModel(IEnumerable<StepDefinition> stepDefinitions)
    {
        ArgumentNullException.ThrowIfNull(stepDefinitions);

        StepProgresses = new ObservableCollection<StepProgress>(
            stepDefinitions.Select(x => new StepProgress(x))
        );

        RunCommand = ReactiveCommand.CreateFromTask(RunStepsAsync);

        _elapsedTimeSubscription = Observable
            .Interval(TimeSpan.FromMilliseconds(100))
            .Select(_ => _stopwatch.ElapsedMilliseconds)
            .DistinctUntilChanged()
            .ObserveOn(RxSchedulers.MainThreadScheduler)
            .Subscribe(time => TotalElapsedTime = time);
    }

    private async Task RunStepsAsync()
    {
        foreach (var stepProgress in StepProgresses)
        {
            stepProgress.Reset();
        }

        TotalElapsedTime = 0;
        IsRunning = true;
        _stopwatch.Restart();

        for (CurrentStepIndex = 0; CurrentStepIndex < StepProgresses.Count; CurrentStepIndex++)
        {
            var stepProgress = StepProgresses[CurrentStepIndex];
            await stepProgress.RunAsync();
        }

        _stopwatch.Stop();
        IsRunning = false;
    }

    /// <summary>
    /// Disposes of the resources used by the view model.
    /// </summary>
    public void Dispose()
    {
        _elapsedTimeSubscription.Dispose();

        foreach (var stepProgress in StepProgresses)
        {
            stepProgress.Dispose();
        }
        StepProgresses.Clear();
    }
}
