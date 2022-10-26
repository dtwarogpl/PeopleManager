using System;
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
using Bogus.DataSets;
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

            LoadPeopleAsyncCommand = new AsyncCommand(async () =>
            {
                await LoadPeopleAsync();
            }, ()=>true);

            SaveCommand = new AsyncCommand(async () =>
            {
                var peopleSource = _mapper.Map<List<Person>>(People.ToList());
                await _peopleRepository.SavePeopleAsync(peopleSource);
               
                SnapShot = new ObservableCollection<PersonDto>(People.Select(x=>x.Duplicate()));
            },()=>
            {
                
                var allHaveGoodState = People.All(y => !y.IsCorrupted);
                return allHaveGoodState && !CurrentStateIsEqualToSnapShoot();
            });

            DiscardChangesCommand = new AsyncCommand(async () =>
            {
                
                await LoadPeopleAsync();
            }, () =>
            {
               
                //if (CurrentStateIsEqualToSnapShoot()) return false;
                return !CurrentStateIsEqualToSnapShoot();
            });

            DeletePersonCommand = new AsyncCommand(async () =>
            {
               DeleteSelected();
            }, () => false);

        }

        private bool CurrentStateIsEqualToSnapShoot()
        {
            return People.SequenceEqual(SnapShot, new PersonEqualityComparer());
        }

        public ObservableCollection<PersonDto> SnapShot = new();
        private async Task LoadPeopleAsync()
        {
          
            var data = await _peopleRepository.GetPeopleAsync();
            var enumerable = data.ToList();
            People = new ObservableCollection<PersonDto>(_mapper.Map<List<PersonDto>>(enumerable.ToList()));
            SnapShot = new ObservableCollection<PersonDto>(_mapper.Map<List<PersonDto>>(enumerable.ToList()));
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

    public class PersonEqualityComparer : IEqualityComparer<PersonDto>
    {
        public bool Equals(PersonDto x, PersonDto y)
        {
            return x.HasSamePropertiesAs(y);
        }

        public int GetHashCode(PersonDto obj)
        {
            var hashCode = new HashCode();
            hashCode.Add(obj.ApartmentNumber);
            hashCode.Add(obj.DateOfBirth);
            hashCode.Add(obj.FirstName);
            hashCode.Add(obj.HouseNumber);
            hashCode.Add(obj.LastName);
            hashCode.Add(obj.PhoneNumber);
            hashCode.Add(obj.PostalCode);
            hashCode.Add(obj.StreetName);
            hashCode.Add(obj.Town);
            return hashCode.ToHashCode();
        }

     
    }
}
