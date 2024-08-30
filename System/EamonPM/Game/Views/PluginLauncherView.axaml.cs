
// PluginLauncherView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using EamonPM.Game.ViewModels;

namespace EamonPM.Game.Views
{
	public partial class PluginLauncherView : UserControl
	{
		public virtual void OutputScrollViewer_PointerWheelChanged(object sender, PointerWheelEventArgs e)
		{
			InputTextBoxLoseFocus();
		}

		public virtual void OutputScrollViewer_PointerPressed(object sender, PointerPressedEventArgs e)
		{
			InputTextBoxLoseFocus();
		}

		public virtual void OutputTextBlock_GotFocus(object sender, GotFocusEventArgs e)
		{
			OutputScrollViewer.ScrollToEnd();

			e.Handled = true;
		}

		public virtual void OutputTextBlock_PointerPressed(object sender, PointerPressedEventArgs e)
		{
			InputTextBoxLoseFocus();
		}

		public virtual void InputTextBox_GotFocus(object sender, GotFocusEventArgs e)
		{
			if (DataContext is PluginLauncherViewModel pluginLauncherViewModel)
			{
				if (App.ProgramName == "EamonPM.Android" && pluginLauncherViewModel.KeepKeyboardVisible)
				{
					App.ShowKeyboardFunc?.Invoke();
				}
			}
		}

		public virtual void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (DataContext is PluginLauncherViewModel pluginLauncherViewModel)
			{
				if (App.ProgramName == "EamonPM.Android" && pluginLauncherViewModel.KeepKeyboardVisible)
				{
					App.HideKeyboardFunc?.Invoke();
				}
			}
		}

		public virtual void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			var viewModel = DataContext as PluginLauncherViewModel;

			var currInputText = viewModel.InputText ?? "";

			var prevInputText = viewModel.PrevInputText ?? "";

			if (currInputText.Length > prevInputText.Length)
			{
				var ch = currInputText[currInputText.Length - 1];

				var ch01 = ch;

				if (App.InputBufSize > 0 && currInputText.Length <= App.InputBufSize)
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
								SetInputTextNoEvents(string.Format("{0}{1}", prevInputText, ch));
							}
						}
						else
						{
							SetInputTextNoEvents(prevInputText);
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
						SetInputTextNoEvents(prevInputText);
					}
				}
				else
				{
					SetInputTextNoEvents(prevInputText);
				}
			}
		}

		public virtual void InputTextBox_KeyUp(object sender, KeyEventArgs e)
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

		public virtual void InputEntry_Completed(object sender, EventArgs e)
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

		public virtual void SetInputTextWatermark(string value, bool useFloating)
		{
			InputTextBox.Watermark = value;

			InputTextBox.UseFloatingWatermark = useFloating;
		}

		public virtual async void SetInputTextNoEvents(string value)
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

		public virtual void InputTextBoxLoseFocus()
		{
			InputTextBox.IsEnabled = false;

			InputTextBox.IsEnabled = true;
		}

		public virtual void OutputScrollViewerScrollToEnd()
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
