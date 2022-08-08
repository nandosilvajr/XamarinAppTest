using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using RestSharp;
using Xamarin.Forms;

namespace XamarinAppTest
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Datum> _datums;

        public ObservableCollection<Datum> Datums
        {
            get { return _datums; }
            set { _datums = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged(); }
        }

        private string _cardLabel;
        public string CardLabel {
            get
            {
                return _cardLabel;
            }
            set
            {
                _cardLabel = value;
                OnPropertyChanged();
            }
        }

        private Datum _selectedModel;

        public Datum SelectedModel
        {
            get { return _selectedModel; }
            set { 
                _selectedModel = value; 
                OnPropertyChanged(); }
        }

        ObservableCollection<MainModel> _lista;

        public ObservableCollection<MainModel> Lista {
            get
            {
                return _lista;
            }

            set { 
                _lista = value;
                OnPropertyChanged();
            }
        
        }

        public ICommand AdicionarCommand
        {
            get
            {
                return new Command(Adicionar);
            }
        }
        public ICommand ApagarCommand
        {
            get
            {
                return new Command(Apagar);
            }
        }

        public ICommand LoadDataCommand
        {
            get
            {
                return new Command(LoadData);
            }
        }

        private async void LoadData(object obj)
        {


            string baseUrl = "https://dummyjson.com/";
            string endPoint = $"products?limit=10&skip={Products.Count}";

            var options = new RestClientOptions(baseUrl)
            {
                RemoteCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            var restClient = new RestClient(options);

            var request = new RestRequest(endPoint, Method.Get);

            try
            {
                var response = await restClient.GetAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var myDeserializedClass = JsonConvert.DeserializeObject<ProductsList>(response.Content);

                    foreach (var item in myDeserializedClass.Products)
                    {
                        Products.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        private void Adicionar(object obj)
        {
            Lista.Add(new MainModel { Name = obj.ToString() });
        }
        private void Apagar(object obj)
        {
            Lista.Remove(obj as MainModel);
        }



        public MainViewModel()
        {

            CardLabel = "Texto Velho";

            Lista = new ObservableCollection<MainModel>();
            
            Lista.Add(new MainModel { Name = "Francisco" });
            Lista.Add(new MainModel { Name = "Jouse" });
            Lista.Add(new MainModel { Name = "Regina" });
            Lista.Add(new MainModel { Name = "Paulo" });

            GetDataAsync();
        }

        private async void GetDataAsync()
        {

            string baseUrl = "https://dummyjson.com/";
            string endPoint = "products?limit=10&skip=10";

            var options = new RestClientOptions(baseUrl)
            {
                RemoteCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            var restClient = new RestClient(options);

            var request = new RestRequest(endPoint, Method.Get);

            try
            {
                var response = await restClient.GetAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var myDeserializedClass = JsonConvert.DeserializeObject<ProductsList>(response.Content);

                    Products = new ObservableCollection<Product>(myDeserializedClass.Products);
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
           
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
