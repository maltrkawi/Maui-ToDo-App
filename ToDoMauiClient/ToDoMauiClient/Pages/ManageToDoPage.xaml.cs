using System.Diagnostics;
using ToDoMauiClient.DataServices;
using ToDoMauiClient.Models;

namespace ToDoMauiClient.Pages;

[QueryProperty(nameof(ToDo), "ToDo")]
public partial class ManageToDoPage : ContentPage
{
	private readonly IRestDataService _dataService;
	ToDo _toDo;
	bool _isNew;

	public ToDo ToDo
	{
		get => _toDo;
		set
		{
			_isNew = IsNew(value);
			_toDo = value;

			// it's required when it comes to updating existing item!
			OnPropertyChanged();
		}
	}

	public ManageToDoPage(IRestDataService dataService)
	{
		InitializeComponent();

		_dataService = dataService;
		BindingContext = this;
	}

	bool IsNew(ToDo toDo)
	{
		if (toDo.Id == 0)
			return true;
		return false;
	}

	async void OnSaveButtonClicked(object sender, EventArgs e)
	{
		if (_isNew)
		{
			Debug.WriteLine("--> Adding new item");
			await _dataService.AddToDoAsync(ToDo);
		}
		else
		{
            Debug.WriteLine("--> Updating existing item");
			await _dataService.UpdateToDoAsync(ToDo);
        }

        await Shell.Current.GoToAsync("..");
    }

	async void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		await _dataService.DeleteToDoAsync(ToDo.Id);
		await Shell.Current.GoToAsync("..");
	}

	async void OnCancelButtonClicked(object sender, EventArgs e)
	{
		// navigate to main page
		await Shell.Current.GoToAsync("..");
	}
}