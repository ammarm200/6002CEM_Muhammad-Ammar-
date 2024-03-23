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


        private void (object sender, EventArgs e)
        {
            var todoItems = (listView.ItemsSource as ObservableCollection<TodoItem>);

            if (todoItems != null)
            {
                var completedItems = todoItems.Where(item => item.Done).ToList();

                // Remove completed tasks from the collection
                foreach (var item in completedItems)
                {
                    todoItems.Remove(item);
                }
            }
        }
    }
}