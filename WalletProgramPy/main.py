from action import Action
from model import WalletModel

if __name__ == "__main__":
    gestion_portefeuilles = Action()
    action = True

    while action:
        print("\nMenu :")
        print("1. Créer un portefeuille pour une personne")
        print("2. Ajouter un source à un portefeuille")
        print("3. Ajouter une dépense à un portefeuille")
        print("4. Afficher le portefeuille d'une personne")
        print("5. Afficher la liste des dépenses d'une personne")
        print("6. Afficher la liste des revenus d'une personne")
        print("7. Vider votre portefeuille")
        print("8. Ranger votre portefeuille")

        choix = int(input("Choix : "))

        if choix == 1:
            name_person = input("Nom de la personne : ")
            gestion_portefeuilles.create_wallet(name_person)
        elif choix == 2:
            name_actif_person = input("Nom de la personne : ")
            name_source = input("Nom du source de revenu : ")
            montant = float(input("Montant : "))
            gestion_portefeuilles.add_source(name_actif_person, name_source, montant)
        elif choix == 3:
            name = input("Nom de la personne : ")
            name_deps = input("Nom de la dépense : ")
            deps = float(input("Dépense : "))
            gestion_portefeuilles.expenses(name, name_deps, deps)
        elif choix == 4:
            name_affichage = input("Nom de la personne : ")
            gestion_portefeuilles.show_wallet_model(name_affichage)
        elif choix == 5:
            name = input("Nom de la personne : ")
            gestion_portefeuilles.show_list_expenses(name)
        elif choix == 6:
            name = input("Nom de la personne : ")
            gestion_portefeuilles.show_list_source(name)
        elif choix == 7:
            reset = input("Nom de la personne : ")
            gestion_portefeuilles.reset_wallet(reset)
        elif choix == 8:
            action = False
        else:
            print("Choix invalide.")

    print("Programme terminé")
