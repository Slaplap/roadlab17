using System;
using Interon.Roadlab.App.Core.Const;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.App.Core.ViewModels;
using Interon.Roadlab.App.Views;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core
{
    public  class ErrorService
    {
        public static void Error(string message)
        {
            var error = new ErrorPage(new ErrorMessageViewModel(){Message = message});
            
            Application.Current.MainPage.Navigation.PushModalAsync(error);
        }
        public static void ErrorExceptionAndAnalytics(string message, string methodName,Exception ex)
        {
            var error = new ErrorPage(new ErrorMessageViewModel() { Message = message });
            
            AnalyticsService.TrackError(ex,methodName);
           
        }
        public static void ErrorExceptionAndAnalyticsMessageCenter(string message, string methodName, Exception ex)
        {
            var error = new ErrorPage(new ErrorMessageViewModel() { Message = "Message : "+ message + "Method :" + methodName + "Exception : " + ex.Message});
            MessagingCenter.Send<object>(new ErrorMessageViewModel() { Message = message }, MessagingCenterValues.Error);
            AnalyticsService.TrackError(ex, methodName);

        }
        public static void ErrorExceptionAndAnalyticsAndModal(string message, string methodName, Exception ex)
        {

            var error = new ErrorPage(new ErrorMessageViewModel() { Message = message + "-" + ex.Message });
           
            AnalyticsService.TrackError(ex, methodName);
            Application.Current.MainPage.Navigation.PushModalAsync(error).ConfigureAwait(true);
           

        }
        public static void EventAndAnalytics( string message, AnalyticsService.EventCategory category)
        {
             
            AnalyticsService.TrackEvent(category,message);
            
           
        }
        public static void EventAndAnalyticsAndModal(string message , AnalyticsService.EventCategory category  )
        {
            var error = new ErrorPage(new ErrorMessageViewModel() { Message = message });
            
            AnalyticsService.TrackEvent(category , message);
            Application.Current.MainPage.Navigation.PushModalAsync(error);
        }
    }
}
