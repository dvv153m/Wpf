using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfMvvm.MVVM;
using WpfMvvm.Service;
using WpfMvvm.Wizard;

namespace WpfMvvm
{
    public class MainViewModel : ViewModelBase
    {
        private readonly PageService _pageService;

        public MainViewModel(PageService pageService)        
        {
            _pageService = pageService;
            Clicks = 5;
            PropertyChanged += MainViewModel_PropertyChanged;                       
        }

        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsBusy))
            {
                ClickCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand ClickCommand => new DelegateCommand((obj) =>
        {            
            IsBusy = true;
            DownloadSomeDataAsync().ContinueWith((obj1) =>
            {
                IsBusy = false;                
            }, TaskScheduler.FromCurrentSynchronizationContext()); 
            
        }, (obj) => !IsBusy);

        /*public AsyncCommand ClickCommand => new AsyncCommand(async() => 
        {            
            IsBusy = true;             
            await DownloadSomeDataAsync().ContinueWith((obj)=> 
            {
                IsBusy = false;                
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }, () => !IsBusy);*/

        private async Task DownloadSomeDataAsync()
        {
            HttpClient client = new HttpClient();
            List<string> urlList = GetUrlList();
            foreach (var url in urlList)
            {
                HttpResponseMessage response = await client.GetAsync(url);                
                byte[] urlContents = await response.Content.ReadAsByteArrayAsync();
            }
        }

        private List<string> GetUrlList()
        {
            return new List<string>
            {
                "https://msdn.microsoft.com",
                "https://msdn.microsoft.com/library/windows/apps/br211380.aspx",
                "https://msdn.microsoft.com/library/hh290136.aspx",
                "https://msdn.microsoft.com/library/ee256749.aspx",
                "https://msdn.microsoft.com/library/ms404677.aspx",
                "https://msdn.microsoft.com/library/ff730837.aspx",
                "https://mail.ru",
                "https://yandex.ru",
                "https://google.ru",
                "https://habrahabr.ru",
                "https://github.com/",
                //"https://logiclike.com/",
                //"https://www.ebay.com/",
                //"https://trello.com/",
                //"https://www.codeproject.com/Articles/140267/Create-Custom-Windows-in-WPF-with-Ease",
            };            
        }

        /*public DelegateCommand ClickCommand => new DelegateCommand((obj) =>
        {
            Clicks++;
        }, (obj) => Clicks < 10);*/

        private Page _view;
        public Page View
        {
            get { return _view; }
            set
            {
                _view = value;
                OnPropertyChanged();
            }
        }

        private int _clicks;
        public int Clicks
        {
            get { return _clicks; }
            set
            {
                _clicks = value;
                //_pageService.ChangePage(new Page1());
                //_pageService.OnPageChanged += (page) => View = page;
                //_pageService.ChangePage(new Page1()); 
                OnPropertyChanged();
            }
        }
    }
}
