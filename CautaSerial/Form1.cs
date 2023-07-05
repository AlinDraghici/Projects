using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CautaSerial
{
    public partial class Form1 : Form
    {
        private TextBox searchTextBox;
        private Button searchButton;
        private TextBox resultTextBox;

        public Form1()
        {
            searchTextBox = new TextBox();
            searchTextBox.Location = new System.Drawing.Point(10, 10);
            searchTextBox.Size = new System.Drawing.Size(200, 20);
            Controls.Add(searchTextBox);

            searchButton = new Button();
            searchButton.Location = new System.Drawing.Point(220, 10);
            searchButton.Size = new System.Drawing.Size(75, 20);
            searchButton.Text = "Cauta";
            searchButton.Click += SearchButton_Click;
            Controls.Add(searchButton);

            resultTextBox = new TextBox();
            resultTextBox.Location = new System.Drawing.Point(10, 40);
            resultTextBox.Multiline = true;
            resultTextBox.Size = new System.Drawing.Size(285, 200);
            resultTextBox.ReadOnly = true;
            Controls.Add(resultTextBox);
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text;
            string apiUrl = $"https://api.tvmaze.com/search/shows?q={searchText}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    List<ShowResult> results = JsonConvert.DeserializeObject<List<ShowResult>>(jsonResponse);

                    resultTextBox.Text = "";
                    foreach (ShowResult result in results)
                    {
                        resultTextBox.AppendText($"Nume: {result.show.name}\r\n");
                        resultTextBox.AppendText($"Genuri: {string.Join(", ", result.show.genres)}\r\n");
                        resultTextBox.AppendText($"Status: {result.show.status}\r\n");
                        resultTextBox.AppendText($"Rating: {result.show.rating?.average}\r\n");
                        resultTextBox.AppendText($"Limba: {result.show.language}\r\n");
                        resultTextBox.AppendText($"URL: {result.show.url}\r\n");
                        resultTextBox.AppendText($"Premiera: {result.show.premiered}\r\n");
                        resultTextBox.AppendText($"Terminat: {result.show.ended}\r\n");
                        resultTextBox.AppendText($"Site Oficial: {result.show.officialSite}\r\n");
                        resultTextBox.AppendText("------------------------------\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                resultTextBox.Text = $"Eroare la efectuarea căutării: {ex.Message}";
            }
        }

        private class ShowResult
        {
            public Show show { get; set; }
        }

        private class Show
        {
            public string name { get; set; }
            public List<string> genres { get; set; }
            public string status { get; set; }
            public Rating rating { get; set; }
            public string language { get; set; }
            public string url { get; set; }
            public string premiered { get; set; }
            public string ended { get; set; }
            public string officialSite { get; set; }
        }

        private class Rating
        {
            public double? average { get; set; }
        }
    }
}
