
// PluginLauncherView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using EamonPM.Game.ViewModels;

namespace EamonPM.Game.Views
{
	public partial class PluginLauncherView : UserControl
	{
		public void OutputScrollViewer_PointerWheelChanged(object sender, PointerWheelEventArgs e)
		{
			InputTextBoxLoseFocus();
		}

		public void OutputScrollViewer_PointerPressed(object sender, PointerPressedEventArgs e)
		{
			InputTextBoxLoseFocus();
		}

		public void OutputTextBlock_GotFocus(object sender, GotFocusEventArgs e)
		{
			OutputScrollViewer.ScrollToEnd();

			e.Handled = true;
		}

		public void OutputTextBlock_PointerPressed(object sender, PointerPressedEventArgs e)
		{
			InputTextBoxLoseFocus();
		}

		private void InputTextBox_GotFocus(object sender, GotFocusEventArgs e)
		{
			if (DataContext is PluginLauncherViewModel pluginLauncherViewModel)
			{
				if (App.ProgramName == "EamonPM.Android" && pluginLauncherViewModel.KeepKeyboardVisible)
				{
					App.ShowKeyboardFunc?.Invoke();
				}
			}
		}

		private void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (DataContext is PluginLauncherViewModel pluginLauncherViewModel)
			{
				if (App.ProgramName == "EamonPM.Android" && pluginLauncherViewModel.KeepKeyboardVisible)
				{
					App.HideKeyboardFunc?.Invoke();
				}
			}
		}

		public void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var viewModel = DataContext as PluginLauncherViewModel;

			var newTextValue = viewModel.InputText ?? "";

			var oldTextValue = viewModel.OldInputText ?? "";

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
							App.DispatcherUIThreadPost(() =>
							{
								InputEntry_Completed(InputTextBox, null);
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

		public void InputTextBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (DataContext is PluginLauncherViewModel pluginLauncherViewModel)
			{
				if (e.Key == Key.Escape)
				{
					InputTextBox.Text = string.Empty;

					InputTextBox.Focus();
				}
				else if (e.Key == Key.Enter)
				{
					InputEntry_Completed(InputTextBox, null);

					if (App.ProgramName != "EamonPM.Android" || pluginLauncherViewModel.KeepKeyboardVisible)
					{
						// Keep focus
						InputTextBox.Focus();
					}
					else
					{
						// Clear focus and hide keyboard
						InputTextBoxLoseFocus();
						App.HideKeyboardFunc?.Invoke();
					}
				}
			}
		}

		public void InputEntry_Completed(object sender, EventArgs e)
		{
			var viewModel = DataContext as PluginLauncherViewModel;

			if (viewModel.InputText.Length > 0 || App.InputEmptyAllowed)
			{
				if (!App.FinishInputSet)         // Xamarin Forms bug workaround ???
				{
					App.FinishInputSet = true;

					if (viewModel.InputText.Length == 0 && App.InputEmptyVal != null)
					{
						SetInputTextNoEvents(App.InputEmptyVal);
					}

					App.DispatcherUIThreadPost(() =>
					{
						if (App.ProgramName == "EamonPM.Android" && !viewModel.KeepKeyboardVisible)
						{
							InputTextBoxLoseFocus();        //InputEntry.Unfocus();
						}

						App.FinishInput.Set();
					});
				}
			}
			else        // Never reached, but just in case
			{
				/*
				Device.StartTimer(new TimeSpan(0, 0, 0, 0, 250), () =>
				{
					InputEntry.Focus();

					return false;
				});
				*/
			}
		}

		public void SetInputTextWatermark(string value, bool useFloating)
		{
			InputTextBox.Watermark = value;

			InputTextBox.UseFloatingWatermark = useFloating;
		}

		public async void SetInputTextNoEvents(string value)
		{
			var viewModel = DataContext as PluginLauncherViewModel;

			if (viewModel.InputText == null || !viewModel.InputText.Equals(value, StringComparison.Ordinal))
			{
				InputTextBox.TextChanged -= InputTextBox_TextChanged;

				viewModel.InputText = value;

				InputTextBox.CaretIndex = InputTextBox.Text.Length;

				await Task.Yield();

				InputTextBox.TextChanged += InputTextBox_TextChanged;
			}
		}

		public void InputTextBoxLoseFocus()
		{
			InputTextBox.IsEnabled = false;

			InputTextBox.IsEnabled = true;
		}

		public void OutputScrollViewerScrollToEnd()
		{
			App.DispatcherUIThreadPost(() =>
			{
				OutputScrollViewer.ScrollToEnd();
			});
		}

		public PluginLauncherView()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(PluginLauncherViewModel));

			OutputScrollViewer.AddHandler(InputElement.PointerPressedEvent, OutputScrollViewer_PointerPressed, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);

			OutputTextBlock.AddHandler(InputElement.PointerPressedEvent, OutputTextBlock_PointerPressed, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);

			var viewModel = DataContext as PluginLauncherViewModel;

			viewModel.PropertyChanged += (o, e) =>
			{
				if (e.PropertyName == "OutputText")
				{
					OutputScrollViewerScrollToEnd();
				}
			};
		}
	}
}
