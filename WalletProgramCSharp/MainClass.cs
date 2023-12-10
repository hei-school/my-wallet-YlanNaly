using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WalletProgram
{
    public class WalletModel
    {
        private Dictionary<string, double> wallet;
        private Dictionary<string, double> deps;
        private Dictionary<string, double> source;

        public WalletModel()
        {
            wallet = new Dictionary<string, double>();
            deps = new Dictionary<string, double>();
            source = new Dictionary<string, double>();
        }

        public void AddSource(string name, string nameSource, double value)
        {
            string key = $"{name}-{nameSource}";
            if (wallet.ContainsKey(name))
            {
                double actualValue = wallet[name];
                wallet[name] = actualValue + value;
            }
            else
            {
                wallet[name] = value;
            }
            source[key] = value;
            Console.WriteLine($"Added {value} from {name} to your wallet.");
        }

        public void CalculateTotal(List<double> lst)
        {
            double total = 0;
            foreach (var value in lst)
            {
                total += value;
            }
        }

        public void AddExpense(string name, string nameExpense, double value)
        {
            string key = $"{name}-{nameExpense}";
            if (wallet.ContainsKey(name))
            {
                double actualValue = wallet[name];
                if (0.0 < value && value < actualValue)
                {
                    wallet[name] = actualValue - value;
                    deps[key] = value;
                    Console.WriteLine($"Added {value} in expense for {name} to your wallet.");
                }
                else if (value < 0.0)
                {
                    Console.WriteLine("Expense of 0 Ariary?");
                }
                else if (actualValue < value)
                {
                    Console.WriteLine($"Too little to buy anything:\nBalance: {actualValue}\nYour supposed expense: {value}");
                }
            }
        }

        public double GetAmountsForWallet(string name)
        {
            List<double> listAmounts = new List<double>();
            foreach (var entry in wallet)
            {
                if (entry.Key.StartsWith(name))
                {
                    listAmounts.Add(entry.Value);
                }
            }
            wallet[name] = CalculateTotal(listAmounts);
            return wallet[name];
        }

        public void GetListExpense(string name)
        {
            List<double> listExpense = new List<double>();
            foreach (var entry in deps)
            {
                if (entry.Key.StartsWith(name))
                {
                    listExpense.Add(entry.Value);
                    Console.WriteLine("------------- ID -------------");
                    Console.WriteLine(entry.Key);
                    Console.WriteLine("---------- EXPENSES ----------");
                    Console.WriteLine(entry.Value);
                }
            }
            Console.WriteLine("---------- TOTAL -------------");
            Console.WriteLine(listExpense.Count);
            Console.WriteLine("------------------------------");
        }

        public void GetListSource(string name)
        {
            List<double> listSource = new List<double>();
            foreach (var entry in source)
            {
                if (entry.Key.StartsWith(name))
                {
                    listSource.Add(entry.Value);
                    Console.WriteLine("------------- ID -------------");
                    Console.WriteLine(entry.Key);
                    Console.WriteLine("---------- SOURCES ----------");
                    Console.WriteLine(entry.Value);
                }
            }
            Console.WriteLine("---------- TOTAL -------------");
            Console.WriteLine(listSource.Count);
            Console.WriteLine("------------------------------");
        }

        public void SetAmount(string value)
        {
            wallet[value] = 0.0;
        }
    }

    public class Action
    {
        private WalletModel walletModel;
        private const string FILENAME = "wallet_data.json";

        public Action()
        {
            walletModel = new WalletModel();
            ChargeData();
        }

        public void ChargeData()
        {
            try
            {
                if (File.Exists(FILENAME))
                {
                    string jsonData = File.ReadAllText(FILENAME);
                    var data = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonData);

                    walletModel = new WalletModel
                    {
                        Wallet = (Dictionary<string, double>)data["Wallet"],
                        Deps = (Dictionary<string, double>)data["Deps"],
                        Source = (Dictionary<string, double>)data["Source"]
                    };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading data: " + e.Message);
            }
        }

        public void SaveData()
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(new { Wallet = walletModel.Wallet, Deps = walletModel.Deps, Source = walletModel.Source });
                File.WriteAllText(FILENAME, jsonData);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving data: " + e.Message);
            }
        }

        public void CreateWallet(string name)
        {
            walletModel.Wallet[name] = 0.0;
            SaveData();
            Console.WriteLine($"Wallet created for {name}");
        }

        public void AddSource(string name, string source, double value)
        {
            walletModel.AddSource(name, source, value);
            SaveData();
        }

        public void AddExpense(string name, string nameExpense, double value)
        {
            walletModel.AddExpense(name, nameExpense, value);
            SaveData();
        }

        public void ShowWalletModel(string name)
        {
            Console.WriteLine($"Your account is: {walletModel.GetAmountsForWallet(name)}");
        }

        public void ShowListExpenses(string name)
        {
            walletModel.GetListExpense(name);
        }

        public void ShowListSource(string name)
        {
            walletModel.GetListSource(name);
        }

        public void ResetWallet(string name)
        {
            walletModel.SetAmount(name);
            SaveData();
            Console.WriteLine($"Wallet reset for {name}");
        }

        public static void Main(string[] args)
        {
            Action gestionPortefeuilles = new Action();
            bool action = true;

            while (action)
            {
                Console.WriteLine("\nMenu :");
                Console.WriteLine("1. Create a wallet for a person");
                Console.WriteLine("2. Add a source to a wallet");
                Console.WriteLine("3. Add an expense to a wallet");
                Console.WriteLine("4. Display the wallet for a person");
                Console.WriteLine("5. Display the expense list for a person");
                Console.WriteLine("6. Display the source list for a person");
                Console.WriteLine("7. Reset your wallet");
                Console.WriteLine("8. Exit");

                Console.Write("Choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Person's name: ");
                        string namePerson = Console.ReadLine();
                        gestionPortefeuilles.CreateWallet(namePerson);
                        break;
                    case 2:
                        Console.Write("Person's name: ");
                        string nameActifPerson = Console.ReadLine();
                        Console.Write("Name of the income source: ");
                        string nameSource = Console.ReadLine();
                        Console.Write("Amount: ");
                        double amount = Convert.ToDouble(Console.ReadLine());
                        gestionPortefeuilles.AddSource(nameActifPerson, nameSource, amount);
                        break;
                    case 3:
                        Console.Write("Person's name: ");
                        string name = Console.ReadLine();
                        Console.Write("Name of the expense: ");
                        string nameDeps = Console.ReadLine();
                        Console.Write("Expense: ");
                        double deps = Convert.ToDouble(Console.ReadLine());
                        gestionPortefeuilles.AddExpense(name, nameDeps, deps);
                        break;
                    case 4:
                        Console.Write("Person's name: ");
                        string nameAffichage = Console.ReadLine();
                        gestionPortefeuilles.ShowWalletModel(nameAffichage);
                        break;
                    case 5:
                        Console.Write("Person's name: ");
                        string nameListExpenses = Console.ReadLine();
                        gestionPortefeuilles.ShowListExpenses(nameListExpenses);
                        break;
                    case 6:
                        Console.Write("Person's name: ");
                        string nameListSource = Console.ReadLine();
                        gestionPortefeuilles.ShowListSource(nameListSource);
                        break;
                    case 7:
                        Console.Write("Person's name: ");
                        string reset = Console.ReadLine();
                        gestionPortefeuilles.ResetWallet(reset);
                        break;
                    case 8:
                        action = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }

            Console.WriteLine("Program terminated.");
        }
    }
}
