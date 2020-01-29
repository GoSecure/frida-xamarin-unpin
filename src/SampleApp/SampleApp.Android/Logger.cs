

using XamarinMulti;

public class Logger : ILogger
{
    public void WriteLine(string msg)
    {
        Android.Util.Log.Info("PINNER", msg);
    }
}