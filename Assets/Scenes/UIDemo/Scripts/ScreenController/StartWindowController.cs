using UIFramework;
using Utils;

public class StartDemoSignal : ASignal { }

public class StartWindowController : WindowController
{
    public void UI_Start()
    {
        Signals.Get<StartDemoSignal>().Dispatch();
    }
}