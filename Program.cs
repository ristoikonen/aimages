using aimages.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//=---------------------------------
// /api/v{version:apiVersion}
// .WithApiVersionSet(versionSet)
var pathToImages = @"C:\tmp\images";


var imagesGroup = app
    .MapGroup("/images/")
    .WithTags("Images");


imagesGroup.MapGet(string.Empty, GetAImages)
    .WithName("GetImages")
    .WithOpenApi(operation =>
     {
        operation.Summary = "Get a list of textual image data";
        operation.Description = "List of AImage object: Name, FileName, Desc";
        return operation;
    });



/*
app.MapGet("/images", () =>
{
    var imagelist = aimages.DirAnalyser.GetImages(pathToImages);
    return imagelist;
})
.WithName("GetImages");
*/


List<AImage> GetAImages()
{
    return new List<AImage>(aimages.DirAnalyser.GetImages(pathToImages));

}


app.Run();


