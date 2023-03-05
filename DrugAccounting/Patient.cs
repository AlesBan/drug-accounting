using System;
using System.Diagnostics;

namespace DrugAccounting
{
    public class Patient
    {
        public int id { get; set; }
        public int P_numOf_patient { get; set; }
        public string P_fullName_patient { get; set; }
        public int P_numOf_center { get; set; }
        public string P_typeOf_drug { get; set; }

        public string MainInf => $"{P_numOf_patient} {P_fullName_patient}";

        public DateTime P_dateOf_visit1 { get; set; }
        public string P_dateOf_visit1_STR
        {
            get => P_dateOf_visit1.ToString();
            set => P_dateOf_visit1 = Convert.ToDateTime(value);
        }
        public DateTime P_dateOf_visit2 { get; set; }
        public string P_dateOf_visit2_STR
        {
            get => P_dateOf_visit2.ToString();
            set => P_dateOf_visit2 = Convert.ToDateTime(value);
        }
        public DateTime P_dateOf_visit3 { get; set; }
        public string P_dateOf_visit3_STR
        {
            get => P_dateOf_visit3.ToString();
            set => P_dateOf_visit3 = Convert.ToDateTime(value);
        }
        public DateTime P_dateOf_visit4 { get; set; }
        public string P_dateOf_visit4_STR
        {
            get => P_dateOf_visit4.ToString();
            set => P_dateOf_visit4 = Convert.ToDateTime(value);
        }

        public DateTime P_timeOf_visit2 { get; set; }
        public string P_timeOf_visit2_STR
        {
            get => P_timeOf_visit2.ToString();
            set => P_timeOf_visit2 = Convert.ToDateTime(value);
        }
        public DateTime P_timeOf_visit3 { get; set; }
        public string P_timeOf_visit3_STR
        {
            get => P_timeOf_visit3.ToString();
            set => P_timeOf_visit3 = Convert.ToDateTime(value);
        }
        public DateTime P_timeOf_visit4 { get; set; }
        public string P_timeOf_visit4_STR
        {
            get => P_timeOf_visit4.ToString();
            set => P_timeOf_visit4 = Convert.ToDateTime(value);
        }

        public int P_doseOf_Drug_visit1 { get; set; }
        public int P_doseOf_Drug_visit2 { get; set; }
        public int P_doseOf_Drug_visit3 { get; set; }

        public int P_numOf_issuedPills_visit1 { get; set; }
        public int P_numOf_issuedPills_visit2 { get; set; }
        public int P_numOf_issuedPills_visit3 { get; set; }

        public string P_serial_number_visit1 { get; set; }
        public string P_serial_number_visit2 { get; set; }
        public string P_serial_number_visit3 { get; set; }

        public DateTime P_dateOf_shelfLife_visit1 { get; set; }
        public string P_dateOf_shelfLife_visit1_STR
        {
            get => P_dateOf_shelfLife_visit1.ToString();
            set => P_dateOf_visit1 = Convert.ToDateTime(value);
        }
        public DateTime P_dateOf_shelfLife_visit2 { get; set; }
        public string P_dateOf_shelfLife_visit2_STR
        {
            get => P_dateOf_shelfLife_visit2.ToString();
            set => P_dateOf_visit2 = Convert.ToDateTime(value);
        }
        public DateTime P_dateOf_shelfLife_visit3 { get; set; }
        public string P_dateOf_shelfLife_visit3_STR
        {
            get => P_dateOf_shelfLife_visit3.ToString();
            set => P_dateOf_visit3 = Convert.ToDateTime(value);
        }

        public DateTime P_dateOf_startTakingPills_visit1 { get; set; }
        public string P_dateOf_startTakingPills_visit1_STR
        {
            get => P_dateOf_startTakingPills_visit1.ToString();
            set => P_dateOf_startTakingPills_visit1 = Convert.ToDateTime(value);
        }
        public DateTime P_dateOf_startTakingPills_visit2 { get; set; }
        public string P_dateOf_startTakingPills_visit2_STR
        {
            get => P_dateOf_startTakingPills_visit2.ToString();
            set => P_dateOf_startTakingPills_visit2 = Convert.ToDateTime(value);
        }
        public DateTime P_dateOf_startTakingPills_visit3 { get; set; }
        public string P_dateOf_startTakingPills_visit3_STR
        {
            get => P_dateOf_startTakingPills_visit3.ToString();
            set => P_dateOf_startTakingPills_visit3 = Convert.ToDateTime(value);
        }

        public DateTime P_timeOf_startTakingPills_visit1 { get; set; }
        public string P_timeOf_startTakingPills_visit1_STR
        {
            get => P_timeOf_startTakingPills_visit1.ToString();
            set => P_timeOf_startTakingPills_visit1 = Convert.ToDateTime(value);
        }
        public DateTime P_timeOf_startTakingPills_visit2 { get; set; }
        public string P_timeOf_startTakingPills_visit2_STR
        {
            get => P_timeOf_startTakingPills_visit2.ToString();
            set => P_timeOf_startTakingPills_visit2 = Convert.ToDateTime(value);
        }
        public DateTime P_timeOf_startTakingPills_visit3 { get; set; }
        public string P_timeOf_startTakingPills_visit3_STR
        {
            get => P_timeOf_startTakingPills_visit3.ToString();
            set => P_timeOf_startTakingPills_visit3 = Convert.ToDateTime(value);
        }

        public DateTime P_timeOf_endTakingPills_visit1 { get; set; }
        public string P_timeOf_endTakingPills_visit1_STR
        {
            get => P_timeOf_endTakingPills_visit1.ToString();
            set => P_timeOf_endTakingPills_visit1 = Convert.ToDateTime(value);
        }

        public DateTime P_timeOf_endTakingPills_visit2 { get; set; }
        public string P_timeOf_endTakingPills_visit2_STR
        {
            get => P_timeOf_endTakingPills_visit2.ToString();
            set => P_timeOf_endTakingPills_visit2 = Convert.ToDateTime(value);
        }
        public DateTime P_timeOf_endTakingPills_visit3 { get; set; }
        public string P_timeOf_endTakingPills_visit3_STR
        {
            get => P_timeOf_endTakingPills_visit3.ToString();
            set => P_timeOf_endTakingPills_visit3 = Convert.ToDateTime(value);
        }

        public int P_numOf_acceptedPills_visit1 { get; set; }
        public int P_numOf_acceptedPills_visit2 { get; set; }
        public int P_numOf_acceptedPills_visit3 { get; set; }

        public int P_numOf_blankBlister_visit1 { get; set; }
        public int P_numOf_blankBlister_visit2 { get; set; }
        public int P_numOf_blankBlister_visit3 { get; set; }

        public int P_numOf_balancePills_visit1 { get; set; }
        public int P_numOf_balancePills_visit2 { get; set; }
        public int P_numOf_balancePills_visit3 { get; set; }
        public Patient() { }
    }
}
