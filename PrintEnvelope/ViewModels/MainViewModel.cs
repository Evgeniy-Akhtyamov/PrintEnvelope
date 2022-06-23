using Microsoft.Win32;
using PrintEnvelope.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using System.Data.Entity;

namespace PrintEnvelope.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IMainViewModel
    {
        private RecipientRepository _recipientRepository;
        private PrintSettingsRepository _printSettingsRepository;
        public MainViewModel(RecipientRepository recipientRepository, PrintSettingsRepository printSettingsRepository)
        {
            _recipientRepository = recipientRepository;
            _printSettingsRepository = printSettingsRepository;
            
            FillRecipientsListView();
            SettingsListView();
        }
        
        private ObservableCollection<PrintSettings> _printSettings;
        public ObservableCollection<PrintSettings> PrintSettings
        {
            get
            {
                return _printSettings;
            }
            set
            {
                _printSettings = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Recipient> _recipients;
        public ObservableCollection<Recipient> Recipients
        {
            get
            {
                return _recipients;
            }
            set
            {
                _recipients = value;
                OnPropertyChanged();
            }
        }

        private string _filter;
        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                FillRecipientsListView();
            }
        }

        private void FillRecipientsListView()
        {
            if (!String.IsNullOrEmpty(_filter))
            {
                Recipients = new ObservableCollection<Recipient>(
                    _recipientRepository.GetAll()
                    .Where(r => r.Who.ToLower().Contains(_filter.ToLower()) || r.Where.ToLower().Contains(_filter.ToLower())));
            }
            else
                Recipients = new ObservableCollection<Recipient>(
                    _recipientRepository.GetAll());
        }

        private void SettingsListView()
        {
            PrintSettings = new ObservableCollection<PrintSettings>(_printSettingsRepository.GetAll());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // команда изменения (обновления) 
        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                  (updateCommand = new RelayCommand((selectedItem) =>
                  {
                      try
                      {
                          Recipient recipient = selectedItem as Recipient;
                          _recipientRepository.Update(recipient);
                          FillRecipientsListView();
                      }
                      catch
                      {
                          MessageBox.Show("Возникла ошибка при обновлении");
                      }
                  },
                  (selectedItem) => selectedItem != null));
            }
        }

        // команда удаления получателя
        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand((selectedItem) =>
                  {
                      try
                      {
                          Recipient recipient = selectedItem as Recipient;
                          _recipientRepository.Remove(recipient);
                          FillRecipientsListView();
                      }
                      catch
                      {
                          MessageBox.Show("Возникла ошибка при удалении");
                      }
                  },
                  (selectedItem) => selectedItem != null));
            }
        }

        // команда добавления получателя
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand((selectedItem) =>
                    {
                        try
                        {
                            Recipient recipient = selectedItem as Recipient;
                            _recipientRepository.Add(recipient);
                            FillRecipientsListView();
                        }
                        catch
                        {
                            MessageBox.Show("Возникла ошибка при добавлении");
                        }
                    },
                    (selectedItem) => selectedItem != null));
            }
        }

        // команда очистки базы данных получателей
        private RelayCommand clearCommand;
        public RelayCommand ClearCommand
        {
            get
            {
                return clearCommand ??
                    (clearCommand = new RelayCommand(obj =>
                    {
                        _recipientRepository.Clear();
                        FillRecipientsListView();
                    },
                    (obj) => Recipients.Count > 0));
            }
        }

        // команда открытия файла Exel и добавления получателей в БД
        private RelayCommand openCommand;
        public RelayCommand OpenCommand
        {
            get
            {
                return openCommand ??
                  (openCommand = new RelayCommand(obj =>
                  {
                      string filePath;
                      MessageBox.Show("Необходимо выбрать документ Excel, столбцы которого должны соответствовать полям: A - Кому, B - Куда, С - Индекс, D - Группа");

                      OpenFileDialog myDialog = new OpenFileDialog();
                      myDialog.CheckFileExists = true;
                      myDialog.Multiselect = false;
                      myDialog.Filter = "Microsoft Excel (*.xls;*.xlsx)|*.xls;*.xlsx";
                      myDialog.Title = "Выберите документ Excel";

                      if (myDialog.ShowDialog() == true)
                      {
                          filePath = myDialog.FileName;
                          Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

                          if (excelApp == null)
                          {
                              MessageBox.Show("Excel is not installed!!");
                              return;
                          }

                          Workbook excelBook = excelApp.Workbooks.Open(filePath);
                          Worksheet excelSheet = excelBook.Sheets[1];
                          Range excelRange = excelSheet.UsedRange;
                          int rows = excelRange.Rows.Count;

                          try
                          {
                              for (int i = 1; i <= rows; i++)
                              {
                                  var recipient = new Recipient();

                                  recipient.Who = excelRange.Cells[i, 1].Value2.ToString();
                                  recipient.Where = excelRange.Cells[i, 2].Value2.ToString();
                                  recipient.Index = excelRange.Cells[i, 3].Value2.ToString();
                                  recipient.Group = excelRange.Cells[i, 4].Value2.ToString();

                                  _recipientRepository.Add(recipient);
                              }
                              FillRecipientsListView();
                              MessageBox.Show("Данные успешно загружены");
                          }
                          catch
                          {
                              MessageBox.Show("Не все ячейки рабочего диапазона данных в файле были заполнены");
                          }

                          excelApp.Quit();
                          System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                      }
                  }));
            }
        }

        private RelayCommand saveSettingsCommand;
        public RelayCommand SaveSettingsCommand
        {
            get
            {
                return saveSettingsCommand ??
                  (saveSettingsCommand = new RelayCommand((selectedItem) =>
                  {
                      try
                      {
                          PrintSettings printSettings = selectedItem as PrintSettings;                         
                          _printSettingsRepository.Update(printSettings);
                          SettingsListView();
                      }
                      catch
                      {
                          MessageBox.Show("Возникла ошибка при обновлении");
                      }
                  },
                  (selectedItem) => selectedItem != null));
            }
        }

        // команда удаления настроек
        private RelayCommand removeSettingsCommand;
        public RelayCommand RemoveSettingsCommand
        {
            get
            {
                return removeSettingsCommand ??
                  (removeSettingsCommand = new RelayCommand((selectedItem) =>
                  {
                      if (MessageBox.Show("Вы действительно хотите удалить данные настройки печати конверта?",
                          "Подтверждение удаления",
                          MessageBoxButton.YesNo,
                          MessageBoxImage.Question) == MessageBoxResult.Yes)
                      {
                          try
                          {
                              PrintSettings printSettings = selectedItem as PrintSettings;
                              _printSettingsRepository.Remove(printSettings);
                              SettingsListView();
                          }
                          catch
                          {
                              MessageBox.Show("Возникла ошибка при удалении");
                          }
                      }    
                  },
                  (selectedItem) => selectedItem != null));
            }
        }

        // команда добавления настроек
        private RelayCommand addSettingsCommand;
        public RelayCommand AddSettingsCommand
        {
            get
            {
                return addSettingsCommand ??
                    (addSettingsCommand = new RelayCommand((selectedItem) =>
                    {
                        try
                        {
                            PrintSettings printSettings = selectedItem as PrintSettings;
                            _printSettingsRepository.Add(printSettings);
                            SettingsListView();
                        }
                        catch
                        {
                            MessageBox.Show("Возникла ошибка при добавлении");
                        }
                    },
                    (selectedItem) => selectedItem != null));
            }
        }
    }
}

