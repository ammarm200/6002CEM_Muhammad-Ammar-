using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoListPage : ContentPage
    {
        public TodoListPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            TodoItemDatabase database = await TodoItemDatabase.Instance;
            var todoItems = await database.GetItemsAsync();

            var sortedItems = todoItems.OrderByDescending(item => item.IsPinned).ToList();

            listView.ItemsSource = new ObservableCollection<TodoItem>(sortedItems);
        }


        async void OnNewAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoItemPage
            {
                BindingContext = new TodoItem()
            });
        }

        async void OnListSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new TodoItemPage
                {
                    BindingContext = e.SelectedItem as TodoItem
                });
            }
        }

        async private void OnDeleteCompletedClicked(object sender, EventArgs e)
        {
            var todoItems = (listView.ItemsSource as ObservableCollection<TodoItem>);

            if (todoItems != null)
            {
                var completedItems = todoItems.Where(item => item.Done).ToList();

                foreach (var item in completedItems)
                {
                    todoItems.Remove(item);
                }

                foreach (var item in completedItems)
                {
                    TodoItemDatabase database = await TodoItemDatabase.Instance;
                    await database.DeleteItemAsync(item);
                }
            }
        }

        public async void OnFilterClicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Filter Tasks", "Cancel", null, "Done tasks", "Important tasks", "Pin tasks");
            ApplyFilter(action);
        }

        private void ApplyFilter(string filter)
        {
            var todoItems = (listView.ItemsSource as ObservableCollection<TodoItem>);
            if (filter == null) return;

            switch (filter)
            {
                case "Done tasks":
                    listView.ItemsSource = todoItems.Where(task => task.Done).ToList();
                    break;
                case "Important tasks":
                    listView.ItemsSource = todoItems.Where(task => task.IsImportant).ToList();
                    break;
                case "Pin tasks":
                    listView.ItemsSource = todoItems.Where(task => task.IsPinned).ToList();
                    break;
                // Handle other filters as needed
                default:
                    listView.ItemsSource = todoItems; // or apply another default filter
                    break;
            }
        }


    }
}