namespace APBD6.DTOs;


public record CreateAnimalRequest(string Name, string Description, string Category, string Area);

public record CreateAnimalResponse(int id, string Name, string Description, string Category, string Area)
{
    public CreateAnimalResponse(int id, CreateAnimalRequest request): this(id, request.Name, request.Description, request.Category, request.Area){}
};