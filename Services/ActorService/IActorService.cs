using AEXMovies.Models;
using AEXMovies.Repositories.Exceptions;
using AEXMovies.Services.Dtos;

namespace AEXMovies.Services.ActorService;

public interface IActorService
{
    Task<ActorDetailsDto?> Get(int id);
    
    /// <summary>
    /// Creates new actor based on provided info
    /// </summary>
    /// <param name="dto">DTO object with new actor's information</param>
    /// <returns>Task that represents the asynchronous operation.</returns>
    Task<ActorDto> Create(CreateActorDto dto);
    
    /// <summary>
    /// Deletes actor with given ID. Throws an exception if actor does not exist.
    /// </summary>
    /// <param name="id">ID of actor</param>
    /// <returns>Task that represents the asynchronous operation.</returns>
    /// <exception cref="RecordNotFoundException">when actor with given ID does not exist</exception>
    Task DeleteById(int id);

    /// <summary>
    /// Restores previously deleted actor.
    /// </summary>
    /// <param name="id">ID of actor</param>
    /// <returns>Task that represents the asynchronous operation.</returns>
    /// <exception cref="RecordNotFoundException">if the actor does not exist or not deleted</exception>
    Task RestoreById(int id);

    Task<ActorDto> Update(int id, UpdateActorDto dto);
}