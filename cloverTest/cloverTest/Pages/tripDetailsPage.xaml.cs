using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cloverTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TripDetailsPage : ContentPage
    {
        Trip mTrip;
        public TripDetailsPage(Trip trip)
        {
            InitializeComponent();
            mTrip = trip;

            if(mTrip!= null)
            {
                tripNameLabel.Text = mTrip.tripName;
                baseFaresLabel.Text = mTrip.baseFares.ToString();
                tollsLabel.Text = mTrip.tolls.ToString();
                tipsLabel.Text = mTrip.tips.ToString();
                processingFeesLabel.Text = mTrip.processingFees.ToString();
                totalLabel.Text = mTrip.total.ToString();

                if (trip.state != null)
                {
                    if (mTrip.state.Equals("open"))
                    {
                        payBtn.IsVisible = true;
                    }
                }
            }
        }


        private void payBtnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PaymentPage(mTrip));
        }
    }
}