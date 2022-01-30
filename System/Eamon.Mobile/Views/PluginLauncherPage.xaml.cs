
// PluginLauncherPage.xaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class PluginLauncherPage : ContentPage
	{
		PluginLauncherViewModel viewModel;

		public void RedrawPluginLauncherPageControls()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				OutputLabel.IsVisible = false;

				OutputLabel.IsVisible = true;

				Separator.IsVisible = false;

				Separator.IsVisible = true;

				InputEntry.IsVisible = false;

				InputEntry.IsVisible = true;

				PluginScrollView.IsVisible = false;

				PluginScrollView.IsVisible = true;
			});
		}

		public void ScrollToBottomOfPluginScrollView()
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				await PluginScrollView.ScrollToAsync(Separator, ScrollToPosition.Start, true);
			});
		}

		public async void SetInputTextNoEvents(string value)
		{
			if (viewModel.InputText == null || !viewModel.InputText.Equals(value, StringComparison.Ordinal))
			{
				InputEntry.TextChanged -= InputEntry_TextChanged;

				viewModel.InputText = value;

				await Task.Yield();

				InputEntry.TextChanged += InputEntry_TextChanged;
			}
		}

		public void InputEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			var newTextValue = e.NewTextValue ?? "";

			var oldTextValue = e.OldTextValue ?? "";

			if (newTextValue.Length > oldTextValue.Length)
			{
				var ch = newTextValue[newTextValue.Length - 1];

				var ch01 = ch;

				if (App.InputBufSize > 0 && newTextValue.Length <= App.InputBufSize)
				{
					if (App.InputModifyCharFunc != null)
					{
						ch = App.InputModifyCharFunc(ch);
					}

					var validChar = true;

					if (App.InputValidCharFunc != null)
					{
						validChar = App.InputValidCharFunc(ch);
					}

					if (validChar)
					{
						if (ch != '\0')
						{
							if (ch != ch01)
							{
								SetInputTextNoEvents(string.Format("{0}{1}", oldTextValue, ch));
							}
						}
						else
						{
							SetInputTextNoEvents(oldTextValue);
						}

						var termChar = false;

						if (App.InputTermCharFunc != null)
						{
							termChar = App.InputTermCharFunc(ch);
						}

						if (termChar)
						{
							Device.BeginInvokeOnMainThread(() =>
							{
								InputEntry_Completed(InputEntry, null);
							});
						}
					}
					else
					{
						SetInputTextNoEvents(oldTextValue);
					}
				}
				else
				{
					SetInputTextNoEvents(oldTextValue);
				}
			}
		}

		public void InputEntry_Completed(object sender, EventArgs e)
		{
			if (viewModel.InputText.Length > 0 || App.InputEmptyAllowed)
			{
				if (!App.FinishInputSet)         // Xamarin Forms bug workaround ???
				{
					App.FinishInputSet = true;

					if (viewModel.InputText.Length == 0 && App.InputEmptyVal != null)
					{
						SetInputTextNoEvents(App.InputEmptyVal);
					}

					Device.BeginInvokeOnMainThread(() =>
					{
						if (!App.SettingsViewModel.KeepKeyboardVisible)
						{
							InputEntry.Unfocus();
						}

						App.FinishInput.Set();
					});
				}
			}
			else		// never reached, but just in case
			{
				Device.StartTimer(new TimeSpan(0, 0, 0, 0, 250), () =>
				{
					InputEntry.Focus();

					return false;
				});
			}
		}

		public void InputEntry_Unfocus()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				InputEntry.Unfocus();
			});
		}

		public void BrowsePluginLauncherPage_SizeChanged(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				InputEntry.Unfocus();
			});

			RedrawPluginLauncherPageControls();

			ScrollToBottomOfPluginScrollView();
		}

		public PluginLauncherPage(string title)
		{
			InitializeComponent();

			BindingContext = viewModel = new PluginLauncherViewModel(title);

			viewModel.PropertyChanged += (o, e) =>
			{
				if (e.PropertyName == "OutputText")
				{
					ScrollToBottomOfPluginScrollView();
				}
			};

			App.PluginLauncherPage = this;
		}
	}
}
