namespace ExampleWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            DependencyInjector.Register<IDataService, DataService>();
            DependencyInjector.Register<IListener, Listener>();

            Startup initiator = DependencyInjector.Retrieve<Startup>();
            initiator.Init();
        }
    }
}