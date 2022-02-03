using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interon.Roadlab.App.Core.Models;
using Interon.Roadlab.App.Core.Services;
using Interon.Roadlab.App.Core.Tasks;
using Interon.Roadlab.LIMS.DTO;
using Interon.Roadlab.LIMS.Services;
using Xamarin.Forms;

namespace Interon.Roadlab.App.Core.ViewModels
{
    public class xTest
    {
        public int Key { get; set; }
        public int parentKey { get; set; }
        public string Value { get; set; }
    }
    public class xTestCategory
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }

    class ClientRequestRequestLineViewModel : ViewModelBase
    {
        private ObservableCollection<ClientRequest> ClientRequestLine { get; set; } = new ObservableCollection<ClientRequest>();

        public List<xTestCategory> TestCategories
        {
            get => _testCategories;
            set => SetProperty(ref _testCategories, value);
        }
        public List<xTestCategory> TestSubCategories
        {
            get => _testSubCategories;
            set => SetProperty(ref _testSubCategories, value);
        }
        public List<xTest> TestTypes
        {
            get => _tests;
            set => SetProperty(ref _tests, value);
        }

        public ClientRequestRequestLineViewModel(ObservableCollection<ClientRequest> transactionLine)
        {

            TestCategories = new List<xTestCategory>();
            TestSubCategories = new List<xTestCategory>();
            foreach (var sageCategory in SecureStorageService.Categories)
            {
                TestCategories.Add(new xTestCategory()
                {
                    Key = Convert.ToInt32(sageCategory.Key),
                    Value = sageCategory.Value.Name
                });
            }
            TestTypes = new List<xTest>();
            ClientRequestLine = transactionLine;
            Commands.Add("Done", new Command(Done));
            Commands.Add("Add", new Command(Add));

            // LoadCategories();

            Qty = 1;
        }

        private void LoadCategories()
        {
            DataService ass = new DataService();
            var testCategories = ass.GetTestCategories();

            if (!testCategories.Any())
            {
                PeriodicAppSettings.Job().Wait(10000);
                testCategories = ass.GetTestCategories();
                ErrorService.EventAndAnalyticsAndModal("ER008 No Test types loaded", AnalyticsService.EventCategory.Error);
            }

            foreach (var test in testCategories)
            {
                TestCategories.Add(new xTestCategory()
                {
                    Key = test.Id,
                    Value = test.Name
                });
            }

        }

        private async Task LoadTestsByCategoryId(int parentId, string Name)
        {
            // Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() => { IsBusy = true;});
            IsBusy = true;
            try
            {

                TestSubCategories = new List<xTestCategory>();
                TestTypes = new List<xTest>();
                var subs = await LIMSOnlineService.ServiceCategories(new InputServiceCategories()
                {
                    RoleId = 136163,
                    ParentId = parentId,
                    ProviderGroupId = 506

                }).ConfigureAwait(true);
                var subList = subs.Values.ToList();
                List<xTestCategory> tsc = new List<xTestCategory>();


                try
                {
                    foreach (var subsValue in subList)
                    {
                        tsc.Add(new xTestCategory()
                        {
                            Key = Convert.ToInt32(subsValue.Id),
                            Value = subsValue.Name

                        });
                    }
                }
                catch (Exception e)
                {
                    Debugger.Break();
                }
                TestSubCategories = tsc;
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
            finally
            {
                IsBusy = false;
            }









        }
        private void Done()
        {
            MessagingCenter.Send(this, "LineItemChange");
            Shell.Current.Navigation.PopModalAsync();
        }

        private void Add()
        {
            if (!IsValidAndSetErrors())
            {
                return;
            }
            //ClientRequestLine.Add(new ClientRequestLine()
            //{

            //    Key  =   Guid.NewGuid(),
            //    TestId =  _setTestType.Key,
            //    CategoryId = _setTestType.parentKey,
            //    Category = _setTestCategory.Value,
            //    Value = _setTestType.Value,
            //    Qty = Qty
            //});
            MessagingCenter.Send(this, "LineItemChange");
            Shell.Current.Navigation.PopModalAsync();
        }

        private bool IsValidAndSetErrors()
        {

            QtyError = "";
            TestTypesError = "";

            int errorcount = 0;
            if (SetTestType != null)
            {
                if (string.IsNullOrWhiteSpace(SetTestType.Value))
                {
                    TestTypesError = "Please enter field";
                    errorcount += 1;
                }
            }
            else
            {
                TestTypesError = "Please enter field";
                errorcount += 1;
            }
            if (SetTestCategory != null)
            {
                if (string.IsNullOrWhiteSpace(SetTestCategory.Value))
                {
                    CategoryError = "Please enter field";
                    errorcount += 1;
                }
            }
            else
            {
                TestTypesError = "Please enter field";
                errorcount += 1;
            }
            if (Qty < 1)
            {
                QtyError = "Please enter field";
                errorcount += 1;
            }


            if (errorcount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string TestTypesError
        {
            get => _testTypesError;
            set => SetProperty(ref _testTypesError, value);
        }
        public string CategoryError
        {
            get => _categoryError;
            set => SetProperty(ref _categoryError, value);
        }
        public string QtyError
        {
            get => _qtyError;
            set => SetProperty(ref _qtyError, value);
        }

        private xTest _setTestType;
        private int _qty;
        private List<xTest> _tests = new List<xTest>();
        private string _testTypesError;
        private string _qtyError;
        private List<xTestCategory> _testCategories;
        private xTestCategory _setTestCategory;
        private string _categoryError;
        private List<xTestCategory> _testSubCategories;
        private xTestCategory _setTestSubCategory;


        public xTest SetTestType
        {
            get => _setTestType;
            set => SetProperty(ref _setTestType, value);
        }
        public xTestCategory SetTestCategory
        {
            get => _setTestCategory;
            set
            {

                LoadTestsByCategoryId(value.Key, value.Value);
                SetProperty(ref _setTestCategory, value);
            }


        }
        public xTestCategory SetTestSubCategory
        {
            get => _setTestSubCategory;
            set
            {
                LoadTestsBySubCategoryId(value.Key, value.Value);
                SetProperty(ref _setTestSubCategory, value);
            }


        }

        private async Task LoadTestsBySubCategoryId(int valueKey, string valueValue)
        {
            IsBusy = true;
            try
            {
                var variants = await LIMSOnlineService.VariantsByCategory(new InputVariantByCategory()
                {
                    CategoryId = valueKey,
                    ProviderGroupId = 506
                });
                List<xTest> tests = new List<xTest>();
                foreach (var variant in variants.Values)
                {
                    tests.Add(new xTest()
                    {
                        Key = Convert.ToInt32(variant.Id),
                        Value = variant.Code + " " + variant.Name
                    });
                }

                TestTypes = tests;
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
            finally
            {
                IsBusy = false;
            }

        }

        public int Qty
        {
            get => _qty;
            set => SetProperty(ref _qty, value);
        }





    }
}