using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using PeopleManager.Domain.Models;
using PeopleManager.Domain.Services;
using PeopleManager.ViewModel.Abstractions;
using PeopleManager.ViewModel.Commands;
using PeopleManager.ViewModel.Helpers;
using PeopleManager.ViewModel.Models;

namespace PeopleManager.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged , IViewModel
    {

        private readonly IPeopleRepository _peopleRepository;
        private readonly IMapper _mapper;
        private ObservableCollection<PersonDto> _people = new();

        public IAsyncCommand LoadPeopleAsyncCommand { get; }
        public IAsyncCommand SaveCommand { get; }
        public IAsyncCommand DiscardChangesCommand { get; }


        public ApplicationViewModel(IPeopleRepository repository, IMapper mapper)
        {
            _peopleRepository = repository;
            _mapper = mapper;

            LoadPeopleAsyncCommand = new AsyncCommand(async () =>
            {
                await LoadPeopleAsync();
            }, ()=>true);

            SaveCommand = new AsyncCommand(async () =>
            {
                var peopleSource = _mapper.Map<List<Person>>(People.ToList());
                await _peopleRepository.SavePeopleAsync(peopleSource);
               
                _snapShot = new ObservableCollection<PersonDto>(People.Select(x=>x.Duplicate()));
            },()=>
            {
                var allHaveGoodState = People.All(y => !y.IsCorrupted);
                return allHaveGoodState && !CurrentStateIsEqualToSnapShoot();
            });

            DiscardChangesCommand = new AsyncCommand(async () =>
            {
                await LoadPeopleAsync();
            }, () => !CurrentStateIsEqualToSnapShoot());
        }

        private bool CurrentStateIsEqualToSnapShoot()
        {
            return People.SequenceEqual(_snapShot, new PersonEqualityComparer());
        }

        private ObservableCollection<PersonDto> _snapShot = new();
        private async Task LoadPeopleAsync()
        {
            var data = await _peopleRepository.GetPeopleAsync();
            var enumerable = data.ToList();
            People = new ObservableCollection<PersonDto>(_mapper.Map<List<PersonDto>>(enumerable.ToList()));
            _snapShot = new ObservableCollection<PersonDto>(_mapper.Map<List<PersonDto>>(enumerable.ToList()));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<PersonDto> People
        {
            get => _people;
            set
            {
                if (Equals(value, _people)) return;
                _people = value;
                OnPropertyChanged();
            }
        }
    }
}
