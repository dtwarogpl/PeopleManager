using Microsoft.Extensions.Options;
using PeopleManager.Domain.Exceptions;
using PeopleManager.Domain.Models;
using PeopleManager.Domain.Services;
using PeopleManager.Infrastructure.Options;
using PeopleManager.Infrastructure.Services.Serialization;
using PeopleManager.Infrastructure.Services.TextHandling;

namespace PeopleManager.Infrastructure;

public class XmlDataRepository : IPeopleRepository
{
    private readonly IAsyncDataFileHandler _asyncDataFileHandler;
    private readonly IDataSerializer<DataStorageModel> _dataSerializer;
    private XmlDataFileOptions Options { get; }

    string DataFilePath => Environment.ExpandEnvironmentVariables(Options.Path!);

    public XmlDataRepository(IOptions<XmlDataFileOptions> options, IAsyncDataFileHandler asyncDataFileHandler,
        IDataSerializer<DataStorageModel> dataSerializer)
    {
        ValidateOptions(options);

        _asyncDataFileHandler = asyncDataFileHandler;
        _dataSerializer = dataSerializer;
        Options = options.Value;
    }

    private static void ValidateOptions(IOptions<XmlDataFileOptions> options)
    {
        if (options.Value is null || string.IsNullOrEmpty(options.Value.Path))
            throw new PeopleManagerDomainException("No data file path configured");
    }

    public async Task<IEnumerable<Person>> GetPeopleAsync()
    {
        var rawData = await _asyncDataFileHandler.LoadTextAsync(DataFilePath);
        return string.IsNullOrEmpty(rawData) ? DataStorageModel.Empty().People : _dataSerializer.Deserialize(rawData).People;
    }

    public async Task SavePeopleAsync(List<Person> people)
    {
        var dataStorage = DataStorageModel.Create(people);
        var serialized = _dataSerializer.Serialize(dataStorage);
        await _asyncDataFileHandler.FileWriteAsync(DataFilePath, serialized);
    }
}