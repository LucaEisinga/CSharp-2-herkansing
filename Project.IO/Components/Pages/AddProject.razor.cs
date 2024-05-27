using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.IO.Components.Pages
{
    internal class AddProject
    {
        private DateTime deadline = DateTime.Today;

        // Optioneel: Methode om de deadline in de toekomst te verwerken
        private void SaveDeadline()
        {
            // Logica om de deadline op te slaan of te verwerken
            Console.WriteLine($"De geselecteerde deadline is: {deadline}");
        }
    }
}
