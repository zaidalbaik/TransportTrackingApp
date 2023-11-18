using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TT_App.Models;
using TT_App.Services;
using TT_App.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TT_App.ViewModels
{
    public class LinesListViewModel : BaseViewModel
    {
        public List<Line> Lines { get; set; }

        public Command FillLinesCommand { get; }

        public Command SearchLineCommand { get; }
        public Command GoToMapPageCommand { get; }

        public LinesListViewModel()
        {
            lineService = new LineService();

            FillLinesCommand = new Command(async () => await FillLines());
            FillLinesCommand.Execute(null);

            SearchLineCommand = new Command(() => SearchLine());

            GoToMapPageCommand = new Command(async () => await GoToMapPageAsync(), () => !IsBusy);
        }

        private string textSearch;
        public string TextSearch
        {
            get { return textSearch; }
            set
            {
                textSearch = value;
                OnPropertyChanged();
                Lines = LinesTemp;
                OnPropertyChanged(nameof(Lines));

            }
        }

        private Line selectedLine;
        public Line SelectedLine
        {
            get { return selectedLine; }
            set
            {
                selectedLine = value;
                OnPropertyChanged();

                GoToMapPageCommand.Execute(null);
            }
        }

        private List<Line> linesTemp;
        public List<Line> LinesTemp
        {
            get { return linesTemp; }
            set
            {
                linesTemp = value;
                OnPropertyChanged();
            }
        }

        public async Task GoToMapPageAsync()
        {
            try
            {
                if (SelectedLine == null)
                    return;

                IsBusy = true;
                var lineSelected = JsonConvert.SerializeObject(SelectedLine);
                Preferences.Set("LineChosenByUser", lineSelected);

                await App.Current.MainPage.Navigation.PushAsync(new MapUserPage());
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                IsBusy = false;
            }

        }
        public async Task FillLines()
        {
            try
            {
                Lines = await lineService.GetAllLinesAsync();
                OnPropertyChanged(nameof(Lines));
                LinesTemp = Lines;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SearchLine()
        {
            try
            {
                List<Line> ResultOfSearch = new List<Line>();

                if (string.IsNullOrEmpty(TextSearch))
                    return;

                if (Lines == null)
                    return;
                foreach (var line in Lines)
                {
                    if (line.LineName.Trim().ToLower() == TextSearch.Trim().ToLower())
                    {
                        ResultOfSearch.Add(line);
                        Lines = ResultOfSearch;
                        OnPropertyChanged(nameof(Lines));
                    }
                
                }
                 
            }
            catch (Exception)
            {

                throw;
            }

        }
        public override void CommandStateChanged()
        {
            GoToMapPageCommand?.ChangeCanExecute();
        }
    }
}
