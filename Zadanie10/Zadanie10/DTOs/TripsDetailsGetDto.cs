using Zadanie10.Models;

namespace Zadanie10.DTOs;

public class TripsDetailsGetDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public ICollection<CountriesDetailsGetDto> Countries { get; set; }
    public ICollection<ClientsGetDetailsDto> Clients { get; set; }
}

public class CountriesDetailsGetDto
{
    public string Name { get; set; }
}

public class ClientsGetDetailsDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

