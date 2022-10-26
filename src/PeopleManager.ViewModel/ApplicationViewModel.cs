using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PeopleManager.Domain.Services;
using PeopleManager.ViewModel.Abstractions;
using PeopleManager.ViewModel.Commands;
using PeopleManager.ViewModel.Models;

namespace PeopleManager.ViewModel;

public class ApplicationViewModel : ObservableDto, IViewModel
{
    private readonly IMapper _mapper;
    private readonly IPeopleRepository _peopleRepository;
    private People _people = new();

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

    public IAsyncCommand LoadPeopleAsyncCommand { get; }
    public IAsyncCommand SaveCommand { get; }
    public IAsyncCommand DiscardChangesCommand { get; }

    public ApplicationViewModel(IPeopleRepository repository, IMapper mapper)
    {
        _peopleRepository = repository;
        _mapper = mapper;

        LoadPeopleAsyncCommand = new AsyncCommand(async () => { await LoadPeopleAsync(); }, () => true);

        SaveCommand = new AsyncCommand(async () => { await People.SaveAsync(_mapper, _peopleRepository); },
            () => People.CanBeSaved());

        DiscardChangesCommand =
            new AsyncCommand(async () => { await LoadPeopleAsync(); }, () => People.HaveBeenChanged());
    }

    private async Task LoadPeopleAsync()
    {
        var data = await _peopleRepository.GetPeopleAsync();
        People = new People(_mapper.Map<List<PersonDto>>(data.ToList()));
    }
}