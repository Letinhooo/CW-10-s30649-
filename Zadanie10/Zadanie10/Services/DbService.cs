using Microsoft.EntityFrameworkCore;
using Zadanie10.Data;
using Zadanie10.DTOs;
using Zadanie10.Exceptions;
using Zadanie10.Models;

namespace Zadanie10.Services;

public interface IDbService
{
    public Task<TripsPagedResponseDto> GetTripsDetailsAsync(int page, int pageSize);
    public Task DeleteClientAsync(int clientId);
    public Task AddClientToTripAsync(int tripId, AddClientToTripDto dto);
}
public class DbService(MasterContext data): IDbService
{
    public async Task<TripsPagedResponseDto> GetTripsDetailsAsync(int page = 1, int pageSize = 10)
    {
        var query = data.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom);

        var totalCount = await query.CountAsync();
        var allPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var trips = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripsDetailsGetDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountriesDetailsGetDto
                {
                    Name = c.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientsGetDetailsDto
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            })
            .ToListAsync();

        return new TripsPagedResponseDto
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = allPages,
            Trips = trips
        };
    }

    public async Task DeleteClientAsync(int clientId)
    {
        var client = await data.Clients.FirstOrDefaultAsync(c=>c.IdClient == clientId);
        if (client is null)
        {
            throw new NotFoundException("Nie ma takiego clienta");
        }
        bool hasTrips = await data.ClientTrips.AnyAsync(c => c.IdClient == clientId);
        if (hasTrips)
        {
            throw new InvalidOperationException("Nie można usunąć klienta, który ma przypisane wycieczki.");
        }
        data.Clients.Remove(client);
        await data.SaveChangesAsync();
    }

    public async Task AddClientToTripAsync(int tripId, AddClientToTripDto dto)
    {
        var client = await data.Clients.FirstOrDefaultAsync(c=>c.Pesel == dto.Pesel);
        if (client is not null)
        {
            bool hasTrips = await data.ClientTrips
                .AnyAsync(ct => ct.IdTrip == tripId && ct.IdClient == client.IdClient);
            if (hasTrips)
            {
                throw new Exception("Klient jest przypisany na te wycieczke");
            }
        }
        var trip = await data.Trips
            .FirstOrDefaultAsync(t=>t.IdTrip == tripId);
        if (trip is null)
        {
            throw new NotFoundException("Nie ma takiej wycieczki");
        }

        if (trip.DateFrom <= DateTime.Now)
        {
            throw new Exception("Ta wycieczka juz sie skonczyla");
        }

        if (client is null)
        {
            client = new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Pesel = dto.Pesel,
                Email = dto.Email,
                Telephone = dto.Telephone,
            };
            data.Clients.Add(client);
            await data.SaveChangesAsync();
        }

        var clientTrip = new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = trip.IdTrip,
            RegisteredAt = 10,
            PaymentDate = dto.PaymentDate
        };
        data.ClientTrips.Add(clientTrip);
        await data.SaveChangesAsync();
    }
}