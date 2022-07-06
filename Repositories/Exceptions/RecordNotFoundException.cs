namespace AEXMovies.Repositories.Exceptions;

[Serializable]
public class RecordNotFoundException : ModelNotFound
{
    public RecordNotFoundException(Type recordType, int id) : base($"Record {recordType.Name} with ID = {id} not found")
    {
    }
}