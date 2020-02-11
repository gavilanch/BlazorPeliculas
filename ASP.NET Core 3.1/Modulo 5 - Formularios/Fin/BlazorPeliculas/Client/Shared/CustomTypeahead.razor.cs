using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace BlazorPeliculas.Client.Shared
{
    public class CustomTypeaheadBase<TItem> : ComponentBase, IDisposable
    {
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public TItem Value { get; set; }
        [Parameter] public EventCallback<TItem> ValueChanged { get; set; }
        [Parameter] public Func<string, Task<IEnumerable<TItem>>> SearchMethod { get; set; }
        [Parameter] public RenderFragment NotFoundTemplate { get; set; }
        [Parameter] public RenderFragment<TItem> ResultTemplate { get; set; }
        [Parameter] public RenderFragment<TItem> SelectedTemplate { get; set; }
        [Parameter] public RenderFragment FooterTemplate { get; set; }
        [Parameter] public int MinimumLength { get; set; } = 1;
        [Parameter] public int Debounce { get; set; } = 300;
        [Parameter] public int MaximumSuggestions { get; set; } = 25;
        [Parameter] public bool DisplayClear { get; set; } = true;

        protected bool IsSearching { get; private set; } = false;
        protected bool IsShowingSuggestions { get; private set; } = false;
        protected bool IsShowingSearchbar { get; private set; } = true;
        protected bool IsShowingMask { get; private set; } = false;
        protected TItem[] Suggestions { get; set; } = new TItem[0];
        protected string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;

                if (value.Length == 0)
                {
                    _debounceTimer.Stop();
                    Suggestions = new TItem[0];
                }
                else if (value.Length >= MinimumLength)
                {
                    _debounceTimer.Stop();
                    _debounceTimer.Start();
                }
            }
        }

        protected ElementReference searchInput;
        protected ElementReference typeahead;

        private Timer _debounceTimer;
        private string _searchText = string.Empty;
        private bool _firstRender = true; // remove in preview 9

        protected override void OnInitialized()
        {
            if (SearchMethod == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a {nameof(SearchMethod)} parameter.");
            }

            if (ResultTemplate == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a {nameof(ResultTemplate)} parameter.");
            }

            _debounceTimer = new Timer();
            _debounceTimer.Interval = Debounce;
            _debounceTimer.AutoReset = false;
            _debounceTimer.Elapsed += Search;

            Initialize();
        }

        protected override void OnParametersSet()
        {
            Initialize();
        }

        private void Initialize()
        {
            IsShowingSuggestions = false;
            if (Value == null)
            {
                IsShowingMask = false;
                IsShowingSearchbar = true;
            }
            else
            {
                IsShowingSearchbar = false;
                IsShowingMask = true;
            }
        }

        protected void HandleClickOnMask()
        {
            IsShowingMask = false;
            IsShowingSearchbar = true;
        }

        protected async Task ShowMaximumSuggestions()
        {
            IsShowingSuggestions = !IsShowingSuggestions;

            if (IsShowingSuggestions)
            {
                SearchText = "";
                IsSearching = true;
                await InvokeAsync(StateHasChanged);

                Suggestions = (await SearchMethod?.Invoke(_searchText)).Take(MaximumSuggestions).ToArray();

                IsSearching = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        protected void ShowSuggestions()
        {
            if (Suggestions.Any())
            {
                IsShowingSuggestions = true;
            }
        }

        protected async Task HandleKeyUpOnSuggestion(KeyboardEventArgs args, TItem item)
        {
            // Maybe on any key except Tab and Enter, continue the typing option.
            switch (args.Key)
            {
                case "Enter":
                    await SelectResult(item);
                    break;
                case "Escape":
                case "Backspace":
                case "Delete":
                    Initialize();
                    break;
                default:
                    break;
            }
        }

        protected async Task HandleKeyUpOnShowMaximum(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
                await ShowMaximumSuggestions();
        }

        protected string GetSelectedSuggestionClass(TItem item)
        {
            if (Value == null)
                return null;
            if (Value.Equals(item))
                return "blazored-typeahead__result-selected";
            return null;
        }

        protected async void Search(Object source, ElapsedEventArgs e)
        {
            IsSearching = true;
            await InvokeAsync(StateHasChanged);
            Suggestions = (await SearchMethod?.Invoke(_searchText)).Take(MaximumSuggestions).ToArray();

            IsSearching = false;
            IsShowingSuggestions = true;
            await InvokeAsync(StateHasChanged);
        }

        protected async Task SelectResult(TItem item)
        {
            SearchText = "";
            await ValueChanged.InvokeAsync(item);
        }

        protected bool ShouldShowSuggestions()
        {
            return IsShowingSuggestions &&
                   Suggestions.Any();
        }

        private bool HasValidSearch => !string.IsNullOrWhiteSpace(SearchText) && SearchText.Length >= MinimumLength;

        private bool IsSearchingOrDebouncing => IsSearching || _debounceTimer.Enabled;

        protected bool ShowNotFound()
        {
            return IsShowingSuggestions &&
                   HasValidSearch &&
                   !IsSearchingOrDebouncing &&
                   !Suggestions.Any();
        }

        protected void OnFocusOut(object sender, EventArgs e)
        {
            Initialize();
            InvokeAsync(StateHasChanged);
        }

        protected void OnEscape(object sender, EventArgs e)
        {
            Initialize();
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _debounceTimer.Dispose();
        }
    }
}
