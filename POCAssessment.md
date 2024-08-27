### POC Assessment

This application produces a single Http Endpoint capable of returning a list of products.

This list can be filtered by applying a set of 3 filters.

- minprice (int)
- maxprice (int)
- size (string)

A forth parameter [highlight - array of strings] is used to enrich the description of the products.
it applies HTML styling to the specified words in the description.

The application was choose to be a single project for simplicity, and it follows a clean architecture pattern.
The layers are:

- Presentation
- Application
- Domain
- Infrasucture

If necessary these can be separated into matching projects to enforce information hiding boundaries and isolated responsibilities of each layer.

The application uses minimal endpoints and utilises the Carter library to isolate the implementation of the Http endpoint to aid testing.
The application also uses a mediatr library to help maintain separation of concerns.

To start:
run :
dotnet run --urls "https://<<specified url here>>"

there is a POCAssessment.http file that you can use to test the endpoint, or if you run under development you can use the swagger UI.
