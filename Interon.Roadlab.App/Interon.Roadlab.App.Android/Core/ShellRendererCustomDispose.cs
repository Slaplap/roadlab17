using System;
using Android.Content;
using Interon.Roadlab.App.Droid.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Shell), typeof(ShellRendererCustomDispose))]
namespace Interon.Roadlab.App.Droid.Core
{
  
 
	public class ShellRendererCustomDispose : ShellRenderer
	{
		bool _disposed;

		public ShellRendererCustomDispose(Context context)
			: base(context)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				Element.PropertyChanged -= OnElementPropertyChanged;
				Element.SizeChanged -= (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), this, "OnElementSizeChanged"); // OnElementSizeChanged is private, so use reflection
			}

			_disposed = true;
		}
	}
}
 