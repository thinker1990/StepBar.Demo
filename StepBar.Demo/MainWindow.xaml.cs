namespace StepBar.Demo;

/// <summary>
/// Interaction logic for MainWindow.xaml.
/// Initializes the main window and sets up the view model with simulated step definitions.
/// </summary>
public partial class MainWindow
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// Sets up the data context with a predefined list of steps for simulation.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        var stepDefinitions = new[]
        {
            new StepDefinition("空闲", WorkloadSimulation),
            new StepDefinition("等待PLC到位信号", WorkloadSimulation),
            new StepDefinition("扫码", WorkloadSimulation),
            new StepDefinition("拍照", WorkloadSimulation),
            new StepDefinition("MES入站", WorkloadSimulation),
            new StepDefinition("保存数据", WorkloadSimulation),
            new StepDefinition("写入PLC完成信号", WorkloadSimulation),
            new StepDefinition("流程完成", WorkloadSimulation),
        };
        DataContext = new MainWindowViewModel(stepDefinitions);
    }

    private static async Task WorkloadSimulation()
    {
        var randomDelay = Random.Shared.Next(1000, 3000);
        await Task.Delay(TimeSpan.FromMilliseconds(randomDelay));
    }
}
