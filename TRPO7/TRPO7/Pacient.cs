using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPO7
{
    public class Pacient
    {
        public int IDP { get; set; } = 00001;
        public string NameP { get; set; } = "Фамилия пациента";
        public string LastNameP { get; set; } = "Имя пациента";
        public string MiddleNameP { get; set; } = "Отчество пациента";
        public DateTime Birthday { get; set; } = DateTime.MinValue;
        public string Diagnosis { get; set; } = "Диагноз пациента";
        public string Recomendations { get; set; } = "Обильное питье, постельный режим, жаропонижающие при высокой температуре";
    }
}
