def calculate_total(lst):
    return sum(lst)


class WalletModel:
    def __init__(self):
        self.wallet = {}
        self.deps = {}
        self.source = {}

    def add_source(self, name, name_source, value):
        key = f"{name}-{name_source}"
        if name in self.wallet:
            actual_value = self.wallet[name]
            self.wallet[name] = actual_value + value
        else:
            self.wallet[name] = value
        self.source[key] = value
        print(f"Ajouté {value} de {name} à votre portefeuille.")

    def add_expense(self, name, name_expense, value):
        key = f"{name}-{name_expense}"
        if name in self.wallet:
            actual_value = self.wallet[name]
            if 0.0 < value < actual_value:
                self.wallet[name] = actual_value - value
                self.deps[key] = value
                print(f"Ajouté {value} de dépense {name} à votre portefeuille.")
            elif value < 0.0:
                print("Dépense 0 ariary ?")
            elif actual_value < value:
                print(
                    f"Trop peu pour acheter quoi que cela soit :\n"
                    f"Le solde : {actual_value}\n"
                    f"Ton soit disant dépense : {value}"
                )

    def get_amounts_for_wallet(self, name):
        list_amounts = [value for key, value in self.wallet.items() if key.startswith(name)]
        self.wallet[name] = calculate_total(list_amounts)
        return self.wallet[name]

    def get_list_expense(self, name):
        list_expense = [value for key, value in self.deps.items() if key.startswith(name)]
        for key, value in self.deps.items():
            if key.startswith(name):
                print("------------- ID -------------")
                print(key)
                print("---------- DEPENSES ----------")
                print(value)
        print("---------- TOTAL -------------")
        print(len(list_expense))
        print("------------------------------")

    def get_list_source(self, name):
        list_source = [value for key, value in self.source.items() if key.startswith(name)]
        for key, value in self.source.items():
            if key.startswith(name):
                print("------------- ID -------------")
                print(key)
                print("---------- REVENUES ----------")
                print(value)
        print("---------- TOTAL -------------")
        print(len(list_source))
        print("------------------------------")

    def set_amount(self, value):
        self.wallet[value] = 0.0
