package org;

import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        Action gestionPortefeuilles = new Action();
        boolean action = true;

        while (action) {
            System.out.println("\nMenu :");
            System.out.println("1. Créer un portefeuille pour une personne");
            System.out.println("2. Ajouter un source à un portefeuille");
            System.out.println("3. Ajouter une dépense à un portefeuille");
            System.out.println("4. Afficher le portefeuille d'une personne");
            System.out.println("5. Afficher la liste des dépenses d'une personne");
            System.out.println("6. Afficher la liste des revenues d'une personne");
            System.out.println("7. Vider votre portefeuille");
            System.out.println("8. Ranger votre portefeuille");

            System.out.print("Choix : ");
            int choix = scanner.nextInt();

            switch (choix) {
                case 1:
                    System.out.print("Nom de la personne : ");
                    scanner.nextLine();
                    String namePerson = scanner.nextLine();
                    System.out.println("Nombre maximum de billet : ");
                    int max_bill = Integer.parseInt(scanner.nextLine());
                    System.out.println("Nombre maximum d'objet à stocker : ");
                    int max_card = Integer.parseInt(scanner.nextLine());
                    System.out.println("Status (DISPO ou INDISPO) par défaut DISPO : ");
                    STATUS status = STATUS.valueOf(scanner.nextLine());
                    gestionPortefeuilles.createWallet(namePerson, max_bill, max_card, status);
                    break;
                case 2:
                    System.out.print("Nom de la personne : ");
                    scanner.nextLine();
                    String nameActifPerson = scanner.nextLine();
                    System.out.println("Nom du source de revenue : ");
                    String nameSource = scanner.nextLine();
                    System.out.print("Montant : ");
                    double montant = scanner.nextDouble();
                    gestionPortefeuilles.addSource(nameActifPerson, nameSource, montant);
                    break;
                case 3:
                    System.out.print("Nom de la personne : ");
                    scanner.nextLine();
                    String name = scanner.nextLine();
                    System.out.println("Nom du dépense : ");
                    String namedeps = scanner.nextLine();
                    System.out.print("Dépense : ");
                    double deps = Double.parseDouble(scanner.nextLine());
                    gestionPortefeuilles.expenses(name,namedeps,deps);
                    break;
                case 4:
                    System.out.print("Nom de la personne : ");
                    scanner.nextLine();
                    String nameAffichage = scanner.nextLine();
                    gestionPortefeuilles.showWalletModel(nameAffichage);
                    break;
                case 5:
                    System.out.print("Nom de la personne : ");
                    scanner.nextLine();
                    name = scanner.nextLine();
                    gestionPortefeuilles.showListExpenses(name);
                    break;
                case 6:
                    System.out.print("Nom de la personne : ");
                    scanner.nextLine();
                    name = scanner.nextLine();
                    gestionPortefeuilles.showListSource(name);
                    break;
                case 7:
                    System.out.print("Nom de la personne : ");
                    scanner.nextLine();
                    String reset = scanner.nextLine();
                    gestionPortefeuilles.resetWallet(reset);
                    break;
                case 8:
                    action = false;
                    break;
                default:
                    System.out.println("Choix invalide.");
                    break;
            }
        }
        System.out.println("Programme terminé");
        scanner.close();
    }
}
