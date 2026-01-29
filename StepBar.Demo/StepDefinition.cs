namespace StepBar.Demo;

/// <summary>
/// Represents a definition of a step in a process, including its name and the workload to be executed.
/// </summary>
public sealed class StepDefinition
{
    /// <summary>
    /// Gets the name of the step.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the workload function associated with the step.
    /// </summary>
    public Func<Task> Workload { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StepDefinition"/> class.
    /// </summary>
    /// <param name="name">The name of the step.</param>
    /// <param name="workload">The asynchronous workload function to be executed.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> or <paramref name="workload"/> is <c>null</c>.</exception>
    public StepDefinition(string name, Func<Task> workload)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(workload);

        Name = name;
        Workload = workload;
    }
}
