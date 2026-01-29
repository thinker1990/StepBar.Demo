using ReactiveUI.Builder;

namespace StepBar.Demo;

/// <summary>
/// Interaction logic for App.xaml.
/// Represents the entry point for the application.
/// </summary>
public partial class App
{
    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// Configures ReactiveUI with WPF support.
    /// </summary>
    public App()
    {
        // Initialize ReactiveUI with RxAppBuilder
        RxAppBuilder.CreateReactiveUIBuilder().WithWpf().BuildApp();
    }
}
