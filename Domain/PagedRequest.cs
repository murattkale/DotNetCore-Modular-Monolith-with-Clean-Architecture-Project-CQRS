using System.Linq.Expressions;

namespace Domain;

public class PagedRequest<T>
{
    public Expression<Func<T, bool>>? Predicate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public Expression<Func<T, object>>? OrderBy { get; set; }
    public string? OrderByColumn { get; set; }
    public bool OrderByDescending { get; set; } = false;
    public bool Cache { get; set; } = true;
    public bool AsNoTracking { get; set; } = true;
    public Expression<Func<T, object>>[]? Includes { get; set; }
    // public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
}

public class PagedResponse<T>
{
    /// <summary>
    /// The draw counter that this object is a response to - from the draw parameter sent as part of the data request.
    /// Note that it is strongly recommended for security reasons that you cast this parameter to an integer, rather than simply echoing back to the client what it sent in the draw parameter, in order to prevent Cross Site Scripting (XSS) attacks.
    /// </summary>
    public int draw { get; set; }

    /// <summary>
    /// Total records, before filtering (i.e. the total number of records in the database)
    /// </summary>
    public int recordsTotal { get; set; }

    /// <summary>
    /// Total records, after filtering (i.e. the total number of records after filtering has been applied - not just the number of records being returned for this page of data).
    /// </summary>
    public int recordsFiltered { get; set; }

    /// <summary>
    /// The data to be displayed in the table.
    /// This is an array of data source objects, one for each row, which will be used by DataTables.
    /// Note that this parameter's name can be changed using the ajaxDT option's dataSrc property.
    /// </summary>
    //public List<T> data { get; set; }
    public List<T> data { get; set; }

    //Customized Field..Pls Ignore
    public string LastModifiedOn { get; set; }
}