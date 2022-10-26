using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using PeopleManager.Domain.Services;
using PeopleManager.ViewModel.Abstractions;
using PeopleManager.ViewModel.Commands;
using PeopleManager.ViewModel.Models;

namespace PeopleManager.ViewModel
{
    public class ApplicationViewModel : ObservableDto, IViewModel
    {

        private readonly IPeopleRepository _peopleRepository;
        private readonly IMapper _mapper;
        private People _people = new();

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

            SaveCommand = new AsyncCommand(async () => { await People.SaveAsync(_mapper, _peopleRepository); },()=> People.CanBeSaved());

            DiscardChangesCommand = new AsyncCommand(async () =>
            {
                await LoadPeopleAsync();
            }, () => People.HaveBeenChanged());
        }
       
      
        private async Task LoadPeopleAsync()
        {
            var data = await _peopleRepository.GetPeopleAsync();
            var enumerable = data.ToList();
            People = new People(_mapper.Map<List<PersonDto>>(enumerable.ToList()));
          
        }


        public People People
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
