using Microsoft.Extensions.DependencyInjection;
using WpfMvvm.Service;

namespace WpfMvvm.DI
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public static void Init()
        {
            var services = new ServiceCollection();
           
            services.AddTransient<MainViewModel>();
            services.AddSingleton<PageService>();

            _provider = services.BuildServiceProvider();

            /*foreach (var item in services)
            {
                _provider.GetRequiredService(item.ServiceType);
            }*/           
        }

        public MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();
    }
}
