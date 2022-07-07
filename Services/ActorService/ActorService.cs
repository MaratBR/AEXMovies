using AEXMovies.Models;
using AEXMovies.Repositories.ActorRepository;
using AEXMovies.Services.Dtos;
using AutoMapper;

namespace AEXMovies.Services.ActorService;

public class ActorService : IActorService
{
    private readonly IActorRepository _actorRepository;
    private readonly IMapper _mapper;

    public ActorService(IActorRepository actorRepository, IMapper mapper)
    {
        _actorRepository = actorRepository;
        _mapper = mapper;
    }

    public Task<ActorDetailsDto?> Get(int id)
    {
        return _actorRepository.GetById<ActorDetailsDto>(id);
    }

    public async Task<ActorDto> Create(CreateActorDto dto)
    {
        var actor = new Actor
        {
            Name = dto.Name
        };
        await _actorRepository.Save(actor);
        return _mapper.Map<ActorDto>(actor);
    }

    public async Task DeleteById(int id)
    {
        var actor = await _actorRepository.GetByIdOrThrow(id);
        await _actorRepository.MarkAsDeleted(actor);
    }

    public async Task RestoreById(int id)
    {
        var actor = await _actorRepository.GetDeletedByIdOrThrow(id);
        await _actorRepository.UnmarkAsDeleted(actor);
    }

    public async Task<ActorDto> Update(int id, UpdateActorDto dto)
    {
        var actor = await _actorRepository.GetByIdOrThrow(id);
        if (dto.Name != null)
            actor.Name = dto.Name;
        await _actorRepository.Save(actor);
        return _mapper.Map<ActorDto>(actor);
    }

    public Task<List<ActorDto>> Search(string? query)
    {
        return _actorRepository.FindMany<ActorDto>(query == null ? null : a => a.Name.Contains(query), take: 500);
    }
}