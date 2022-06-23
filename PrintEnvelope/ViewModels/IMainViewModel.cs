using PrintEnvelope.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintEnvelope.ViewModels
{
    public interface IMainViewModel
    {
        ObservableCollection<Recipient> Recipients { get; set; }
        string Filter { get; set; }
        ObservableCollection<PrintSettings> PrintSettings { get; set; }
    }
}
