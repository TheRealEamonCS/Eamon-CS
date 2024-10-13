
// MainView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Interactivity;
using EamonPM.Game.ViewModels;

namespace EamonPM.Game.Views
{
	public partial class MainView : UserControl
	{
		public virtual TopLevel TopLevelView { get; set; }

		public virtual void BackButton_Clicked(object sender, RoutedEventArgs e)
		{
			if (DataContext is MainViewModel viewModel)
			{
				var currentView = viewModel.ViewStack.Peek();

				Debug.Assert(currentView != null);

				if (currentView is DocumentationView documentationView)
				{
					App.ResetListBox(documentationView.FileListBox);
				}
				else if (currentView is EamonDDView eamonDDView)
				{
					App.ResetListBox(eamonDDView.FileListBox);
				}
				else if (currentView is EamonMHView eamonMHView)
				{
					App.ResetListBox(eamonMHView.FileListBox);
				}
				else if (currentView is EamonRTView eamonRTView)
				{
					App.ResetListBox(eamonRTView.FileListBox);
				}
				else if (currentView is QuickLaunchView quickLaunchView)
				{
					App.ResetListBox(quickLaunchView.FolderListBox);
				}

				viewModel.ViewStack.Pop();

				viewModel.MainTitleStack.Pop();

				viewModel.IsBackButtonActiveStack.Pop();

				viewModel.CurrentView = viewModel.ViewStack.Peek();

				Debug.Assert(viewModel.CurrentView != null);

				viewModel.MainTitle = viewModel.MainTitleStack.Peek();

				Debug.Assert(!string.IsNullOrWhiteSpace(viewModel.MainTitle));

				viewModel.IsBackButtonActive = viewModel.IsBackButtonActiveStack.Peek();
			}
		}

		public virtual void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is TabControl tabControl && DataContext is MainViewModel viewModel)
			{
				var selectedIndex = tabControl.SelectedIndex;
				
				viewModel.MainTabControlSelectionChanged(selectedIndex);
			}
		}

		public virtual void InputPane_StateChanged(object sender, InputPaneStateEventArgs e)
		{
			var screenBounds = this.VisualRoot.ClientSize;

			var pluginLauncherView = App.GetView(typeof(PluginLauncherView)) as PluginLauncherView;

			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is IInputPane inputPane && e != null && pluginLauncherViewModel != null && pluginLauncherView != null)
			{
				if (e.NewState == InputPaneState.Open && e.EndRect.Top >= screenBounds.Height - e.EndRect.Height)
				{
					var keyboardHeight = inputPane.OccludedRect.Height;

					pluginLauncherViewModel.InputTextBoxMarginChanged(new Thickness(0, 0, 0, keyboardHeight + 10));

					pluginLauncherView.OutputScrollViewerScrollToEnd();
				}
				else if (e.NewState == InputPaneState.Closed)
				{
					pluginLauncherViewModel.InputTextBoxMarginChanged(new Thickness(0, 0, 0, 10));

					pluginLauncherView.OutputScrollViewerScrollToEnd();
				}
			}
		}

		public virtual void VisualTree_Attached(object sender, VisualTreeAttachmentEventArgs e)
		{
			TopLevelView = TopLevel.GetTopLevel(this);

			if (TopLevelView != null)
			{
				var inputPane = TopLevelView.InputPane;

				if (inputPane != null)
				{
					inputPane.StateChanged += InputPane_StateChanged;
				}
			}
		}

		public virtual void VisualTree_Detached(object sender, VisualTreeAttachmentEventArgs e)
		{
			if (TopLevelView != null)
			{
				var inputPane = TopLevelView.InputPane;

				if (inputPane != null)
				{
					inputPane.StateChanged -= InputPane_StateChanged;
				}
			}
		}

		public MainView()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(MainViewModel));

			if (App.ProgramName == "EamonPM.Android")
			{
				this.AttachedToVisualTree += VisualTree_Attached;

				this.DetachedFromVisualTree += VisualTree_Detached;
			}
		}
	}
}
