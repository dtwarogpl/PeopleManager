using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PeopleManager.Domain.Models;
using PeopleManager.Domain.Services;

namespace PeopleManager.Application.Queries;

public class GetPeople
{
    public record Query() : IRequest<IEnumerable<Person>>;

    internal class Handler : IRequestHandler<Query, IEnumerable<Person>>
    {
        private readonly IPeopleRepository _repository;

        public Handler(IPeopleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Person>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _repository.GetPeopleAsync();
        }
    }

    
}