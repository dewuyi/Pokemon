Install an IDE of your choice, either visual studio or Rider and run the project.
On project start a local swagger should display in your browser where you can test the endpoint.
Alternatively use postman or powershell to query the endpoint
For basic pokemon the endpoint is http GET https://localhost:portnumber/pokemon/{pokemonName}

For translated pokemon the endpoint is http GET https://localhost:portnumber/translated/{pokemonName}.

If this was in production the api will require an api key in the header to ensure the user has permissions to call the api endpoints.
The application will also not display swagger on startup, user will have to go to the swagger url to see swagger.
The unit test will use a pokemon builder to build pokemon objects