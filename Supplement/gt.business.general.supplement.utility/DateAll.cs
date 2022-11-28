using System;

namespace gt.business.general.supplement.utility
{
    public static class DateAll
    {

        public static int? dateRefresh = null;
        private static string _dateRefresh = null;

        public static bool isDateToday()
        {

            DateTime dateValue = DateTime.Now;
            if (dateRefresh == null)
                dateRefresh = (int?)dateValue.DayOfWeek;
            int? _datecurrent = (int?)dateValue.DayOfWeek;

            switch (dateRefresh)
            {

                case 1: //Segunda-feira
                case 2: //Terça-feira
                case 3: //Quarta-feira
                case 4: //Quinta-feira
                case 5: //Sexta-feira
                case 6: //Sábado
                case 7: //Domingo

                    if (_datecurrent != dateRefresh)
                    {
                        return false;
                    }

                    break;
            }

            return true;
        }
    }
}
