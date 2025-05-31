namespace Zadanie10.DTOs;

public class TripsPagedResponseDto
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public IEnumerable<TripsDetailsGetDto> Trips { get; set; }  
}