// Replace the old logic with this:
private List<SensorData> FilteredSensors = new();
private string _searchText = "";
private string? SelectedLocationFilter = null;

// We replace the auto getter with a property having a setter to intercept input
private string SearchText
{
    get => _searchText;
    set
    {
        _searchText = value;
        _ = ExecuteSearch(); // We relaunch the SQL search
    }
}

protected override async Task OnInitializedAsync()
{
    await ExecuteSearch();
}

private async Task OnChartClick(SeriesClickEventArgs args)
{
    string clickedLocation = args.Category.ToString();
    SelectedLocationFilter = SelectedLocationFilter == clickedLocation ? null : clickedLocation;
    await ExecuteSearch(); // We relaunch the SQL search
}

// The central method that queries the database
private async Task ExecuteSearch()
{
    FilteredSensors = await SensorService.SearchSensorsAsync(SelectedLocationFilter, SearchText);
}