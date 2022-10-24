using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using PeopleManager.Application;

using PeopleManager.Domain.Models;
using PeopleManager.Domain.Services;
using PeopleManager.ViewModel.Abstractions;
using PeopleManager.ViewModel.Commands;
using PeopleManager.ViewModel.Models;

namespace PeopleManager.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged , IViewModel
    {

        private readonly IPeopleRepository _peopleRepository;
        protected Visibility _progressIndicatorVisibility = Visibility.Visible;
        private IMapper _mapper;

        public ApplicationViewModel(IPeopleRepository repository, IMapper mapper)
        {
            _peopleRepository = repository;
            _mapper = mapper;

            LoadPeopleAsyncCommand = new AsyncCommand(async () =>
            {
                var data=  await _peopleRepository.GetPeopleAsync();
                var peopleSource = _mapper.Map<List<PersonDto>> (data.ToList());
                People = new ObservableCollection<PersonDto>(peopleSource);
                OnPropertyChanged(nameof(People));
                ProgressIndicatorVisibility = Visibility.Collapsed;
            });
        }

        public Visibility ProgressIndicatorVisibility
        {
            get => _progressIndicatorVisibility;
            private set
            {
                if (value == _progressIndicatorVisibility) return;
                _progressIndicatorVisibility = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<PersonDto> People { get; set; }


        public IAsyncCommand LoadPeopleAsyncCommand { get; private set; }

   
       

      
    }
}
