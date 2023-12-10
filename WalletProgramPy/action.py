import pickle

from model import WalletModel


class Action:
    def __init__(self):
        self.wallet = {}
        self.FILENAME = "wallet.pkl"
        self.charge_data()

    def charge_data(self):
        try:
            with open(self.FILENAME, 'rb') as file:
                obj = pickle.load(file)
                if isinstance(obj, dict):
                    self.wallet = obj
        except (IOError, pickle.UnpicklingError) as e:
            print("Impossible de charger les données existantes.")

    def save_data(self):
        try:
            with open(self.FILENAME, 'wb') as file:
                pickle.dump(self.wallet, file)
        except IOError as e:
            print("Erreur lors de la sauvegarde des données.")

    def create_wallet(self, name):
        self.wallet[name] = WalletModel()
        self.save_data()
        print(f"Portefeuille créé pour {name}")

    def add_source(self, name, source, value):
        wallet_model = self.wallet.get(name)
        if wallet_model:
            wallet_model.add_source(name, source, value)
            self.save_data()
        else:
            print(f"Portefeuille introuvable pour {name}")

    def expenses(self, name, source, value):
        wallet_model = self.wallet.get(name)
        if wallet_model:
            wallet_model.add_expense(name, source, value)
        else:
            print(f"Portefeuille introuvable pour {name}")

    def reset_wallet(self, name):
        wallet_model = self.wallet.get(name)
        if wallet_model:
            wallet_model.set_amount(name)
            self.save_data()
            print(f"Portefeuille réinitialisé pour {name}")
        else:
            print(f"Portefeuille introuvable pour {name}")

    def show_list_expenses(self, name):
        wallet_model = self.wallet.get(name)
        if wallet_model:
            wallet_model.get_list_expense(name)
        else:
            print(f"Portefeuille introuvable pour {name}")

    def show_list_source(self, name):
        wallet_model = self.wallet.get(name)
        if wallet_model:
            wallet_model.get_list_source(name)
        else:
            print(f"Portefeuille introuvable pour {name}")

    def show_wallet_model(self, name):
        wallet_model = self.wallet.get(name)
        if wallet_model:
            print(f"Votre compte est de : {wallet_model.get_amounts_for_wallet(name)}")
        else:
            print(f"Portefeuille introuvable pour {name}")
