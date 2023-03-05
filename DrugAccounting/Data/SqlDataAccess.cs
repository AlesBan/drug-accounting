using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Dapper;

namespace DrugAccounting.Data
{
    static class SqlDataAccess
    {
        //Достаем главную информацию всех пациенттов для отображения в главном меню
        public static List<Patient> LoadPatients()
        {
            using IDbConnection cnn = new SQLiteConnection(LoadConnectingString());
            var output = cnn.Query<Patient>(
                $"SELECT id, P_numOf_patient, " +
                $"P_fullName_patient, P_numOf_center, " +
                $"P_typeOf_drug FROM Table_Patient", new DynamicParameters());
            return output.ToList();
        }
        //Сохранение нового пациента
        public static void SavePatients(Patient patient)
        {
            using IDbConnection cnn = new SQLiteConnection(LoadConnectingString());
            Console.WriteLine();
            Trace.WriteLine("SAVE: \n" + patient.P_dateOf_visit1.ToString(CultureInfo.InvariantCulture) + " \n" + patient.P_dateOf_shelfLife_visit1.ToString(CultureInfo.InvariantCulture));
            string command = $"INSERT INTO Table_Patient (" +
                $"P_numOf_patient, P_fullName_patient, P_numOf_center, P_typeOf_drug, " +
                $"P_dateOf_visit1_STR, P_dateOf_visit2_STR, P_dateOf_visit3_STR, P_dateOf_visit4_STR, " +
                $"P_timeOf_visit2_STR, P_timeOf_visit3_STR, P_timeOf_visit4_STR, " +
                $"P_doseOf_Drug_visit1, P_doseOf_Drug_visit2, P_doseOf_Drug_visit3, " +
                $"P_numOf_issuedPills_visit1, P_numOf_issuedPills_visit2, P_numOf_issuedPills_visit3, " +
                $"P_serial_number_visit1, P_serial_number_visit2, P_serial_number_visit3, " +
                $"P_dateOf_shelfLife_visit1_STR, P_dateOf_shelfLife_visit2_STR, P_dateOf_shelfLife_visit3_STR, " +
                $"P_dateOf_startTakingPills_visit1_STR, P_dateOf_startTakingPills_visit2_STR, P_dateOf_startTakingPills_visit3_STR, " +
                $"P_timeOf_startTakingPills_visit1_STR, P_timeOf_startTakingPills_visit1_STR, P_timeOf_startTakingPills_visit1_STR, " +
                $"P_timeOf_endTakingPills_visit1_STR, P_timeOf_endTakingPills_visit2_STR, P_timeOf_endTakingPills_visit3_STR, " +
                $"P_numOf_acceptedPills_visit1, P_numOf_acceptedPills_visit2, P_numOf_acceptedPills_visit3, " +
                $"P_numOf_blankBlister_visit1, P_numOf_blankBlister_visit2, P_numOf_blankBlister_visit3, " +
                $"P_numOf_balancePills_visit1, P_numOf_balancePills_visit2, P_numOf_balancePills_visit3" +
                $") " +
               $"values (" +
               $"{patient.P_numOf_patient}, '{patient.P_fullName_patient}', {patient.P_numOf_center}, '{patient.P_typeOf_drug}', " +
               $"'{patient.P_dateOf_visit1}', '{patient.P_dateOf_visit2}', '{patient.P_dateOf_visit3}', '{patient.P_dateOf_visit4_STR}', " +
               $"'{patient.P_timeOf_visit2_STR}', '{patient.P_timeOf_visit3_STR}', '{patient.P_timeOf_visit4_STR}', " +
               $"{patient.P_doseOf_Drug_visit1}, {patient.P_doseOf_Drug_visit2}, {patient.P_doseOf_Drug_visit3}, " +
               $"{patient.P_numOf_issuedPills_visit1}, {patient.P_numOf_issuedPills_visit2}, {patient.P_numOf_issuedPills_visit3}, " +
               $"'{patient.P_serial_number_visit1}', '{patient.P_serial_number_visit2}', '{patient.P_serial_number_visit3}', " +
               $"'{patient.P_dateOf_shelfLife_visit1}', '{patient.P_dateOf_shelfLife_visit2_STR}', '{patient.P_dateOf_shelfLife_visit3_STR}', " +
               $"'{patient.P_dateOf_startTakingPills_visit1_STR}', '{patient.P_dateOf_startTakingPills_visit2_STR}', '{patient.P_dateOf_startTakingPills_visit3_STR}', " +
               $"'{patient.P_timeOf_startTakingPills_visit1_STR}', '{patient.P_timeOf_startTakingPills_visit2_STR}', '{patient.P_timeOf_startTakingPills_visit3_STR}'," +
               $"'{patient.P_timeOf_endTakingPills_visit1_STR}', '{patient.P_timeOf_endTakingPills_visit2_STR}', '{patient.P_timeOf_endTakingPills_visit3_STR}', " +
               $"{patient.P_numOf_acceptedPills_visit1}, {patient.P_numOf_acceptedPills_visit2}, {patient.P_numOf_acceptedPills_visit3}, " +
               $"{patient.P_numOf_blankBlister_visit1}, {patient.P_numOf_blankBlister_visit2}, {patient.P_numOf_blankBlister_visit3}, " +
               $"{patient.P_numOf_balancePills_visit1}, {patient.P_numOf_balancePills_visit2}, {patient.P_numOf_balancePills_visit3}" +
               $")";
            cnn.Execute(command);
        }
        //Обновление существующего пациента
        public static void UpdatePatient(Patient patient, int id_selectedPatient)
        {
            using IDbConnection cnn = new SQLiteConnection(LoadConnectingString());
            Console.WriteLine("UPDATE: \n" + patient.P_dateOf_visit1.ToString(CultureInfo.InvariantCulture) + " \n" + patient.P_dateOf_shelfLife_visit1.ToString(CultureInfo.InvariantCulture));
            Trace.WriteLine("UPDATE: \n" + patient.P_dateOf_visit1.ToString(CultureInfo.InvariantCulture) + " \n" + patient.P_dateOf_shelfLife_visit1.ToString(CultureInfo.InvariantCulture));

            string command = $"UPDATE Table_Patient " +
                $"SET " +
                $"P_numOf_patient = {patient.P_numOf_patient}, " +
                $"P_fullName_patient = '{patient.P_fullName_patient}', " +
                $"P_numOf_center = {patient.P_numOf_center}, " +
                $"P_typeOf_drug = '{patient.P_typeOf_drug}', " +
                $"P_dateOf_visit1_STR = '{patient.P_dateOf_visit1_STR}', " +
                $"P_dateOf_visit2_STR = '{patient.P_dateOf_visit2_STR}', " +
                $"P_dateOf_visit3_STR = '{patient.P_dateOf_visit3_STR}', " +
                $"P_dateOf_visit4_STR = '{patient.P_dateOf_visit4_STR}', " +
                $"P_timeOf_visit2_STR = '{patient.P_timeOf_visit2_STR}', " +
                $"P_timeOf_visit3_STR = '{patient.P_timeOf_visit3_STR}', " +
                $"P_timeOf_visit4_STR = '{patient.P_timeOf_visit4_STR}', " +
                $"P_doseOf_Drug_visit1 = {patient.P_doseOf_Drug_visit1}, " +
                $"P_doseOf_Drug_visit2 = {patient.P_doseOf_Drug_visit2}, " +
                $"P_doseOf_Drug_visit3 = {patient.P_doseOf_Drug_visit3}, " +
                $"P_numOf_issuedPills_visit1 = {patient.P_numOf_issuedPills_visit1}, " +
                $"P_numOf_issuedPills_visit2 = {patient.P_numOf_issuedPills_visit2}, " +
                $"P_numOf_issuedPills_visit3 = {patient.P_numOf_issuedPills_visit3}, " +
                $"P_serial_number_visit1 = '{patient.P_serial_number_visit1}', " +
                $"P_serial_number_visit2 = '{patient.P_serial_number_visit2}', " +
                $"P_serial_number_visit3 = '{patient.P_serial_number_visit3}', " +
                $"P_dateOf_shelfLife_visit1_STR = '{patient.P_dateOf_shelfLife_visit1_STR}', " +
                $"P_dateOf_shelfLife_visit2_STR = '{patient.P_dateOf_shelfLife_visit2_STR}', " +
                $"P_dateOf_shelfLife_visit3_STR = '{patient.P_dateOf_shelfLife_visit3_STR}', " +
                $"P_dateOf_startTakingPills_visit1_STR = '{patient.P_dateOf_startTakingPills_visit1_STR}', " +
                $"P_dateOf_startTakingPills_visit2_STR = '{patient.P_dateOf_startTakingPills_visit2_STR}', " +
                $"P_dateOf_startTakingPills_visit3_STR = '{patient.P_dateOf_startTakingPills_visit3_STR}', " +
                $"P_timeOf_startTakingPills_visit1_STR = '{patient.P_timeOf_startTakingPills_visit1_STR}', " +
                $"P_timeOf_startTakingPills_visit2_STR = '{patient.P_timeOf_startTakingPills_visit2_STR}', " +
                $"P_timeOf_startTakingPills_visit3_STR = '{patient.P_timeOf_startTakingPills_visit3_STR}', " +
                $"P_timeOf_endTakingPills_visit1_STR = '{patient.P_timeOf_endTakingPills_visit1_STR}', " +
                $"P_timeOf_endTakingPills_visit2_STR = '{patient.P_timeOf_endTakingPills_visit2_STR}', " +
                $"P_timeOf_endTakingPills_visit3_STR = '{patient.P_timeOf_endTakingPills_visit3_STR}', " +
                $"P_numOf_acceptedPills_visit1 = {patient.P_numOf_acceptedPills_visit1}, " +
                $"P_numOf_acceptedPills_visit2 = {patient.P_numOf_acceptedPills_visit2}, " +
                $"P_numOf_acceptedPills_visit3 = {patient.P_numOf_acceptedPills_visit3}, " +
                $"P_numOf_blankBlister_visit1 = {patient.P_numOf_blankBlister_visit1}, " +
                $"P_numOf_blankBlister_visit2 = {patient.P_numOf_blankBlister_visit2}, " +
                $"P_numOf_blankBlister_visit3 = {patient.P_numOf_blankBlister_visit3}, " +
                $"P_numOf_balancePills_visit1 = {patient.P_numOf_balancePills_visit1}, " +
                $"P_numOf_balancePills_visit2 = {patient.P_numOf_balancePills_visit2}, " +
                $"P_numOf_balancePills_visit3 = {patient.P_numOf_balancePills_visit3} " +
                $"WHERE id={id_selectedPatient}";
            cnn.Execute(command);
        }
        //Достаем нужного пациента, на которого нажади в главном окне 
        public static Patient LoadSelectedPatient(int id_selectedPatient)
        {
            using IDbConnection cnn = new SQLiteConnection(LoadConnectingString());
            var output = cnn.Query<Patient>($"SELECT * FROM Table_Patient WHERE id={id_selectedPatient}", new DynamicParameters());
            return output.ToList()[0];
        }
        //Созжание новой таблицы есои ее нет 
        public static void CreateTableIfNotExist()
        {
            string command = "CREATE TABLE IF NOT EXISTS Table_Patient (" +
                "id INTEGER NOT NULL UNIQUE," +
                "P_numOf_patient INTEGER NOT NULL," +
                "P_fullName_patient TEXT NOT NULL," +
                "P_numOf_center INTEGER NOT NULL," +
                "P_typeOf_drug TEXT NOT NULL," +
                "P_dateOf_visit1_STR TEXT NOT NULL," +
                "P_dateOf_visit2_STR TEXT," +
                "P_dateOf_visit3_STR TEXT," +
                "P_dateOf_visit4_STR TEXT," +
                "P_timeOf_visit2_STR TEXT," +
                "P_timeOf_visit3_STR TEXT," +
                "P_timeOf_visit4_STR TEXT," +
                "P_doseOf_Drug_visit1 INTEGER," +
                "P_doseOf_Drug_visit2 INTEGER," +
                "P_doseOf_Drug_visit3 INTEGER," +
                "P_numOf_issuedPills_visit1 INTEGER," +
                "P_numOf_issuedPills_visit2 INTEGER," +
                "P_numOf_issuedPills_visit3 INTEGER," +
                "P_serial_number_visit1 INTEGER," +
                "P_serial_number_visit2 INTEGER," +
                "P_serial_number_visit3 INTEGER," +
                "P_dateOf_shelfLife_visit1_STR TEXT," +
                "P_dateOf_shelfLife_visit2_STR TEXT," +
                "P_dateOf_shelfLife_visit3_STR TEXT," +
                "P_dateOf_startTakingPills_visit1_STR TEXT," +
                "P_dateOf_startTakingPills_visit2_STR TEXT," +
                "P_dateOf_startTakingPills_visit3_STR TEXT," +
                "P_timeOf_startTakingPills_visit1_STR TEXT," +
                "P_timeOf_startTakingPills_visit2_STR TEXT," +
                "P_timeOf_startTakingPills_visit3_STR TEXT," +
                "P_numOf_acceptedPills_visit1 INTEGER," +
                "P_numOf_acceptedPills_visit2 INTEGER," +
                "P_numOf_acceptedPills_visit3 INTEGER," +
                "P_numOf_blankBlister_visit1 INTEGER," +
                "P_numOf_blankBlister_visit2 INTEGER," +
                "P_numOf_blankBlister_visit3  INTEGER," +
                "P_numOf_balancePills_visit1 INTEGER," +
                "P_numOf_balancePills_visit2 INTEGER," +
                "P_numOf_balancePills_visit3 INTEGER," +
                "P_timeOf_endTakingPills_visit1_STR TEXT," +
                "P_timeOf_endTakingPills_visit2_STR TEXT," +
                "P_timeOf_endTakingPills_visit3_STR TEXT," +
                "PRIMARY KEY('id' AUTOINCREMENT)); ";
            using IDbConnection cnn = new SQLiteConnection(LoadConnectingString());
            cnn.Execute(command);
        }
        //Ужаление таблицы
        public static void DeleteTable()
        {
            using IDbConnection cnn = new SQLiteConnection(LoadConnectingString());
            cnn.Execute("DROP TABLE Table_Patient;");
            CreateTableIfNotExist();
        }
        //Путь к базе данных
        private static string LoadConnectingString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
