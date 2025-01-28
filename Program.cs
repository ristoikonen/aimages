using aimages;
using aimages.Models;
using Microsoft.AspNetCore.Mvc;
using System.Buffers;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost",
                                "http://localhost:4200");
        });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//app.UseCors(x => x
//            .AllowAnyOrigin()
//            .AllowAnyMethod()
//            .AllowAnyHeader());





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();



//=---------------------------------
// /api/v{version:apiVersion}
// .WithApiVersionSet(versionSet)



var pathToImages = @"C:\tmp\images";


var imagesGroup = app
    .MapGroup("/images/")
    .WithTags("Images");


imagesGroup.MapGet(string.Empty, GetAImageDetails)
    .WithName("GetImageDetails")
    .WithOpenApi(operation =>
     {
        operation.Summary = "Get a list of textual image data";
        operation.Description = "List of AImage object: Name, FileName, Desc";
        return operation;
    });

imagesGroup.MapGet("names", GetAImages)
    .WithName("GetImages")
    .WithOpenApi(operation =>
    {
        operation.Summary = "Get a list names";
        operation.Description = "List names";
        return operation;
    });

// (int? pageNumber) => $"Requesting page {pageNumber ?? 1}


imagesGroup.MapGet("search/{desc}", SearchByDesc)
    .WithName("SearchByDesc")
    .WithOpenApi(operation =>
    {
        operation.Summary = "Search for image(s)";
        operation.Description = "Search of text in images Desc -field";
        return operation;
    });

imagesGroup.MapGet("id/{desc}", (  [FromQuery(Name = "desc")] string desc )
                     => { return "ABC"; });


//imagesGroup.MapGet("search/{desc}", ([FromQuery(Name = "desc")] string desc)
//                     => { return "ABC"; });


//imagesGroup.MapGet("images/{desc}", SearchByDesc)
//    .WithName("SearchByDesc")
//    .WithOpenApi(operation =>
//    {
//        operation.Summary = "Search for image(s)";
//        operation.Description = "Search of text in images Desc -field";
//        return operation;
//    });
//imagesGroup.MapGet(string.Empty, SearchByDesc)
//    .WithName("SearchByDesc")
//    .WithOpenApi(operation =>
//    {
//        operation.Summary = "Search for image(s)";
//        operation.Description = "Search of text in images Desc -field";
//        return operation;
//    });


/*
app.MapGet("/images", () =>
{
    var imagelist = aimages.DirAnalyser.GetImages(pathToImages);
    return imagelist;
})
.WithName("GetImages");
*/


List<AImage> GetAImageDetails()
{
    return new List<AImage>(aimages.DirAnalyser.GetImages(pathToImages));

}

IResult GetAImages()
{
    return Results.Json("Test me"); // List<AImage>(aimages.DirAnalyser.GetImages(pathToImages));
}


List<AImage> SearchByDesc(string desc)
{
    List<AImage> lista = new List<AImage>();

    // get collection of AImages
    ImageFileVisitor visitor = new ImageFileVisitor(pathToImages, "Descriptions");
    var list = visitor.ReadDirectory(pathToImages);

    foreach ( AImage image in list )
    { 

        // search for text in 'Desc' field of AImages
        if (!string.IsNullOrWhiteSpace(image.Desc))
        {
            SearchValues<string> searchValues = SearchValues.Create(image.Desc.Split(new char[] { ' ', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries), StringComparison.OrdinalIgnoreCase);
            //var ixx = image.Desc.AsSpan().ContainsAny(desc);

            // testing BinarySearch..
            var splitArray = image.Desc.Split(new char[] { ' ', '.', ',', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            int index = Array.BinarySearch(splitArray, desc);

            if (searchValues.Contains(desc))
            {
                lista.Add(image);
            }
        }

    }
    return lista;
}


app.Run();


