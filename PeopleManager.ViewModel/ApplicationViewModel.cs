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
        private readonly IMapper _mapper;
        private ObservableCollection<PersonDto> _people;

        public ApplicationViewModel(IPeopleRepository repository, IMapper mapper)
        {
            _peopleRepository = repository;
            _mapper = mapper;

            LoadPeopleAsyncCommand = new AsyncCommand(async () =>
            {
                var data=  await _peopleRepository.GetPeopleAsync();
                var peopleSource = _mapper.Map<List<PersonDto>> (data.ToList());
                People = new ObservableCollection<PersonDto>(peopleSource);
                foreach (var person in People)
                {
                    person.CreateSnapShot();
                }   
               
            }, ()=>true);

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
            }); //todo
        }
        

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<PersonDto>? People
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

   
       

      
    }
}
