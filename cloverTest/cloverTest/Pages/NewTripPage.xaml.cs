using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cloverTest.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTripPage : ContentPage
    {
        int tipsPercentage = 20;
        Double tips = 0;
        Double tolls = 0;
        Double total =0 ;
        Double baseFare = 0;
        Double totalWithoutProcessingFees = 0 ;
        String mTripName = "";
        Double processingFees = 3;
        string state = "open";
        string startDate = DateTime.Now.ToString();
        string finishDate = "";
        Trip trip;
        public NewTripPage(string tripName)
        {
            InitializeComponent();
            tips20Btn.BackgroundColor = Color.Green;
            calculateTotalWithoutFees();

            mTripName = tripName;
            //addBtn.HeightRequest = baseFaresEntry.HeightRequest/2;
            //subBtn.HeightRequest = baseFaresEntry.Height / 2;
        }

        private void calculateTotalWithoutFees()
        {
            baseFare = Convert.ToDouble(baseFaresEntry.Text);
            tolls = Convert.ToDouble(tollsEntry.Text);
            tips = (baseFare + tolls) * tipsPercentage / 100;
            totalWithoutProcessingFees = baseFare + tolls + tips;
            totalLabel.Text = totalWithoutProcessingFees.ToString();
        }

        private void finishTripClicked(object sender, EventArgs e)
        {
            if(totalWithoutProcessingFees < 40)
            {
                processingFees = 3;
            }
            else
            {
                processingFees = 4.5;
            }
            total = totalWithoutProcessingFees + processingFees;
            finishDate = DateTime.Now.ToString();
            trip = new Trip(mTripName, baseFare, tolls, tips, processingFees, total, null, "USD", state, null, null, null, null, startDate, finishDate);
            Navigation.PushAsync(new TripDetailsPage(trip));

            //SAVE TRIP IN DATABASE
        }

        private void addTollsBtnClicked(object sender, EventArgs e)
        {
            int tolls = Int32.Parse(tollsEntry.Text) +5;
            tollsEntry.Text = tolls.ToString();
            calculateTotalWithoutFees();
        }

        private void subTollsBtnClicked(object sender, EventArgs e)
        {
            int tolls = Int32.Parse(tollsEntry.Text) - 5;
            tollsEntry.Text = tolls.ToString();
            calculateTotalWithoutFees();
        }

        private void tips20BtnClicked(object sender, EventArgs e)
        {
            tips20Btn.BackgroundColor = Color.Green;
            tips25Btn.BackgroundColor = Color.Gray;
            tips30Btn.BackgroundColor = Color.Gray;
            tipsPercentage = 20;
            calculateTotalWithoutFees();
        }
        private void tips25BtnClicked(object sender, EventArgs e)
        {
            tips20Btn.BackgroundColor = Color.Gray;
            tips25Btn.BackgroundColor = Color.Green;
            tips30Btn.BackgroundColor = Color.Gray;
            tipsPercentage = 25;
            calculateTotalWithoutFees();
        }
        private void tips30BtnClicked(object sender, EventArgs e)
        {
            tips20Btn.BackgroundColor = Color.Gray;
            tips25Btn.BackgroundColor = Color.Gray;
            tips30Btn.BackgroundColor = Color.Green;
            tipsPercentage = 30;
            calculateTotalWithoutFees();
        }

        private void payClicked(object sender, EventArgs e)
        {

        }

        private void baseFaresChanged(object sender, TextChangedEventArgs e)
        {
            if (Double.TryParse(baseFaresEntry.Text,out baseFare))
            {
                calculateTotalWithoutFees();
            }
            else
            {
                baseFare = 0;
            }
        }
    }
}