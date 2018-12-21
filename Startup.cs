using Unity.Attributes;

class Startup
{
    private IDataService _dataService;
    private IListener _listener;

    public Startup(IDataService dataService, IListener listener)
    {
        _dataService = dataService;
        _listener = listener;
    }

    public void Init()
    {
        _dataService.Connect();
        _listener.Listen();
    }
}