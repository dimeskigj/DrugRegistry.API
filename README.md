# DrugRegistry.API

This Swagger document contains the API specification for DrugRegistry, which is a platform for querying information about drugs and pharmacies.
Paths

## The following paths are available:

### /api/drugs/search

This endpoint allows users to search for drugs based on a query string. It accepts the following parameters:

    query: a string representing the search query
    page (optional): an integer representing the page number of the search results
    size (optional): an integer representing the number of results to show per page

### /api/pharmacies/byLocation

This endpoint allows users to search for pharmacies based on their location. It accepts the following parameters:

    lon: a double representing the longitude of the user's location
    lat: a double representing the latitude of the user's location
    page (optional): an integer representing the page number of the search results
    size (optional): an integer representing the number of results to show per page
    municipality (optional): a string representing the municipality of the pharmacy
    place (optional): a string representing the place of the pharmacy

### /api/pharmacies/search

This endpoint allows users to search for pharmacies based on their name and address. It accepts the following parameters:

    query: a string representing the search query
    page (optional): an integer representing the page number of the search results
    size (optional): an integer representing the number of results to show per page
    municipality (optional): a string representing the municipality of the pharmacy
    place (optional): a string representing the place of the pharmacy

### /api/pharmacies/municipalitiesByFrequency

This endpoint allows users to query the municipalities where the most pharmacies are located. It does not accept any parameters.
### /api/pharmacies/placesByFrequency

This endpoint allows users to query the places where the most pharmacies are located. It does not accept any parameters.

## Components

The following components are used in the API:

    DrugPagedResult: a paged result containing a list of drugs or pharmacies and pagination information.