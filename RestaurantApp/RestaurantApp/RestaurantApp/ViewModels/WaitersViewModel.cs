using MvvmHelpers;
using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class WaitersViewModel : BaseViewModel
    {
        private WaitersApiService _waitersApiService = new WaitersApiService();

        public WaitersViewModel()
        {
            Waiters = new ObservableRangeCollection<WaiterItem>();
        }

        public ObservableRangeCollection<WaiterItem> Waiters
        {
            get; private set;
        }


        public async void Refresh(object sender, EventArgs e)
        {
            await ExecuteLoadWaitersCommand();
        }

        public ICommand LoadWaiters
        {
            get
            {
                return new Command(async () => await ExecuteLoadWaitersCommand());
            }
        }

        public ICommand DeleteWaiterCommand
        {
            get
            {
                return new Command<string>(async (id) =>
                {
                    await _waitersApiService.DeleteWaiter(id);
                    Refresh(this, EventArgs.Empty);
                });
            }
        }

        private async Task ExecuteLoadWaitersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var items = await _waitersApiService.GetWaiters();
                Waiters.ReplaceRange(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
