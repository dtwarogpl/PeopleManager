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
                await _peopleRepository.SavePeopleAsync(Enumerable.Empty<Person>());
                foreach (var person in People!)
                {
                    person.CreateSnapShot();
                }
            },()=>
            {
                return People!=null && People.Any(x=>x.HasBeenModified());
            });

            DiscardChangesCommand = new AsyncCommand(async () =>
            {
                await LoadPeopleAsync();
            }, () =>
            {
                var result = (People.Any(x => x.HasBeenModified()) || Modified.Count > 0);
                return result;
            });

            DeletePersonCommand = new AsyncCommand(async () =>
            {
               DeleteSelected();
            }, () => false);

        }

      

        private async Task LoadPeopleAsync()
        {
        
            People?.Clear();
            Modified.Clear();
            var data = await _peopleRepository.GetPeopleAsync();
            var peopleSource = _mapper.Map<List<PersonDto>>(data.ToList());
            People = new ObservableCollection<PersonDto>(peopleSource);
            foreach (var person in People)
            {
                person.CreateSnapShot();
            }

            People.CollectionChanged += CollectionChangedMethod;
           
        }

        private List<PersonDto> Modified = new();
     

        private void CollectionChangedMethod(object? sender, NotifyCollectionChangedEventArgs e)
        {
            //different kind of changes that may have occurred in collection
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems is null) return;

                foreach (PersonDto item in e.NewItems)
                {
                    if (item is not null)
                    {
                        Modified.Add(item);
                    }
                }
            }
           
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems is null) return;
              
                foreach (PersonDto item in e.OldItems)
                {
                    if (item is not null)
                    {
                      Modified.Add(item);  
                    }
                }
            }
           
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
            if (null != SelectedItem)
            {
                People?.Remove(SelectedItem);
            }
        }

        private PersonDto _selectedItem;
        public PersonDto SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
    }
}
