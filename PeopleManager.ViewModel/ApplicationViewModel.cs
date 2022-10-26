using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
        private readonly IMapper _mapper;
        private ObservableCollection<PersonDto> _people = new();

        public ApplicationViewModel(IPeopleRepository repository, IMapper mapper)
        {
            _peopleRepository = repository;
            _mapper = mapper;

            LoadPeopleAsyncCommand = new AsyncCommand(async () => { await LoadPeopleAsync(); }, ()=>true);

            SaveCommand = new AsyncCommand(async () =>
            {
                var peopleSource = _mapper.Map<List<Person>>(People.ToList());
                await _peopleRepository.SavePeopleAsync(peopleSource);
                foreach (var person in People!)
                {
                    person.CreateSnapShot();
                }
            },()=>
            {
                if (CurrentStateIsEqualToSnapShoot()) return false;

                var somethingMidfied = People.Any(x => x.HasBeenModified());
                var allHaveGoodState = People.All(y => !y.IsCorrupted);

                var SomethingHasBeenAdded = Modified.Count>0;
                var SomethingDeleted = Removed.Count > 0;

                return allHaveGoodState && (somethingMidfied || SomethingHasBeenAdded || SomethingDeleted);
            });

            DiscardChangesCommand = new AsyncCommand(async () =>
            {
                await LoadPeopleAsync();
            }, () =>
            {
                if (CurrentStateIsEqualToSnapShoot()) return false;
                return (People.Any(x => x.HasBeenModified()) || Modified.Count > 0 || Removed.Count>0);
            });

            DeletePersonCommand = new AsyncCommand(async () =>
            {
               DeleteSelected();
            }, () => false);

        }

        private bool CurrentStateIsEqualToSnapShoot()
        {
            return People.SequenceEqual(SnapShot);
        }

        public ObservableCollection<PersonDto> SnapShot = new();
        private async Task LoadPeopleAsync()
        {
            SnapShot.Clear();
            People?.Clear();
            Modified.Clear();
            var data = await _peopleRepository.GetPeopleAsync();
            var peopleSource = _mapper.Map<List<PersonDto>>(data.ToList());
            People = new ObservableCollection<PersonDto>(peopleSource);
            SnapShot = new ObservableCollection<PersonDto>(peopleSource);
            foreach (var person in People)
            {
                person.CreateSnapShot();
            }

            People.CollectionChanged += CollectionChangedMethod;
           
        }

        private List<PersonDto> Modified = new();
        private List<PersonDto> Removed = new();
     

        private void CollectionChangedMethod(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add when e.NewItems is null:
                    return;
                case NotifyCollectionChangedAction.Add:
                    Modified.AddRange(e.NewItems!.Cast<PersonDto>().Where(item => item is not null));
                    break;
                case NotifyCollectionChangedAction.Remove when e.OldItems is null:
                    return;
                case NotifyCollectionChangedAction.Remove:
                    Removed.AddRange(e.OldItems!.Cast<PersonDto>().Where(item => item is not null));
                    break;
            }
        }

        private void LoadItems(NotifyCollectionChangedEventArgs e, List<PersonDto> destination)
        {
            destination.AddRange(e.NewItems!.Cast<PersonDto>().Where(item => item is not null));
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


        public IAsyncCommand LoadPeopleAsyncCommand { get; private set; }
        public IAsyncCommand SaveCommand { get; private set; }
        public IAsyncCommand DiscardChangesCommand { get; private set; }

        public ICommand DeletePersonCommand { get; set; }

        private void DeleteSelected()
        {
            People.Remove(SelectedItem);
        }

        private PersonDto _selectedItem;
        public PersonDto SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }
    }
}
