namespace CQRSInAction.Application.Common.Exceptions;



public sealed class NotFoundException(string entity, Guid Id) : Exception($"The {entity} with id : '{Id}' not found.");