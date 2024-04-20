using System.Data.SqlClient;
using APBD6.DTOs;
using Dapper;
using FluentValidation;

namespace APBD6.endpoints;

public static class AnimalDapperEndpoints
{
    public static void RegisterAnimalDapperEndpoints(this WebApplication app)
    {
        var animals = app.MapGroup("minimal-animals-dapper");
        animals.MapGet("/", GetAnimals);
        animals.MapPost("/", CreateAnimal);
        animals.MapDelete("{id:int}", RemoveAnimal);
        animals.MapPut("{id:int}", ReplaceAnimal);
    }
    private static IResult GetAnimals(IConfiguration configuration, string? orderBy = "")
    {
        var orderColumnsName = new List<string> {"Id","Name", "Description", "Category", "Area"};
        string chosenColumn;
        if (string.IsNullOrEmpty(orderBy))
        {
            chosenColumn = "Name";
        }
        else if (orderColumnsName.Contains(orderBy))
        {
            chosenColumn = orderBy;
        }
        else
        {
            return Results.NotFound("This column does not exist, cannot sort using it!");
        }

        using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default")))
        {
            var animals = sqlConnection.Query<GetAllAnimalResponse>($"SELECT * FROM Animal ORDER BY {chosenColumn}");
            return Results.Ok(animals);
        }
    }

    private static IResult ReplaceAnimal(IConfiguration configuration, IValidator<ReplaceAnimalRequest> validator, int id, ReplaceAnimalRequest request)
    {
        var validation = validator.Validate(request);
        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }

        using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default")))
        {
            var affectedRows = sqlConnection.Execute(
                "UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE ID = @Id",
                new
                {
                    Name = request.Name,
                    Description = request.Description,
                    Category = request.Category,
                    Area = request.Area,
                    Id = id
                }
            );

            if (affectedRows == 0) return Results.NotFound();
        }

        return Results.NoContent();

    }
    private static IResult RemoveAnimal(IConfiguration configuration, int id)
    {
        using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default")))
        {
            var affectedRows = sqlConnection.Execute(
                "DELETE FROM Animal WHERE ID = @Id",
                new { Id = id }
            );
            return affectedRows == 0 ? Results.NotFound() : Results.NoContent();
        }
    }
    private static IResult CreateAnimal(IConfiguration configuration, IValidator<CreateAnimalRequest> validator, CreateAnimalRequest request)
    {
        var validation = validator.Validate(request);
        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }

        using (var sqlConnection = new SqlConnection(configuration.GetConnectionString("Default")))
        {
            var id = sqlConnection.ExecuteScalar<int>(
                "INSERT INTO Animal(Name, Description, Category, Area) values (@Name, @Description, @Category, @Area); SELECT CAST(SCOPE_IDENTITY() as int)",
                new
                {
                    Name = request.Name,
                    Description = request.Description,
                    Category = request.Category,
                    Area = request.Area
                }
            );

            return Results.Created($"/animal-dapper/{id}", new CreateAnimalResponse(id, request));
        }
    }


}